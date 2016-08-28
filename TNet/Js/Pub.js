(function () {
    Pub = {
        ajax: _ajax_call, //增加统一请求服务-lxw
        call: _ajax_call,
        post: _ajax_post,
        get: _ajax_get,
        rootUrl: rootUrl,
        wsCheck: wsCheck,//服务检查
        showLoading: showLoading,//显示进度条
        showMsg: showMsg,//显示进度条
        showError: showError,//显示进度条
        hieLoading: hieLoading

    };
    var default_root_url = "";
    var showCount = 0;
    /*****进度条******/
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
        if (msgHost) {
            showCount++;
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
    }

    function hieLoading() {

        var msgHost = document.getElementById("RAMsgObj");
        if (msgHost) {
            showCount--;
            msgHost.style.display = "none";
            clearMsgTime();
        }

    }
    function clearMsgTime() {
        if (msgTimeTag) {
            window.clearTimeout(msgTimeTag);
            msgTimeTag = 0;
        }
    }

    //加载动画
    function _if_loading(str) {
        if (showCount <= 0) {
            showLoading(str);
        }
    }
    /***************获取跟路径******************/

    //获取服务基地址


    //获取服务基地址
    function rootUrl() {
        var root_url = $(document.body).attr("root");
        if (!root_url) {
            root_url = default_root_url;
        }
        root_url = decodeURIComponent(root_url);
        if ((root_url.charAt(root_url.length - 1) != '/')) {
            root_url += "/";
        }
        return root_url;
    }

    /**********Ajax请求***************/

    //ajax请求-跨域解决
    function _ajax_call(request) {
        request.url = rootUrl() + request.url;
        request.headers = {
            accept: "application/json"
        };
        request.contentType = "application/json";
        request.xhrFields = {
            withCredentials: true
        };
        request.crossDomain = true;
        request.cache = false;
        request.async = true;
        if (!request.dataType) {
            request.dataType = "json";
        }
        request.timeout = 1000 * 60 * 3;
        var success = request.success;
        request.success = function (data) {
            hieLoading();
            if (success) {
                success(data);
            }
        };
        var error = request.error;
        request.error = function (xhr, status, e) {
            hieLoading();
            if (error) {
                error(xhr, status, e);
            }
        };
        var ld = false;
        try {
            ld = request.noLoading;
        } catch (e) {

        }
        if (!ld) {
            _if_loading();
        }
        $.ajax(request);
    }

    //ajax请求-POST-跨域解决
    function _ajax_post(request) {
        request.type = "POST";
        _ajax_call(request);
    }

    //ajax请求-POST-跨域解决
    function _ajax_get(request) {
        request.type = "GET";
        _ajax_call(request);
    }

    /*********检查服务结果*********/


    //检查服务
    function wsCheck(data, showMsg) {
        if (data) {
            if (data.Code == -1000) {
                //_login();
            } else if (data.Code == 1) {
                return true;
            }
        }
        return false;
    }
})();