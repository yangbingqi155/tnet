Pub.checkUser(true);


function getOps(so) {
    var html = "";
    if (so.ops) {
        var op = so.ops.split("|");

        for (var i = 0; i < op.length; i++) {
            var p = op[i];
            if (p == "pause") {
                html += '<a class="pause" href="javascript:void(0)">暂停</a>';
            } else if (p == "finish") {
                html += '<a class="finish" href="="javascript:void(0)">完工</a>';
            }
        }
    }
    return html;
}


function getTimeNum(workTime, unit) {
    if (!unit) {
        unit = "分";
    }
    var tt = "";
    if (!workTime) {
        return tt;
    }
    if (workTime >= 60) {
        var wt = parseInt(workTime / 60);
        if (wt > 24) {
            tt = ">1天";
        } else {
            if (wt == 24) {
                wt = 1;
                tt = wt + "天";
            } else {
                tt = wt + "时";
            }

            wt = (workTime % 60);
            if (wt) {
                tt += wt + "分";
            }
        }
    } else {
        tt = wt + "分";
    }
    return tt;
}
