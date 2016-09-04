(function () {
    Pub = {
        ajax: _ajax_call,
        call: _ajax_call,
        post: _ajax_post,
        get: _ajax_get,
        jsonDate: jsonDate,
        rootUrl: rootUrl,
        wsCheck: wsCheck,//服务检查
        showLoading: showLoading,//显示进度条
        showMsg: showMsg,//显示进度条
        showError: showError,//显示进度条
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
        }

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
                data = jsonDate(data);
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
                            if (h > 0) {
                                h = " " + (h <= 9 ? "0" + h : h);
                            }
                            if (m > 0) {
                                m = ":" + (m <= 9 ? "0" + m : m);
                            }
                            if (s > 0) {
                                s = ":" + (s <= 9 ? "0" + s : s);
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
        var uurl = "http://app.i5shang.com/tnet/user?ru=" + encodeURIComponent(realu);
        if (!tn_u) {
            uurl = encodeURIComponent(uurl);
            u = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc530ec3ce6a52233&redirect_uri=' + uurl + '&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect';
            if (go) {
               // window.location.href = u;
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


})();