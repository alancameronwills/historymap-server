module.exports = function (context, req, intable) { 
    var outtable = intable; 
    // z is a selection of geo zones: moylgrove, cardigan, stdogmaels, ....
    // Return places within the selected zones, plus principals anywhere.
    // A principal is a pin representing a whole town.
    if (req.query.z) {
        var f = new ZoneFilter(req); 
        outtable = intable.filter(place =>
        { 
            var inZone = f.ok(place);
            if (inZone && place.Principal) {
                // Flag a principal that is within a selected zone.
                place.Principal = -1;
            }
            return inZone || place.Principal;
        });
    } else {
        outtable = intable.filter(place =>
        { 
            if (place.Principal) place.Principal = -1;
            return !(place.Deleted == '1');
        });
    }
    // Fields query is used to get just index in place editor.
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

class ZoneFilter {
    constructor (req) {
        this.cardigan = req.query.z.indexOf("cardigan")>=0;
        this.moylgrove = req.query.z.indexOf("moylgrove")>=0;
        this.stDogs = req.query.z.indexOf("stdogmaels")>=0;
    }
    // 
    ok (place) {
            if (place.Deleted == '1') return false;
            var lat = parseFloat(""+place.Latitude);
            var lon = parseFloat(""+place.Longitude);

            // Hard-wired areas!

            // Moylgrove includes the west of Cemais
            if (this.moylgrove && (lat + lon*0.97868 - 47.4631 < 0)) return true;
            var eastOfTeifi = eastOf(lat, lon, 52.108721, -4.693521, 52.080538, -4.662244)
                ||  lat>52.083175 && lon>-4.681352; // Castle Farm
            if (this.cardigan && eastOfTeifi) return true;
            if (this.stDogs) {
                if (lat < 52.057273 || lat > 52.121499 || lon > -4.646737) return false;
                if (eastOfTeifi) 
                    // include Bridgend:
                    return !northOf(lat, lon, 52.079930, -4.665626, 52.082778, -4.648631);
                else 
                    // east of Ceibwr:
                    return eastOf(lat, lon, 52.077010, -4.761375, 52.068727, -4.746698);
            }
            return false;
    }
}

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

