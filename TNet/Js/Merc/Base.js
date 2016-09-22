
//报装业务
function isSetup() {
    var r = /((\/Setup)[\/]*)|(tag=Setup)/gi;
    return r.test(window.location.href + "");
}
