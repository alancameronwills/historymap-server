module.exports = function (context, req, intable) {
    context.log("Retrieved records:", intable.length);
    var outtable = intable.filter(x => x.RowKey == req.query.id);
    context.res = {
        status: 200,
        body: outtable
    };
    context.done();
};