using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class PhotoToTexturePipeline : MonoBehaviour
{
    public RawImage TestImage;

    // Arrays to sotre textures and byte arrays
    List<byte[]> imageByteArrays = new List<byte[]>();
    List<MyComponent.ImageTask> processedImageTasks = new List<MyComponent.ImageTask>();

    // Job Size Variables - Change to reflect the value JobNum/5
    int taskSize = 10;

    // Global Random Object
    System.Random indexPicker = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        Stopwatch WPstop = new Stopwatch();
        WPstop.Start();
        LoadTextures(imageByteArrays);
        ImageCreationManager();
        WPstop.Stop();
        TimeSpan ts = WPstop.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        UnityEngine.Debug.Log($"Whole Program Runtime: {elapsedTime}");
        TestImage.texture = processedImageTasks[0].taskImage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadTextures(List<byte[]> workingArray)
    {
        /// <summary>
        /// Iterates over all images in a directory and converts them to textures
        /// </summary>
        /// <param name="workingArray">The container for our textures</param>
        /// <remarks>
        /// Currently optimized to work with local storage, can be gutted and 
        /// rewritted to work with delivery over air
        /// </remarks>

        Stopwatch LTstop = new Stopwatch();
        LTstop.Start();
        // Variable declarations (Tempo paths, future version would use WebRequests)
        string dummyPhotoPath = Environment.CurrentDirectory + "/Assets/DummyPhotos";
        string[] dummyPhotoPathArray = Directory.GetFiles(dummyPhotoPath);
        String metaComparison = ".meta";
        foreach (string path in dummyPhotoPathArray)
        {

            if (!path.EndsWith(metaComparison))
            {
                byte[] currentPhoto = File.ReadAllBytes(path);
                workingArray.Add(currentPhoto);
            }

        }
        LTstop.Stop();
        TimeSpan ts = LTstop.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        UnityEngine.Debug.Log($"Load Textures Runtime: {elapsedTime}");
      

    }

    List<List<byte[]>> RandomizedTaskAssignment()
    {
        // DATA LOSS POTENTIAL!! This script uses a direct reference to the
        // original imageByteArrays list. This should be fine. But check here
        // if you're experienceing data loss issues.

        List<List<byte[]>> TaskSets = new List<List<byte[]>>();

        for(int i = 0; i < 5; i++)
        {
            List<byte[]> currentTaskSet = new List<byte[]>();

            for (int j = 0; j <taskSize; j++)
            {
                int newIndex = indexPicker.Next(0, imageByteArrays.Count);
                currentTaskSet.Add(BytePop(imageByteArrays, newIndex));
            }
            TaskSets.Add(currentTaskSet);
        }

        return TaskSets;
    }

    void ImageCreationManager()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        foreach(byte[] unProcessedImage in imageByteArrays)
        {
            Texture2D tempByteToImage = ByteArrayToTexture2D(unProcessedImage);
            processedImageTasks.Add(imageTaskFactory(tempByteToImage));
        }
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        UnityEngine.Debug.Log($"ByteArrayToTexture Runtime: {elapsedTime}");
    }

    Texture2D ByteArrayToTexture2D(byte[] byteArrayToConvert)
    {
        Texture2D imgText = new Texture2D(2,2 );
        imgText.LoadImage(byteArrayToConvert);
        UnityEngine.Debug.Log(imgText.width);
        imgText.Apply();

        return imgText;
    }

    MyComponent.ImageTask imageTaskFactory(Texture2D convertedImage)
    {
        MyComponent.ImageTask currentImageTask = new MyComponent.ImageTask();
        currentImageTask.taskImage = convertedImage;
        currentImageTask.photoID = indexPicker.Next(1000, 10000);

        return currentImageTask;
    }

    byte[] BytePop(List<byte[]> imageArray, int index)
    {
        byte[] desiredArray = imageArray[index];
        imageArray.RemoveAt(index);

        return desiredArray;
    }

    public List<MyComponent.ImageTask> GetImageTextures()
    {
        return processedImageTasks;
    }
}
