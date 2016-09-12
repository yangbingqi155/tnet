var idAddr = 0;
var addr_data_cache = null;
var update_Addr_ing = false;
function getAddrList() {
    var u = Pub.getUser();
    if (u != null) {
        Pub.get({
            url: "Service/Addr/All/" + u.iduser,
            noLoading: true,
            success: function (data) {
                var html = "";
                if (Pub.wsCheck(data)) {
                    if (data.Data) {
                        var addrs = data.Data;
                        addr_data_cache = addrs;
                        var cao = Pub.getCache("Addr");
                        if (addrs) {
                            var html = "";
                            var di = 0;
                            for (var i = 0; i < addrs.length; i++) {
                                var ao = addrs[i];
                                if (cao && cao.idaddr === cao.idaddr) {
                                    di = i;
                                } else if (!cao && ao.isdv) {
                                    di = i;
                                }
                                var real_addr = ao.province + ao.city + ao.district + ao.street;
                                html += '<div class="addr_item">';
                                html += '<a href="javascript:void(0)" class="addr_item_c" onclick="doSetAddr(' + i + ')">';
                                html += '<i id="addr_i_k_' + i + '" class="iconfont">&#xe615</i>';
                                html += '<div class="addr_info"><div class="np_host">';
                                var tag = "";
                                if (ao.tag && ao.tag != "") {
                                    tag = " [" + ao.tag + "]";
                                }
                                html += '<span class="contact">' + ao.contact + tag + '</span>';
                                html += '<span class="phone">' + ao.phone + '</span>';
                                html += '</div>';
                                html += '<div class="real_addr">' + real_addr + '</div>';

                                html += '</div></a>';
                                html += '<a href="javascript:void(0)" class="addr_list_op" onclick="doEditAddr(' + i + ')" ><i class="iconfont">&#xe60f</i></a>';

                                html += '</div>';
                            }
                            if (html) {
                                $('.Addr_List').html(html);
                                // $('#addr_i_k_' + di).css("color", "red");
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
    try {
        $("#distpicker").distpicker('destroy');
    } catch (e) {

    }
    $("#distpicker").distpicker({
        province: "—— 省 ——",
        city: "—— 市 ——",
        district: "—— 区 ——"
    });
}


$(window).ready(initAddr);

function checkAddr() {
    if (!Pub.str($("#contact").val())) {
        alert("请输入姓名");
        $("#contact").focus();
        return false;
    }
    if (!Pub.str($("#phone").val())) {
        alert("请输入电话");
        $("#phone").focus();
        return false;
    }
    if (!Pub.str($("#street").val())) {
        alert("请输入街道");
        $("#street").focus();
        return false;
    }
    var province = Pub.str($("#province").val());
    if (!province || province == '—— 省 ——') {
        alert("请选择省");
        $("#province").focus();
        return false;
    }

    var city = Pub.str($("#city").val());
    if (!city || city == '—— 市 ——') {
        alert("请选择市");
        $("#city").focus();
        return false;
    }

    var street = Pub.str($("#street").val());
    if (!street) {
        alert("请输入街道");
        $("#street").focus();
        return false;
    }
    return true;
}

//保存地址
function saveAddr(del) {
    var u = Pub.getUser();
    if (u != null) {
        if (checkAddr()) {
            if (!update_Addr_ing) {
                var msg = "保存";
                if (del) {
                    msg = "删除";
                }
                update_Addr_ing = true;
                var city = $("#city").val();
                if (city == "—— 市 ——") {
                    city = "";
                }
                var district = $("#district").val();
                if (district == "—— 区 ——") {
                    district = "";
                }
                var isune = del ? false : true;
                var data = {
                    idaddr: idAddr,
                    iduser: u.iduser,
                    contact: Pub.str($("#contact").val()),
                    phone: Pub.str($("#phone").val()),
                    province: $("#province").val(),
                    city: city,
                    district: district,
                    street: Pub.str($("#street").val()),
                    tag: Pub.str($("#tag").val()),
                    notes: Pub.str($("#notes").val()),
                    isdv: $("#isdv").is(":checked"),
                    inuse: isune
                };
                Pub.post({
                    url: "Service/Addr/Update",
                    data: JSON.stringify(data),
                    //noLoading: true,
                    success: function (data) {
                        update_Addr_ing = false;
                        if (Pub.wsCheck(data)) {
                            if (data.Data) {
                                alert(msg + "成功");
                                if (del) {
                                    var da = Pub.getCache("Addr");
                                    if (da && da .idaddr == idAddr) {
                                        Pub.delCache("Addr");
                                        loadAddr();
                                    }
                                }
                                saveAddrFinish();
                                return;
                            }
                        }
                        // load_fail("商品不存在 或 已下架");
                    },
                    error: function (xhr, status, e) {
                        alert(msg + "失败");
                        update_Addr_ing = false;
                        // load_fail("加载数据失败");
                    }
                });
            }
        }
    }
}

function delAddr() {
    saveAddr(true);
}

function showAdrBox() {
    $("#Addr_Host").toggle();
    $("#delAddr").hide();
    setAddrOp();
    getAddrList();
    setTopMenuEvent(autoAddrBack, "Top_Menu_Back");
}

function hiddenAddrBox() {
    $("#OC").show();
    $("#Addr_Host").hide();
    $("#delAddr").hide();
    setTopMenuEvent();
}

function setAddrOp() {
    clearAddr();
    var addr_Op = $("#Addr_Op");
    if ($(".Addr_Edit").is(":hidden")) {
        addr_Op.html("新增");
    } else {
        addr_Op.html("保存");
    }

}

function autoAddrBack() {
    if ($(".Addr_Edit").is(":visible")) {
        $(".Addr_Edit").hide();
        $(".Addr_List").show();
        $("#delAddr").hide();
    } else if ($(".Addr_List").is(":visible")) {
        hiddenAddrBox();
    }
    setAddrOp();
    idAddr = 0;
}

//新增
function opAddr() {
    if ($(".Addr_Edit").is(":hidden")) {
        $(".Addr_Edit").show();
        $(".Addr_List").hide();
        setAddrOp();
    } else {
        saveAddr();
    }
}

function doSetAddr(pos) {
    var ad = addr_data_cache[pos];
    Pub.setCache("Addr", ad);
    hiddenAddrBox();
    loadAddr();
}


function doEditAddr(pos) {
    opAddr();
    $("#delAddr").show();
    var ad = addr_data_cache[pos];
    clearAddr();
    idAddr = ad.idaddr;
    $("#contact").val(ad.contact);
    $("#phone").val(ad.phone);
    $("#distpicker").distpicker('destroy');
    $("#street").val(ad.street);
    $("#tag").val(ad.tag);
    $("#notes").val(tag.notes);
    if (ad.isdv) {
        $("#isdv").attr("checked", true);
    } else {
        $("#isdv").removeAttr("checked");
    }
    var district = ad.district;
    if (!district) {
        district = "—— 区 ——";
    }
    $("#distpicker").distpicker({
        province: ad.province,
        city: ad.city,
        district: district
    });
}



function saveAddrFinish() {
    $(".Addr_Edit").hide();
    $("#delAddr").hide();
    $(".Addr_List").show();
    setAddrOp();
    getAddrList();
    clearAddr();
}

function clearAddr() {
    $("#contact").val("");
    $("#phone").val("");
    initAddr();
    $("#street").val("");
    $("#tag").val("");
    $("#notes").val("");
    $("#isdv").removeAttr("checked");

}