using System;
using Windows.Security.Credentials;

namespace MsGraph.Library
{
    public interface ICredentialService
    {
        CredentialResult GetCredentialsFromLocker(Action<string> error);
        CredentialResult WriteCredentialsToLocker(string userId, string token, Action<string> error);
        bool DeleteCredentialsFromLocker(Action<string> error);
    }

    public class CredentialService : ICredentialService
    {
        private const string AppName = "AppConsultSample";
        public CredentialResult GetCredentialsFromLocker(Action<string> error)
        {
            try
            {
                var vault = new PasswordVault();
                var credentialList = vault.FindAllByResource(AppName);
                if (credentialList?.Count == 1)
                {
                    var c = credentialList[0];
                    c.RetrievePassword();
                    return new CredentialResult
                    {
                        UserId = c.UserName,
                        Token = c.Password
                    };
                }

                error($"Unexpected amount ({credentialList?.Count}) of credentials found");
            }
            catch (Exception e)
            {
                error($"Exception thrown {e.Message}");
            }

            return null;
        }

        public CredentialResult WriteCredentialsToLocker(string userId, string token, Action<string> error)
        {
            try
            {
                var vault = new PasswordVault();
                vault.Add(new PasswordCredential(AppName, userId, token));
                return new CredentialResult
                {
                    UserId = userId,
                    Token = token
                };
            }
            catch (Exception e)
            {
                error($"Exception thrown {e.Message}");
            }

            return null;
        }

        public bool DeleteCredentialsFromLocker(Action<string> error)
        {
            try
            {
                var vault = new PasswordVault();
                var credentialList = vault.FindAllByResource(AppName);
                if (credentialList?.Count == 1)
                {
                    var c = credentialList[0];
                    vault.Remove(c);
                    return true;
                }

                error($"Unexpected amount ({credentialList?.Count}) of credentials found");
            }
            catch (Exception e)
            {
                error($"Exception thrown: {e.Message}");
            }

            return false;
        }
    }

    public class CredentialResult
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
