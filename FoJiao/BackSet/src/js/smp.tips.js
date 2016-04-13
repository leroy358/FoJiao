/*
*author:smallerpig
*time:20140724
*to:用于友好的向用户提示信息.
*readme:使用页面必须包含body元素，否则直接报错。
*/





/*
* paramer:data:显示内容。
* paramer:type:显示类型，如果是警告框请传入：warn
* keeptime:希望显示的时间


*/





if(typeof(smp) =="undefined"){
	var smp = {};
	smp.prototype={
		constructor:smp,
		version:"0.1 beat",
		author:"smallerpig"
	}
	smp.showing=false;//正在显示中？
}

smp.show = function(data,type,duration){
	if(smp.showing) return;
	var tipsContainer = document.createElement("div");
	tipsContainer.id = "smpTips";
	if (type=="warn") {
		var background = "background-color:#c9302c";
	} else{
		var background = "background-color:#449d44;";
	};
	if(duration == undefined){
		duration = 2000;
	}
	
	var textColor =  "color:#fff;";
	var strStyle = "position:fixed;top:50%;left:50%;width:200px;margin-left:-100px;margin-top:-24px;padding:16px 16px;" + textColor
				+"border-radius: 8px; text-align: center; min-height:16px; line-height:16px;font-size:14px; box-shadow:rgba(0,0,0,0.3) 1px 1px 4px;" + background;
	tipsContainer.setAttribute("style",strStyle);
	var dataContainer = document.createElement("span");
	dataContainer.innerText = data;	
	tipsContainer.appendChild(dataContainer);

	document.getElementsByTagName("body")[0].appendChild(tipsContainer);
	smp.showing=true;
	setTimeout(function() {
		document.getElementsByTagName("body")[0].removeChild(tipsContainer);
		smp.showing = false;
	}, duration);
};