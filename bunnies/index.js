module.exports = function (context, req, inTable) {
    //context.log(inTable);
    //context.log(req);

    context.res = {
        status: 200,
        body: req.query.after 
        ? inTable.filter(p => 
            p.Updated ? p.Updated.toISOString().localeCompare(req.query.after)>0 : false)
        : inTable.filter(p => { if (!p.Updated) {p.Updated = p.Timestamp} return true; })
    };
    //inTable.forEach(p=> context.log(p.Updated.toISOString()));
    //context.log(req.query);
    context.done();
};