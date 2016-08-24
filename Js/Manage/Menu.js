
var g_menuData = [];       //菜单数据
var g_currentMainId = 0;   //当前点击的主菜单索引
var g_currentSubId = 0;      //当前点击的子菜单索引


if (!g_menuJson || g_menuJson.errcode) {
    g_menuJson = {
        "menu": {
            "button": [
                {
                    "type": "click",
                    "name": "菜单名称",
                    "key": "v0",
                    "sub_button": []
                },
                {
                    "type": "click",
                    "name": "菜单名称",
                    "key": "v0",
                    "sub_button": []
                }
            ]
        }
    }
}

//加载主菜单
function loadMainMenu(isEdit) {
    if (g_menuJson && g_menuJson.menu && g_menuJson.menu.button && g_menuJson.menu.button.length > 0) {
        var mainUlObj = $(".menu ul");
        mainUlObj.html("");
        var mainMenuHtml = "";
        if (isEdit == undefined || !isEdit) {
            g_menuData = g_menuJson.menu.button;
        }
        var itemCount = g_menuData.length;
        var className = "";
        switch (itemCount) {
            case 1:
                className = "of2";
                break;
            case 2:
            case 3:
                className = "of3";
                break;
            default:
                break;

        }
        for (var i = 0; i < itemCount; i++) {
            mainMenuHtml += "<li id='main_" + (i + 1) + "' class='" + className + "'>" + g_menuData[i].name + "</li>";
        }
        mainUlObj.prepend(mainMenuHtml);
        mainUlObj.children("li").click(function () {
            editMainMenu(this);
            showSubMenu(this);
            loadSubMenu(this);
        });

        if (g_menuData.length < 3) {

            mainUlObj.append("<li class='" + className + "'><i class='add_icon'></i></li>");
            mainUlObj.children("li:last").click(function () {
                addMainMenuListener(this);
            });
        }

    }
}

//添加主菜单
function addMainMenuListener(obj) {
    // $(".menu ul li").click(function ()
    // {
    var itemCount = g_menuData.length;
    if (itemCount < 3) {
        var id = "main_" + (g_menuData.length + 1);
        g_menuData.push({ name: "菜单名称", type: "media_id", media_id: "", sub_button: [] });
        switch (itemCount) {
            case 0:
                $(obj).attr("class", "of2")
                // $(this).siblings("li").attr("class", "of2");
                $(this).before("<li id='" + id + "' class='of2 selected'>菜单名称</li>");

                break;
            case 1:
                $(obj).attr("class", "of3")
                $(obj).siblings("li").attr("class", "of3");
                $(obj).before("<li id='" + id + "' class='of3 selected'>菜单名称</li>");
                break;
            case 2:
                $(obj).siblings("li").attr("class", "of3");
                $(obj).before("<li id='" + id + "' class='of3 selected'>菜单名称</li>");
                $(obj).remove();
                break;
            default:
                break;
        }
        //showSubMenu(this);
        $("#" + id).click(function () {
            editMainMenu(this);
            showSubMenu(this);
            loadSubMenu(this);
        }
        );

        $("#" + id).click();
    }
    // });
}

//添加子菜单
function addSubMenuListener(obj) {
    var itemCount = g_menuData[g_currentMainId - 1].sub_button.length;
    if (itemCount < 5) {
        var id = "sub_" + g_currentMainId + "_" + (itemCount + 1);
        g_menuData[g_currentMainId - 1].sub_button.push({ key: id, name: id, type: "", mediaid: "", url: "" });
        $(obj).siblings("li").removeClass("selected");
        $(obj).before("<li id='" + id + "' class='selected'>" + id + "</li>");
        $("#" + id).click(function () {
            editSubMenu(this);

        });
        if (itemCount >= 4) {
            $(obj).remove();
        }

        $("#" + id).click();
    }

}



//显示子菜单布局
function showSubMenu(obj) {
    var id = $(obj).attr("id");
    $(obj).siblings("li").removeClass("selected");
    $(obj).addClass("selected");
    var subMenu = $(".subMenuBox");
    var mainMenuLength = $("#" + id).siblings("li").length;
    if (mainMenuLength == 1) {
        subMenu.addClass("of2");
        g_currentMainId = 1;
        g_currentSubId = 0;
    }
    else if (mainMenuLength == 2) {
        if (id.indexOf("1") >= 0) {
            subMenu.css("left", "0");
            g_currentMainId = 1;
            g_currentSubId = 0;
        }
        else if (id.indexOf("2") >= 0) {
            subMenu.css("left", "33.33%");
            g_currentMainId = 2;
            g_currentSubId = 0;
        }
        else {
            subMenu.css("left", "66.66%");
            g_currentMainId = 3;
            g_currentSubId = 0;
        }
        subMenu.addClass("of3");
    }
    subMenu.show();
}

//加载子菜单
function loadSubMenu(obj) {
    var subUlObj = $(".subMenuBox ul");
    subUlObj.empty();
    //var id = $(obj).attr("id");
    if (g_menuData.length > 0) {
        var subMenu = g_menuData[g_currentMainId - 1].sub_button;
        var subMenuHtml = "";
        if (subMenu.length > 0) {
            var id = "";
            for (var i = 0; i < subMenu.length; i++) {
                id = "sub_" + g_currentMainId + "_" + (i + 1);
                subMenuHtml += "<li id='" + id + "'>" + subMenu[i].name + "</li>";
            }
        }

        subUlObj.prepend(subMenuHtml);
        subUlObj.children("li").click(function () {
            editSubMenu(this);
        });
        if (subMenu.length < 5) {
            subUlObj.append("<li><i class='add_icon'></i></li>");
            subUlObj.children("li:last").click(function () {
                addSubMenuListener(this);
            });
        }
    }
}

//编辑主菜单
function editMainMenu(obj) {
    $("#hasSub").show();
    $("#material_detail_box").hide();
    var id = $(obj).attr("id");
    var indexArray = id.split("_");
    if (g_menuData && g_menuData.length > 0) {
        var selectedMenuData;
        if (indexArray.length == 2) {
            selectedMenuData = g_menuData[indexArray[1] - 1];
            if (selectedMenuData && selectedMenuData != undefined) {
                $("#menuName").val(selectedMenuData.name);
                if (selectedMenuData.sub_button.length <= 0)  //没有子菜单
                {
                    var type = selectedMenuData.type;
                    $("#menuType").val(type);
                    if (type == "media_id") {
                        $("#res_1").show();
                        $("#res_2").hide();
                        $("#menuMediaId").val(selectedMenuData.media_id);
                        getMaterialDetail(selectedMenuData.media_id);
                    }
                    else if (type == "view") {
                        $("#res_1").hide();
                        $("#res_2").show();
                        $("#menuUrl").val(selectedMenuData.url);
                    }
                    else if (type == undefined || type == "") {
                        //$("#hasSub").hide();
                        $("#res_1").show();
                        $("#res_2").hide();
                        $("#material_detail_box").show();
                        $("#material_detail").hide();
                        $("#material_detail_cover").show();
                    }
                }
                else {
                    $("#hasSub").hide();
                }
            }
        }
    }

}

//编辑子菜单
function editSubMenu(obj) {
    $("#hasSub").show();
    $("#material_detail_box").hide();
    $("#main_" + g_currentMainId).removeClass("selected");
    $(obj).siblings("li").removeClass("selected");
    $(obj).addClass("selected");
    //alert($(obj).attr("id"));
    var id = $(obj).attr("id");
    var indexArray = id.split("_");
    if (g_menuData && g_menuData.length > 0) {
        g_currentSubId = indexArray[2];
        var selectedMenuData;
        if (indexArray.length == 3) {
            selectedMenuData = g_menuData[indexArray[1] - 1].sub_button[indexArray[2] - 1];
        }
        if (selectedMenuData && selectedMenuData != undefined) {
            var type = selectedMenuData.type;
            $("#menuName").val(selectedMenuData.name);
            $("#menuType").val(type);
            if (type == "media_id") {
                $("#res_1").show();
                $("#res_2").hide();
                $("#menuMediaId").val(selectedMenuData.media_id);
                getMaterialDetail(selectedMenuData.media_id);
            }
            else if (type == "view") {
                $("#res_1").hide();
                $("#res_2").show();
                $("#menuUrl").val(selectedMenuData.url);
            }

        }
    }

}

//切换菜单类型
function setMenuType(selectType) {
    if (selectType == "media_id") {
        $("#res_1").show();
        $("#res_2").hide();
        var type;
        if (g_currentSubId > 0) {
            type = g_menuData[g_currentMainId - 1].sub_button[g_currentSubId - 1].type;
        }
        else {

            type = g_menuData[g_currentMainId - 1].type;
        }
        if (type == "view") {
            $("#material_detail").hide();
            $("#material_detail_cover").show();
        }
        $("#material_detail_box").show();
    }
    else if (selectType == "view") {
        $("#res_1").hide();
        $("#res_2").show();
        $("#menuUrl").val("");
        $("#material_detail_box").hide();
    }

}

//拉取菜单列表
function getMaterialList() {
    var d = new Date();
    $.ajax({
        url: "GetMaterialList?t=" + d.getTime(),
        type: 'POST',
        async: true,
        dataType: 'json',
        contentType: 'application/json',
        timeout: 1000 * 60 * 2,
        data: JSON.stringify({ type: "news", offset: 0, count: 10 }),
        success: function (data) {
            data = JSON.parse(data);
            if (data.item && data.item.length > 0) {
                var materialHtml = "";
                for (var i = 0; i < data.item.length; i++) {
                    materialHtml += "<li id='" + data.item[i].media_id + "' onmouseover='showOrHideItemMask(event,this)' onmouseout='showOrHideItemMask(event,this)'>";
                    var newsItem = data.item[i].content.news_item;
                    if (newsItem && newsItem.length > 0) {
                        materialHtml += "<div class='msg_content'>";
                        for (var j = 0; j < newsItem.length; j++) {
                            if (j == 0) {
                                materialHtml += "<div class='title'>";
                                materialHtml += "<img src='' id='" + newsItem[j].thumb_media_id + "'/>";
                                materialHtml += "<p>" + newsItem[j].title + "</p>";
                                materialHtml += "</div>";
                            }
                            else {
                                materialHtml += "<div class='item'>";
                                materialHtml += "<img src='' id='" + newsItem[j].thumb_media_id + "'/>";
                                materialHtml += "<span>" + newsItem[j].title + "</span>";
                                materialHtml += "</div>";
                            }

                            loadImage(newsItem[j].thumb_media_id, false);
                        }
                        materialHtml += "</div>";
                        materialHtml += "<div class='msg_mask'  onclick=\"updateMenuData(this,1,'media_id')\"><i class='icon_select'></i></div>";
                    }
                    materialHtml += "</li>";
                }
                $(".dialog_bd ul").html(materialHtml);
                $(".popWindow").show();
                $("#maskLayout").show();
            }
        },
        error: function (data) {
        }
    });
}

//加载图片
function loadImage(thumb_media_id, isDetail) {
    if (thumb_media_id && thumb_media_id != "") {
        var d = new Date();
        $.ajax({
            url: "GetImageById?t=" + d.getTime(),
            type: 'POST',
            async: true,
            //dataType: 'json',
            contentType: 'application/json',
            timeout: 1000 * 60 * 2,
            data: JSON.stringify({ media_id: thumb_media_id }),
            success: function (data) {
                //alert(data);
                let id = thumb_media_id;
                if (isDetail) {
                    id = "detail_" + id;
                }
                $("#" + id).attr("src", data);
            },
            error: function (data) {
            }
        });
    }
}

//拉去菜单明细
function getMaterialDetail(media_id) {
    if (media_id && media_id != undefined && media_id != "") {
        var d = new Date();
        $.ajax({
            url: "GetMaterialDetail?t=" + d.getTime(),
            type: 'POST',
            async: true,
            dataType: 'json',
            contentType: 'application/json',
            timeout: 1000 * 60 * 2,
            data: JSON.stringify({ media_id: media_id }),
            success: function (data) {
                // alert(data);
                if (data && data != "" && data.news_item.length > 0) {
                    var conetentHtml = "";
                    for (var i = 0; i < data.news_item.length; i++) {
                        if (i == 0) {
                            conetentHtml += "<div class='title'>";
                            conetentHtml += "<img  src=''  id='detail_" + data.news_item[i].thumb_media_id + "'/>";
                            conetentHtml += "<p>" + data.news_item[i].title + "</p>";
                            conetentHtml += "</div>";
                        }
                        else {
                            conetentHtml += "<div class='item'>";
                            conetentHtml += "<img src='' id='detail_" + data.news_item[i].thumb_media_id + "'/>";
                            conetentHtml += "<span>" + data.news_item[i].title + "</span>";
                            conetentHtml += "</div>";
                        }
                        loadImage(data.news_item[i].thumb_media_id, true);
                    }
                    conetentHtml += "<div class='msg_mask' onclick=\"updateMenuData(this,0,'media_id')\"><i class='icon_select'></i></div>";
                    $("#material_detail_box").show();
                    $("#material_detail").html(conetentHtml);
                    $("#material_detail").show();
                    $("#material_detail_cover").hide();
                }

            },
            error: function (data) {
            }
        });
    } else {
        $("#material_detail_box").show();
        $("#material_detail").hide();
        $("#material_detail_cover").show();

    }
}


//更新菜单数据 0-删除，1--新增，2--编辑
function updateMenuData(obj, tag, type) {
    // var media_id = $(obj).attr("id");
    if (g_menuData && g_menuData.length > 0) {
        switch (tag) {
            case 0:
                if (g_currentSubId > 0) {
                    var subItem = g_menuData[g_currentMainId - 1].sub_button[g_currentSubId - 1];
                    if (type == "media_id") {
                        subItem.type = "";
                        subItem.media_id = "";
                        $("#material_detail").hide();
                        $("#material_detail").empty();
                        $("#material_detail_cover").show();
                    }
                    else if (type == "menu") {
                        g_menuData[g_currentMainId - 1].sub_button.splice(g_currentSubId - 1, 1);
                        $("#sub_" + g_currentMainId + "_" + g_currentSubId).remove();
                        $("#main_" + g_currentMainId).click();
                    }
                }
                else {
                    var mainItem = g_menuData[g_currentMainId - 1];
                    if (type == "media_id") {
                        mainItem.type = "";
                        mainItem.media_id = "";
                        $("#material_detail").hide();
                        $("#material_detail").empty();
                        $("#material_detail_cover").show();
                    }
                    else if (type == "menu") {
                        g_menuData.splice(g_currentMainId - 1, 1);
                        $("#main_" + g_currentMainId).remove();
                        loadMainMenu(true);
                        //var mainUlObj = $(".menu ul");
                        //var className="of"+(g_menuData.length+1);
                        //mainUlObj.append("<li class='" + className + "'><i class='add_icon'></i></li>");
                        //mainUlObj.children("li:last").click(function ()
                        //{
                        //    addMainMenuListener(this);
                        //});
                    }

                }
                break;
            case 1:
                if (g_currentSubId > 0) {
                    var subItem = g_menuData[g_currentMainId - 1].sub_button[g_currentSubId - 1];
                    if (type == "media_id") {
                        $(obj).parent().siblings("li").removeClass("selected");
                        $(obj).parent().siblings("li").children(".msg_mask").hide();
                        var media_id = $(obj).parent().attr("id");
                        $(obj).show();
                        $(obj).parent().attr("class", "selected");
                        delete subItem.url;
                        subItem.type = "media_id";
                        subItem.media_id = media_id;
                        $("#material_detail").html($(obj).siblings(".msg_content").html());
                        $("#material_detail").append("<div onclick=\"updateMenuData(this,0,'media_id')\" class='msg_mask' style='display: none;'><i class='icon_select'></i></div>");
                        $("#material_detail").show();
                        $("#material_detail_cover").hide();
                    }
                }
                else {
                    var mainItem = g_menuData[g_currentMainId - 1];
                    if (type == "media_id") {
                        $(obj).parent().siblings("li").removeClass("selected");
                        $(obj).parent().siblings("li").children(".msg_mask").hide();
                        var media_id = $(obj).parent().attr("id");
                        $(obj).show();
                        $(obj).parent().attr("class", "selected");
                        delete mainItem.url;
                        mainItem.type = "media_id";
                        mainItem.media_id = media_id;
                        $("#material_detail").html($(obj).siblings(".msg_content").html());
                        $("#material_detail").append("<div onclick=\"updateMenuData(this,0,'media_id')\" class='msg_mask' style='display: none;'><i class='icon_select'></i></div>");
                        $("#material_detail").show();
                        $("#material_detail_cover").hide();

                    }
                }
                break;
            case 2:

                var name = $(obj).val();
                if (g_currentSubId > 0) {
                    var subItem = g_menuData[g_currentMainId - 1].sub_button[g_currentSubId - 1];
                    if (type == "") {
                        subItem.name = name;
                        $("#sub_" + g_currentMainId + "_" + g_currentSubId).text(name);
                    }
                    else if (type == "url") {
                        delete subItem.media_id;
                        subItem.type = "view";
                        subItem.url = name;
                    }
                }
                else {
                    var mainItem = g_menuData[g_currentMainId - 1];
                    if (type == "") {
                        mainItem.name = name;
                        $("#main_" + g_currentMainId).text(name);
                    }
                    else if (type == "url") {
                        mainItem.url = name;
                        delete mainItem.media_id;
                        mainItem.type = "view";
                        mainItem.url = name;
                    }
                }
                break;
            default:
        }
    }
}

//更新菜单
function updateWeChatMenu() {
    if (g_menuData && g_menuData.length > 0) {
        for (var i = 0; i < g_menuData.length; i++) {

            if (g_menuData[i].sub_button && g_menuData[i].sub_button.length > 0) {
                for (var j = 0; j < g_menuData[i].sub_button.length; j++) {
                    delete g_menuData[i].sub_button[j].sub_button;
                }
            }
            else {
                delete g_menuData[i].sub_button;
            }
        }
        var d = new Date();
        Pub.showLoading("保存中...");
        $.ajax({
            url: "UpdateMenu?t=" + d.getTime(),
            type: 'POST',
            async: true,
            //dataType: 'json',
            contentType: 'application/json',
            timeout: 1000 * 60 * 2,
            data: JSON.stringify({ menu: JSON.stringify({ button: g_menuData }) }),
            success: function (data) {
                data = JSON.parse(data);
                if (data && data.errcode == 0) {
                    Pub.showMsg("保存成功.");
                } else {
                    Pub.showError("保存失败.(>0.0<)!");
                }

            },
            error: function (data) {
                Pub.showError("保存失败(>0.0<)!");
            }
        });

    }

}

//控制鼠标移入、移出图文菜单
function showOrHideItemMask(e, obj) {
    e = window.event || e;
    if (!$(obj).hasClass("selected")) {
        if (e.type == "mouseover") {
            $(obj).children(".msg_mask").show();
        }
        else {
            $(obj).children(".msg_mask").hide();
        }
    }
}




function closePopWindow(obj) {
    $("#maskLayout").hide();
    $(".popWindow").hide();
}

//$(document).ready(addMainMenuListener);
$(document).ready(function () {
    loadMainMenu(false);
});