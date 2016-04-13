window.addEventListener("load",function(e){
	var openedMenuItemsArea=document.getElementById("openedMenuItemsAreaInnerWrapper");
	var contentArea=document.getElementById("contentAreaInnerWrapper");
	var menuItems=document.getElementsByClassName("treeItem");
	
	processMenuWrapperRight();
	
	for(var i=0;i<menuItems.length;i++){
		processMenuItem(menuItems[i]);
	}
	
	function processMenuItem(menuItem){
		var openedMenuItem=document.createElement("div");
		var iframeItem=document.createElement("iframe");
		//
		menuItem.addEventListener("click",clickMenuItemF);
		openedMenuItem.setAttribute("class","openedMenuItem");
		openedMenuItem.innerHTML='<a class="hitArea">'+menuItem.innerHTML+'</a>'
                +'<a class="refresh"></a>'
                +'<a class="close"></a>'
		openedMenuItem.querySelector(".hitArea").addEventListener("click",clickOpenedMenuItemF);
		openedMenuItem.querySelector(".refresh").addEventListener("click",clickOpenedMenuItemRefreshF);
		openedMenuItem.querySelector(".close").addEventListener("click",clickOpenedMenuItemCloseF);
		openedMenuItemsArea.appendChild(openedMenuItem);
		//
		iframeItem.setAttribute("class","contentIframe");
		contentArea.appendChild(iframeItem);
		
		//
		
		function clickMenuItemF(e){
			if(menuItem.getAttribute("link")==null||menuItem.getAttribute("link")=="") return;
			openMenuItem();
			activeMenuItem();
		}
		
		function clickOpenedMenuItemF(e){
			activeMenuItem();
		}
		
		function clickOpenedMenuItemRefreshF(e){
			iframeItem.contentWindow.location.reload(true);
		}
		
		function clickOpenedMenuItemCloseF(e){
			openedMenuItem.removeAttribute("open");
			iframeItem.removeAttribute("open");
			openedMenuItem.removeAttribute("active");
			iframeItem.removeAttribute("active");
			iframeItem.src="";
			//
			var activedOpenedMenuItem=openedMenuItemsArea.querySelector(".openedMenuItem[active]");
			if(!activedOpenedMenuItem){
				openedMenuItem.setAttribute("toCloseMark","");
				var firstOpenedMenuItem=openedMenuItemsArea.querySelector(".openedMenuItem[toCloseMark] ~ .openedMenuItem[open]")
				openedMenuItem.removeAttribute("toCloseMark");	
				if(firstOpenedMenuItem){
					firstOpenedMenuItem.querySelector(".hitArea").click();
				}else{
					firstOpenedMenuItem=openedMenuItemsArea.querySelector(".openedMenuItem[open]");
					if(firstOpenedMenuItem){
						firstOpenedMenuItem.querySelector(".hitArea").click();
					}
				}
			}
			
		}
		
		//
		
		function openMenuItem(){
			if(openedMenuItem.getAttribute("open")!=""){
				openedMenuItem.setAttribute("open","");
				openedMenuItemsArea.insertBefore(openedMenuItem,openedMenuItemsArea.firstChild);
			}
			if(iframeItem.getAttribute("open")!=""){
				iframeItem.setAttribute("open","");
				iframeItem.src=menuItem.getAttribute("link");
			}
		}
		
		function activeMenuItem(){
			var activedOpenedMenuItem=openedMenuItemsArea.querySelector(".openedMenuItem[active]");
			if(activedOpenedMenuItem){
				activedOpenedMenuItem.removeAttribute("active");
			}
			var activedContentIframe=contentArea.querySelector(".contentIframe[active]");
			if(activedContentIframe){
				activedContentIframe.removeAttribute("active");
			}
			openedMenuItem.setAttribute("active","");
			iframeItem.setAttribute("active","");
		}
	}
	
	function processMenuWrapperRight(){
		document.getElementById("menuTreeAreaInnerWrapper").style.right ="-" + getScrollBarWidth() +"px";
		function getScrollBarWidth(){
			var div1=document.createElement("div");
			var div2=document.createElement("div");
			div1.appendChild(div2);
			div1.style.width=100+"px";
			div1.style.overflow="scroll";
			document.body.appendChild(div1);
			var scrollBarWidth=div1.offsetWidth - div2.offsetWidth;
			document.body.removeChild(div1);			
			return scrollBarWidth;
		}
	}

});