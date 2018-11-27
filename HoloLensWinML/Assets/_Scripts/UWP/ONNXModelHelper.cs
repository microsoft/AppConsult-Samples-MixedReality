#if UNITY_WSA && !UNITY_EDITOR
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;

public class ONNXModelHelper
{
    private ONNXModel Model = null;
    private string ModelFilename = "ONNXModel.onnx";
    private Stopwatch TimeRecorder = new Stopwatch();
    private IUnityScanScene UnityApp;

    public ONNXModelHelper()
    {
        UnityApp = null;
    }

    public ONNXModelHelper(IUnityScanScene unityApp)
    {
        UnityApp = unityApp;
    }

    public async Task LoadModelAsync()
    {
        ModifyText($"Loading {ModelFilename}... Patience");

        try
        {
            TimeRecorder = Stopwatch.StartNew();

            var modelFile = await StorageFile.GetFileFromApplicationUriAsync(
                new Uri($"ms-appx:///Data/StreamingAssets/{ModelFilename}"));
            Model = await ONNXModel.CreateOnnxModel(modelFile);

            TimeRecorder.Stop();
            ModifyText($"Loaded {ModelFilename}: Elapsed time: {TimeRecorder.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            ModifyText($"error: {ex.Message}");
            Model = null;
        }
    }

    public async Task EvaluateVideoFrameAsync(VideoFrame frame)
    {
        if (frame != null)
        {
            try
            {
                TimeRecorder.Restart();
                ONNXModelInput inputData = new ONNXModelInput();
                inputData.Data = frame;
                var output = await Model.EvaluateAsync(inputData).ConfigureAwait(false);

                var product = output.ClassLabel.GetAsVectorView()[0];
                var loss = output.Loss[0][product];
                TimeRecorder.Stop();

                var lossStr = string.Join(product, " " + (loss * 100.0f).ToString("#0.00") + "%");
                string message = $"({DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second})" +
                    $" Evaluation took {TimeRecorder.ElapsedMilliseconds}ms\n";

                string prediction = $"Prediction: {product} {lossStr}";
                if (loss > 0.5f)
                {
                    message += prediction;
                }

                message = message.Replace("\\n", "\n");

                ModifyText(message);
            }
            catch (Exception ex)
            {
                var err_message = $"error: {ex.Message}";
                ModifyText(err_message);
            }
        }
    }


    private void ModifyText(string text)
    {
        System.Diagnostics.Debug.WriteLine(text);
        if (UnityApp != null)
        {
            UnityApp.ModifyOutputText(text);
        }
    }
}
#endif