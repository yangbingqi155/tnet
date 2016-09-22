var mercData = null, selectSpec = null;
var sepcCount = 0;
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

                loadingMsg: "加载中...",
                success: function (data) {
                    var html = "";
                    if (Pub.wsCheck(data)) {
                        if (data.Data && data.Data.Merc && data.Data.Spec) {
                            mercData = data;
                            $(".merc_title").html(data.Data.Merc.merc1);
                            $(".sellpt").html(data.Data.Merc.sellpt);
                            $(".price").html("￥" + data.Data.Merc.baseprice);
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
                            var spec = data.Data.Spec;
                            if (spec) {
                                var specHtml = "";
                                var scss = "";
                                for (var i = 0; i < spec.length; i++) {
                                    var so = spec[i];
                                    if (!selectSpec) {
                                        selectSpec = so;
                                        scss = "select";
                                    } else {
                                        scss = "";
                                    }
                                    specHtml += "<p><a href='javascript:void(0)' id='spec_" + so.idspec + "' onclick='updateForSelectSpec(" + i + ")' class='" + scss + "'>" + so.spec1 + "</a></p>";
                                }
                                if (specHtml) {
                                    $('.spec').html(specHtml);
                                }
                            }

                            var shtml = "";
                            if (data.Data.Setups) {
                                for (var z = 0; z < data.Data.Setups.length; z++) {
                                    var s = data.Data.Setups[z];
                                    if (s.idtype == data.Data.Merc.idtype) {
                                        shtml += '<div class="setup">';
                                        shtml += '<div class="setup_title"><span>主题:</span>' + s.setup1 + '</div>';
                                        shtml += '<div class="setup_resource"><span>材料:</span>' + s.resource + '</div>';
                                        shtml += '<div class="setup_setuptype"><span>方式:</span>' + s.setuptype + '</div>';

                                        var sahtml = "";
                                        for (var h = 0; h < data.Data.SetupAddrs.length; h++) {
                                            var sa = data.Data.SetupAddrs[h];
                                            if (sa.idtype == data.Data.Merc.idtype && sa.idsetup == s.idsetup) {
                                                sahtml += '<div class="setup_addr"><div><span>电话:</span>' + sa.phone + "&nbsp;&nbsp;" + sa.service + '</div>';
                                                sahtml += '<div><span>受理:</span>' + sa.acceptime + '</div>';
                                                sahtml += '<div><span>安装:</span>' + sa.setuptime + '</div>';
                                                sahtml += '<div><span>地址:</span>' + sa.addr + '</div></div>';

                                            }
                                        }
                                        if (sahtml) {
                                            shtml += "<div class='setup_addr_title'>办理点:</div>" + sahtml;
                                        }
                                        shtml += '</div>';
                                    }
                                }
                                if (shtml) {
                                    $(".setup_host").html(shtml);
                                } else {
                                    $(".setup_host").html('<span class="load_error">暂无报装</span>');
                                }
                            }
                            updateForSelectSpec(-1);
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

function updateForSelectSpec(pos) {
    if (pos >= 0) {
        selectSpec = mercData.Data.Spec[pos];
    }
    if (selectSpec) {
        if (selectSpec.unit > 0) {
            $("#buy_num").show();
            sepcCount = 1;
        } else {
            $("#buy_num").hide();
            sepcCount = 1;
        }
        doUpdateMerc();
    }
}

function doUpdateMerc() {
    $(".spec p a").removeClass("select");
    $("#spec_" + selectSpec.idspec).addClass("select");
    $(".price").html("￥" + selectSpec.price);
    $(".sellcount").html("销量：" + selectSpec.sellcount);
    $(".buy_spec").html("已选择：" + selectSpec.spec1);
    updateAmount();
}
function updateAmount() {
    $(".amount").html("￥ " + (selectSpec.price * sepcCount));
    $("#nums").html(sepcCount);
}
//数量
function opSpecNum(op) {
    sepcCount += op;
    if (sepcCount <= 0) {
        sepcCount = 1;
    }
    updateAmount();
    return false;
}


function save() {
    if (!selectSpec || !mercData || !mercData.Data || !mercData.Data.Merc || !mercData.Data.Spec) {
        alert("亲！请选择一个宝贝");
        return;
    }
    if (Pub.setCache("order_cart", { Merc: mercData.Data.Merc, Spec: selectSpec, Count: sepcCount, Setup: isSetup() })) {
        window.location.href = Pub.rootUrl() + "Order/Submit";
        return;
    }
    alert("亲！保存订单失败");
}

function load_fail(msg) {
    Pub.noData(".merc_title", msg, getDetailData);
    Pub.noData(".merc_title", msg, getDetailData);
    Pub.noData(".sellpt", msg, getDetailData);
    Pub.noData(".price", "￥0.0", getDetailData);
    Pub.noData(".price", "销量：0", getDetailData);
    Pub.noData(".spec", msg, getDetailData);
    Pub.noData(".setup_host", msg, getDetailData);

}


function initParam() {
    getDetailData();
    if (isSetup()) {
        $(".go_buy").html("预约报装");
    }
}

$(document).ready(initParam);


