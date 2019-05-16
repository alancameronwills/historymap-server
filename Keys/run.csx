using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    var b = new Dictionary<string, string>();
    var dict = Environment.GetEnvironmentVariables();
    foreach (var k in dict.Keys) {
        if (("" + k).StartsWith ("Client_")) 
            b[k as String] = dict[k] as String;
    } 
    return req.CreateResponse(HttpStatusCode.OK, b, "application/json");
}
