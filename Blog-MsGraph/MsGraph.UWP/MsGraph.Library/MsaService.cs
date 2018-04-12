using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace MsGraph.Library
{
    public interface IMsaService
    {
        void Init(string appId);
        Task SignInAsync(Action<string> error);
        void SignOut(Action<string> error);
    }

    public class MsaService : IMsaService
    {
        private static readonly IEnumerable<string> Scopes = new List<string> { "User.Read", "Files.ReadWrite.All" };
        private PublicClientApplication client;
        private ICredentialService credentials;

        public void Init(string appId)
        {
            if(string.IsNullOrWhiteSpace(appId)) throw new ArgumentException();

            //if only Unity would support serious injection...
            this.client = new PublicClientApplication(appId);
            this.credentials = new CredentialService();
        }

        public async Task SignInAsync(Action<string> error)
        {
            var authResult = await AcquireTokenAsync(error);
            if (authResult != null)
            {
                var c = this.credentials.GetCredentialsFromLocker(error);
                if(c == null)
                {
                    credentials.WriteCredentialsToLocker(authResult.User.Name, authResult.AccessToken, error);
                }
                else
                {
                    error("Credentials already found in locker.");
                }
            }
            else
            {
                error("Failed to aquire token.");
            }
        }

        public void SignOut(Action<string> error)
        {
            var c = this.credentials.GetCredentialsFromLocker(error);
            if (c == null) return;

            var user = client.GetUser(c.UserId);
            if (user == null) return;

            client.Remove(user);
            this.credentials.DeleteCredentialsFromLocker(error);
        }

        private async Task<AuthenticationResult> AcquireTokenAsync(Action<string> error)
        {
            try
            {
                var user = GetCurrentUser(error);
                return await client.AcquireTokenSilentAsync(Scopes, user).ConfigureAwait(false);
            }
            catch (MsalUiRequiredException)
            {
                //give it another try, this time with a UI
                try
                {
                    return await client.AcquireTokenAsync(Scopes).ConfigureAwait(false);
                }
                catch (MsalException msaEx)
                {
                    error($"Failed to acquire token: {msaEx.Message}");
                }
                catch (Exception e)
                {
                    //user pressed cancel, server died etc
                    error($"Failed to acquire token: {e.Message}");
                }
            }
            catch (Exception e)
            {
                error($"Failed to acquire token silently: {e}");
            }

            return null;
        }

        private IUser GetCurrentUser(Action<string> error)
        {
            var c = credentials.GetCredentialsFromLocker(error);
            IUser user = null;
            if (c != null)
            {
                user = client.GetUser(c.UserId);
            }

            //fallback, needed?
            return user ?? (client.Users.FirstOrDefault());
        }
    }


}