# HoloLens Windows MachineLearning

The way to train your model in the Cloud and run it locally on the HoloLens!

All the details, code extracts, steps by steps are written in the article linked here https://aka.ms/HoloLensWinML

## HoloLensWinMLmin

The minimal sample code. The objectives are:
- Capture frames from the HoloLens camera
- Load the MachineLearning model
- Execute the model and get the predictions from each captured frame

> **Note:** Windows version 1809 i.e. build > 17738 is needed to be able to run the Windows.AI.MachineLearning APIs

## Versions used
- Windows version 1809 for the PC and HoloLens
- Visual Studio 2017 version 15.9.0
- Windows SDK version 10.0.17763.0
- Unity 2017.4.13f1

> **Note:** Keep the Unity folder/project as close as possible to the root of the disk like C:_Dev\MyApp in order to prevent Unity or Visual studio loading issues with very long paths

