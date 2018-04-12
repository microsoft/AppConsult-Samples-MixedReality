using System;
using System.Threading.Tasks;

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

        public void Init(string appId)
        {
        }

        public async Task SignInAsync(Action<string> error)
        {
            error("running editor only");
        }

        public void SignOut(Action<string> error)
        {
            error("running editor only");
        }
    }


}