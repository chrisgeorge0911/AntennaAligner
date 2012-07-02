var c_DistanceLimit = 100;

function log10(val) {
    return Math.log(val) / Math.log(10);
}

function getFieldStrength(power, distance, channel) {

    var d = distance * 1000;  // need distance in metres
    var e = (Math.sqrt(30 * power)) / d;

    //var dbuvm = 20 * log10(e / 0.000001);

    var freq = 8 * (channel - 21) + 474;
    var lambda = 300000000 / (freq * 1000000);

    var V = 0.5 * e * lambda / 3.14159;

    var signalStrength = 20 * log10(V / 0.000001);

    signalStrength = parseFloat(signalStrength);
    return signalStrength.toFixed(1);
}



// sort the transmitters by signal strength in descending order (strongest first)
function signalSort(a, b) {
    return b.signalStrength - a.signalStrength;
}


function getListOfTxBySignalStrength(lastPosition) {

    var txList = getListOfTx();

    for (var i = 0; i < txList.length; i++) {
        txList[i].distance = getDistanceBetween(lastPosition, txList[i].position);
        txList[i].signalStrength = getFieldStrength(txList[i].ch1pwr, txList[i].distance, txList[i].ch1);
    }

    txList.sort(signalSort);

    return txList;
}

function getNearestTx(position) {

    var txList = getListOfTxBySignalStrength(position);

    var txListTop5 = new Array();

    var top5ListCount = 0;
    // get the top 5 transmitters that have signal strength 66db+
    for (var i = 0; i < txList.length; i++) {
        var currentTx = txList[i];

        if (currentTx.distance < c_DistanceLimit) {
            txListTop5[top5ListCount] = txList[i];
            txListTop5[top5ListCount].distanceMiles = txListTop5[top5ListCount].distance * 0.62137119;
            top5ListCount++;
        }

        if (txListTop5.length == 5)
            break;
    }

    return txListTop5;
}