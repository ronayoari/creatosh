using System.Net;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace StudentWebRole
{
        using System;
        using System.Collections.Specialized;
        using System.Configuration;
        using System.Globalization;
        using System.Linq;
        using System.Web.UI.WebControls;
        using Microsoft.WindowsAzure.Storage;
        using Microsoft.WindowsAzure.ServiceRuntime;
        using Microsoft.WindowsAzure;
        using Microsoft.WindowsAzure.Storage.Blob;
        using System.IO;
        using System.Collections.Generic;
        using Microsoft.WindowsAzure.Storage.Queue;
        using System.Web;
        using System.Web.UI;


    public partial class MultiChoicePlatform : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.EnsureContainerExists();
                }
                this.RefreshGallery();
            }
            catch (System.Net.WebException we)
            {
                status.Text = "Network error: " + we.Message;
                if (we.Status == System.Net.WebExceptionStatus.ConnectFailure)
                {
                    status.Text += "<br />Please check if the blob service is running at " + ConfigurationManager.AppSettings["storageEndpoint"];
                }
            }
            catch (StorageException se)
            {
                Console.WriteLine("Storage service error: " + se.Message);
            }
        }

        protected void upload_Click(object sender, EventArgs e)
        {
            if (imageFile1.HasFile)
            {
                status.Text = "Inserted [" + imageFile1.FileName + "] - Content Type [" +
                imageFile1.PostedFile.ContentType + "] - Length [" +
                imageFile1.PostedFile.ContentLength + "]";
                string message = "";
                message = this.SaveImage(
                Guid.NewGuid().ToString(),
                "user",
                correctAnswer1.Text,
                wrongAnswer11.Text,
                wrongAnswer21.Text,
                wrongAnswer31.Text,
                imageFile1.PostedFile.ContentType,
                imageFile1.PostedFile.InputStream
                );
                message += ",";
                message += this.SaveImage(
                Guid.NewGuid().ToString(),
                "user",
                correctAnswer2.Text,
                wrongAnswer12.Text,
                wrongAnswer22.Text,
                wrongAnswer32.Text,
                imageFile2.PostedFile.ContentType,
                imageFile2.PostedFile.InputStream
                );
                sendToQueue(message);
                RefreshGallery();
            }
            else
                status.Text = "No image file";
        }

        protected void OnBlobDataBound(object sender, ListViewItemEventArgs e)
        {

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var metadataRepeater = e.Item.FindControl("blobMetadata") as Repeater;
                var blob = ((ListViewDataItem)(e.Item)).DataItem as CloudBlockBlob;
                // If this blob is a snapshot, rename button to "Delete Snapshot"
                if (blob != null)
                {
                    if (metadataRepeater != null)
                    {
                        //bind to metadata
                        metadataRepeater.DataSource = from key in blob.Metadata.Keys
                                                      select new
                                                      {
                                                          Name = key,
                                                          Value = blob.Metadata[key]
                                                      };
                        metadataRepeater.DataBind();
                    }
                }
            }

        }

        protected void OnDeleteImage(object sender, CommandEventArgs e)
        {

            try
            {


                if (e.CommandName == "Delete")
                {
                    var blobUri = (string)e.CommandArgument;
                    var blob = this.GetContainer().GetBlockBlobReference(blobUri);
                    bool result = blob.DeleteIfExists();
                    status.Text = "";
                }
            }
            catch (StorageException se)
            {
                status.Text = "Storage client error: " + se.Message;
            }
            catch (Exception) { }
            RefreshGallery();
        }

        protected void OnCopyImage(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Copy")
            {
                // Prepare an Id for the copied blob
                var newId = Guid.NewGuid();
                // Get source blob
                var blobUri = (string)e.CommandArgument;
                var srcBlob = this.GetContainer().GetBlockBlobReference(blobUri);
                // Create new blob
                var newBlob = this.GetContainer().GetBlockBlobReference(newId.ToString());
                // Copy content from source blob
                newBlob.StartCopyFromBlob(srcBlob.Uri);
                // Explicitly get metadata for new blob
                newBlob.FetchAttributes();
                // Change metadata on the new blob to reflect this is a copy via UI
                newBlob.Metadata["ImageName"] = "Copy of \"" +
                newBlob.Metadata["ImageName"] + "\"";
                newBlob.Metadata["Id"] = newId.ToString();
                newBlob.SetMetadata();
                // Render all blobs
                RefreshGallery();
            }
        }

        protected void OnListItemDeleting(object sender, EventArgs e)
        {
        }

        #region
        private void EnsureContainerExists()
        {
            var container = GetContainer();
            container.CreateIfNotExists();
            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);
        }

        private CloudBlobContainer GetContainer()
        {
            // Get a handle on account, create a blob service client and get container proxy

            var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-photo");
        }


        private void RefreshGallery()
        {
            images.DataSource =
            this.GetContainer().ListBlobs(null, true, BlobListingDetails.All, new BlobRequestOptions(), null);
            images.DataBind();
        }

        private string SaveImage(string id, string user, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3, string contentType, Stream fiileStream)
        {
            // Create a blob in container and upload image bytes to it
            var blob = this.GetContainer().GetBlockBlobReference(correctAnswer);
            blob.Properties.ContentType = contentType;
            // Create some metadata for this image
            blob.Metadata.Add("Id", id);
            blob.Metadata.Add("Template", "MultiChoicePlatform");
            blob.Metadata.Add("Username", String.IsNullOrEmpty(user) ? "unknown" : user);
            blob.Metadata.Add("CorrectAnswer", String.IsNullOrEmpty(correctAnswer) ? "unknown" : correctAnswer);
            blob.Metadata.Add("IncorrectAnswer1", String.IsNullOrEmpty(incorrectAnswer1) ? "unknown" : incorrectAnswer1);
            blob.Metadata.Add("IncorrectAnswer2", String.IsNullOrEmpty(incorrectAnswer2) ? "unknown" : incorrectAnswer2);
            blob.Metadata.Add("IncorrectAnswer3", String.IsNullOrEmpty(incorrectAnswer3) ? "unknown" : incorrectAnswer3);

            blob.UploadFromStream(fiileStream);
            blob.SetMetadata();
            //send to queue 
            return correctAnswer;
        }

        #endregion


        #region QUEUE

        private void sendToQueue(string imageName)
        {
            // initialize the account information 
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            // retrieve a reference to the messages queue 
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("photoresize");

            queue.CreateIfNotExists();

            var msg = new CloudQueueMessage(imageName);
            queue.AddMessage(msg);
            Response.AppendHeader("Refresh", "2");

        }
        #endregion
    }
}