// JavaScript Document
$(document).ready(function () {
  //����
  $(".nav_haoyou").hover(function () {
    $(this).children("div").show();
		$(".nav_haoyou > a").css("background","url(/Content/images/nav_haoyou.png)");
		$(".nav_haoyou > a").css("width","120px");
		$(".nav_haoyou > a").css("height","39px")
  },function () {
     $(this).children("div").hide();
				$(".nav_haoyou > a").css("background","url(/Content/images/nav_haoyou_1.gif)");

  });
  //Ӧ��
  $(".nav_apply").hover(function () {
    $(this).children("div").show();
		$(".nav_apply > a").css("background","url(/Content/images/nav_yingyong.png)");
		$(".nav_apply > a").css("width","120px");
		$(".nav_apply > a").css("height","39px")
  },function () {
    $(this).children("div").hide();
				$(".nav_apply > a").css("background","url(/Content/images/nav_yingyong_1.gif)");

  });
	  //��Ϸ
  $(".nav_game").hover(function () {
    $(this).children("div").show();
		$(".nav_game > a").css("background","url(/Content/images/nav_game.png)");
		$(".nav_game > a").css("width","120px");
		$(".nav_game > a").css("height","39px")
  },function () {
    $(this).children("div").hide();
				$(".nav_game > a").css("background","url(/Content/images/nav_game_1.gif)");

  });


})