var ZoneFilter=require("..\Shared\ZoneFilter.js");

module.exports = function (context, req, intable) {
    var outtable = intable;
    // z is a selection of geo zones: moylgrove, cardigan, stdogmaels, ....
    // Return places within the selected zones, plus principals anywhere.
    // A principal is a pin representing a whole town.
    if (req.query.z) {
        var f = new ZoneFilter(req);
        outtable = intable.filter(place => {
            var inZone = !!f.ok(place);
            if (inZone && place.Principal) {
                // Flag a principal that is within a selected zone.
                place.Principal = -1;
            }
            return inZone || place.Principal;
        });
    } else {
        outtable = intable.filter(place => {
            if (place.Principal) place.Principal = -1;
            return !(place.Deleted == '1');
        });
    }
    // Fields query is used to get just index in place editor.
    if (req.query.fields) {
        outtable = outtable.map(x => { return { RowKey: x.RowKey, Title: x.Title }; });
    }
    context.log(outtable.length);
    context.res = {
        status: 200,
        body: outtable
    };
    context.done();
};
