using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MsGraph.Library
{

    public interface IMsGraphService
    {
        Task<IEnumerable<DriveItem>> GetDriveItemsAsync(string driveId, Action<string> error);

        Task<IEnumerable<Drive>> GetDrivesAsync(Action<string> error);
    }

    public class MsGraphService : IMsGraphService
    {
        private const string graphBaseUri = "https://graph.microsoft.com/v1.0/me";
        private const string drivesEndpoint = "/drives";
        private const string drivesListRootEndpoint = "/drives/{0}/root/children";

        private ICredentialService credentials;
        private IJsonService jsonService;

        public MsGraphService()
        {
            // did i already mention HOW much Unity dependency management sucks?
            this.credentials = new CredentialService();
            this.jsonService = new JsonService();
        }

        public async Task<IEnumerable<DriveItem>> GetDriveItemsAsync(string driveId, Action<string> error)
        {
            try
            {
                var json = await GetJsonFromGraphEndpoint($"{graphBaseUri}{string.Format(drivesListRootEndpoint, driveId)}", error);
                error($"JSON returned {json}");

                return jsonService.Deserialize<List<DriveItem>>(json);
            }
            catch (Exception e)
            {
                error($"GetDriveItemsAsync Exception: {e.Message}");
            }

            return null;
        }

        public async Task<IEnumerable<Drive>> GetDrivesAsync(Action<string> error)
        {
            try
            {
                var json = await GetJsonFromGraphEndpoint($"{graphBaseUri}{drivesEndpoint}", error);
                error($"JSON returned {json}");

                return jsonService.Deserialize<List<Drive>>(json);
            }
            catch (Exception e)
            {
                error($"GetDrivesAsync Exception: {e.Message}");
            }

            return null;
        }
        
        private async Task<string> GetJsonFromGraphEndpoint(string endpoint, Action<string> error)
        {
            string result;
            try
            {
                var c = this.credentials.GetCredentialsFromLocker(error);
                if (c != null)
                {
                    using (var httpClient = new HttpClient())
                    {

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", c.Token);
                        var response = await httpClient.GetAsync(endpoint);
                        if (response.IsSuccessStatusCode)
                        {
                            result = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            result = jsonService.Serialize(new {WebError = response.ReasonPhrase});
                        }
                    }
                }
                else
                {
                    result = jsonService.Serialize(new { AuthError = "You  need to log in first!" });
                }
            }
            catch (Exception e)
            {
                result = jsonService.Serialize(new { Exception = e.Message });
            }

            return result;
        }
    }

    public class Root
    {
        public Root Type { get; set; }
    }

    public class Drive
    {
        //see: https://docs.microsoft.com/en-us/onedrive/developer/rest-api/resources/drive

        public string Id { get; set; }
        public string Name { get; set; }
        public string DriveType { get; set; }
    }

    public class DriveItem
    {
        //see: https://docs.microsoft.com/en-us/onedrive/developer/rest-api/resources/driveitem
        public string Id { get; set; }
        public string Name { get; set; }
        public long? Size { get; set; }
    }
}