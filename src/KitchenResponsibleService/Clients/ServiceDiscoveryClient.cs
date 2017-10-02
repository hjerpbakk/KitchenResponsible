using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using KitchenResponsibleService.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace KitchenResponsibleService.Clients
{
    public class ServiceDiscoveryClient
    {
        const string ContainerName = "discovery";
        const string ComicsServiceURLBlobName = "comics-service.txt";

        readonly AppConfiguration configuration;

        readonly CloudBlobClient blobClient;
        readonly CloudBlobContainer discoveryContainer;

        public ServiceDiscoveryClient(BlobStorageConfiguration blobStorageConfiguration, AppConfiguration configuration)
		{
            this.configuration = configuration;
			var storageAccount = CloudStorageAccount.Parse(blobStorageConfiguration.ConnectionString);

			blobClient = storageAccount.CreateCloudBlobClient();

			discoveryContainer = blobClient.GetContainerReference(ContainerName);
			discoveryContainer.CreateIfNotExistsAsync().GetAwaiter();
        }

        public async Task SetComicServiceURL() {
            var blobRef = discoveryContainer.GetBlobReference(ComicsServiceURLBlobName);
            string ip = null;
            using (var memoryStream = new MemoryStream())
    	    {
    	       await blobRef.DownloadToStreamAsync(memoryStream);
    	       ip = Encoding.UTF8.GetString(memoryStream.ToArray());
    	    }

            if (ip == null) {
                throw new ArgumentException($"Could not find Comics Service IP in container {ContainerName} with blob name {ComicsServiceURLBlobName}");
            }

			configuration.ComicsServiceURL = "http://" + ip.Trim('"', '\n') + "/";
        }
    }
}