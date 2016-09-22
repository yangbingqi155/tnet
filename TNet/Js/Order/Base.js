Pub.checkUser(true);




//取消订单
function cancelOrder(orderno) {
    if (confirm("确定取消订单")) {
        doCancelOrder(orderno);
    }
}

function doCancelOrder(orderno) {
    var u = Pub.getUser();
    if (u != null) {
        Pub.get({
            url: "Service/Order/Cancel/" + u.iduser + "/" + orderno,

            loadingMsg: "取消中...",
            success: function (data) {
                if (Pub.wsCheck(data)) {
                    alert("取消订单成功");
                    getData();
                }
            },
            error: function (xhr, status, e) {
                alert("取消订单失败");
            }
        });
    }
}

function setStatus(s) {
    var sObj = {};
    if (s) {
        for (var so in s) {
            if (so) {
                sObj[s[so].Key] = s[so].Value;
            }
        }
    }
    return sObj;
}

function getStatus(s, status) {
    if (s) {

        //for (var i = 0; i < s.length; i++) {
        var so = s[status];
        if (so) {
            return so;
        }
        //}
    }
    return { code: "", text: "未知", ops: "" };
}

function getOps(status, order) {
    var html = "";
    if (status.ops) {
        var op = status.ops.split("|");
        for (var i = 0; i < op.length; i++) {
            var p = op[i];
            if (p == "cancel") {
                html += '<a class="cancel" href="javascript:void(0)" onclick="cancelOrder(\'' + order.orderno + '\')">取消</a>';
            } else if (p == "pay") {
                var pu = Pub.rootUrl() + "Order/Pay/" + order.orderno;
                html += '<a class="pay" href="' + pu + '">支付</a>';
            }
        }
    }
    return html;
}
