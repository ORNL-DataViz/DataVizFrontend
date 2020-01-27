using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class ExperimentInitializer : MonoBehaviour
{

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private bool GridView;

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public GameObject linkedListContainer;
    public GameObject photoToTexturePipelineContainer;
    public RawImage testImage;
    public Text tagText;
    public Canvas loginUI;
    public Canvas trialUI;
    public GameObject loginContainer;
    public GameObject GridCanvas;
    public GameObject SingleCanvas;
    public ImageDisplay ImageUpdate;
    public ServerToLinkedListPipeline stllp;

    // Awake is called when the program starts
    private void Awake()
    {
        // Turns off all views except the Login UI
        GridCanvas.SetActive(false);
        SingleCanvas.SetActive(false);
      
    }

    // Start is called before the first frame update
    void Start()
    { 

        // Connects the GridView variable with the static GridView Boolean
        GridView = GridViewIdentifier.GridView;

    }

    // Update is called once per frame
    void Update()
    {

    }

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + + Begin Initialization Functions  + + + + + + + + + +\\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + Should contain the function necessary to initialize the experiment + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    public IEnumerator LoadTheProgram(string userPin)
    {
        loginContainer.SetActive(false);
        stllp.ServerToImagePipelineV1();
        whichView();
        RequestTagDataV1(userPin);
        loginUI.enabled = false;

        yield return null;
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = = End Initialization Function = = = = = = = = = = = =\\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + + Begin Canvas Assignment Functions + + + + + + + + +\\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + Should contain all functions which activate and populate the canvas +\\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    public void RequestTagDataV1(string userPin)
    {
        // Dummy Process to simulate data until server is set up
        System.Random rnd = new System.Random();
        string[] dummyTags = {
            "Truck", "Academic Gown", "Car", "AR-15", "AK-47",
            "Dog", "Cat", "Person", "Water Gun", "Street Sign"
        };

        int tagDecider = (rnd.Next(0,10000) * rnd.Next(0, 10) * rnd.Next(0, 10)) % 10;
        tagText.text = dummyTags[tagDecider];
    }

    public void RequestTagDataV2()
    {
        tagText.text = stllp.GetTag();
    }

    public void whichView()
    {
        if (GridView)
        {
            Debug.Log("The program read GridView==True");
            GridCanvas.SetActive(true);
            ImageUpdate.UpdateGridView();
        }
        else
        {
            Debug.Log("The program read GridView==False");
            SingleCanvas.SetActive(true);
            ImageUpdate.UpdateSingleView();
        }
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = End Canvas Assignment Function  = = = = = = = = = = =\\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
}

// ========================================================================== //
// ============================ Code Graveyard ============================== //
// =================== Here lies the remains of functions past ============== //
// ========================================================================== //

//void LoadByteArraysIntoList()
//{
//    /// <summary>
//    /// Iterates over all images in a directory and converts them to textures
//    /// </summary>
//    /// <param name="workingArray">The container for our textures</param>
//    /// <remarks>
//    /// Currently built to work with local storage, can be gutted and
//    /// rewritted to work with delivery over air
//    /// </remarks>

//    // Variable declarations (Tempo paths, future version would use WebRequests)
//    string dummyPhotoPath = Environment.CurrentDirectory + "/Assets/DummyPhotos";
//    string[] dummyPhotoPathArray = Directory.GetFiles(dummyPhotoPath);
//    String metaComparison = ".meta";
//    System.Random rnd = new System.Random();

//    foreach (string path in dummyPhotoPathArray)
//    {
//        if (!path.EndsWith(metaComparison))
//        {
//            int booleanDecider = rnd.Next(0, 2);
//            bool dummyTaskCorrect;
//            if (booleanDecider % 2 == 1)
//            {
//                dummyTaskCorrect = true;
//            }
//            else
//            {
//                dummyTaskCorrect = false;
//            }
//            byte[] tempPhoto = File.ReadAllBytes(path);
//            MyComponent.Node tempImageTask = new MyComponent.Node(null, tempPhoto, dummyTaskCorrect);
//            imageTaskArr.Add(tempImageTask);
//        }
//    }


//}
//private MyComponent.Node ListPop(int index, List<MyComponent.Node> prePopList)
//{
//    /// <summary>
//    /// Given an index, remove and return the value at said index
//    /// </summary>
//    /// <param name="index">int index of targeted node</param>
//    /// <param name="prePopList">list to pop from</param>
//    /// <return>ImageTask object at the designated index</return>

//    MyComponent.Node temp = prePopList[index];
//    prePopList.RemoveAt(index);

//    return temp;
//}

//private IEnumerator ListInitialization(List<MyComponent.Node> orderedImages)
//{
//    /// <summary>
//    /// Begins by randomly popping ImageTasks from the passed list, in order
//    /// to generate a randomized LinkedList in the static location pointed
//    /// to by rndImageList. Following rndImageList generation the first 6
//    /// nodes have their .LoadTexture() function invoked to prepare for the
//    /// experiment. Within rndImageList: PEN, REN, and CDP nodes are properly
//    /// assigned, and RawImage Frame dimensions are derived for each of the
//    /// generated textures.
//    /// </summary>
//    /// <param name="orderedImages">List of ImageTasks for assignment</param>
//    /// <remarks>
//    /// This function is currently built to initialize the single user
//    /// experiment. For this reason, most of the values are hardcoded
//    /// (i.e. generating the first 6). In the future, this could be
//    /// minorily rewritten to initialize both the single-user experiment
//    /// and the grid experiment
//    /// </remarks>

//    // Variable Toolbox
//    List<MyComponent.Node> workingList = orderedImages;
//    System.Random rnd = new System.Random();
//    int imageCount = orderedImages.Count;
//    rndImageList.len = orderedImages.Count;

//    // Randomization Loop - A fail safe in case the task order is not
//    // delivered pre-randomized
//    if (!GridView)
//    {
//        for (int i = 0; i < imageCount; i++)
//        {
//            rndImageList.Add(ListPop(rnd.Next(0, workingList.Count), workingList));
//            listLenCounter++;
//        }
//        rndImageList.len = listLenCounter;

//        // First 5 Image Generation
//        MyComponent.Node currentNode = rndImageList.getHead();

//        for (int i = 0; i < 6; i++)
//        {
//            currentNode.forceLoadTexture();
//            //Vector2 origDimensions = new Vector2(
//            //    currentNode.taskImage.width,
//            //    currentNode.taskImage.height
//            //    );
//            currentNode.GenerateResizedRawImageDimensions();

//            if (i == 0)
//            {
//                rndImageList.CDP = currentNode;
//            }

//            if (i == 4)
//            {
//                rndImageList.REN = currentNode;
//                currentNode = currentNode.getNext();
//                currentNode = currentNode.getNext();
//                rndImageList.PEN = currentNode;
//                currentNode = rndImageList.REN;
//            }

//            currentNode = currentNode.getNext();
//        }
//    }
//    else
//    {
//        for (int i = 0; i < imageCount; i++)
//        {
//            rndImageList.Add(ListPop(rnd.Next(0, workingList.Count), workingList));
//            listLenCounter++;
//        }
//        rndImageList.len = listLenCounter;

//        // Load entire board
//        MyComponent.Node currentNode = rndImageList.getHead();

//        for (int i = 0; i < rndImageList.len; i++)
//        {
//            currentNode.forceLoadTexture();
//            currentNode.GenerateResizedRawImageDimensions();

//            if (i == 0)
//            {
//                rndImageList.CDP = currentNode;
//            }

//            if (i == 9)
//            {
//                rndImageList.REN = currentNode;
//            }

//            currentNode = currentNode.getNext();
//        }
//    }