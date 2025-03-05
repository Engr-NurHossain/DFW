using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using HS.API.Models;
using HS.Facade;
using HS.Framework;

namespace HS.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var UserName = context.UserName;
            HSMainApiFacade hsMainApi = new HSMainApiFacade();
            var CompanyConnection = hsMainApi.GetCompanyConnectionByUserName(UserName);
            string MasterConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString;
            HSApiFacade hsapiFacd = new HSApiFacade(CompanyConnection.ConnectionString, MasterConnectionString);
            var Password = MD5Encryption.GetMD5HashData(context.Password);
            var UserDetails = hsapiFacd.GetUserLoginByUserPass(UserName, Password, CompanyConnection.MasterPassword, CompanyConnection.CompanyId);
            if (UserDetails != null)
            {
                ClaimsIdentity oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

                identity.AddClaim(new Claim("username", UserDetails.UserName));
                identity.AddClaim(new Claim("userid", UserDetails.UserId.ToString()));
                identity.AddClaim(new Claim("connectionstring", CompanyConnection.ConnectionString));
                identity.AddClaim(new Claim("CompanyId", CompanyConnection.CompanyId.ToString()));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
            }
            return;
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            //if (context.TokenIssued)
            //{
            //    var accessExpiration = DateTimeOffset.Now.AddSeconds(60);
            //    context.Properties.ExpiresUtc = accessExpiration;
            //}
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}