using System.Net;
using Microsoft.WindowsAzure.Storage;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("Req");
         // parse query parameter
        string name = req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
            .Value;


        if (name == null)
        {
            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();
            name = data?.name;
        }

        var s = new System.Text.StringBuilder();
        s.AppendLine("<qq>");
        foreach (var p in req.Properties)
        {
            s.AppendLine("<p>" + p.Key + " => " + p.Value.ToString() + "</p>");
        }
        try
        {
            var auth = req.Headers.FirstOrDefault(kv => kv.Key == "Authorization").Value.SingleOrDefault();
            var referer = req.Headers.FirstOrDefault(kv => kv.Key == "Referer").Value.SingleOrDefault();
            if (!string.IsNullOrWhiteSpace(auth))
            {
                s.AppendLine("<p>Auth: " + auth + "</p>");

                if (referer.Contains("google"))
                {

                    var greq = System.Net.WebRequest.Create("https://www.googleapis.com/oauth2/v2/userinfo");
                    greq.Headers.Add("Authorization", auth);

                    var reader = new System.IO.StreamReader(greq.GetResponse().GetResponseStream());
                    var gresponse = reader.ReadToEnd();

                    if (System.Text.RegularExpressions.Regex.IsMatch(gresponse, "\"id\": *\"[0-9]{5}"))
                    {
                        s.AppendLine("<sas>" + GetAccountSASToken() + "</sas>");
                    }
                    else
                    {
                        s.AppendLine("<h3>No id in user info from Google</h3>");
                    }
                    s.AppendLine("<h2>Google User</h2>");
                    s.AppendLine("<pre><user>");
                    s.Append(gresponse);
                    s.AppendLine("</user></pre>");
                }
                else s.AppendLine("<h3>No referer - can't identify sign-in authority</h3>");
            }
            else s.AppendLine("<h3>No authorization passed from signed-in page</h3>");
        } catch (Exception e)
        {
            s.AppendLine("<h3>Exception in id function</h3><pre>" + e.Message + "</pre>");
        }

        s.AppendLine("<h2>Headers</h2>");
        foreach (var x in req.Headers) foreach (var y in x.Value) { s.AppendLine("<p>" + x.Key + ": " + y + "</p>"); }


        s.AppendLine("<pre>" + req.Headers.ToString() + "</pre>");

        s.AppendLine("</qq>");

        return req.CreateResponse(HttpStatusCode.OK, s.ToString());
}

public static string GetAccountSASToken()
{
            
            var storageAccount = CloudStorageAccount.Parse
              ("DefaultEndpointsProtocol=https;AccountName=moylgrovehistory;AccountKey=FYrLaOQASw3oLaEscmMUWtV70VbcTFGZXxwp0GuaTvJZKguM/C9AiI1nAKp6uw7AP4+k1gXwXTKNw9pcLDiFYA==;EndpointSuffix=core.windows.net");

            // Create a new access policy for the account.
            SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.Write | SharedAccessAccountPermissions.List 
                    | SharedAccessAccountPermissions.Add | SharedAccessAccountPermissions.Update | SharedAccessAccountPermissions.Delete,
                Services = SharedAccessAccountServices.Blob | SharedAccessAccountServices.Table,
                ResourceTypes = SharedAccessAccountResourceTypes.Object,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Protocols = SharedAccessProtocol.HttpsOrHttp,
            };

            // Return the SAS token.
           // return storageAccount.Credentials.SASToken;
            return storageAccount.GetSharedAccessSignature(policy);

            
}
