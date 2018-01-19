(function ($) {
	$.fn.imgrespond=function(){    
     	return this.each(function() {    
      		$this = $(this);  
      		var imgs=$(this).find("img");
          respondUI();
      		$(window).resize(function(){
   
              respondUI();
      		});

          function respondUI()
          {
          var bdw=$(window).width();

          imgs.each(function(){
            var $img=$(this);
            var imgw=$img.width();
            var imgh=$img.height();
            if(imgw>=bdw)
            {
              $img.removeAttr("height").removeAttr("width");

              $img.css({'width':"100%",'max-width':imgw,'max-height':imgh});
            }

          });

          }
  		});

	}
})(jQuery);