
function getListUrl() {
    if (isSetup()) {
        return "Service/Merc/Setup/List"
    }
    return "Service/Merc/List";
}


function getMercList() {
    Pub.get({
        url: getListUrl(),
        loadingMsg:"加载中...",
        success: function (data) {
            var html = "";
            if (Pub.wsCheck(data)) {
                if (data.Data) {
                    var types = data.Data.Types;
                    for (var i = 0; i < types.length; i++) {
                        var to = types[i];
                        var mhtml = "";
                        for (var j = 0; j < data.Data.Mercs.length; j++) {
                            var lo = data.Data.Mercs[j];
                            if (to.idtype == lo.idtype) {
                                var ro = null;
                                if (++j < data.Data.Mercs.length) {
                                    ro = data.Data.Mercs[j];
                                    if (to.idtype != ro.idtype) {
                                        ro = null;
                                    }
                                }
                                mhtml += '<div class="pitem">';
                                mhtml += crateItem(lo, 'l', ro);
                                mhtml += crateItem(ro, 'r', null);
                                mhtml += ' </div>';
                            }
                        }
                        var shtml = "";
                        if (data.Data.Setups) {
                            for (var z = 0; z < data.Data.Setups.length; z++) {
                                var s = data.Data.Setups[z];
                                if (s.idtype == to.idtype) {
                                    shtml += '<div class="setup">';
                                    shtml += '<div class="setup_title"><span>主题:</span>' + s.setup1 + '</div>';
                                    shtml += '<div class="setup_resource"><span>材料:</span>' + s.resource + '</div>';
                                    shtml += '<div class="setup_setuptype"><span>方式:</span>' + s.setuptype + '</div>';

                                    var sahtml = "";
                                    for (var h = 0; h < data.Data.SetupAddrs.length; h++) {
                                        var sa = data.Data.SetupAddrs[h];
                                        if (sa.idtype == to.idtype && sa.idsetup == s.idsetup) {
                                            sahtml += '<div class="setup_addr_line"></div>';
                                            sahtml += '<div class="setup_addr"><div><span>电话:</span>' + sa.phone + "&nbsp;&nbsp;" + sa.service + '</div>';
                                            sahtml += '<div><span>受理:</span>' + sa.acceptime + '</div>';
                                            sahtml += '<div><span>安装:</span>' + sa.setuptime + '</div>';
                                            sahtml += '<div><span>地址:</span>' + sa.addr + '</div></div>';

                                        }
                                    }
                                    if (sahtml) {
                                        shtml += "<div class='setup_addr_title'>办理点:</div>" + sahtml;
                                    }
                                    shtml += '</div>';
                                }
                            }
                        }
                        if (mhtml || shtml) {

                            if (html) {
                                html += '<div class="vline"></div>';
                            }
                            html += '<div class="title">' + to.name + ':</div>';
                            html += shtml;
                            html += mhtml;
                        }
                    }

                }
            }
            if (html) {
                $('#merc').html(html);
            } else {
                Pub.noData("#merc", "暂无数据.", getMercList);
            }
        },
        error: function (data) {
            Pub.noData("#merc", "加载数据失败", getMercList);
        }
    });
}

function crateItem(o, tag, o2) {
    var setup = isSetup() ? "?tag=Setup" : "";
    var html = '';
    if (o) {
        var img = null;
        if (o.imgs) {
            var ms = o.imgs.split('|');
            for (var i = 0; i < ms.length; i++) {
                img = ms[i];
                if (img && img != "") {
                    break;
                }
                img = null;
            }

        }
        html += '<div class="pitem_' + tag + '">';
        html += '<a href="' + Pub.url('Merc/Detail/' + o.idmerc + setup) + '">';
        html += '<img src="' + Pub.url(img, "Images/default_bg.png") + '" />';
        html += o.merc1 + '</a>';
        if (tag == "l") {
            html += '<div class="HLive"></div>';
        }
        html += '</div>';
    }
    return html;
}

$(document).ready(getMercList);