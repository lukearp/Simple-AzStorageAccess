# What is this?
> A simple web app that authenticates an AAD user, and then allows downloads from Blob Storage Accounts that the user has been granted Azure RBAC access to.  

# What Roles does the user need?
> Storage Blob Data Reader to the Blob Storage Account or Container the user is needing to access.

# How does it work?
>  The user will browse to the application passing GET variables in the URL:
> * storageAccountName
> * containerName
> * path
> * handler
>
> The Handler varible routes the request to the OnGetBlob Page Method.
> 
> Example URL: https://www.mysite.com?handler=Blob&storageAccountName=lukedevhosting&containerName=rbac&path=childPath/GeneralQuestions-Page1.pdf

# How do I set it up?
> You will need global admin within an Azure Active Directory.  You will need to register an application with the following parameters:
> * Redirect URIs configured to return to your Web App /signin-oidc Ex: https://mysit.com/signin-oidc.  
> * Enable ID Tokents
> * Add Azure Storage/user_impersonation Delegated to API Permissions
> * Generate an App Secret

> Based on the settings within the App Service, fillout the following AppSettings.json paramters:
> * "Domain": "yourdomain.onmicrosoft.com"
> * "TenantId": "Directory ID on the App Registration Overview"
> * "ClientId": "Application ID on the App Registration Overview"
> * "ClientSecret": "Secret You Generated"

# Why did you do this?
> To get a better understanding of using secured downstream APIs.  Also, to allow direct linking to files within a private Blob Storage Container without using SAS tokens.  