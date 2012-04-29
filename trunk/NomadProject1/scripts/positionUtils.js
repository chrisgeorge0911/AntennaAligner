Number.prototype.toDeg = function () {
    return this * 180 / Math.PI;
}

Number.prototype.toRad = function () {
    return this * Math.PI / 180;
}


function getBearing(pos1, pos2) {
    var lat1 = pos1.coords.latitude;
    var lon1 = pos1.coords.longitude;

    var lat2 = pos2.coords.latitude;
    var lon2 = pos2.coords.longitude;


    var dLon = (lon2 - lon1).toRad();
    var _lat1 = lat1.toRad();
    var _lat2 = lat2.toRad();


    var y = Math.sin(dLon) * Math.cos(_lat2);
    var x = Math.cos(_lat1) * Math.sin(_lat2) - Math.sin(_lat1) * Math.cos(_lat2) * Math.cos(dLon);
    var brng = Math.atan2(y, x).toDeg();
    return brng;
}

function getDistanceBetween(pos1, pos2) {
    var lat1 = pos1.coords.latitude;
    var lon1 = pos1.coords.longitude;

    var lat2 = pos2.coords.latitude;
    var lon2 = pos2.coords.longitude;

    var R = 6371; // km
    var dLat = (lat2 - lat1).toRad();
    var dLon = (lon2 - lon1).toRad();
    var lat1 = lat1.toRad();
    var lat2 = lat2.toRad();

    var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
            Math.sin(dLon / 2) * Math.sin(dLon / 2) * Math.cos(lat1) * Math.cos(lat2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c;

    return d;
}