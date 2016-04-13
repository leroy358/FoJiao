window.addEventListener("load",function(e){
	processPopupWindowElement();
});

//为class包含popupWindow的a元素注册点击处理事件
function processPopupWindowElement(){
	var elements=document.querySelectorAll("a.popupWindow[href]");
	for(var i=0;i<elements.length;i++){
		elements[i].addEventListener("click",popupWindowElementClickF);
	}
}

function popupWindowElementClickF(e){
	window.open (e.currentTarget.getAttribute("href"),'','height=600,width=1200,left=50,top=20,toolbar=no,menubar=no,scrollbars=yes, location=no, status=no');
	e.preventDefault();
}