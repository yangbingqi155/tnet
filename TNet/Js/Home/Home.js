 
function getMercList() {
    Pub.get({
        url: "Service/Merc/List",
        noLoading: true,
        success: function (data) {
            var html = "";
            if (Pub.wsCheck(data)) {
                if (data.Data) {
                    for (var i = 0; i < data.Data.length; i++) {
                        var lo = data.Data[i];
                        var ro = null;
                        if (i++ < data.Data.length) {
                            ro = data.Data[i];
                        }

                        html += '<div class="pitem">';
                        html += crateItem(lo, 'l', ro);
                        html += crateItem(ro, 'r', null);
                        html += ' </div>';

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
        if (o2) {
            html += '<div class="HLive"></div>';
        }
        html += '</div>';
    }
    return html;
}

$(document).ready(getMercList);



