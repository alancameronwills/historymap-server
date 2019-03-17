#r "System.Web"

using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    string fileName = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "f", true) == 0)
        .Value;

    if (string.IsNullOrEmpty(fileName))
    {
        log.Info("redirect");
            var query = req.RequestUri.Query;
            var res = req.CreateResponse(HttpStatusCode.RedirectKeepVerb);
            res.Headers.Add("Location", "/history/index.htm" + query);
            return res;
    }
    log.Info (fileName);
    var home = Environment.GetEnvironmentVariable("HOME");
    var root = home + @"\site\wwwroot\";
    try {
        var stream = new FileStream (root + fileName.Replace('/', '\\'), FileMode.Open);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StreamContent(stream);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue(System.Web.MimeMapping.GetMimeMapping(fileName));
        return response;
    }
    catch (Exception ex)
    {
        var res = new HttpResponseMessage(HttpStatusCode.NotFound); 
        return res;
    }
    
}
