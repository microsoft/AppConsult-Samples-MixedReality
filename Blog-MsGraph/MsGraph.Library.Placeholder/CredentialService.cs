using System;

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
        public CredentialResult GetCredentialsFromLocker(Action<string> error)
        {
            error("running editor only");
            return new CredentialResult {UserId = "marc.plogas@microsoft.com", Token = "abc12345"};
        }

        public CredentialResult WriteCredentialsToLocker(string userId, string token, Action<string> error)
        {
            error("running editor only");
            return new CredentialResult {UserId = userId, Token = token};
        }

        public bool DeleteCredentialsFromLocker(Action<string> error)
        {
            error("running editor only");
            return true;
        }
    }

    public class CredentialResult
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
