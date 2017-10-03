using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using KitchenResponsibleService.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using KitchenResponsibleService.Model;

namespace KitchenResponsibleService.Db
{
    public class BlobStorage : IStorage
    {
        const string EmployeeExtension = ".txt";
        const string ResponsiblesId = "responsibles.txt";

        readonly char[] employeeExtension;

        readonly CloudBlobClient blobClient;

        readonly CloudBlobContainer kitchenContainer;
        readonly CloudBlobContainer employeesContainer;

        public BlobStorage(IReadOnlyAppConfiguration configuration)
        {
            employeeExtension = EmployeeExtension.ToCharArray();

            var storageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);

            blobClient = storageAccount.CreateCloudBlobClient();

            kitchenContainer = blobClient.GetContainerReference("kitchen");
            kitchenContainer.CreateIfNotExistsAsync().GetAwaiter();

            employeesContainer = blobClient.GetContainerReference("employees");
            employeesContainer.CreateIfNotExistsAsync().GetAwaiter();
        }

        public async Task<List<ResponsibleForWeek>> GetWeeksAndResponsibles() {
			var token = new BlobContinuationToken();
			var blobs = await kitchenContainer.ListBlobsSegmentedAsync(token);
			var kitchenResponsibles = (CloudBlockBlob)blobs.Results.Single();
			using (var memoryStream = new MemoryStream())
			{
				await kitchenResponsibles.DownloadToStreamAsync(memoryStream);
				var blobContent = Encoding.UTF8.GetString(memoryStream.ToArray());
                return ResponsibleForWeek.Parse(blobContent);
			}
        }

        public async Task Save(List<ResponsibleForWeek> weeksAndResponsibles) {
            var weeksAndResponsiblesString = ResponsibleForWeek.Parse(weeksAndResponsibles);
			var blobRef = kitchenContainer.GetBlockBlobReference(ResponsiblesId);
			await blobRef.UploadTextAsync(weeksAndResponsiblesString);
		}

        public async Task<Employee[]> GetEmployees() {
			var token = new BlobContinuationToken();
			var blobs = await employeesContainer.ListBlobsSegmentedAsync(token);
            var employees = new List<Employee>();
            foreach (var blob in blobs.Results.Cast<CloudBlockBlob>())
            {
                var slackUserId = blob.Name.TrimEnd(employeeExtension);
                var name = "";
                using (var memoryStream = new MemoryStream())
                {
                	await blob.DownloadToStreamAsync(memoryStream);
                	name = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                
                employees.Add(new Employee(slackUserId, name));
            }

            return employees.ToArray();
        }

        public async Task AddNewEmployee(Employee employee) {
			var token = new BlobContinuationToken();
            var blobs = await employeesContainer.ListBlobsSegmentedAsync(token);
            var uploadNewEmployee = true;
            foreach (var blob in blobs.Results.Cast<CloudBlockBlob>())
            {
                if (blob.Name.StartsWith(employee.SlackUserId, StringComparison.Ordinal)) {
                    uploadNewEmployee = false;
                    break;
                }
            }

            if (uploadNewEmployee) {
				var blobRef = employeesContainer.GetBlockBlobReference(employee.SlackUserId + EmployeeExtension);
				await blobRef.UploadTextAsync(employee.Name);    
            }
        }

        public async Task RemoveEmployee(string employeeId) {
            var blobRef = employeesContainer.GetBlockBlobReference(employeeId + EmployeeExtension);
            await blobRef.DeleteIfExistsAsync();
        }
    }
}
