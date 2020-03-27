module.exports = function (context, req) {
        
    var names = ["pantywlan@gmail.com", "Sue Sturges"];
    var name = req.Headers["x-ms-client-principal-name"] || "";
    var ok = names.indexOf(name) >=0;
    
        context.res = {
            status: ok? 200 : 401,
            body: name
        };
    
    context.done();
};