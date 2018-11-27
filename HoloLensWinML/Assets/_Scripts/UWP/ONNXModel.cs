#if UNITY_WSA && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Media;
using Windows.Storage;

public sealed class ONNXModelInput
{
    public VideoFrame Data { get; set; }
}

public sealed class ONNXModelOutput
{
    public TensorString ClassLabel = TensorString.Create(new long[] { 1, 1 });
    public IList<IDictionary<string, float>> Loss = new List<IDictionary<string, float>>();
}

/// <summary>
/// These classes ONNXModel, ONNXModelInput, ONNXModelOutput never change
/// We just load a different custom model
/// 
/// In order to evalutate a model:
/// 1. Call the static method ONNXModel.CreateOnnxModel(<<MyModelStorageFile>>)
/// 2. Call EvaluateAsync method on the created Model before
/// </summary>
public sealed class ONNXModel
{
    private LearningModel _learningModel = null;
    private LearningModelSession _session;

    public static async Task<ONNXModel> CreateOnnxModel(StorageFile file)
    {
        LearningModel learningModel = null;

        try
        {
            learningModel = await LearningModel.LoadFromStorageFileAsync(file);
        }
        catch (Exception e)
        {
            var exceptionStr = e.ToString();
            Debug.WriteLine(exceptionStr);
            throw e;
        }
        return new ONNXModel()
        {
            _learningModel = learningModel,
            _session = new LearningModelSession(learningModel)
        };
    }

    public async Task<ONNXModelOutput> EvaluateAsync(ONNXModelInput input)
    {
        var output = new ONNXModelOutput();
        var binding = new LearningModelBinding(_session);
        binding.Bind("data", input.Data);
        binding.Bind("classLabel", output.ClassLabel);
        binding.Bind("loss", output.Loss);
        LearningModelEvaluationResult evalResult = await _session.EvaluateAsync(binding, "0");

        return output;
    }
}
#endif
