
module.exports = function (context, req, intable) {
    var outtable = intable;
    context.log(req);
    context.res = {
        status: 200,
        body: outtable
    };
    context.done();
};

