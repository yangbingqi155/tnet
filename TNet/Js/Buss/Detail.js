//获取周边商圈详情
function getData() {
    var idbuss = $("#OC").attr("idbuss");
    if (idbuss) {
        Pub.get({
            url: "Service/Buss/Detail/" + idbuss,
            // noLoading: true,
            success: function (data) {
                if (Pub.wsCheck(data)) {
                    if (data.Data) {
                        try {
                            $(".buss_title").html(data.Data.buss);
                            $(".sellpt").html(data.Data.sellpt);
                            $(".price").html("￥" + data.Data.price);
                            $("#buss_addr").html("<i class='iconfont'>&#xe615</i>" + data.Data.addr);
                            $("#buss_st_et").html(data.Data.runtime);
                            $("#busstime").html(getTimeYYMMHH(data.Data.busstime));
                            $("#notes").html(data.Data.notes);
                            $("#contact").html(data.Data.contact);
                            $("#phone").html(data.Data.phone);

                            $("#call").attr("href","tel:"+data.Data.phone);
                            

                            var imgs = data.Data.imgs;
                            if (imgs) {
                                imgs = imgs.split('|');
                                var imgHtml = "";
                                for (var i = 0; i < imgs.length; i++) {
                                    var ur = Pub.rootUrl() + "Images/Buss/" + imgs[i];
                                    var bur = Pub.rootUrl() + "Images/default_bg.png";
                                    imgHtml += "<div class='swiper-slide'><img src='" + bur + "' data-src='" + ur + "' class='swiper-lazy' /><div class='swiper-lazy-preloader swiper-lazy-preloader-white'></div></div>";
                                }
                                if (imgHtml) {
                                    $('.swiper-wrapper').html(imgHtml);
                                    initBase();
                                }
                            }
                        } catch (e) {
                            //$('#buss_host').html("加载异常" + e.message);
                            return;
                        }

                    }
                }
                load_fail("亲,暂无商家详情");
            },
            error: function (xhr, status, e) {
                alert("加载商家详情失败");
            }
        });
    }
}

$(document).ready(getData);

function load_fail(msg) {
    //Pub.noData("#buss_host", msg, getData);
}