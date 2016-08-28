//轮播控件
function initPage() {
    var tabsSwiper = new Swiper('.swiper-container', {
        autoplay: 3000,
        visibilityFullFit: true,
        loop: true,
        pagination: '.swiper-pagination',
        paginationClickable: true,
        preloadImages: false,
        lazyLoading: true
    });
    getMercList();
}

function getMercList() {
    Pub.get({
        url: "Service/Merc/List",
        noLoading: true,
        success: function (data) {
            var html = "";
            if (Pub.wsCheck(data)) {
                if (data.Data) {
                    for (var i = 0; i < data.Data.length; i++) {
                        var lo = data.Data[i];
                        var ro = null;
                        if (i++ < data.Data.length) {
                            ro = data.Data[i];
                        }
                        
                        html += '<div class="pitem">';
                        html += crateItem(lo, 'l', ro);
                        html += crateItem(ro, 'r', null);
                        html += ' </div>';

                    }
                }

                if (html) {
                    $('#merc').html(html);
                }
            }
        },
        error: function (data) {

        }
    });
}

function crateItem(o, tag, o2) {
    var html = '';
    if (o) {
        html += '<div class="pitem_' + tag + '">';
        html += '<a href="#">';
        html += '<img src="'+Pub.rootUrl()+'Images/Merc/0.jpg" />';
        html += o.merc1 + '</a>';
        if (o2) {
            html += '<div class="HLive"></div>';
        }
        html += '</div>';
    }
    return html;
}
$(document).ready(initPage);



