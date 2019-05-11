#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req,  CloudTable outTable, TraceWriter log)
{
    //string name = req.GetQueryNameValuePairs()
    var place = await req.Content.ReadAsAsync<PlaceEntity>();
    var u1 = req.Headers.FirstOrDefault(kv => kv.Key.ToLower() == "x-ms-client-principal-name").Value;
    var user = u1 != null ? u1.SingleOrDefault() : place.User; // not strict about getting it from headers - could be spoofed by virt client

    //foreach (var kv in req.Headers) {log.Info(kv.Key + " " + kv.Value.Aggregate("", (s, x) => s + " " + x));}
 
    if (place == null) { log.Info("no place"); return new HttpResponseMessage(HttpStatusCode.BadRequest); }
    
    if (string.IsNullOrWhiteSpace(user) || user == "undefined") {
        log.Info("no user");
        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
    }
    else { 
        place.User = user;
        place.Updated = DateTime.Now;
        TableOperation updateOperation = TableOperation.InsertOrReplace(place);
        TableResult result = outTable.Execute(updateOperation);
        return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
    }
    
}


    public class PlaceEntity : TableEntity
    {
        public PlaceEntity() { }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Postcode { get; set; }
        //public double Latitude { get; set; }
        //public double Longitude { get; set; }
        public string Tags { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Zoom { get; set; }
        public string Pic1 { get; set; }
        public string Pic2 { get; set; }
        public string Text { get; set; }
        public string Year { get; set; }
        public DateTime Updated {get; set;}
        public string User {get; set;}
        public string DeleteOK { get; set; }
        public string UpdateTrail { get; set; }
    }
