#r "Newtonsoft.Json"
#r "System.Web"
#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using System.Web; 
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Blob;

static string storageAccountConnectionString = System.Environment.GetEnvironmentVariable("AzureWebJobsStorage");
static string codeContainerName = "code";

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    string response = "Files: ";
    bool ok = true;
    var data = await req.Content.ReadAsStringAsync();
    
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference(codeContainerName);
    
    foreach (var piece in HttpUtility.HtmlDecode(data).Split('&')) 
    {
        var bits = piece.Split(new char[] {'='},2);
        if (bits.Length == 2 && bits[0] == "payload") { 
            
            var s = HttpUtility.UrlDecode(bits[1]);
            Match mm = Regex.Match(s, "\"modified\":\\s*\\[(?:[^\\]\"]*\"([^\"\\]]+)\")*" );
            foreach (Capture m in mm.Groups[1].Captures)
            {
                var filename = "" + m.Value;
                try {
                    var rawFN = "https://raw.githubusercontent.com/alancameronwills/historymap/master/" + filename;
                    var wreq = WebRequest.Create(rawFN);
                    using (var wrs = wreq.GetResponse().GetResponseStream())
                    {
                        log.Info("Got " + filename );
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference("history/" + filename);
                        blockBlob.Properties.ContentType = Mime(filename);
                        blockBlob.UploadFromStream(wrs);
                    }
                    response += filename + ", ";
                    
                } catch (Exception e)
                {
                    response += "/*" + filename + ": " + e.Message + "*/";
                    log.Error(filename + " " + e.Message);
                    ok = false;
                }
            } 
        }
    }
    return req.CreateResponse(ok ? HttpStatusCode.OK : HttpStatusCode.NotFound, response);
}

private static string Mime(string filename) 
{
    var ex = System.IO.Path.GetExtension(filename).ToLower();
    if (ex == ".js") return "application/javascript";
    if (ex == ".md") return "text/markdown";
    return MimeMapping.GetMimeMapping(filename);
}