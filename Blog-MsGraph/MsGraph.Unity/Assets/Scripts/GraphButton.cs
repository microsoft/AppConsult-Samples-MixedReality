using HoloToolkit.Unity.InputModule;
using MsGraph.Library;
using System.Linq;

public class GraphButton : BaseButton
{
    private IMsGraphService graphService;

	protected override void Start () {
        base.Start();
		graphService = new MsGraphService();
	}

    public override async void OnInputClicked(InputClickedEventData eventData)
    {
        updateTextContent("graph event received");
        var drives = await graphService.GetDrivesAsync(updateTextContent);
        updateTextContent($"{drives?.Count()} drives found");
        foreach (var drive in drives)
        {
            var item = await graphService.GetDriveItemsAsync(drive.Id, updateTextContent);

        }
        
    }
}
