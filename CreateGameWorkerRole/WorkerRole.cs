using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.Drawing;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Drawing.Imaging;

namespace CreateGameWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("CreateGameWorkerRole is running");

            try
            {
                //this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("CreateGameWorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("CreateGameWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("CreateGameWorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine("Album_WorkerRole entry point called", "Information");

            // initialize the account information 
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            // retrieve a reference to the messages queue 
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("photoresize");
            // retrieve messages and write them to the development fabric log 
            while (true)
            {
                if (queue.Exists())
                {
                    var msg = queue.GetMessage();

                    if (msg != null)
                    {
                        Trace.TraceInformation(string.Format("Message '{0}' processed.", msg.AsString));

                        Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(Callback);
                        //get photo as stream
                        MemoryStream memStream = new MemoryStream();
                        var blob = this.GetContainer("photo").GetBlockBlobReference(msg.AsString);
                        blob.DownloadToStream(memStream);

                        //create a bitmap from photo
                        Bitmap myBitmap = new Bitmap(memStream);
                        //create thumbnail
                        Image myThumbnail = myBitmap.GetThumbnailImage(
                        40, 40, myCallback, IntPtr.Zero);


                        var ms = new MemoryStream();
                        myThumbnail.Save(ms, ImageFormat.Jpeg);

                        // If you're going to read from the stream, you may need to reset the position to the start
                        ms.Position = 0;

                        SaveThumb(msg.AsString, ms);

                        queue.DeleteMessage(msg);
                    }
                }
                else
                    Thread.Sleep(1000);
            }
        }

        private CloudBlobContainer GetContainer(string type)
        {
            // Get a handle on account, create a blob service client and get container proxy

            var account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            var client = account.CreateCloudBlobClient();
            CloudBlobContainer container;

            if (type == "photo")
                container = client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-photo");
            else
                container = client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName") + "-thumb");
            container.CreateIfNotExists();
            return container;
        }

        private void SaveThumb(string name, Stream fiileStream)
        {
            try
            {
                // Create a blob in container and upload image bytes to it
                var blob = this.GetContainer("thumb").GetBlockBlobReference(name);
                // blob.Properties.ContentType = contentType;

                blob.UploadFromStream(fiileStream);
            }
            catch (Exception ex)
            {

            }


        }

        private bool Callback()
        {
            return false;
        }
    }
}
