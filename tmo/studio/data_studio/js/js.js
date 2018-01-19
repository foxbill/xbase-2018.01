// JavaScript Document
//以下菜单tag滑动门效果的函数
function selectTag(showContent,selfObj){
	// 操作标签
	var tag = document.getElementById("tags").getElementsByTagName("li");
	var taglength = tag.length;
	for(i=0; i<taglength; i++){
		tag[i].className = "";
	}
	selfObj.parentNode.className = "selectTag";
	// 操作内容
	for(i=0; j=document.getElementById("tagContent"+i); i++){
		j.style.display = "none";
	}
	document.getElementById(showContent).style.display = "block";
	
	
}
//以下鼠标滑过表格行变色函数
function senfe(o,a,b,c,d){
 var t=document.getElementById(o).getElementsByTagName("tr");
 var len=t.length;
 for(var i=0;i<t.length;i++){
  if(i==0 || i==len){
	continue;
  }
  t[i].style.backgroundColor=(t[i].sectionRowIndex%2==0)?a:b;
  t[i].onclick=function(){
   if(this.x!="1"){
    this.x="1";//本来打算直接用背景色判断，FF获取到的背景是RGB值，不好判断
    this.style.backgroundColor=d;
   }else{
    this.x="0";
    this.style.backgroundColor=(this.sectionRowIndex%2==0)?a:b;
   }
  }
  t[i].onmouseover=function(){
   if(this.x!="1")this.style.backgroundColor=c;
  }
  t[i].onmouseout=function(){
   if(this.x!="1")this.style.backgroundColor=(this.sectionRowIndex%2==0)?a:b;
  }
 }
}