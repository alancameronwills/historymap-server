module.exports = function (context, req) {
    var roles = JSON.parse(process.env.Roles);    
    var names = roles.filter(r => r.r.indexOf("residents")>=0).map(r => r.n);
    var name = req.headers["x-ms-client-principal-name"] || "";
    var ok = names.indexOf(name) >=0;
    context.log(name);
        context.res = {
            status: ok? 200 : 401,
            body: name
        };
    
    context.done();
};