function __init() {
    Pub.onCity(function (city) {
        getData(city);
    });
}
//获取
function getData(city) {
    var c = city ? city.code : "";
    Pub.get({
        url: "Service/Notice/List/" + c,
        loadingMsg: "加载中...",
        success: function (data) {
            if (Pub.wsCheck(data)) {
                if (data.Data) {
                    var html = "";
                    try {
                        for (var i = 0; i < data.Data.length; i++) {
                            var o = data.Data[i];
                            html += '<div class="weui_cell">';
                            html += '<div class="weui_cell_bd weui_cell_primary">';
                            html += '<a  href="' + Pub.url("Notice/Detail?id=" + o.idnotice) + '">';
                            html += '<p>' + o.title + '</p>';
                            html += '<span>' + getTime(o.publish_time) + '</span>';
                            html += '</div>';
                            html += '<div class="weui_cell_ft"></a>';
                            html += '</div>';
                            html += '</div>';
                        }
                        if (html) {
                            $('#notice').html(html);
                            return;
                        }
                    } catch (e) {
                        $('#order_host').html("加载异常");
                        return;
                    }

                }
            }
            load_fail("亲,暂无订单");
        },
        error: function (xhr, status, e) {
            load_fail("加载订单失败");
        }
    });

}


$(document).ready(__init);

function load_fail(msg) {
    Pub.noData("#order_host", msg, getData);
}