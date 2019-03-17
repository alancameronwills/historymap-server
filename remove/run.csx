#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

public static HttpResponseMessage Run(Place place, CloudTable outTable, TraceWriter log)
{
    place.PartitionKey = "p1";
    place.Deleted = "1";
    place.ETag = "*";

    TableOperation updateOperation = TableOperation.Merge(place);
    TableResult result = outTable.Execute(updateOperation);
    return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
}

public class Place : TableEntity
{
    public string Deleted { get; set; }
}
