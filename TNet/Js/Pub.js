(function () {
    Pub = {
        ajax: _ajax_call,
        call: _ajax_call,
        post: _ajax_post,
        get: _ajax_get,
        jsonDate: jsonDate,
        rootUrl: rootUrl,
        wsCheck: wsCheck,//服务检查
        showLoading: showLoading,// 
        showMsg: showMsg,// 
        showError: showError,// 
        hieLoading: hieLoading,
        noData: noData,//无数据
        checkUser: checkUser,
        isHome: isHome,
        auth: auth,
        setCache: setCache,
        getCache: getCache,
        delCache: delCache,
        setUser: function (v, e) {
            return setCache('tn_u', v, e);
        },
        getUser: function () {
            return getCache('tn_u');
        },
        delUser: function () {
            return delCache('tn_u');
        },
        str: getStr

    };
    var full_root_url = "http://app.i5shang.com/tnet/";
    var default_root_url = "";
    var showCount = 0;
    /*****进度条******/
    var msgTimeTag = 0, cur_toast_id = "";
    function showLoading(msg) {
        doShowMsg(msg, 'l', false);
    }
    function showMsg(msg) {
        doShowMsg(msg, 'f', true);
    }
    function showError(msg) {
        doShowMsg(msg, 'e', true);
    }

    function doShowMsg(msg, type, hid) {
        var ix = -1;
        if (type == 'e') {
            if (!msg) {
                msg = "失败了!";
            }
            ix = 1;
        } else if (type == 'l') {
            if (!msg) {
                msg = "处理中...";
            }
            ix = 2;
            showCount++;
        } else {
            ix = 0;
            if (!msg) {
                msg = "已完成";
            }
        }
        var tobj = "";
        var tids = ["#toast", "#toast_e", "#toast_l"]
        for (var i = 0; i < 3; i++) {
            if (i == ix) {
                if (i == 2) {
                    cur_toast_id = tids[i];
                    tobj = null;
                } else {
                    tobj = tids[i];
                }
                $(tids[i]).show();
                $(tids[i] + "_c").html(msg);
            } else {
                $(tids[i]).hide();
            }
        }
        if (hid) {
            clearMsgTime();
            msgTimeTag = window.setTimeout(function () {
                hieLoading(tobj + "");
            }, 1000 * 3);
        }
    }

    function hieLoading(tobj) {
        if (!tobj) {
            tobj = cur_toast_id;
        }
        if (tobj) {
            $(tobj).hide();
            if (tobj == cur_toast_id) {
                cur_toast_id = null;
            }
            showCount--;
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
    function _if_loading(msg) {
        if (showCount <= 0) {
            showLoading(msg);
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
        var isJson = false;
        if (request.headers == undefined) {
            request.headers = {
                accept: "application/json"
            };
        }
        if (request.headers && request.headers.accept == "application/json") {
            isJson = true;
        }
        if (request.contentType == undefined) {
            request.contentType = "application/json";
        }
        request.xhrFields = {
            withCredentials: true
        };
        request.crossDomain = true;
        request.cache = false;
        request.async = true;
        if (request.dataType == undefined) {
            request.dataType = "json";
        }
        request.timeout = 1000 * 60 * 3;
        var success = request.success;
        request.success = function (data) {
            hieLoading();
            if (success) {
                try{
                    data = jsonDate(data);
                } catch (e) {

                }
                
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
        var msg = "";
        var ld = false;
        try {
            ld = request.noLoading;
        } catch (e) {
        }
        try {
            msg = request.loadingMsg;
        } catch (e) {
        }
        if (!ld) {
            _if_loading(msg);
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

    //处理Json Date 格式问题
    // data: 如果为 Json 对象会把 /Date(1472173921327+0800)/ to yyyy-MM-dd HH:mm:ss
    // data: 如果为 string 会把 yyyy-MM-dd HH:mm:ss to {jdate:/Date(1472173921327+0800)/,date:Date对象}
    //如：
    //var d = Pub.jsonDate("2016-08-26 09:12:01");
    //d = new Date();
    //alert(d.jdate + "==" + d.date.toLocaleString())
    function jsonDate(data) {
        if (data) {
            if (typeof (data) === 'object') {
                for (var o in data) {
                    var v = data[o];
                    var dr = /(\/Date)[\(]([\d]+)[\+](0800)[)][\/]/gi;
                    if (typeof (v) === 'string') {
                        if (v && v.length > 18 && dr.test(v)) {
                            v = v.substr(6, v.length - 6 - 7) - 0;
                            v = new Date(v);
                            var y = v.getYear() + 1900;
                            var M = v.getMonth() + 1;
                            var d = v.getDate();
                            var h = v.getHours();
                            var m = v.getMinutes();
                            var s = v.getSeconds();
                            M = M <= 9 ? "0" + M : M;
                            d = d <= 9 ? "0" + d : d;
                            if (h >= 0) {
                                h = " " + ((h <= 9) ? " 0" + h : h);
                            }
                            if (m >= 0) {
                                m = ":" + ((m <= 9) ? "0" + m : m);
                            }
                            if (s >= 0) {
                                s = ":" + ((s <= 9) ? "0" + s : s);
                            }
                            v = y + "-" + M + "-" + d + h + m + s;
                            data[o] = v;
                        }
                    } else if (typeof (v) === 'object') {
                        data[o] = jsonDate(v);
                    }
                }
            } else if (typeof (data) === 'string') {
                data = data.replace(/[-]/g, '/');
                data = new Date(data);
                data = {
                    jdate: "/Date(" + data.getTime() + "+0800)/",
                    date: data
                };
            }
        }
        return data;
    }
    /*********检查服务结果*********/


    //检查服务
    function wsCheck(data, showMsg) {
        if (data) {
            if (data.Code == -1000) {
                //_login();
            } else if (data.Code == 1) {
                return true;
            } else {
                if (data.Msg) {
                    alert(data.Msg);
                }
            }
        }
        return false;
    }

    //无数据统一处理
    function noData(id, msg, fun) {
        var obj = $(id);
        if (obj) {
            if (!msg) {
                msg = "暂无数据";
            }
            obj.html("<span class='load_error'>" + msg + "</span>");
            obj.children(":first").click(fun);
        }
    }

    function checkUser(go) {
        var tn_u = Pub.getUser();
        if (!tn_u || tn_u == "") {
            auth(go);
            return false;
        }
        return true;
    }
    function isHome() {
        var h = $(document.body).attr("__Is_Home_Tag");
        if (h != undefined) {
            return true;
        }
        return false;
    }

    function auth(go) {
        var tn_u = Pub.getUser();
        var ru = rootUrl();
        var realu = "";
        //if (!isHome()) {
        realu = window.location.href + "";
        //}
        var u = "";
        var uurl = full_root_url + "user?ru=" + encodeURIComponent(realu);
        if (!tn_u) {
            uurl = encodeURIComponent(uurl);
            u = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc530ec3ce6a52233&redirect_uri=' + uurl + '&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect';
            if (go) {
                if (window.navigator.userAgent.indexOf("MicroMesseng") > 0) {
                    window.location.href = u;
                }
                // 
                return true;
            }
            //return false;
        } else {
            u = uurl;
        }
        $(".Top_User").attr("href", u);
        if (tn_u && tn_u.avatar) {
            var uo = $("#Top_User");
            uo.css("background-image", "url(" + tn_u.avatar + ")");
            uo.css("background-size", "1.5em");
        }
        return false;
    }

    //设置缓存
    function setCache(key, value, expires) {
        if (window.localStorage) {
            if (value) {
                if (!expires) {
                    expires = new Date();
                    expires.setDate(expires.getDate() + 1);
                    expires = expires.getTime();
                }
                var k = "tnet_app_" + key;
                window.localStorage[k] = JSON.stringify({ value: value, expires: expires });
                return true;
            } else {
                _clearCache(key);
            }
        } else {
            //alert("不支持-localStorage");
        }
        return false;
    }


    //获取缓存
    function getCache(key) {
        if (window.localStorage) {
            var k = "tnet_app_" + key;
            var v = window.localStorage[k];
            if (v) {
                v = JSON.parse(v);
                if (v && v.expires <= (new Date().getTime())) {
                    v = null;
                    // alert("清空了");
                }
                //window.localStorage.removeItem(k);
                if (v) {
                    return v.value;
                }
            }
        } else {
            // alert("不支持-localStorage");
        }
        return null;
    }


    //清空缓存
    function delCache(key) {
        if (window.localStorage && key) {
            var k = "tnet_app_" + key;
            window.localStorage.removeItem(k);
            return true;
        } else {
            // alert("不支持-localStorage");
        }
        return false;
    }
    //处理ios半输入状态字乱码问题
    function getStr(str) {
        if (str) {
            var s = String.fromCharCode(8198);
            var r = new RegExp("[" + s + "]", "gi");
            str = str.replace(r, "");
        }
        return str;
    }

})();


//错误
window.onerror = function (errorMessage, scriptURI, lineNumber, columnNumber, errorObj) {
    if (errorMessage) {
        alert(errorMessage + "," + scriptURI + ",lineNumber=" + lineNumber);
    }
    return false;
};