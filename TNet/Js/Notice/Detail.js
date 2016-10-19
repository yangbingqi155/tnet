
//获取公告
function getNotice() {
    var id = Pub.urlParam("id");
    Pub.get({
        url: "Service/Notice/Detail/" + id,
        loadingMsg: "加载中...",
        success: function (data) {

            if (Pub.wsCheck(data)) {
                if (data.Data) {
                    setNotice("Title", data.Data.title);
                    setNotice("Time", data.Data.publish_time);
                    setNotice("Content", data.Data.content);
                    return;
                }
            }
            load_fail("亲,暂无通知");
        },
        error: function (xhr, status, e) {
            load_fail("加载失败");
        }
    });


}

function setNotice(id, msg) {
    if (!msg) {
        msg = "";
    }
    $("." + id).html(msg);

}

function load_fail(msg) {
    $(".Title").html(msg);
    $(".Time").html(msg);
    $(".Time").html(msg);
}
$(document.body).ready(getNotice);