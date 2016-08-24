(function () {
    Pub = {
        showLoading: showLoading,//显示进度条
        showMsg: showMsg,//显示进度条
        showError: showError,//显示进度条
        hieLoading: hieLoading

    };
    var msgTimeTag = 0;
    function showLoading(msg) {
        doShowMsg(msg, 'l', true);
    }
    function showMsg(msg) {
        doShowMsg(msg, 'f', false);
    }
    function showError(msg, noHid) {
        doShowMsg(msg, 'e', noHid);
    }

    function doShowMsg(msg, type, noHid) {
        var msgHost = document.getElementById("RAMsgObj");
        msgHost.style.display = "block";
        msgHost = document.getElementById("RAMsgObj_Context");

        if (type == 'e') {
            type = "load_error";
        } else if (type == 'l') {
            type = "load_ing";
        } else {
            type = "load_ok";
        }
        msgHost.innerHTML = '<span class="' + type + '">' + msg + '</span>';
        clearMsgTime();
        if (!noHid) {
            msgTimeTag = window.setTimeout(hieLoading, 1000 * 8);
        }
    }



    function hieLoading() {
        var msgHost = document.getElementById("RAMsgObj");
        msgHost.style.display = "none";
        clearMsgTime();
    }
    function clearMsgTime() {
        if (msgTimeTag) {
            window.clearTimeout(msgTimeTag);
            msgTimeTag = 0;
        }
    }

})();