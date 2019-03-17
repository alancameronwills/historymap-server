#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Table;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, CloudTable placeTable, TraceWriter log)
{
    var q = new TableQuery().Where("PartitionKey eq 'p1' and RowKey eq 'index'");
    var index = placeTable.ExecuteQuery(q).FirstOrDefault();
    var i = index["index"].Int32Value;
    index["index"].Int32Value = ++i;
    log.Info($"index={i}");
    

    TableOperation updateOperation = TableOperation.InsertOrReplace(index);
    TableResult result = placeTable.Execute(updateOperation);
    var response = new HttpResponseMessage(HttpStatusCode.OK);
    response.Content = new System.Net.Http.StringContent(""+i);
    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/text");

    return response;
}
