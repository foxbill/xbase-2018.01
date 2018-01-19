var user;
$(function  () {

	user=$.rCall("LoginUser");
	//喜欢
	$(".my-like").click(function(){
		var obj=$(this);
		obj.toggleClass("collect");
		obj.addClass("animated tada");
        prodOperaType(2);
		setTimeout(function(){
			obj.removeClass("animated tada");
		},1000);

	});
		//加入购物车
		/*
		$(".addshopcart").click(function(){
			
			 alert("ddd");

			AddShoppingCart();


		});
*/

/*清空购物车*/
$(".clearCart").click(function(){
	$(".shopping-list li.shopping-item").remove();

	$(".product_add").html(0);
	$(".total-price").html(0);

	$(".product_add").removeClass("opacity");

	$(".bg-mask").click();
    
    //清空操作
	$.rCall("dspt_shoppingCart.delete");
});

/*点击购物车*/
$(".shppping-cart").click(function(){	
	var v=$(".product_add").html();	
	if(v!="0")
	{
		$(".bg-mask").click();	
	}else
	{
		alert("购物车为空，请先添加商品");
	}

});
/*背景遮罩*/
$(".bg-mask").click(function(){
	$(this).toggleClass("bggray");
	var obj=$(".shopping-cart-content");
	obj.toggleClass("showcart");
});
/*添加商品*/
$(".add-prod").click(function(){

	var v=parseInt($(this).prev().val());
	v++;
	$(this).prev().val(v);

	countNum();

	//updateShoppingCart();

});
/*减少商品*/
$(".minus-prod").click(function(){
	var v=parseInt($(this).next().val());
	v--;
	if(v==0)
	{
		$(this).parents("li.shopping-item").remove();
	}else
	{
		$(this).next().val(v);
	}

	countNum();


	//updateShoppingCart();
});

$(".prod-num").change(function  () {
	var p=$(this).parents(".shopping-item");
	var pid=p.find(".data-prod-id");
	var pnum=$(this).val();
	var uid=user.Id;
})

})
   //计算商品总数以及总额
   function countNum()
   {
   	var sum=0;
   	var total=0;
   	$(".shopping-list li.shopping-item").each(function(){

   		var num=$(this).find(".prod-num");
   		var price=$(this).find(".prod-price");
   		var v=parseInt(num.val());
   		sum+=v;
   		total+=v*parseInt(price.html());
   	});
   	if(sum==0)
   	{
   		$(".product_add").removeClass("opacity");

   		$(".bg-mask").click();
   	}else
   	{
   		$(".product_add").addClass("opacity");
   	}

   	$(".product_add").html(sum);
   	$(".total-price").html(total);

   	return {"sum":sum,"total":total};
   }
	//初始化购物车
	function initShoppingCart(){
		countNum();
	}

	function AddShoppingCart()
	{
		var isAdd=true;
		$(".addshopcart").addClass("disabled");

			//var val=parseInt($(".product_add").html());
			var num=parseInt($(".product-num").val());

			//$(".product_add").html(val);

			$(".product_add").addClass("opacity animated tada");

			var pid=$("#productId").val();
			var price=$("#product-price").html();
			var pname=$("#product-title").html();

			//判断是否已存在该商品
			$(".shopping-list li.shopping-item").each(function(){
				var prodid=$(this).find(".data-prod-id").val();
				if(pid==prodid)
				{
					var pnum=$(this).find(".prod-num");
					var vl=parseInt(pnum.val());
					vl+=num;
					pnum.val(vl);
					isAdd=false;
					return false;
				}
			});

			 //新增到购物车
			 if(isAdd)
			 {
			 	var obj=$(".list-group-item.hidden").clone(true);

			 	obj.find(".data-prod-id").val(pid);
			 	obj.find(".prod-num").val(num);
			 	obj.find(".prod-title").html(pname);
			 	obj.find(".prod-price").html(price);

			 	obj.addClass("shopping-item").removeClass("hidden");
			 	$(".shopping-list").append(obj);
			 }


			 countNum();

            // updateShoppingCart();

			 setTimeout(function(){
			 	$(".addshopcart").removeClass("disabled");
			 	$(".product_add").removeClass("animated tada");
			 },1000);
			}
	//收藏
	function prodOperaType(type){
		var obj=$(".prodCollectId");
		var prodCollectId=obj.val();
		if(obj==undefined||obj.length==0||prodCollectId=="0")
		{
			prodCollectId=$.rCall("GUID.NewGUID");
			var pid=$("#productId").val();
			var operator=user.Id;
			var operadate=getNowDate();

			try {
				var res=$.rCall("ds.pt_productOperate.insert",{ row: { ID: prodCollectId, ProductId: pid, OperateId: type, Creater: operator, CreateTime: operadate,Updater: operator, UpdateTime: operadate} });
				$(".prodCollectId").val(operaGuid);
			}
			catch(e){

			}
			if (res && res.Err)
				alert(res.Err.Text);
		}else
		{
			var reult = $.rCall("pt_productOperate.delete", {row:{pk_ID: prodCollectId} });
		}

	}

	function updateShoppingCart () {
		$.rCall("dspt_shoppingCart.delete");

		     var rows=[];
		     var operator=user.Id;
			var operadate=getNowDate();
			$(".shopping-list li.shopping-item").each(function(){
				
					var pnum=$(this).find(".prod-num").val();
					var pid=$(this).find(".data-prod-id").val();

					var row={ ID: '1',SessionId:'0',ProductId: pid,Total:'0', sCount: pnum,DelFlag:0, Creater: operator, CreateTime: operadate,Updater: operator, UpdateTime: operadate};
					rows.push(row);
			});

			//alert(rows);

			 var rest = $.rCall("pt_shoppingCart.update", { insertRows: rows, updateRows: [], deleteRows: [] });
	}
