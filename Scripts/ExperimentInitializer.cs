using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class ExperimentInitializer : MonoBehaviour
{

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    public GameObject linkedListContainer;
    public GameObject photoToTexturePipelineContainer;
    public RawImage testImage;
    public DynamicResizer vectorGenerator;
    private MyComponent.LinkedList rndImageList;
    private List<MyComponent.ImageTask> imageTaskArr = new List<MyComponent.ImageTask>();


    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
        LoadTextures();
        StartCoroutine(ListInitialization(imageTaskArr));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // = = = = = = = = = Begin Photo Managment Functions == = = = = = = = = = \\

    private MyComponent.ImageTask ListPop(int index, List<MyComponent.ImageTask> prePopList)
    {
        MyComponent.ImageTask temp = prePopList[index];
        prePopList.RemoveAt(index);

        return temp;
    }

    private IEnumerator ListInitialization(List<MyComponent.ImageTask> orderedImages)
    {
        // Variable Toolbox
        List<MyComponent.ImageTask> workingList = orderedImages;
        System.Random rnd = new System.Random();
        int imageCount = orderedImages.Count;
        int loadingIncrement = 0;

        // Randomization Loop
        for (int i = 0; i < imageCount; i++)
        {
            rndImageList.Add(ListPop(rnd.Next(0, workingList.Count), workingList));
            loadingIncrement++;
        }
        rndImageList.len = loadingIncrement;
        Debug.Log(rndImageList.len);



        // First 5 Image Generation
        MyComponent.Node currentNode = rndImageList.getHead();


        for (int i = 0; i < 6; i++)
        {
            currentNode.nodeImage.forceLoadTexture();
            Vector2 origDimensions = new Vector2(
                currentNode.nodeImage.taskImage.width,
                currentNode.nodeImage.taskImage.height
                );
            currentNode.nodeImage.taskDimensions = vectorGenerator.GenerateResizedRawImageDimensions(origDimensions);

            if (i == 0)
            {
                rndImageList.CDP = currentNode;
            }

            if (i == 4)
            {
                rndImageList.REN = currentNode;
                currentNode = currentNode.getNext();
                currentNode = currentNode.getNext();
                rndImageList.PEN = currentNode;
                currentNode = rndImageList.REN;
            }

            currentNode = currentNode.getNext();
            loadingIncrement++;
        }

        //testImage.texture = rndImageList.getHead().getNodeText();
        yield return null;
    }

    // = = = = = = = = = = End Photo Managment Functions = = = = = = = = = =  \\

    // = = = = = = = = = = Begin Task Generation Functions == = = = = = = = = \\
    void LoadTextures()
    {
        /// <summary>
        /// Iterates over all images in a directory and converts them to textures
        /// </summary>
        /// <param name="workingArray">The container for our textures</param>
        /// <remarks>
        /// Currently built to work with local storage, can be gutted and 
        /// rewritted to work with delivery over air
        /// </remarks>

        // Variable declarations (Tempo paths, future version would use WebRequests)
        string dummyPhotoPath = Environment.CurrentDirectory + "/Assets/DummyPhotos";
        string[] dummyPhotoPathArray = Directory.GetFiles(dummyPhotoPath);
        String metaComparison = ".meta";
        System.Random rnd = new System.Random();

        foreach (string path in dummyPhotoPathArray)
        {
            if (!path.EndsWith(metaComparison))
            {
                byte[] tempPhoto = File.ReadAllBytes(path);
                MyComponent.ImageTask tempImageTask = new MyComponent.ImageTask();
                tempImageTask.serializedPhoto = tempPhoto;
                int booleanDecider = rnd.Next(0, 2);
                if (booleanDecider % 2 == 1) { tempImageTask.taskCorrect = true; } else { tempImageTask.taskCorrect = false; }

                imageTaskArr.Add(tempImageTask);
            }
        }


    }
}
