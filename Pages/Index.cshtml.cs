using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Core;
using WebAppOpenIDConnectDotNet;
using System.Text;
using System.IO;

namespace Simple_AzStorageAccess.Pages
{
    
    [AuthorizeForScopes(Scopes = new string[] {"https://storage.azure.com/.default"})]
    public class IndexModel : PageModel
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        public void OnGet()
        {

        }

        public FileStreamResult OnGetBlob(string storageAccountName, string containerName, string path)
        {
            string storageAccount = "https://" + storageAccountName + ".blob.core.windows.net/" + containerName + "/" + path;
            Stream stream;
            string contentType;
            Uri blobUri = new Uri(storageAccount);
            try
            {
                TokenCredential token = new TokenAcquisitionTokenCredential(_tokenAcquisition);
                BlobClient blobClient = new BlobClient(blobUri, token);

                try
                {
                    contentType = blobClient.GetProperties().Value.ContentType;
                    stream = blobClient.OpenRead();
                }
                catch (Exception e)
                {
                    contentType = "application/json";
                    string exceptionString = e.Message.Replace(Environment.NewLine, " ");
                    string jsonBody = "{\"Error\": \"" + exceptionString + "\"}";
                    stream = new MemoryStream(Encoding.ASCII.GetBytes(jsonBody));
                }
            }
            catch (Exception e)
            {
                contentType = "application/json";
                string exceptionString = e.Message.Replace(Environment.NewLine, " ");
                string jsonBody = "{\"Error\": \"" + exceptionString + "\"}";
                stream = new MemoryStream(Encoding.ASCII.GetBytes(jsonBody));
            }

            return new FileStreamResult(stream, contentType);
        }

        public void OnPost(string body)
        {
            // string storageaccount = "https://lukedevhosting.blob.core.windows.net/rbac";
            // Uri blobUri = new Uri(storageaccount + "/test.txt");            
            // string accessToken = _tokenAcquisition.GetAccessTokenForUserAsync(scopesToAccessDownstreamApi).Result;
            // TokenCredential token = new TokenAcquisitionTokenCredential(_tokenAcquisition);
            // BlobClient blobClient = new BlobClient(blobUri, token);
            // string blobContents = "Blob created by Azure AD authenticated user.";
            // byte[] byteArray = (blobContents);
            // using (MemoryStream stream = new MemoryStream(byteArray))
            // {
            //     blobClient.Upload(stream);
            // }
        }
    }
}
