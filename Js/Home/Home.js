﻿var g_connkey, g_conname;
function initPage() {
    $(".header").hide();
    var tabsSwiper = new Swiper('.swiper-container', {
        autoplay: 3000,
        visibilityFullFit: true,
        loop: true,
        pagination: '.swiper-pagination',
        paginationClickable: true,
        preloadImages: false,
        lazyLoading: true
    });
}

$(document).ready(initPage);