
function initPageParam() {
    var u = Pub.getUser();
    if (u && u.mu && (u.mu.recvSetup || u.mu.sendSetup)) {
        $("#Task").show().css("display", "block");
    }
   
}

$(document.body).ready(initPageParam);
