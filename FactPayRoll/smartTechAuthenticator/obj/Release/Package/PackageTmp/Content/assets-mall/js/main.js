$(document).ready(function () {
  
  $("header .cart .cart-wrap").click(function () {
    $(this).next().slideToggle(200);
  });


  $("body,.mobile-trigger").click(function(){
    $("header .product-wrapper").slideUp(200);
  });


  $("header .cart .cart-wrap,header .product-wrapper").click(function(e){
    e.stopPropagation();
  });
  

  $(".common .menu>ul>li>a>i.fas.fa-angle-down,.common .menu>ul>li>ul>li>a i").click(function () {
    $(this).parent().parent().siblings("li.hs-item-has-children").children("ul.menu-chilren-wrapper").slideUp(250);
    $(this).parent().parent().children("ul.menu-chilren-wrapper").slideToggle(250);
  });


  $(".childtrigger").click(function () {
    $(this).next().slideToggle(200);
  });


  $('.mobile-trigger').click(function () {
    $('body').toggleClass('mobile-open');
  });


  $('body,.primary-menu a.close').click(function () {
    $('body').removeClass('mobile-open');
  });

  $('.mobile-trigger,.primary-menu').click(function (e) {
    e.stopPropagation();
  });


  $(".shop .nice-select").click(function () {
    $(this).toggleClass("open");
  });

  $("body").click(function () {
    $(".shop .nice-select").removeClass("open");
  });

  $(".nice-select .list,.shop .nice-select").click(function (e) {
    e.stopPropagation();
  });


  $(".shop .nice-select.sortBy").click(function () {
    $(".shop .nice-select.show").removeClass("open");
  });

  $(".shop .nice-select.show").click(function () {
    $(".shop .nice-select.sortBy").removeClass("open");
  });

  // List Open On Product Page
  $(".shop .view-mode li a.list").click(function(){
      $("section.product-area").addClass("list-open");
      $(this).addClass("active");
      $(this).parent().siblings().find("a").removeClass("active");
  });

  $(".shop .view-mode li a.grid").click(function(){
      $("section.product-area").removeClass("list-open");
      $(this).addClass("active");
      $(this).parent().siblings().find("a").removeClass("active");
  });


      $(".common .title").click(function(){
        $(this).next().parent().toggleClass("active");
      });

      $("body").click(function(){
          $(".categoreis.common").removeClass("active");
      });

      $(".header-bottom .common").click(function(e){
          e.stopPropagation();
      });
   

  // Current Page Active

  var value1 = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
  $('.primary-menu ul li a').each(function () {
    var url = $(this).attr('href');
    var lastSegment = url.split('/').pop();
    if (lastSegment == value1) {
      $(this).parent().addClass('active');
      $(this).parent().siblings().removeClass('active');
    }
  });



  // Increment And Decrement


    $('#adds').click(function add() {
        var $rooms = $("#quantity");
        var a = $rooms.val();
        var price = parseInt($("#hddPrice").val());
        a++;
        price = price * a;
        $("#subs").prop("disabled", !a);
        $rooms.val(a);
        $(".discount").text("RM" + price + ".00");
    });
$("#subs").prop("disabled", !$("#quantity").val());

$('#subs').click(function subst() {
    var $rooms = $("#quantity");
    var ActualPrice = parseInt($("#hddPrice").val());
    var b = $rooms.val();
    if (b > 1) {
        b--;
       // var price = parseInt($(".discount").text());
        price = ActualPrice*b;
        $rooms.val(b);
        $(".discount").text("RM" + price + ".00");
    }
    else {
        $("#subs").prop("disabled", true);
    }
});


  //back to top

    $('footer').after('<a id="back-to-top"><i class="fas fa-angle-double-up"></i></a>');

    $("#back-to-top").hide();

    $(window).scroll(function(){
    if ($(window).scrollTop()>100){
        $("#back-to-top").fadeIn(500);
    }
    else
    {
        $("#back-to-top").fadeOut(500);
    }
    });
    
    $("#back-to-top").click(function(){
        $('body,html').animate({scrollTop:0},500);
        return false;
    });

});


