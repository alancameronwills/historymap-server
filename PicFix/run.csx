#r "Microsoft.Azure.WebJobs.Extensions.EventGrid"
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Host.Bindings.Runtime;
using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Blob;

using ImageResizer;
using ImageResizer.ExtensionMethods;



static string storageAccountConnectionString = System.Environment.GetEnvironmentVariable("AzureWebJobsStorage");

static string picContainerName = "history-img";

public static void Run(Stream pic, string name, TraceWriter log)
{
    log.Info($"Processing pic\n Name:{name} \n Size: {pic.Length} Bytes");
    // https://docs.microsoft.com/en-us/azure/event-grid/resize-images-on-storage-blob-upload-event
    // ImageResizer NuGet loaded to filestore and referenced in project.json


    if (pic.Length > 1e6){
        var instructions = new Instructions { OutputFormat = OutputFormat.Jpeg, JpegQuality = 25 };

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference(picContainerName);
        CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
        blockBlob.Properties.ContentType = "image/jpg";

        using(MemoryStream outStream = new MemoryStream())
        {  
            ImageBuilder.Current.Build(new ImageJob(pic, outStream, instructions));
            outStream.Position = 0;
            log.Info($" - resized to {outStream.Length}");
            blockBlob.UploadFromStream(outStream);
        }
    }


}
