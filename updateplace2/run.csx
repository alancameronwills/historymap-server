#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req,  CloudTable outTable, TraceWriter log)
{
    //string name = req.GetQueryNameValuePairs()
    var place = await req.Content.ReadAsAsync<Place2Entity>();
    var host = req.Headers.FirstOrDefault(kv => kv.Key.ToLower() == "origin").Value;
    var u1 = req.Headers.FirstOrDefault(kv => kv.Key.ToLower() == "x-ms-client-principal-name").Value;

    foreach (var kv in req.Headers) {log.Info(kv.Key + " " + kv.Value.Aggregate("", (s, x) => s + " " + x));}

    if (place == null) { 
        log.Info("no place"); 
        return new HttpResponseMessage(HttpStatusCode.BadRequest); 
    }

    if (host.IndexOf("//localhost")<0 && u1 == null) {
        log.Info("no user");
        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
    }

    TableOperation updateOperation = TableOperation.InsertOrReplace(place);
    TableResult result = outTable.Execute(updateOperation);
    return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
}


    public class Place2Entity : TableEntity
    {
        public Place2Entity() { }

        public string Description { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Phone { get; set; }
        public string c3 { get; set; }
        public string c4 { get; set; }
        public string health { get; set; }
    }
