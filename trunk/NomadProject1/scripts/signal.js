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