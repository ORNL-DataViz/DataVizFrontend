using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class ExperimentInitializer : MonoBehaviour
{

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private MyComponent.LinkedList rndImageList;
    private List<MyComponent.ImageTask> imageTaskArr = new List<MyComponent.ImageTask>();

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public GameObject linkedListContainer;
    public GameObject photoToTexturePipelineContainer;
    public RawImage testImage;
    public DynamicResizer vectorGenerator;


    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
        LoadByteArraysIntoList();
        StartCoroutine(ListInitialization(imageTaskArr));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // = = = = = = = = = Begin Photo Management Functions = = = = = = = = = = \\

    private MyComponent.ImageTask ListPop(int index, List<MyComponent.ImageTask> prePopList)
    {
        /// <summary>
        /// Given an index, remove and return the value at said index
        /// </summary>
        /// <param name="index">int index of targeted node</param>
        /// <param name="prePopList">list to pop from</param>
        /// <return>ImageTask object at the designated index</return>

        MyComponent.ImageTask temp = prePopList[index];
        prePopList.RemoveAt(index);

        return temp;
    }

    private IEnumerator ListInitialization(List<MyComponent.ImageTask> orderedImages)
    {
        /// <summary>
        /// Begins by randomly popping ImageTasks from the passed list, in order
        /// to generate a randomized LinkedList in the static location pointed
        /// to by rndImageList. Following rndImageList generation the first 6
        /// nodes have their .LoadTexture() function invoked to prepare for the 
        /// experiment. Within rndImageList: PEN, REN, and CDP nodes are properly
        /// assigned, and RawImage Frame dimensions are derived for each of the
        /// generated textures.
        /// </summary>
        /// <param name="orderedImages">List of ImageTasks for assignment</param>
        /// <remarks>
        /// This function is currently built to initialize the single user 
        /// experiment. For this reason, most of the values are hardcoded
        /// (i.e. generating the first 6). In the future, this could be
        /// minorily rewritten to initialize both the single-user experiment
        /// and the grid experiment
        /// </remarks>

        // Variable Toolbox
        List<MyComponent.ImageTask> workingList = orderedImages;
        System.Random rnd = new System.Random();
        int imageCount = orderedImages.Count;
        rndImageList.len = orderedImages.Count;
        int loadingIncrement = 0;

        // Randomization Loop - A fail safe in case the task order is not
        // delivered pre-randomized
        for (int i = 0; i < imageCount; i++)
        {
            rndImageList.Add(ListPop(rnd.Next(0, workingList.Count), workingList));
            loadingIncrement++;
        }
        rndImageList.len = loadingIncrement;

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
        yield return null;
    }

    // = = = = = = = = = = End Photo Management Functions = = = = = = = = = = \\

    // = = = = = = = = = = Begin Task Generation Functions == = = = = = = = = \\
    void LoadByteArraysIntoList()
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
