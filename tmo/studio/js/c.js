$(document).ready(function(){

	$('.loginbtn').hover(function(){
		$(this).addClass("loginbtnHover");
		},function(){
		$(this).removeClass("loginbtnHover");	
		});	
	$('.regbtn').hover(function(){
		$(this).addClass("regbtnHover");
		},function(){
		$(this).removeClass("regbtnHover");	
		});	
	
	//$('body').not($('#login-pannel')).click(function(){$('.choose-type ul').css('display','none')});
	
	$('.gotos').toggle(function(){
		$('.gotos ul').css('display','block');	
		},function(){
		$('.gotos ul').css('display','none');	
		});

	$('.gotos li').mouseover(function(){$(this).addClass('hovers');}).mouseout(function(){
		$(this).removeClass('hovers');
		});
	
	
	$('.gotos li').click(function(){
		$('.choose-content').text($(this).text());	
	});
});