using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{  
    var res = req.CreateResponse(HttpStatusCode.RedirectKeepVerb);
    res.Headers.Add("Location", "/h/index.htm" + req.RequestUri.Query);
    return res;
}