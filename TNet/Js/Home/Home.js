
function initPageParam() {
    var u = Pub.getUser();
    if (u && u.mu && (u.mu.recvSetup || u.mu.sendSetup)) {
        $("#Task").show();
    }
}

$(document.body).ready(initPageParam);
