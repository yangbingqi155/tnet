﻿Pub.checkUser(true);

function init() {
    var order_cart = Pub.getCache("order_cart");
    if (order_cart && order_cart.Merc && order_cart.Spec) {
        $("#merc").attr("href", Pub.rootUrl() + "Merc/Detail/" + order_cart.Merc.idmerc);
        $("#merc_title").html(order_cart.Merc.merc1);
        $("#merc_spec").html(order_cart.Spec.spec1);
        $("#merc_price").html("￥" + order_cart.Spec.price);
        $("#merc_count").html("x" + order_cart.Count);
        $("#buy_att_month").html("送:" + order_cart.Spec.attmonth + " 月");
        $("#amount").html("￥ " + (order_cart.Spec.price * order_cart.Count));
        var imgs = order_cart.Merc.imgs;
        if (imgs) {
            imgs = imgs.split('|')[0];
            $("#ico").attr("src", Pub.rootUrl() + "Images/Merc/" + imgs);
        }
    }
    loadAddr();
}

$(document.body).ready(init);
function loadAddr() {
    var ao = Pub.getCache("Addr");
    if (ao) {
        var real_addr = ao.province + ao.city + ao.district + ao.street;
        var html = '<i class="iconfont">&#xe615</i>';
        html += '<div class="addrInfo">';
        html += '<div class="npHost">';
        html += '<span class="contacts">' + ao.contact + '</span>';
        html += '<span class="phones">' + ao.phone + '</span>';
        html += '</div>';
        html += '<div class="realAddr">' + real_addr + '</div>';
        html += '</div>';
        html += '<span class="choice"></span>';
        $("#addr").html(html);
    } else {
        var html = '<i class="iconfont">&#xe615</i>';
        html += '<span>请选择地址...</span>';
        html += '<span class="choice"></span>';
        $("#addr").html(html);
    }
}

function selectAddr() {
    $("#OC").toggle();
    showAdrBox();
}



//下订单
function submit() {
    var order_cart = Pub.getCache("order_cart");
    if (order_cart && order_cart.Merc && order_cart.Spec) {
        var u = Pub.getUser();
        if (u != null) {
            var contact = "";
            var addr = "";
            var phone = "";
            var ao = Pub.getCache("Addr");
            if (ao) {
                contact = ao.contact;
                addr = ao.province + ao.city + ao.district + ao.street;
                phone = ao.phone;
            }
            var img = order_cart.Merc.imgs;
            if (img) {
                img = img.split('|')[0];
            }
            if (!img) {
                img = "";
            }
            var data = {
                iduser: u.iduser,
                idmerc: order_cart.Merc.idmerc,
                merc: order_cart.Merc.merc1,
                idspec: order_cart.Spec.idspec,
                spec: order_cart.Spec.spec1,
                price: order_cart.Spec.price,
                count: order_cart.Count,
                month: order_cart.Spec.month,
                attmonth: order_cart.Spec.attmonth,
                contact: contact,
                addr: addr,
                phone: phone,
                notes: Pub.str($("#notes").val()),
                img: img
            };
            Pub.post({
                url: "Service/Order/Create",
                data: JSON.stringify(data),
                //noLoading: true,
                success: function (data) {
                    if (Pub.wsCheck(data)) {
                        if (data.Data) {
                            Pub.delCache("order_cart")
                            //alert("\n下单成功,订单号: " + data.Data.orderno);
                            window.location.href = Pub.rootUrl() + "Order/Pay/" + data.Data.orderno;
                            return;
                        }
                    }
                },
                error: function (xhr, status, e) {
                    alert("下单失败");
                }
            });
        }
    }
}