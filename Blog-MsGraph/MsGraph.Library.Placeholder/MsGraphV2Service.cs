using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MsGraph.Library
{
    public class MsGraphV2Service : IMsGraphService
    {
        public async Task<IEnumerable<DriveItem>> GetDriveItemsAsync(string driveId, Action<string> error)
        {
            error("running editor only");

            return new List<DriveItem>();
        }

        public async Task<IEnumerable<Drive>> GetDrivesAsync(Action<string> error)
        {
            error("running editor only");

            return new List<Drive>();
        }
    }


}
