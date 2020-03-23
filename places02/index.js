
module.exports = function (context, req, intable) {
    var outtable = intable;
    context.res = {
        status: 200,
        body: outtable
    };
    context.done();
};

