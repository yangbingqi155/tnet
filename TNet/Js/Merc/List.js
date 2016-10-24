
function __init() {

    Pub.onCity(function (city) {
        getMercList(city);
    });
}




$(document.body).ready(__init);