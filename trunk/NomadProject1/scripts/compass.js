function CalculateBezelHeading(newMagHeading, lastMagHeading, lastBezelHeading) {

    //alert(newMagHeading + ", " + lastMagHeading + ", " + lastBezelHeading);

    var relativeHeading;

    if (( lastMagHeading > 270 && lastMagHeading < 360 ) && (newMagHeading >= 0 && newMagHeading < 90))
    {
        relativeHeading = (360 - lastMagHeading) + newMagHeading;
    }
    else if ((newMagHeading > 270 && newMagHeading < 360) && (lastMagHeading >= 0 && lastMagHeading < 90)) {
        relativeHeading = ((newMagHeading-360) - lastMagHeading);
    }
    else {
        relativeHeading = newMagHeading - lastMagHeading;
    }

    var newBezelHeading = lastBezelHeading + relativeHeading;

    return newBezelHeading;
}


/*
n    l   lastbezel  rel  res
10   350  350      20   370

10   280  280      90   370 

350   10   10     -20   -10

350  370  370     -20   350
 

*/