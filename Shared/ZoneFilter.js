
class ZoneFilter {
    constructor(req) {
        if (req) {
            this.cardigan = req.query.z.indexOf("cardigan") >= 0;
            this.moylgrove = req.query.z.indexOf("moylgrove") >= 0;
            this.stDogs = req.query.z.indexOf("stdogmaels") >= 0;
        }
        else { this.cardigan = true; this.moylgrove = true; this.stdogmaels = true; }
    }
    // 
    ok(place) {
        if (place.Deleted == '1') return "";
        var lat = parseFloat("" + place.Latitude);
        var lon = parseFloat("" + place.Longitude);

        // Hard-wired areas!

        // Moylgrove includes the west of Cemais
        if (this.moylgrove && (lat + lon * 0.97868 - 47.4631 < 0)) return "moylgrove";
        var eastOfTeifi = ZoneFilter.eastOf(lat, lon, 52.108721, -4.693521, 52.080538, -4.662244)
            || lat > 52.083175 && lon > -4.681352; // Castle Farm
        if (this.cardigan && eastOfTeifi) return "cardigan";
        if (this.stDogs) {
            if (lat < 52.057273 || lat > 52.121499 || lon > -4.646737) return "";
            if (eastOfTeifi)
                // include Bridgend:
                return !ZoneFilter.northOf(lat, lon, 52.079930, -4.665626, 52.082778, -4.648631) ? "stdogmaels" : "";
            else
                // east of Ceibwr:
                return ZoneFilter.eastOf(lat, lon, 52.077010, -4.761375, 52.068727, -4.746698) ? "stdogmaels" : "";
        }
        return false;
    }


    static northOf(lat, lon, y1, x1, y2, x2) {
        var slope = (y1 - y2) / (x1 - x2);
        var intersect = (lon - x1) * slope;
        return lat - y1 > intersect;
    }

    static eastOf(lat, lon, y1, x1, y2, x2) {
        var inverseSlope = (x1 - x2) / (y1 - y2);
        var intersect = (lat - y1) * inverseSlope;
        return lon - x1 > intersect;
    }
}
module.exports = ZoneFilter;