//新添加的treeWrapper需要用regTreeWrapper函数注册事件。

function regTreeWrapper(treeWrapper){
	var treePackItems = document.querySelectorAll(".treePackItem");
	for(var i=0;i<treePackItems.length;i++){
		treePackItems[i].addEventListener("click",switchTreePackItemStateF);
	}
	
	function switchTreePackItemStateF(e){
		if(e.currentTarget.getAttribute("open")==""){
			e.currentTarget.removeAttribute("open");
		}else{
			e.currentTarget.setAttribute("open","");
		}
	}
}

window.addEventListener("load",function(e){
	var treeWrappers=document.querySelectorAll(".treeWrapper");
	for(var i=0; i < treeWrappers.length;i++){
		regTreeWrapper(treeWrappers[i]);
	}
});