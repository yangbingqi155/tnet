
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
var g_base_x_v = 0;
function autoShowTopMenu() {
    if (g_base_x_v == 0) {
        g_base_x_v = $(document.body).scrollTop();
    }
    var cObj = $('#C');
    cObj.toggle();
    $('#Top_Main_Menu_Host').toggle();
    if (cObj.is(":hidden")) {
        $(document.body).removeClass("body_bg").addClass("body_bg");
    } else {
        $(document.body).removeClass("body_bg");
        window.setTimeout(function () {
            $(document).scrollTop(g_base_x_v);
            g_base_x_v = 0;
        }, 80);
    }
}
$(document).ready(initBase);


