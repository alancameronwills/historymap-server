#r "Microsoft.WindowsAzure.Storage"
#r "System.Web.Extensions"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;
using System.Web.Script.Serialization;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, CloudTable appTable, TraceWriter log)
{
/*    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(qq => string.Compare(qq.Key, "add", true) == 0)
        .Value;
*/

var s = await req.Content.ReadAsStringAsync();

    var app = await req.Content.ReadAsAsync<AppEntity>();

    if (app != null && app.PartitionKey != null) {
        TableOperation updateOperation = TableOperation.InsertOrReplace(app);
        TableResult result = appTable.Execute(updateOperation);
    }
    
    var query = new TableQuery<AppEntity>();
    var items = await appTable.ExecuteQuerySegmentedAsync(query, null);

    return req.CreateResponse(HttpStatusCode.OK, items.Results);
}


    public class AppEntity : TableEntity
    {
        public AppEntity() { }
        public string User {get;set;}
        public string App {get;set;}
        public string Link {get;set;}
    }
