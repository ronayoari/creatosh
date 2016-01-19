using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentWebRole
{
    public class ContainerBlobHandler
    {
        public void EnsureContainerExists(CloudBlobContainer container)
        {
            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
        }

        public CloudBlobContainer GetContainer(string containerName)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            return container;
        }

        public void UploadBlobIntoContainer(CloudBlockBlob blockBlob, CloudBlobContainer container, string categoty, string template, string user, string gameName)
        {
            // Create or overwrite the blockBlob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(String.Format(@"{0}\{1}\{2}\{3}\myfile", categoty, template, user, gameName)))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }
    }
}