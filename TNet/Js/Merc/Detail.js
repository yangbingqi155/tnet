
function getDetailData() {
    var idmerc = window.location.href + "";
    if (idmerc) {
        var ms = idmerc.split('/');
        if (ms[ms.length - 1] == '') {
            idmerc = ms[ms.length - 2];
        } else {
            idmerc = ms[ms.length - 1];
        }
        if (idmerc) {
            Pub.get({
                url: "Service/Merc/Detail/" + idmerc,
                //noLoading: true,
                success: function (data) {
                    var html = "";
                    if (Pub.wsCheck(data)) {
                        if (data.Data || data.Data.Merc) {
                            $(".merc_title").html(data.Data.Merc.merc1);
                            $(".sellpt").html(data.Data.Merc.sellpt);
                            $(".price").html("￥"+data.Data.Merc.baseprice);
                            $(".sellcount").html("销量：" + data.Data.Merc.sellcount);

                            
                            var imgs = data.Data.Merc.imgs;
                            if (imgs) {
                                imgs = imgs.split('|');
                                var imgHtml = "";
                                for (var i = 0; i < imgs.length; i++) {
                                    var ur = Pub.rootUrl() + "Images/Merc/" + imgs[i];
                                    var bur = Pub.rootUrl() + "Images/default_bg.png";
                                    imgHtml += "<div class='swiper-slide'><img src='" + bur + "' data-src='" + ur + "' class='swiper-lazy' /><div class='swiper-lazy-preloader swiper-lazy-preloader-white'></div></div>";
                                }
                                if (imgHtml) {
                                    $('.swiper-wrapper').html(imgHtml);
                                    initBase();
                                }
                            }
                            return;
                        }
                    }
                    load_fail("商品不存在 或 已下架");
                },
                error: function (data) {
                    load_fail("加载数据失败");
                }
            });
        }
    }

}

function load_fail(msg) {
    Pub.noData(".merc_title", msg, getDetailData);
    Pub.noData(".merc_title", msg, getDetailData);
    Pub.noData(".sellpt", msg, getDetailData);
    Pub.noData(".price", "￥0.0", getDetailData);
    Pub.noData(".price", "销量：0", getDetailData);
    
}

$(document).ready(getDetailData);