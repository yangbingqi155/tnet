Pub.checkUser(true);

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
}

$(document.body).ready(init);

function selectAddr() {
    $("#OC").toggle();
    $("#Addr_Host").toggle();
    getAddrList();
}

//下订单
function submit() {

}
