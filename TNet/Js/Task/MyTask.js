Pub.checkUser(true);

//获取订单
function getData() {
    var u = Pub.getUser();

    if (u != null && u.mu) {

        Pub.get({
            url: "Service/Task/List/" + u.mu.code,
            loadingMsg: "加载中...",
            success: function (data) {

                if (Pub.wsCheck(data)) {
                    if (data.Data) {
                        var html = "";
                        try {
                            //data.Data.Status = setStatus(data.Data.Status);
                            for (var i = 0; i < data.Data.length; i++) {
                                var o = data.Data[i];
                                var so = o.statusObj;//getStatus(data.Data.Status, o.status);
                                if (html) {
                                    html += '<div class="vline"></div>';
                                }
                                //var otype = (o.otype == 2) ? "<div class='setup_tag'></div> " : "";
                                html += '<div class="order_item">';
                                html += '<div class="order">';
                                html += '<div class="no">工单：' + o.orderno + '</div>';
                                html += '<div class="status">' + so.text + '</div>';
                                html += '</div>';
                                var da = Pub.rootUrl() + "/Order/Detail/" + o.orderno;
                                html += '<a href="' + da + '"   class="task">';

                                html += '<div class="titem_host">';
                                html += '<div class="ttitle">任务:</div>';
                                html += '<div class="tvalue">' + o.title + '</div>';
                                html += '</div>';

                                html += '<div class="titem_host">';
                                html += '<div class="ttitle">客户:</div>';
                                html += '<div class="tvalue">' + o.contact + '</div>';
                                html += '</div>';


                                html += '<div class="titem_host">';
                                html += '<div class="ttitle">电话:</div>';
                                html += '<div class="tvalue">' + o.phone + '</div>';
                                html += '</div>';


                                html += '<div class="titem_host">';
                                html += '<div class="ttitle">地址:</div>';
                                html += '<div class="tvalue">' + o.addr + '</div>';
                                html += '</div>';
                                

                               

                                html += '</a>';
                                html += '<div class="task_ops">';
                                // html += '<div class="amont">￥' + o.totalfee + '</div>';
                                // html += '<div class="ops">' + getOps(so, o) + '</div>';
                                html += '</div>';
                                html += '</div>';
                            }
                            if (html) {
                                $('#order_host').html(html);
                                return;
                            }
                        } catch (e) {
                            $('#order_host').html("加载异常" + e.message);
                            return;

                        }

                    }
                }
                load_fail("亲,您暂无工单");
            },
            error: function (xhr, status, e) {
                load_fail("加载工单失败");
            }
        });
    }
}



$(document).ready(getData);

function load_fail(msg) {
    Pub.noData("#order_host", msg, getData);
}