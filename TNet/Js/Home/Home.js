var noticeList = null;
var noticeList_pos = 0, notice_tag = 0, notice_load = false;
function initPageParam() {
    var u = Pub.getUser();
    if (u && u.mu && (u.mu.recvSetup || u.mu.sendSetup)) {
        $("#Task").show().css("display", "block");
    }
    getNotice();
}


//获取公告
function getNotice() {
    if (!notice_load) {
        notice_load = true;
        noticeList = null;
        load_fail("加载...");
        Pub.get({
            url: "Service/Notice/List",
            noLoading:true,
            //loadingMsg: "加载中...",
            success: function (data) {
                notice_load = false;
                if (Pub.wsCheck(data)) {
                    if (data.Data) {
                        noticeList_pos = -1;
                        noticeList = data.Data;
                        setNotice();
                        return;
                    }
                }
                load_fail("亲,暂无通知");
            },
            error: function (xhr, status, e) {
                notice_load = false;
                load_fail("加载失败");
            }
        });
    }

}


function setNotice() {
    if (!notice_tag) {
        notice_tag = window.setInterval(setNotice, 1000 * 5);
    }
    noticeList_pos++;
    if (noticeList_pos >= noticeList.length) {
        noticeList_pos = 0;
    }
    var o = noticeList[noticeList_pos];
    $("#notice").html("<a href='" + Pub.url("Notice/Detail?id=" + o.idnotice) + "'>" + o.title + "&nbsp;&nbsp;&nbsp;&nbsp;" + getTime(o.publish_time) + "</a>");
}

function load_fail(msg) {
    $("#notice").html(msg);
    if (notice_tag) {
        window.clearInterval(notice_tag);
    }
    notice_tag = null;
}


$(document.body).ready(initPageParam);
