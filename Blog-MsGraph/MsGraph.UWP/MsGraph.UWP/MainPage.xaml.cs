using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MsGraph.Library;

namespace MsGraph.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IMsaService msaService;
        private readonly ICredentialService credentialService;
        private readonly IMsGraphService graphService;
        private readonly Action<string> displayLog;

        public MainPage()
        {
            this.InitializeComponent();
            credentialService = new CredentialService();

            msaService = new MsaService();
            msaService.Init("your application id");

            //graphService = new MsGraphService();
            graphService = new MsGraphV2Service();

            displayLog = s => { TextBox_Log.Text += Environment.NewLine + s; };

            //starting with a clean slate
            credentialService.DeleteCredentialsFromLocker(displayLog);
            msaService.SignOut(displayLog);
        }

         

        private async void Button_Login_OnClick(object sender, RoutedEventArgs e)
        {
            displayLog("---- Login clicked ----");
            
            await msaService.SignInAsync(displayLog);
            displayLog("finished login flow");
        }

        private async void Button_OneDrive_OnClick(object sender, RoutedEventArgs e)
        {
            displayLog("---- Graph clicked ----");

            var drives = await graphService.GetDrivesAsync(displayLog);

            foreach (var drive in drives)
            {
                displayLog($"drive found: {drive.Name} - {drive.DriveType}");

                var items = await graphService.GetDriveItemsAsync(drive.Id, displayLog);
                if (items == null) continue;

                foreach (var item in items)
                {
                    displayLog($"{item.Id} - {item.Name} - {item.Size}");
                }
            }

            displayLog("finsihed onedrive flow");
        }

        private void Button_Credentials_OnClick(object sender, RoutedEventArgs e)
        {
            displayLog("---- Credentials clicked ----");

            var c = credentialService.GetCredentialsFromLocker(displayLog);
            if (c != null)
            {
                displayLog($"Username: {c.UserId}");
                displayLog($"Token: {c.Token}");
            }
            else
            {
                displayLog("Failed to get credentials from locker.");
            }

            displayLog("finished credentials flow");
        }
    }
}
