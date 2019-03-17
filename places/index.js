module.exports = function (context, req, intable) {
    //context.log(req);
    var outtable = intable.filter(x =>
    { 
        return x.Deleted != '1' 
    });
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