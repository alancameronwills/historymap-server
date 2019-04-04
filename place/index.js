var ZoneFilter=require("../Shared/ZoneFilter.js");

// Find a single place, given its id
module.exports = function (context, req, intable) {
    //context.log("Retrieved records:", intable.length);
    var outtable = intable.filter(x => x.RowKey == req.query.id);

    if (outtable.length > 0){
        // Append a zone that the place can be found in:
        var zf = new ZoneFilter(null);
        outtable[0].HomeZone = zf.ok(outtable[0]);
    }

    context.res = {
        status: outtable.length > 0 ? 200 : 400,
        body: outtable
    };
    context.done();
};