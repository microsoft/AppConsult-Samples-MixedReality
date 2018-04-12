using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace MsGraph.Library
{
    public class MsGraphV2Service : IMsGraphService
    {
        private const string GraphBaseUri = "https://graph.microsoft.com/v1.0";

        private GraphServiceClient graphClient = null;
        private readonly ICredentialService credentials;

        public MsGraphV2Service()
        {
            credentials = new CredentialService();
        }

        public async Task<IEnumerable<DriveItem>> GetDriveItemsAsync(string driveId, Action<string> error)
        {
            if (graphClient == null)
            {
                error("Service not initialized! Initializing now.");
                InitializeGraphClient(error);
            }

            var result = new List<DriveItem>();
            try
            {
                IDriveItemChildrenCollectionPage driveItems = null;
                if (string.IsNullOrWhiteSpace(driveId))
                {
                    driveItems = await graphClient.Me.Drive.Root.Children.Request().GetAsync();
                }
                else
                {
                    driveItems = await graphClient.Me.Drives[driveId].Root.Children.Request().GetAsync();
                    if (driveItems == null)
                    {
                        error("Invalid driveid.!");
                        return null;
                    }
                }
                 
                result.AddRange(driveItems.Select(item => new DriveItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Size = item.Size
                }));
            }
            catch (Exception e)
            {
                error(e.Message);
                return null;
            }

            return result;
        }

        public async Task<IEnumerable<Drive>> GetDrivesAsync(Action<string> error)
        {
            if (graphClient == null)
            {
                error("Service not initialized! Initializing now.");
                InitializeGraphClient(error);
            }

            var result = new List<Drive>();
            try
            {
                var drives = await graphClient.Me.Drives.Request().GetAsync();

                result.AddRange(drives.Select(drive => new Drive
                {
                    Id = drive.Id,
                    DriveType = drive.DriveType,
                    Name = drive.Name
                }));
            }
            catch (Exception e)
            {
                error(e.Message);
            }

            return result;
        }


        private void InitializeGraphClient(Action<string> error)
        {
            if (graphClient != null) return;

            //you probably should lock that here?
            try
            {
                graphClient = new GraphServiceClient(GraphBaseUri, new DelegateAuthenticationProvider(
                    async (message) =>
                    {
                        //to satisfy the async requirement of authentication provider 
                        await Task.Run(() =>
                        {
                            var c = credentials.GetCredentialsFromLocker(error);
                            if (c!=null)
                            {
                                message.Headers.Authorization = new AuthenticationHeaderValue("bearer", c.Token);
                            }
                            else
                            {
                                error("Failed to get credentials from locker.");
                            }
                        });
                    }));
            }
            catch (Exception ex)
            {
                error("Could not create a graph client: " + ex.Message);
            }
        }
    }


}
