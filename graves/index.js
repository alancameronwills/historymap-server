module.exports = function (context, req, intable) {
    //context.log("census>>", req.query.first, req.query.last, req.query.year);

    var year = Number(req.query.year);

    var outtable = intable.filter(x =>
    { 
        var r = (!req.query.first || new RegExp(req.query.first, "i").test(x.firstNames))
        && (!req.query.last || new RegExp(req.query.last, "i").test(x.lastName))
        && (!req.query.year || x.yob >= year-1 && x.yob <= year+1)
        && (!req.query.place || x.assigned == req.query.place);
        return r;
    });
    context.res = {
        status: 200,
        body: outtable
    };
    context.done();
};