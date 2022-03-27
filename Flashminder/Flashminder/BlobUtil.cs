using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flashminder
{
    public class BloblUtil
    {
        public static BlobServiceClient GetBlobClient()
        {
            string connectionString = Environment.GetEnvironmentVariable("APPSETTING_AZURE_STORAGE_CONNECTION_STRING");
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            return blobServiceClient;
        }

        public static BlobContainerClient GetBlobContainer()
        {
            return GetBlobClient().GetBlobContainerClient("images");
        }
    }
}