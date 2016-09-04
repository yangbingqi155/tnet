var idAddr = 0;
var update_Addr_ing = false;
function getAddrList() {
    var u = Pub.getUser();
    if (u != null) {
        Pub.get({
            url: "Service/Addr/All/" + u.iduser,
            //noLoading: true,
            success: function (data) {

                var html = "";
                if (Pub.wsCheck(data)) {
                    if (data.Data) {
                        var addrs = data.Data;
                        if (addrs) {
                            var html = "";
                            var di = 0;
                            for (var i = 0; i < addrs.length; i++) {
                                var ao = addrs[i];
                                if (ao.isdv) {
                                    di = i;
                                }
                                var real_addr = ao.province + " " + ao.city + " " + ao.district + " " + ao.street;
                                html += '<div class="addr_item">';
                                html += '<a href="javascript:void(0)" onclick="doSetAddr()">';
                                html += '<i id="addr_i_k_'+i+'" class="iconfont">&#xe615</i>';
                                html += '<div class="addr_info"><div class="np_host">';
                                html += '<span class="contact">' + ao.contact + '</span>';
                                html += '<span class="phone">' + ao.phone + '</span>';
                                html += '</div>';
                                html += '<div class="real_addr">' + real_addr + '</div>';
                                
                                html += '</div>';
                                html += '<span class="choice"></span>';
                                html += '</a>';
                                html += '</div>';
                            }
                            if (html) {
                                $('.Addr_List').html(html);
                                $('#addr_i_k_' + di).css("color", "red");
                                return;
                            }
                        }
                       
                    }
                }
                load_fail("暂无地址");
            },
            error: function (data) {

                load_fail("加载地址失败");
            }
        });
    }
}
function load_fail(msg) {
    Pub.noData(".Addr_List", msg, getAddrList);
}

function initAddr() {
    $("#distpicker").distpicker({
        province: "—— 省 ——",
        city: "—— 市 ——",
        district: "—— 区 ——"
    });
}


$(window).ready(initAddr);

function checkAddr() {
    if (!$("#contact").val()) {
        alert("请输入姓名");
        $("#contact").focus();
        return false;
    }
    if (!$("#phone").val()) {
        alert("请输入电话");
        $("#phone").focus();
        return false;
    }
    if (!$("#street").val()) {
        alert("请输入街道");
        $("#street").focus();
        return false;
    }
    var province = $("#province").val();
    if (!province || province == '—— 省 ——') {
        alert("请选择省");
        $("#province").focus();
        return false;
    }

    var city = $("#city").val();
    if (!city || city == '—— 市 ——') {
        alert("请选择市");
        $("#city").focus();
        return false;
    }

    var district = $("#district").val();
    if (!district || district == '—— 区 ——') {
        alert("请选择区");
        $("#district").focus();
        return false;
    }
    var street = $("#street").val();
    if (!street) {
        alert("请输入街道");
        $("#street").focus();
        return false;
    }
    return true;
}

function saveAddr() {
    if (!update_Addr_ing) {
        update_Addr_ing = true;

        var u = Pub.getUser();
        if (u != null) {
            if (checkAddr()) {
                var data = {
                    idaddr: idAddr,
                    iduser: u.iduser,
                    contact: $("#contact").val(),
                    phone: $("#phone").val(),
                    province: $("#province").val(),
                    city: $("#city").val(),
                    district: $("#district").val(),
                    street: $("#street").val(),
                    tag: $("#tag").val(),
                    notes: $("#notes").val(),
                    isdv: $("#isdv").is(":checked"),
                    inuse: true
                };
                Pub.post({
                    url: "Service/Addr/Update",
                    data: JSON.stringify(data),
                    //noLoading: true,
                    success: function (data) {
                        update_Addr_ing = false;
                        var html = "";
                        if (Pub.wsCheck(data)) {
                            if (data.Data) {
                                alert("保存成功");
                                getAddrList();
                                return;
                            }
                        }
                        // load_fail("商品不存在 或 已下架");
                    },
                    error: function (xhr, status, e) {
                        alert("保存失败" + e);
                        update_Addr_ing = false;
                        // load_fail("加载数据失败");
                    }
                });
            }
        }
    }
}


function opAddr() {

    if ($(".Addr_Edit").is(":hidden")) {
        alert("222");
    } else {
        saveAddr();
    }
}


function doSetAddr() {
    $("#OC").toggle();
    $("#Addr_Host").toggle();
}