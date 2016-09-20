
function getMercList() {
    Pub.get({
        url: "Service/Merc/List",
        noLoading: true,
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
                        if (mhtml) {
                            if (html) {
                                html += '<div class="vline"></div>';
                            }
                            html += '<div class="title">' + to.name + ':</div>';
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
            if (img) {
                img = 'Images/Merc/' + img;
            }
        }
        if (!img) {
            img = "Images/default_bg.png";
        }
        html += '<div class="pitem_' + tag + '">';
        html += '<a href="' + Pub.rootUrl() + 'Merc/Detail/' + o.idmerc + '">';
        html += '<img src="' + Pub.rootUrl() + img + '" />';
        html += o.merc1 + '</a>';
        if (tag == "l") {
            html += '<div class="HLive"></div>';
        }
        html += '</div>';
    }
    return html;
}

$(document).ready(getMercList);



