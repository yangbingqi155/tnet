Pub.checkUser(true);


function init() {
    var order_cart = Pub.getCache("order_cart");
    if (order_cart && order_cart.Merc && order_cart.Spec) {
        $("#merc").attr("href", Pub.rootUrl() + "Merc/Detail/" + order_cart.Merc.idmerc);
        $("#merc_title").html(order_cart.Merc.merc1);
        $("#merc_spec").html(order_cart.Spec.spec1);
        $("#merc_price").html("￥" + order_cart.Spec.price);
        $("#merc_count").html("x" + order_cart.Count);
        $("#buy_att_month").html("送:" + order_cart.Spec.attmonth + " 月");
        $("#amount").html("￥ " + (order_cart.Spec.price * order_cart.Count));
        var ur = Pub.rootUrl() + "Images/default_bg.png";
        var imgs = order_cart.Merc.imgs;
        if (imgs) {
            imgs = imgs.split('|')[0];
            ur = Pub.rootUrl() + "Images/Merc/" + imgs;

        }
        $("#ico").attr("src", ur);
        loadAddr();
        if (order_cart.Setup) {
            $(".go_buy").html("提交");
            $(".resource").show();
        }
    } else {
        toHome();
    }

}

$(document.body).ready(init);


function loadAddr() {
    var ao = Pub.getCache("Addr");
    if (ao) {
        var real_addr = ao.province + ao.city + ao.district + ao.street;
        var html = '<i class="iconfont">&#xe615</i>';
        html += '<div class="addrInfo">';
        html += '<div class="npHost">';
        html += '<span class="contacts">' + ao.contact + '</span>';
        html += '<span class="phones">' + ao.phone + '</span>';
        html += '</div>';
        html += '<div class="realAddr">' + real_addr + '</div>';
        html += '</div>';
        html += '<span class="choice"></span>';
        $("#addr").html(html);
    } else {
        var html = '<i class="iconfont">&#xe615</i>';
        html += '<span>请选择地址...</span>';
        html += '<span class="choice"></span>';
        $("#addr").html(html);
    }
}

function selectAddr() {
    $("#OC").toggle();
    showAdrBox();
}



//下订单
function submit() {
    var order_cart = Pub.getCache("order_cart");
    if (order_cart && order_cart.Merc && order_cart.Spec) {
        var u = Pub.getUser();
        if (u != null) {
            var contact = "";
            var addr = "";
            var phone = "";
            var ao = Pub.getCache("Addr");
            if (ao) {
                contact = ao.contact;
                addr = ao.province + ao.city + ao.district + ao.street;
                phone = ao.phone;
            }
            if (!addr) {
                alert("请选择地址");
                return;
            }
            var otype = "merc";
            var idc = "", idc_img1 = "", idc_img2 = "";
            if (order_cart.Setup) {
                otype = "setup";
                idc = $.trim(Pub.str($("#idc").val()));
                if (!checkIdc(idc)) {
                    return;
                }
                idc_img1 = $.trim($("#idc_img1").attr("title"))
                if (!idc_img1) {
                    alert("请上传身份证 正面 照");
                    return;
                }

                idc_img2 = $.trim($("#idc_img2").attr("title"))
                if (!idc_img2) {
                    alert("请上传身份证 反面 照");
                    return;
                }
            }
            var img = order_cart.Merc.imgs;
            if (img) {
                img = img.split('|')[0];
            }
            if (!img) {
                img = "";
            }
            var data = {
                iduser: u.iduser,
                idmerc: order_cart.Merc.idmerc,
                merc: order_cart.Merc.merc1,
                idspec: order_cart.Spec.idspec,
                spec: order_cart.Spec.spec1,
                price: order_cart.Spec.price,
                count: order_cart.Count,
                month: order_cart.Spec.month,
                attmonth: order_cart.Spec.attmonth,
                contact: contact,
                addr: addr,
                phone: phone,
                notes: Pub.str($("#notes").val()),
                img: img,
                otype: otype,
                idc: idc,
                idc_img1: idc_img1,
                idc_img2: idc_img2
            };
            Pub.post({
                url: "Service/Order/Create",
                data: JSON.stringify(data),
                loadingMsg: "提交...",
                success: function (data) {
                    if (Pub.wsCheck(data)) {
                        if (data.Data) {
                            Pub.delCache("order_cart")
                            //alert("\n下单成功,订单号: " + data.Data.orderno);
                            window.location.href = Pub.rootUrl() + "Order/Pay/?orderno=" + data.Data.orderno;
                            return;
                        }
                    }
                },
                error: function (xhr, status, e) {
                    Pub.showMsg("提交失败");
                }
            });
        }
    } else {
        toHome();
    }
}


function toHome() {
    alert("亲！您现在还没有挑选宝贝呢");
    window.location.href = Pub.rootUrl();
}




function selectImg(id, e) {
    //if (e.target.files) {
    //    var f = e.target.files[0];
    //    // for (var i = 0, f; f = files[i]; i++) {
    //    //if (!f.type.match('image.*')) continue;
    //    var reader = new FileReader();
    //    reader.onload = (function (theFile) {
    //        return function (e) {
    //            //var img = document.createElement('img');
    //            $("#" + id).attr("title", theFile.name);
    //            $("#" + id).attr("src", e.target.result);
    //            //img.title = theFile.name;
    //            //img.src = e.target.result;
    //            //document.body.appendChild(img);   
    //        }
    //    })(f);
    //    reader.readAsDataURL(f);
    //    // }
    //}
    lrz(e.files[0], {
        width: 800
    }).then(function (rst) {
        // 处理成功会执行
        // console.log(rst);       
        uploadImg(rst.base64, id);
    }).catch(function (err) {
        // 处理失败会执行
    }).always(function () {
        // 不管是成功失败，都会执行
    });
}


function uploadImg(imgData, id) {
    Pub.post({
        url: "Service/File/Upload",
        data: JSON.stringify({ data: imgData }),
        loadingMsg: "上传图片中...",
        success: function (data) {
            if (Pub.wsCheck(data)) {
                $("#" + id).attr("src", imgData);
                $("#" + id).attr("title", data.Data.name);
                return;
            }
            Pub.showError("上传失败");
        },
        error: function (xhr, status, e) {
            Pub.showError("上传失败");
        }
    });
}


function checkIdc(idc) {
    if (!(/(^\d{15}$)|(^\d{17}([0-9]|[a-zA-Z])$)/.test(idc))) {
        alert('身份证号码格式不对');
        return false;
    }
    return true;
}