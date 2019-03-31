module.exports = function (context, req, intable) {
    //context.log(req);
    var outtable = intable;
    if (req.query.z) {
        var cardigan = req.query.z.indexOf("cardigan")>=0;
        var moylgrove = req.query.z.indexOf("moylgrove")>=0;
        var stDogs = req.query.z.indexOf("stdogmaels")>=0;
        outtable = intable.filter(place =>
        { 
            if (place.Deleted == '1') return false;
            var lat = parseFloat(""+place.Latitude);
            var lon = parseFloat(""+place.Longitude);
            // Moylgrove includes the west of Cemais
            if (moylgrove && (lat + lon*0.97868 - 47.4631 < 0)) return true;
            var eastOfTeifi = eastOf(lat, lon, 52.108721, -4.693521, 52.080538, -4.662244);
            // Include castle farm:
            if (cardigan && (eastOfTeifi || lat>52.083175 && lon>-4.681352)) return true;
            if (stDogs) {
                if (lat < 52.057273 || lat > 52.121499 || lon > -4.646737) return false;
                if (eastOfTeifi) 
                    // include Bridgend:
                    return !northOf(lat, lon, 52.079930, -4.665626, 52.082778, -4.648631);
                else 
                    // east of Ceibwr:
                    return eastOf(lat, lon, 52.077010, -4.761375, 52.068727, -4.746698);
            }
            return false;
        });
    } else {
        outtable = intable.filter(place =>
        { 
            return !(place.Deleted == '1');
        });
    }

    if(req.query.fields)
    {
        outtable = outtable.map(x => { return {RowKey:x.RowKey, Title:x.Title};});
    }
    context.log(outtable.length);
    context.res = {
        status: 200,
        body: outtable
    };
    context.done();
};


function northOf(lat, lon, y1, x1, y2, x2)
{
    var slope = (y1-y2)/(x1-x2);
    var intersect = (lon-x1)*slope;
    return lat-y1 > intersect;
}

function eastOf(lat, lon, y1, x1, y2, x2) 
{
    var inverseSlope = (x1-x2)/(y1-y2);
    var intersect = (lat-y1)*inverseSlope;
    return lon-x1 > intersect;
}
