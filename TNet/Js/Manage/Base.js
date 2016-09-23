
function initParam() {
    var href = window.location.href + "";
    
    if ((/(Manage\/MercList)/gi).test(href) || (/(Manage\/MercEdit)/gi).test(href)) {
        $("#MercList").addClass("select");
    } else if ((/(Manage\/MercTypeList)/gi).test(href) || (/(Manage\/MercTypeEdit)/gi).test(href)) {
        $("#MercTypeList").addClass("select");
    } else if ((/(Manage\/OrderList)/gi).test(href)) {
        $("#OrderList").addClass("select");
    } else if ((/(Manage\/BusinessList)/gi).test(href) || (/(Manage\/BusinessEdit)/gi).test(href)) {
        $("#BusinessList").addClass("select");
    }

}

$(document.body).ready(initParam);