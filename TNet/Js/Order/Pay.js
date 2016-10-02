Pub.checkUser(true);
var pay_ing_tag = false;

//获取订单
function getData() {
   
    var orderno = $(".order_value").html();
    var u = Pub.getUser();
    if (u != null && orderno) {
        Pub.get({
            url: "Service/Order/DetailFoyPay/" + u.iduser + "/" + orderno,
            loadingMsg: "加载中...",
            success: function (data) {
                if (Pub.wsCheck(data)) {
                    if (data.Data) {
                        try {
                            $("#totalfee").html("￥" + data.Data.totalfee);

                        } catch (e) {
                            Pub.showError("订单有误");
                        }
                    }
                }

            },
            error: function (xhr, status, e) {
                Pub.showError("拉取订单失败");
            }
        });
    }
}

$(document.body).ready(getData);


function create() {
    if (!pay_ing_tag) {
        pay_ing_tag = true;
        var orderno = $(".order_value").html();
        var u = Pub.getUser();
        if (u != null && orderno) {
            var data = {
                iduser: u.iduser,
                orderno: orderno,
                paytype: "weixin"
            };
            Pub.post({
                url: "Service/Pay/Create",
                data: JSON.stringify(data),
                loadingMsg: "处理订单中...",
                success: function (data) {
                    pay_ing_tag = false;
                    if (Pub.wsCheck(data)) {
                        //alert(JSON.stringify(data))
                        if (data.Data) {
                            try {
                                data.Data = JSON.parse(data.Data.order);
                                goPay(data.Data);
                            } catch (e) {
                                Pub.showError("支付订单有误");
                            }
                        }
                    }

                },
                error: function (xhr, status, e) {
                    pay_ing_tag = false;
                    alert("生成支付错误")
                    Pub.showError("生成支付错误");
                }
            });
        } else {
            pay_ing_tag = false;
            Pub.showError("订单号有误");
        }
    }
}

function goPay(order) {

    // function onBridgeReady() {
    WeixinJSBridge.invoke(
        'getBrandWCPayRequest',
        order,
        function (res) {
           // alert(res.err_msg);
            if (res.err_msg == "get_brand_wcpay_request:ok") {
                finishPay();
            } else if (res.err_msg == "get_brand_wcpay_request:cancel") {
                Pub.showError("支付过程中用户取消");
            } else if (res.err_msg == "get_brand_wcpay_request:fail") {
                Pub.showError("支付失败");
            }
            // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。
        }
    );


}


function finishPay() {
    if (!pay_ing_tag) {
        pay_ing_tag = true;
        var orderno = $(".order_value").html();
        // var u = Pub.getUser();
        if (orderno) {
            Pub.get({
                url: "Order/WeixinPayNotice?orderno=" + orderno,
                dataType: "XML",
                contentType: "",
                loadingMsg: "更新订单中...",
                success: function (data) {
                    pay_ing_tag = false;
                    var ok = false;
                    try {
                        ok = $(data).find("return_code").text() == "SUCCESS";
                    } catch (e) {

                    }
                    if (ok) {
                        //alert("完成");
                        var da = Pub.rootUrl() + "/Order/Detail/" + orderno;
                        window.location.href = da;
                    } else {
                        alert("更新等待失败");
                    }

                },
                error: function (xhr, status, e) {

                    pay_ing_tag = false;
                    Pub.showError("更新订单错误" + status);
                }
            });
        } else {
            Pub.showError("订单号有误");
        }
    }
}
