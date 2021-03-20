using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Core;
using WebAppOpenIDConnectDotNet;
using System.Text;
using System.IO;

namespace Simple_AzStorageAccess.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly ILogger<IndexModel> _logger;
        static readonly string[] scopesToAccessDownstreamApi = new string[] { "https://storage.azure.com/user_impersonation" };

        public IndexModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        public void OnGet()
        {
            string storageaccount = "https://lukedevhosting.blob.core.windows.net/rbac";
            Uri blobUri = new Uri(storageaccount + "/test.txt");            
            string accessToken = _tokenAcquisition.GetAccessTokenForUserAsync(scopesToAccessDownstreamApi).Result;
            TokenCredential token = new TokenAcquisitionTokenCredential(_tokenAcquisition);
            BlobClient blobClient = new BlobClient(blobUri, token);
            string blobContents = "Blob created by Azure AD authenticated user.";
            byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                blobClient.Upload(stream);
            }
        }
    }
}
