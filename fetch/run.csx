using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{

    // parse query parameter
    string src = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "src", true) == 0)
        .Value;

    // log.Info("Getting " + src);
    try {
      using(var client = new HttpClient())
      {
        client.BaseAddress = new Uri("http://" + src);
        var result = await client.GetAsync("");
        string article = await result.Content.ReadAsStringAsync();
        var ix1 = article.IndexOf("<article ");
        var ix2 = article.IndexOf("</article>");
        if (ix1 >=0 && ix2 >= ix1) {
          article = "<!DOCTYPE HTML><html><body>" 
            + article.Substring(ix1, ix2-ix1)
            + "</body></html>";
        }
        article = Regex.Replace(article, "<a ", "<a target='blog' ", RegexOptions.IgnoreCase);

        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.RequestMessage = req; 
        response.Content = new StringContent(article, System.Text.Encoding.UTF8);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        response.Content.Headers.ContentType.CharSet  = "UTF-8";

        return response;
      }
    } catch (Exception e) {
        return req.CreateResponse(HttpStatusCode.BadRequest, e.Message);
    }
}
