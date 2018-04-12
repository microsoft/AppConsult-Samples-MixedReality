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
        public async Task<IEnumerable<DriveItem>> GetDriveItemsAsync(string driveId, Action<string> error)
        {
            error("running editor only");
            return new List<DriveItem>();
        }

        public async Task<IEnumerable<Drive>> GetDrivesAsync(Action<string> error)
        {
            return new List<Drive>();
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