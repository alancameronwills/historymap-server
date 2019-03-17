

module.exports = function (context, req) {
    context.log("Getting,...");
    var uri = "http://" + req.query.src;
    context.log('Get ' + uri);

    try {
        var blogreq = new XMLHttpRequest();
        blogreq.open("GET", uri, false);
        blogreq.send();
        console.log("Response " + blogreq.status);
    } catch (err) {
        context.log("error " + err);
    }
    context.done();
};