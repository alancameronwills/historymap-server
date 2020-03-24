
module.exports = function (context, req, intable) {
    if (req.headers["origin"] && req.headers["origin"].indexOf("//localhost") >0 ||
        req.headers["x-ms-client-principal-id"] && req.params["q"] && req.headers["x-ms-client-principal-id"] == req.params["q"]) {
        var outtable = intable;
        context.log(req);
        context.res = {
            status: 200,
            body: outtable
        };
        context.done();
    } else {
        context.res.status = 401;
        context.done();
    }
};

