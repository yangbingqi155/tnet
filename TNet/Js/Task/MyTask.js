

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
                            for (var i = 0; i < data.Data.length; i++) {
                                var o = data.Data[i];
                                var so = o.statusObj;
                                if (html) {
                                    html += '<div class="vline"></div>';
                                }
                                //var otype = (o.otype == 2) ? "<div class='setup_tag'></div> " : "";
                                html += '<div class="order_item">';
                                html += '<div class="order">';
                                html += '<div class="no">工单：' + o.orderno + "  (" + o.tasktype_t + ")" + '</div>';
                                html += '<div class="status">' + so.text + '</div>';
                                html += '</div>';
                                var da = Pub.rootUrl() + "/Order/Detail/" + o.orderno;
                                // html += '<a href="' + da + '"   class="task">';

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
                                html += '<div class="press">';
                                //html += '<div class="press_time">';
                                var ts = [
                                    { t: "受理", v: o.accpeptime },
                                    { t: "派工", v: o.revctime },
                                    { t: "完工", v: o.finishtime },
                                    { t: "回访", v: o.echotime }
                                ];
                                html += getPress(ts);
                                html += '</div>';

                                //html += '</a>';
                                html += '<div class="task_ops">';
                                html += '<div class="time_num">耗时:' + getTimeNum(o.workTime) + '</div>';
                                
                                html += '<div class="ops">' + getOps(so) + '</div>';
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

function getPress(ts) {
    //alert(ts.join(','));
    var html = "";
    html += '<div class="p_item_box"></div>';

    for (var i = 0; i < ts.length; i++) {
        var lcss = "", rcss = "", ptcss = "";
        if (ts[i].v) {
            lcss = " p_select";
            ptcss = 'p_t_select';
        }
        if ((i + 1) < ts.length && ts[i + 1].v) {
            rcss = " p_select";
            lcss = " p_select";

        }
        html += '<div class="p_item_box">';

        html += '<div class="p_item_time">';
        html += '<div class="p_start ' + lcss + '"></div>';
        if ((i + 1) < ts.length) {
            html += '<div class="p_line_l ' + lcss + '"></div>';
            html += '<div class="p_line_r ' + rcss + '"></div>';
        }
        html += '</div>';
        //if (i == 0) 

        html += '<div class="p_item_text ' + ptcss + '">' + ts[i].t + "<br/>" + getTime(ts[i].v) + '</div>';
        //}
        html += '</div>';
    }

    // alert(html);

    return html;

}



$(document).ready(getData);

function load_fail(msg) {
    Pub.noData("#order_host", msg, getData);
}

