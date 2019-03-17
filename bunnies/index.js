module.exports = function (context, req, inTable) {
    //context.log(inTable);
    //context.log(req);

    context.res = {
        status: 200,
        body: req.query.after ? inTable.filter(p => p.Updated.toISOString().localeCompare(req.query.after)>0) : inTable
    };
    //inTable.forEach(p=> context.log(p.Updated.toISOString()));
    //context.log(req.query);
    context.done();
};