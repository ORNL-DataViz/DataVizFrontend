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
    private bool GridView;
    private List<MyComponent.Node> imageTaskArr = new List<MyComponent.Node>();
    private float loadingSteps;
    private float loadingProgress = 0;
    private bool startLoading = true;

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public GameObject linkedListContainer;
    public GameObject photoToTexturePipelineContainer;
    public RawImage testImage;
    public DynamicResizer vectorGenerator;
    public Text tagText;
    public Canvas loginUI;
    public Canvas trialUI;
    public Image experimentLoadingBar;
    public Text experimentLoadingText;
    public GameObject loginContainer;
    public GameObject loadingContainer;
    public GameObject GridCanvas;
    public GameObject SingleCanvas;
    public ImageDisplay GridInitializer;

    private void Awake()
    {
        GridCanvas.SetActive(false);
        SingleCanvas.SetActive(false);
        loadingContainer.SetActive(false);
        trialUI.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
        GridView = GridViewIdentifier.GridView;
        Debug.Log(GridView);

    }

    // Update is called once per frame
    void Update()
    {
        if (startLoading && (loadingContainer.activeSelf == true))
        {
            float progress = loadingProgress / loadingSteps;
            experimentLoadingText.text = $"{((int)Math.Round(progress * 100)).ToString()}%";
            experimentLoadingBar.fillAmount = progress;
        }
    }


    // = = = = = = = = = = Begin Tie Together Functions = = = = = = = = = = = \\

    public IEnumerator LoadTheProgram(string userPin)
    {
        loadingContainer.SetActive(true);
        loginContainer.SetActive(false);
        LoadByteArraysIntoList();
        StartCoroutine(ListInitialization(imageTaskArr));
        RequestData(userPin);
        trialUI.enabled = true;
        loginUI.enabled = false;


        yield return null;
    }

    // = = = = = = = = = = = Begin Data Pull Functions == = = = = = = = = = = \\
    public void RequestData(string userPin)
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

    // = = = = = = = = = Begin Photo Management Functions = = = = = = = = = = \\

    private MyComponent.Node ListPop(int index, List<MyComponent.Node> prePopList)
    {
        /// <summary>
        /// Given an index, remove and return the value at said index
        /// </summary>
        /// <param name="index">int index of targeted node</param>
        /// <param name="prePopList">list to pop from</param>
        /// <return>ImageTask object at the designated index</return>

        MyComponent.Node temp = prePopList[index];
        prePopList.RemoveAt(index);

        return temp;
    }

    private IEnumerator ListInitialization(List<MyComponent.Node> orderedImages)
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
        List<MyComponent.Node> workingList = orderedImages;
        System.Random rnd = new System.Random();
        int imageCount = orderedImages.Count;
        rndImageList.len = orderedImages.Count;
        loadingSteps = rndImageList.len + 6;

        // Randomization Loop - A fail safe in case the task order is not
        // delivered pre-randomized
        if (!GridView)
        {
            for (int i = 0; i < imageCount; i++)
            {
                rndImageList.Add(ListPop(rnd.Next(0, workingList.Count), workingList));
                loadingProgress++;
            }
            rndImageList.len = loadingProgress;

            // First 5 Image Generation
            MyComponent.Node currentNode = rndImageList.getHead();

            for (int i = 0; i < 6; i++)
            {
                currentNode.forceLoadTexture();
                //Vector2 origDimensions = new Vector2(
                //    currentNode.taskImage.width,
                //    currentNode.taskImage.height
                //    );
                currentNode.GenerateResizedRawImageDimensions();

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
                loadingProgress++;
            }
        } else
        {
            for (int i = 0; i < imageCount; i++)
            {
                rndImageList.Add(ListPop(rnd.Next(0, workingList.Count), workingList));
                loadingProgress++;
            }
            rndImageList.len = loadingProgress;

            // Load entire board
            MyComponent.Node currentNode = rndImageList.getHead();

            for (int i = 0; i < rndImageList.len; i++)
            {
                currentNode.forceLoadTexture();
                currentNode.GenerateResizedRawImageDimensions();

                if (i == 0)
                {
                    rndImageList.CDP = currentNode;
                }

                if (i == 9)
                {
                    rndImageList.REN = currentNode;
                }

                currentNode = currentNode.getNext();
                loadingProgress++;
            }
        }


        whichView();
        if (GridView){
          GridInitializer.StepGridViewFoward();
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
                int booleanDecider = rnd.Next(0, 2);
                bool dummyTaskCorrect;
                if (booleanDecider % 2 == 1)
                {
                    dummyTaskCorrect = true;
                } else {
                    dummyTaskCorrect = false;
                }
                byte[] tempPhoto = File.ReadAllBytes(path);
                MyComponent.Node tempImageTask = new MyComponent.Node(null, tempPhoto, dummyTaskCorrect);
                imageTaskArr.Add(tempImageTask);
            }
        }


    }

    public void whichView()
    {
        if (GridView)
        {
            Debug.Log("The program read GridView==True");
            GridCanvas.SetActive(true);
        } else
        {
            Debug.Log("The program read GridView==False");
            SingleCanvas.SetActive(true);
        }
    }
}
