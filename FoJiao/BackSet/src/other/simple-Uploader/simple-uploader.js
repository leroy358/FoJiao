/*

	var uploader = simple.uploader.create({
			appendTo:domElement, 			//必填。在指定的html元素容器内创建上传组件，此html元素position值不能是static，上传组件会铺满容器
			url:urlString,					//必填。上传文件的服务器接收路径。服务器端必须以post方式接收文件，文件对应的键值是file0,file1,file2......
			valueBindTo:inputElement,		//必填。在指定的input元素上绑定上传完毕的服务器返回值
			multiple:boolean				//是否支持多文件上传，布尔值，如果不设置，默认值为false;
			limit:{							//上传文件限制条件，这个字段以及其下的任何参数都是可选的，未设置的字段表示没有限制
				fileSizeMin:fileSize(KB),		//上传的单个文件最小文件尺寸，实数，以KB为单位，如果想指定为不限制，填写0
				fileSizeMax:fileSize(KB),		//上传的单个文件最大文件尺寸，实数，以KB为单位，如果想指定为不限制，填写0
				fileFormat:formatString			//上传文件格式，字符串，例如："zip|jpg|png"，如果想指定为任意文件，使用"*"，此字段会作用于一部分浏览器文件选框的格式要求（有些浏览器不支持文件格式筛选），并在追加js验证。
			},
			onEvent:{						//在特定事件下执行的回调函数
				onSelectFile:function,			//用户选择需要上传的文件后执行该函数，两个传入参数：选定的文件列表、limit参数的验证结果。该函数可通过返回false阻止上传，通过返回true忽略验证结果继续上传。
				onProgress:function,			//上传进度有变化时执行的函数，一个传入参数：标准的xhr事件传入参数。
				onError:function,				//发生上传错误时执行的函数，一个传入参数：标准的xhr事件传入参数。
				onCancelWhenUpload:function,	//用户手动取消上传时执行的函数。该函数可通过返回false来阻止用户的取消行为。
				onCancelWhenComplete:function,	//用户手动取消上传完毕的文件时执行的函数。该函数可通过返回false来阻止用户的取消行为。
				onComplete:function				//上传完毕时执行的函数。两个传入值：上传的文件列表、服务器返回字符串。可返回false重置界面至选择文件状态，并阻止组件给valueBindTo的input赋值（服务器端并不任何受影响）。
			},
			uiText:{							//指定ui文字，不设定则使用默认值
				selectFile:"选择文件",
				limit:"文件不符合要求",
				limit_moreInfo:"详情",
				limit_fileSize_main:"当前选择的文件大小不符合要求！",
				limit_fileSize_min:"最小文件尺寸：",
				limit_fileSize_max:"最大文件尺寸：",
				limit_fileSize_fileList:"不符合尺寸要求的文件有：",
				limit_fileFormat_main:"当前选择的文件格式不符合要求！",
				limit_fileFormat_formats:"支持的格式有：",
				limit_fileFormat_fileList:"不符合格式要求的文件有：",
				error:"上传时发生错误",
				error_moreInfo:"详情",
				error_moreInfo_detail:"因络不稳定或服务器未正常响应，上传中断。\n\n请检查你的网络连接和网速是否稳定。",
				complete:"上传完毕",
				complete_moreInfo:"详情",
				complete_moreInfo_main:"文件上传完毕",
				complete_moreInfo_count:"共有文件数量：",
				complete_moreInfo_fileList:"上传的文件有："
			}
		});
	
	uploader.setData();					//设置上传组件参数，参数格式为一个object对象，和simple.uploader.create参数一样，但不支持"appendTo"字段，未被设置的字段将使用原来的值。valueBindTo指向的input元素在下次提交完成时才会被赋值。
	uploader.getData();					//获取上传组件参数，参数格式为一个object对象，和simple.uploader.create参数一样
	uploader.getState();				//获取上传组件当前的状态，返回值为字符串
											//selectFile	可以点击选择文件，初始界面
											//uploading		正在上传文件
											//uploadFail	上传过程出现问题而失败，处在“查看上传错误的界面”
											//uploadSucceed	上传成功
											//uploadLimit	因用户选择的文件不符合limit参数规定的要求而出现错误，处在“查看文件不符合要求的界面”
	uploader.cancel();					//如果正在上传，取消上传。如果已上传完毕，则取消上传并清空valueBindTo的绑定值。如果处于其他状态，则无效。
	
*/


if(typeof(simple)=="undefined"){
	var simple = {};
}

simple.uploader=function(){
	var indexCounter=0;
	
	function create(dataObj){
		return new class_uploader(dataObj);
	}
	
	function class_uploader(dataObj){
		var index;
		var appendTo;
		var url;
		var valueBindTo;
		var multiple;
		var innerElements;
		var limit={};
		var onEvent={};
		var uiText={};
		var htmlElement={};
		var state;//selectFile,uploading,uploadFail,uploadSucceed,uploadLimit
		var xhr;
		var xhrFormData;
		var alertInfo;
		var lastRecordByteLoaded;
		var lastRecordTime;
		this.getData=getData;
		this.setData=setData;
		this.getState=getState;
		this.cancel=cancel;
		//赋值
		index=indexCounter++;
		appendTo=dataObj.appendTo;
		buildHtml();
		setData(dataObj);
		addEvents();
		gotoAndResetStage("selectFile");
		
/////////////////////////////////
//公有函数

		function setData(_dataObj){
			//set
			url=checkValid(_dataObj.url,url);
			valueBindTo=checkValid(_dataObj.valueBindTo,valueBindTo);
			multiple=checkValid(_dataObj.multiple,multiple,false);
			_dataObj.limit=checkValid(_dataObj.limit,{});
			limit.fileSizeMin=checkValid(_dataObj.limit.fileSizeMin,limit.fileSizeMin,0);
			limit.fileSizeMax=checkValid(_dataObj.limit.fileSizeMax,limit.fileSizeMax,0);
			limit.fileFormat=checkValid(_dataObj.limit.fileFormat,limit.fileFormat,"*");
			_dataObj.onEvent=checkValid(_dataObj.onEvent,{});
			onEvent.onSelectFile=checkValid(_dataObj.onEvent.onSelectFile,onEvent.onSelectFile,function(e){});
			onEvent.onProgress=checkValid(_dataObj.onEvent.onProgress,onEvent.onProgress,function(e){});
			onEvent.onError=checkValid(_dataObj.onEvent.onError,onEvent.onError,function(e){});
			onEvent.onCancelWhenUpload=checkValid(_dataObj.onEvent.onCancelWhenUpload,onEvent.onCancelWhenUpload,function(e){});
			onEvent.onCancelWhenComplete=checkValid(_dataObj.onEvent.onCancelWhenComplete,onEvent.onCancelWhenComplete,function(e){});
			onEvent.onComplete=checkValid(_dataObj.onEvent.onComplete,onEvent.onComplete,function(e){});
			_dataObj.uiText=checkValid(_dataObj.uiText,{});
			uiText.selectFile=checkValid(_dataObj.uiText.selectFile,uiText.selectFile,defaultUiText.selectFile);
			uiText.limit=checkValid(_dataObj.uiText.limit,uiText.limit,defaultUiText.limit);
			uiText.limit_moreInfo=checkValid(_dataObj.uiText.limit_moreInfo,uiText.limit_moreInfo,defaultUiText.limit_moreInfo);
			uiText.limit_fileSize_main=checkValid(_dataObj.uiText.limit_fileSize_main,uiText.limit_fileSize_main,defaultUiText.limit_fileSize_main);
			uiText.limit_fileSize_min=checkValid(_dataObj.uiText.limit_fileSize_min,uiText.limit_fileSize_min,defaultUiText.limit_fileSize_min);
			uiText.limit_fileSize_max=checkValid(_dataObj.uiText.limit_fileSize_max,uiText.limit_fileSize_max,defaultUiText.limit_fileSize_max);
			uiText.limit_fileSize_fileList=checkValid(_dataObj.uiText.limit_fileSize_fileList,uiText.limit_fileSize_fileList,defaultUiText.limit_fileSize_fileList);
			uiText.limit_fileFormat_main=checkValid(_dataObj.uiText.limit_fileFormat_main,uiText.limit_fileFormat_main,defaultUiText.limit_fileFormat_main);
			uiText.limit_fileFormat_formats=checkValid(_dataObj.uiText.limit_fileFormat_formats,uiText.limit_fileFormat_formats,defaultUiText.limit_fileFormat_formats);
			uiText.limit_fileFormat_fileList=checkValid(_dataObj.uiText.limit_fileFormat_fileList,uiText.limit_fileFormat_fileList,defaultUiText.limit_fileFormat_fileList);
			uiText.error=checkValid(_dataObj.uiText.error,uiText.error,defaultUiText.error);
			uiText.error_moreInfo=checkValid(_dataObj.uiText.error_moreInfo,uiText.error_moreInfo,defaultUiText.error_moreInfo);
			uiText.error_moreInfo_detail=checkValid(_dataObj.uiText.error_moreInfo_detail,uiText.error_moreInfo_detail,defaultUiText.error_moreInfo_detail);
			uiText.complete=checkValid(_dataObj.uiText.complete,uiText.complete,defaultUiText.complete);
			uiText.complete_moreInfo=checkValid(_dataObj.uiText.complete_moreInfo,uiText.complete_moreInfo,defaultUiText.complete_moreInfo);
			uiText.complete_moreInfo_main=checkValid(_dataObj.uiText.complete_moreInfo_main,uiText.complete_moreInfo_main,defaultUiText.complete_moreInfo_main);
			uiText.complete_moreInfo_count=checkValid(_dataObj.uiText.complete_moreInfo_count,uiText.complete_moreInfo_count,defaultUiText.complete_moreInfo_count);
			uiText.complete_moreInfo_fileList=checkValid(_dataObj.uiText.complete_moreInfo_fileList,uiText.complete_moreInfo_fileList,defaultUiText.complete_moreInfo_fileList);
			//refresh
			gotoAndResetStage(state);
		}
		
		function getData(){
			return{
				appendTo:appendTo,
				url:url,
				valueBindTo:valueBindTo,
				multiple:multiple,
				limit:{
					fileSizeMin:limit.fileSizeMin,
					fileSizeMax:limit.fileSizeMax,
					fileFormat:limit.fileFormat
				},
				onEvent:{	
					onSelectFile:onEvent.onSelectFile,
					onProgress:onEvent.onProgress,
					onError:onEvent.onError,
					onCancelWhenUpload:onEvent.onCancelWhenUpload,
					onCancelWhenComplete:onEvent.onCancelWhenComplete,
					onComplete:onEvent.onComplete
				},
				uiText:{
					selectFile:uiText.selectFile,
					limit:uiText.limit,
					limit_moreInfo:uiText.limit_moreInfo,
					limit_fileSize_main:uiText.limit_fileSize_main,
					limit_fileSize_min:uiText.limit_fileSize_min,
					limit_fileSize_max:uiText.limit_fileSize_max,
					limit_fileSize_fileList:uiText.limit_fileSize_fileList,
					limit_fileFormat_main:uiText.limit_fileFormat_main,
					limit_fileFormat_formats:uiText.limit_fileFormat_formats,
					limit_fileFormat_fileList:uiText.limit_fileFormat_fileList,
					error:uiText.error,
					error_moreInfo:uiText.error_moreInfo,
					error_moreInfo_detail:uiText.error_moreInfo_detail,
					complete:uiText.complete,
					complete_moreInfo:uiText.complete_moreInfo,
					complete_moreInfo_main:uiText.complete_moreInfo_main,
					complete_moreInfo_count:uiText.complete_moreInfo_count,
					complete_moreInfo_fileList:uiText.complete_moreInfo_fileList
				}
			};
		}
		
		function getState(){
			return state;
		}
		
		function cancel(){
			if(state=="uploading"){
				xhr.abort();
				gotoAndResetStage("selectFile");
			}else if(state=="uploadSucceed"){
				gotoAndResetStage("selectFile");
				valueBindTo.value="";
			}
		}
		
/////////////////////////////////
//私有函数

		function buildHtml(){
			htmlElement["s-uploader-wrapper"]=document.createElement("div");
			htmlElement["s-uploader-wrapper"].setAttribute("s-uploader-wrapper","");
			var htmlText=
				'<input type="file" style="display:none;" accept="" s-uploader-inputFile id="s-uploader-inputFile'+index+'"/>'

+				'<span style="visibility:hidden;" s-uploader-wrapper-selectFile>'
+					'<label for="s-uploader-inputFile'+index+'" s-uploader-selectFile>'
+						'<div s-uploader-selectFile-text>选择文件</div>'
+					'</label>'
+				'</span>'

+				'<span style="visibility:hidden;" s-uploader-wrapper-uploadFile>'
+					'<div s-uploader-progressBarWrapper>'
+						'<div s-uploader-progressBarInnerWrapper>'
+							'<div s-uploader-progressBar style="left:-100%;"></div>'
+							'<div s-uploader-speedInfo>000 KB/s</div>'
+						'</div>'
+					'</div>'
+					'<div s-uploader-cancelUploadButton></div>'
+				'</span>'

+				'<span style="visibility:hidden;" s-uploader-wrapper-succeedInfo>'
+					'<div s-uploader-success>'
+						'<div s-uploader-successTip>'
+							'<span s-uploader-successTip-text>上传成功</span>'
+							'<span s-uploader-successTip-moreInfo>[详情]</span>'
+						'</div>'
+					'</div>'
+					'<div s-uploader-cancelSucceedButton></div>'
+				'</span>'

+				'<span style="visibility:hidden;" s-uploader-wrapper-FailInfo>'
+					'<div s-uploader-fail>'
+						'<div s-uploader-failTip>'
+							'<span s-uploader-failTip-text>上传失败</span>'
+							'<span s-uploader-failTip-moreInfo>[详情]</span>'
+						'</div>'
+					'</div>'
+					'<div s-uploader-returnButton></div>'
+				'</span>'
			htmlElement["s-uploader-wrapper"].innerHTML=htmlText;
			htmlElement["s-uploader-inputFile"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-inputFile]");
			htmlElement["s-uploader-wrapper-selectFile"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-wrapper-selectFile]");
			htmlElement["s-uploader-selectFile-text"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-selectFile-text]");
			htmlElement["s-uploader-wrapper-uploadFile"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-wrapper-uploadFile]");
			htmlElement["s-uploader-progressBar"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-progressBar]");
			htmlElement["s-uploader-speedInfo"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-speedInfo]");
			htmlElement["s-uploader-cancelUploadButton"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-cancelUploadButton]");
			htmlElement["s-uploader-wrapper-succeedInfo"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-wrapper-succeedInfo]");
			htmlElement["s-uploader-successTip-text"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-successTip-text]");
			htmlElement["s-uploader-successTip-moreInfo"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-successTip-moreInfo]");
			htmlElement["s-uploader-cancelSucceedButton"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-cancelSucceedButton]");
			htmlElement["s-uploader-wrapper-FailInfo"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-wrapper-FailInfo]");
			htmlElement["s-uploader-failTip-text"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-failTip-text]");
			htmlElement["s-uploader-failTip-moreInfo"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-failTip-moreInfo]");
			htmlElement["s-uploader-returnButton"]=htmlElement["s-uploader-wrapper"].querySelector("[s-uploader-returnButton]");
			
			appendTo.appendChild(htmlElement["s-uploader-wrapper"]);
		}
		
		function addEvents(){
			htmlElement["s-uploader-successTip-moreInfo"].addEventListener("click",onClickMoreInfoF);
			htmlElement["s-uploader-failTip-moreInfo"].addEventListener("click",onClickMoreInfoF);
			htmlElement["s-uploader-returnButton"].addEventListener("click",onClickReturnButton);
			htmlElement["s-uploader-cancelUploadButton"].addEventListener("click",onClickCancelUploadButtonF);
			htmlElement["s-uploader-cancelSucceedButton"].addEventListener("click",onClickCancelSucceedButtonF);
		}
		
		function onSelectFileF(e){
			//checkLimit
			var tempFile;
			var fileFormatUnmatchIndexArray=[];
			var fileSizeUnmatchArray=[];
			for(var i=0;i<htmlElement["s-uploader-inputFile"].files.length;i++){
				tempFile=htmlElement["s-uploader-inputFile"].files[i];
				if(limit.fileFormat!="*"){
					var tempFileNameArray=tempFile.name.split(".");
					var hasFormat=limit.fileFormat.indexOf(tempFileNameArray[tempFileNameArray.length-1]);
					if(tempFileNameArray.length==1||hasFormat==-1){
						fileFormatUnmatchIndexArray.push(i);
					}
				}
				if(
					(tempFile.size<limit.fileSizeMin*1024&&limit.fileSizeMin>0)||(tempFile.size>limit.fileSizeMax*1024&&limit.fileSizeMax>0)
				){
					fileSizeUnmatchArray.push(i);
				}
			}
			var onSelectFileEReturn=onEvent.onSelectFile(htmlElement["s-uploader-inputFile"].files,fileFormatUnmatchIndexArray.length+fileSizeUnmatchArray.length==0);
			if(onSelectFileEReturn==false){
				gotoAndResetStage("selectFile");
			}else if(fileFormatUnmatchIndexArray.length+fileSizeUnmatchArray.length!=0&&onSelectFileEReturn!=true){
			//checkFail
				alertInfo="";
				if(fileFormatUnmatchIndexArray.length>0){
					alertInfo+=uiText.limit_fileFormat_main;
					alertInfo+="\n\n"+uiText.limit_fileFormat_formats;
					alertInfo+="\n"+limit.fileFormat.replace(/\|/g," ");
					alertInfo+="\n\n"+uiText.limit_fileFormat_fileList;
					alertInfo+=""+function(){
							var tempStr="";
							for(var i=0;i<fileFormatUnmatchIndexArray.length;i++){
								tempStr+="\n- "+htmlElement["s-uploader-inputFile"].files[fileFormatUnmatchIndexArray[i]].name;
							}
							return tempStr;
						}()+"\n\n";
				}
				if(fileSizeUnmatchArray.length>0){
					alertInfo+=uiText.limit_fileSize_main+"\n";
					if(limit.fileSizeMin>0){
						alertInfo+="\n"+uiText.limit_fileSize_min;
						alertInfo+=fileSizeConvert(limit.fileSizeMin*1024);
					}
					if(limit.fileSizeMax>0){
						alertInfo+="\n"+uiText.limit_fileSize_max;
						alertInfo+=fileSizeConvert(limit.fileSizeMax*1024);
					}
					alertInfo+="\n\n"+uiText.limit_fileSize_fileList;
					alertInfo+=""+function(){
							var tempStr="";
							for(var i=0;i<fileSizeUnmatchArray.length;i++){
								tempStr+="\n- "+htmlElement["s-uploader-inputFile"].files[fileSizeUnmatchArray[i]].name;
								tempStr+=" ("+fileSizeConvert(htmlElement["s-uploader-inputFile"].files[fileSizeUnmatchArray[i]].size)+")";
							}
							return tempStr;
						}()+"\n\n";
				}
				gotoAndResetStage("uploadLimit");
			}else{
			//checkPass
				gotoAndResetStage("uploading");
				htmlElement["s-uploader-speedInfo"].innerHTML="0 KB/s";
				htmlElement["s-uploader-progressBar"].style.left="-100%";
				xhrFormData = buildFormData(htmlElement["s-uploader-inputFile"].files);
				xhr = new XMLHttpRequest();
		
				xhr.upload.addEventListener("progress",onUploadProgressF);
				lastRecordByteLoaded=0;
				lastRecordTime=0;
				xhr.addEventListener("loadstart", onUploadStartF);
				xhr.addEventListener("load", onUploadCompleteF);
				xhr.addEventListener("error", onUploadErrorF);
				xhr.addEventListener("abort", onUploadCancelF);

				xhr.open("post",url);
				xhr.send(xhrFormData);
			}
		}
		
		function onClickMoreInfoF(e){
			alert(alertInfo);
		}
		
		function onClickReturnButton(e){
			gotoAndResetStage("selectFile");
		}
		
		function onClickCancelUploadButtonF(e){
			var onCancelWhenUploadEReturn=onEvent.onCancelWhenUpload();
			if(onCancelWhenUploadEReturn!=false){
				xhr.abort();
				gotoAndResetStage("selectFile");
			}
		}
		
		function onClickCancelSucceedButtonF(e){
			var onCancelWhenCompleteEReturn=onEvent.onCancelWhenComplete();
			if(onCancelWhenCompleteEReturn!=false){
				gotoAndResetStage("selectFile");
				valueBindTo.value="";
			}
		}
		
		//
		function onUploadStartF(e){
			
		}
		
		function onUploadProgressF(e){
			var speed;
			var newTime=(new Date()).getTime();
			var percentage=100*e.loaded/e.total;
			htmlElement["s-uploader-progressBar"].style.left=(percentage-100)+"%";
			if(lastRecordTime!=0){
				speed=(e.loaded-lastRecordByteLoaded)*1000/(newTime-lastRecordTime);
				htmlElement["s-uploader-speedInfo"].innerHTML=fileSizeConvert(speed)+"/s"
			}
			lastRecordByteLoaded=e.loaded;
			lastRecordTime=newTime;
			onEvent.onProgress(e);
		}
		
		function onUploadErrorF(e){
			gotoAndResetStage("uploadFail");
			alertInfo=uiText.error_moreInfo_detail;
			onEvent.onError(e);
		}
		
		function onUploadCancelF(e){
			
		}
		
		function onUploadCompleteF(e){
			var onCompleteEReturn=onEvent.onComplete(htmlElement["s-uploader-inputFile"].files,e.target.responseText);
			if(onCompleteEReturn!=false){
				gotoAndResetStage("uploadSucceed");
				alertInfo="";
				alertInfo+=uiText.complete_moreInfo_main;
				alertInfo+="\n\n"+uiText.complete_moreInfo_count;
				alertInfo+=htmlElement["s-uploader-inputFile"].files.length;
				alertInfo+="\n\n"+uiText.complete_moreInfo_fileList;
				alertInfo+=""+function(){
								var tempStr="";
								for(var i=0;i<htmlElement["s-uploader-inputFile"].files.length;i++){
									tempStr+="\n- "+htmlElement["s-uploader-inputFile"].files[i].name;
								}
								return tempStr;
							}()+"\n\n";
				valueBindTo.value=e.target.responseText;
			}else{
				gotoAndResetStage("selectFile");
			}
		}
		
		//selectFile,uploading,uploadSucceed,uploadFail,uploadLimit
		function gotoAndResetStage(stageName){
			if(stageName=="uploadFail"){
				state="uploadFail";
				htmlElement["s-uploader-wrapper-selectFile"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-uploadFile"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-succeedInfo"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-FailInfo"].style.visibility="visible";
				htmlElement["s-uploader-failTip-text"].innerHTML=uiText.error;
				htmlElement["s-uploader-failTip-moreInfo"].innerHTML=" ["+uiText.error_moreInfo+"]";
			}else if(stageName=="uploadLimit"){
				state="uploadLimit";
				htmlElement["s-uploader-wrapper-selectFile"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-uploadFile"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-succeedInfo"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-FailInfo"].style.visibility="visible";
				htmlElement["s-uploader-failTip-text"].innerHTML=uiText.limit;
				htmlElement["s-uploader-failTip-moreInfo"].innerHTML=" ["+uiText.limit_moreInfo+"]";
			}else if(stageName=="uploading"){
				state="uploading";
				htmlElement["s-uploader-wrapper-selectFile"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-uploadFile"].style.visibility="visible";
				htmlElement["s-uploader-wrapper-succeedInfo"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-FailInfo"].style.visibility="hidden";
			}else if(stageName=="uploadSucceed"){
				state="uploadSucceed";
				htmlElement["s-uploader-wrapper-selectFile"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-uploadFile"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-succeedInfo"].style.visibility="visible";
				htmlElement["s-uploader-wrapper-FailInfo"].style.visibility="hidden";
				htmlElement["s-uploader-successTip-text"].innerHTML=uiText.complete;
				htmlElement["s-uploader-successTip-moreInfo"].innerHTML=" ["+uiText.complete_moreInfo+"]";
			}else if(stageName=="selectFile"){
				state="selectFile";
				htmlElement["s-uploader-wrapper-selectFile"].style.visibility="visible";
				htmlElement["s-uploader-wrapper-uploadFile"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-succeedInfo"].style.visibility="hidden";
				htmlElement["s-uploader-wrapper-FailInfo"].style.visibility="hidden";
				htmlElement["s-uploader-selectFile-text"].innerHTML=uiText.selectFile;
				htmlElement["s-uploader-inputFile"].parentNode.removeChild(htmlElement["s-uploader-inputFile"]);
				htmlElement["s-uploader-inputFile"]=document.createElement("input");
				htmlElement["s-uploader-inputFile"].setAttribute("type","file");
				htmlElement["s-uploader-inputFile"].setAttribute("style","display:none;");
				htmlElement["s-uploader-inputFile"].setAttribute("s-uploader-inputFile","");
				htmlElement["s-uploader-inputFile"].setAttribute("id","s-uploader-inputFile"+index);
				if(multiple){
					htmlElement["s-uploader-inputFile"].setAttribute("multiple","");
				}else{
					htmlElement["s-uploader-inputFile"].removeAttribute("multiple");
				}
				if(limit.fileFormat=="*"){
					htmlElement["s-uploader-inputFile"].setAttribute("accept","");
				}else{
					var tempFileFormat=limit.fileFormat.split("|");
					var tempStr="";
					for(var i=0;i<tempFileFormat.length;i++){
						tempStr+=",."+tempFileFormat[i];
					}
					var tempStr=tempStr.substr(1);
					htmlElement["s-uploader-inputFile"].setAttribute("accept",tempStr);
				}
				htmlElement["s-uploader-wrapper"].appendChild(htmlElement["s-uploader-inputFile"]);
				htmlElement["s-uploader-inputFile"].addEventListener("change",onSelectFileF);
			}
		}
		
/////////////////////////////
		
	}
	
	var defaultUiText={
		selectFile:"选择文件",
		limit:"文件不符合要求",
		limit_moreInfo:"详情",
		limit_fileSize_main:"====================\n\n当前所选文件大小不符合要求",
		limit_fileSize_min:"文件最小：",
		limit_fileSize_max:"文件最大：",
		limit_fileSize_fileList:"不符合大小要求的文件有：",
		limit_fileFormat_main:"====================\n\n当前选择的文件格式不符合要求",
		limit_fileFormat_formats:"支持的格式有：",
		limit_fileFormat_fileList:"格式符合要求的文件有：",
		error:"上传时发生错误",
		error_moreInfo:"详情",
		error_moreInfo_detail:"====================\n\n因络不稳定或服务器未正常响应，上传中断。\n请检查你的网络连接和网速是否稳定。",
		complete:"上传完毕",
		complete_moreInfo:"详情",
		complete_moreInfo_main:"====================\n\n文件上传完毕",
		complete_moreInfo_count:"上传文件数量：",
		complete_moreInfo_fileList:"上传的文件："
	}
	
	function checkValid(value,altValue,altValue2){
		if(typeof(value)!="undefined"&&typeof(value)!="null"){
			return value;
		}else if(typeof(altValue)!="undefined"&&typeof(altValue)!="null"){
			return altValue;
		}else{
			return altValue2;
		}
	}
	
	function buildFormData(files){
		var tempFormData=new FormData();
		for(var i=0;i<files.length;i++){
			tempFormData.append("file"+i,files[i]);
		}
		return tempFormData;
	}
	
	//byte2Auto
	function fileSizeConvert(bytes){
		var mark;
		var convertedSize;
		if(bytes>=1024*1024*1024){
			mark="GB";
			convertedSize=bytes/(1024*1024*1024);
		}else if(bytes>=1024*1024){
			mark="MB";
			convertedSize=bytes/(1024*1024);
		}else if(bytes>=1024){
			mark="KB";
			convertedSize=bytes/(1024);
		}else{
			mark="B";
			convertedSize=bytes;
		}
		convertedSize=convertedSize.toFixed(1);
		return convertedSize+" "+mark;
	}
	
	return {
		create:create
	};

}();

