using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

namespace WebAppOpenIDConnectDotNet
{
    public class TokenAcquisitionTokenCredential : TokenCredential
    {
        private ITokenAcquisition _tokenAcquisition;

        /// <summary>
        /// Constructor from an ITokenAcquisition service.
        /// </summary>
        /// <param name="tokenAcquisition">Token acquisition.</param>
        public TokenAcquisitionTokenCredential(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }

        /// <inheritdoc/>
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            try{
                AuthenticationResult result = _tokenAcquisition.GetAuthenticationResultForUserAsync(requestContext.Scopes)
                    .GetAwaiter()
                    .GetResult();
                return new AccessToken(result.AccessToken, result.ExpiresOn);
            }/*
            catch (MicrosoftIdentityWebChallengeUserException ex)
            {
                string message = ex.Message;
            }
            */
            catch(MicrosoftIdentityWebChallengeUserException ex){
               _tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeaderAsync(ex.Scopes,ex.MsalUiRequiredException).GetAwaiter().GetResult();
            }
            catch (MsalUiRequiredException ex)
            {
                 _tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeaderAsync(new string[] {"user_impersonation"},ex).GetAwaiter().GetResult();
            }
            DateTime now = DateTime.Now;
            return new AccessToken(string.Empty,new DateTimeOffset(now,TimeZoneInfo.Local.GetUtcOffset(now)));
        }

        /// <inheritdoc/>
        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            AuthenticationResult result = await _tokenAcquisition.GetAuthenticationResultForUserAsync(requestContext.Scopes).ConfigureAwait(false);
            return new AccessToken(result.AccessToken, result.ExpiresOn);
        }
    }
}
