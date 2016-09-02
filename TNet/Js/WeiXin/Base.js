
function initBase() {
    var tabsSwiper = new Swiper('.swiper-container', {
        autoplay: 3000,
        visibilityFullFit: true,
        loop: true,
        pagination: '.swiper-pagination',
        paginationClickable: true,
        preloadImages: false,
        lazyLoading: true
    });
    var u = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc530ec3ce6a52233&redirect_uri=http://app.i5shang.com/tnet/user&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect';
    $(".Top_User").attr("href", u);
}
function autoShowTopMenu() {
    $('#Top_Main_Menu_Host').toggle();
    var cObj = $('#C');
    cObj.toggle();
    if (cObj.is(":hidden")){
        $(document.body).removeClass("body_bg").addClass("body_bg");
    } else {
        $(document.body).removeClass("body_bg");
    }
    
     
}
$(document).ready(initBase);


