module.exports = function (context, req) {
        
    var names = ["pantywylan@gmail.com", "Sue.sturges@btconnect.com", "Katharine Whitehead"];
    
    var name = req.headers["x-ms-client-principal-name"] || "";
    //context.log(req);
    var ok = names.indexOf(name) >=0;
    //context.log(ok);
    context.log(name);
        context.res = {
            status: ok? 200 : 401,
            body: name
        };
    
    context.done();
};