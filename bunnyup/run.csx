#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req,  CloudTable outTable, TraceWriter log)
{
    log.Info(">>>>>> " + req.RequestUri);
    // Log all the headers
    foreach (var kv in req.Headers) {log.Info(">> " + kv.Key + " " + kv.Value.Aggregate("", (s, x) => s + " " + x));}
    var c = req.Content.ToString();
    for (var i = 0; i<c.Length; i+=60)
    {
        log.Info("[" + i + "]" + c.Substring(i, Math.Min(60, c.Length - i)));
    }

    var u1 = req.Headers.FirstOrDefault(kv => kv.Key.ToLower() == "x-ms-client-principal-name").Value;
    var place = await req.Content.ReadAsAsync<PlaceEntity>();
    var user = u1 != null ? u1.SingleOrDefault() : place.User; // not strict about getting it from headers - could be spoofed by virt client


    if (place == null) {
        log.Info("NO PLACE");
        return new HttpResponseMessage(HttpStatusCode.BadRequest); 
    }

    log.Info($">>>> {user} {place.Title} {place.RowKey} {place.Latitude}");
   
    if (string.IsNullOrWhiteSpace(user)) { 
        log.Info (">> NO USER");
        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
    }

    if (string.IsNullOrWhiteSpace(place.Title) && string.IsNullOrWhiteSpace(place.Text)) {
        place.User = user;
        place.Updated = DateTime.Now;
        place.ETag = "*";
        log.Info ("Delete place");
        TableOperation deleteOperation = TableOperation.Delete(place);
        TableResult result = outTable.Execute(deleteOperation);
        return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
    }


    {
        place.User = user;
        place.Updated = DateTime.Now;
        if (place.Postcode == null) place.Postcode = "";
        if (place.Subtitle == null) place.Subtitle = "";
        TableOperation updateOperation = TableOperation.InsertOrMerge(place);
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
        public string Principal { get; set; }
    }



    public class PlaceEntityOld : TableEntity
    {
        public PlaceEntityOld() { }
        public string Title { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Text { get; set; }
        public DateTime Updated {get; set;}
        public string User {get; set;}
    }
