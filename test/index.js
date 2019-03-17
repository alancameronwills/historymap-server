module.exports = function (context, req) {
    
        context.res = {
            status: 200,
            body: req
        };
    
    context.done();
};