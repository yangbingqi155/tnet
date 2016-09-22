
//获取订单
function getData() {
    var orderno = $("#OC").attr("orderno");
    var u = Pub.getUser();
    if (u != null && orderno) {

        $("#orderno").html("单号：" + orderno);
        Pub.get({
            url: "Service/Order/Detail/" + u.iduser + "/" + orderno,
            loadingMsg: "加载中...",
            success: function (data) {
                if (Pub.wsCheck(data)) {
                    if (data.Data) {
                        var html = "";
                        try {
                            var o = data.Data.Order;
                            $("#merc").attr("href", Pub.rootUrl() + "Merc/Detail/" + o.idmerc);
                            $("#contacts").html(o.contact);
                            $("#phones").html(o.phone);
                            $("#realAddr").html(o.addr);
                            var img = o.img;
                            var ur = Pub.rootUrl() + "Images/default_bg.png";
                            if (img) {
                                ur = Pub.rootUrl() + "Images/Merc/" + img;
                            }
                            $("#ico").attr("src", ur);
                            $("#merc_title").html(o.merc);
                            $("#merc_spec").html(o.spec);
                            $("#merc_price").html("￥" + o.price);
                            $("#merc_count").html("x" + o.count);
                            var so = data.Data.Status; 
                            $("#status").html(so.text);
                            $("#notes").val(o.notes);
                            $("#attmonth").html("送: " + o.attmonth + "  月");
                            $("#amount").html("￥" + (o.price * o.count));
                            $("#ops").html(getOps(so, o));
                            $("#merc_st_et").html(getTime(o.stime) + " 至 " + getTime(o.entime));
                            var phtml = "";
                            if (data.Data.Presses) {
                                for (var i = 0; i < data.Data.Presses.length; i++) {
                                    var po = data.Data.Presses[i];
                                    phtml += "<div class='p_item'><span class='p_time'>" + po.cretime + "</span>";
                                    phtml += "<span class='p_statust'>" + po.statust + "</span>";
                                    phtml += "<span class='p_oper'>" + po.oper + "</span></div>";
                                }
                            }
                            $("#Presses").html(phtml);

                        } catch (e) {
                            //$('#order_host').html("加载异常" + e.message);
                            return;
                        }

                    }
                }
                //   load_fail("亲,您暂无订单");
            },
            error: function (xhr, status, e) {
                //load_fail("加载订单失败");
            }
        });
    }
}

$(document).ready(getData);