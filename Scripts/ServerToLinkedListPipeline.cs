
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Diagnostics;
using UnityEngine.UI;

public class ServerToLinkedListPipeline : MonoBehaviour
{
    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private MyComponent.LinkedList rndImageList;
    private List<MyComponent.Node> serverImageNodes = new List<MyComponent.Node>();
    private string trialTag;
    private string trialID;
    private string userPin;
    private bool gridView;
    private System.Random indexPicker = new System.Random();

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\

    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + +  Begin Pipeline Related Functions  + + + + + + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + Should contain all functions that directly manage the pipeline + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\


    public void ServerToImagePipelineV1()
    {

        /// <summary>
        /// The local ServerToImagePipeline. Utilizes the Dummy Photo library in
        /// unity to simulate photos, and fakes all data related to the image set
        /// using random number generation. To test gridView vs singlePhoto,
        /// adjust the gridView variable below, as well as the one in GridViewImageIdentifier
        /// to be either true or false.
        /// </summary>
        
        gridView = true;

          if ( gridView )
          {
            LoadByteArraysIntoList();
            StartCoroutine(GridViewListInitialization());
          } else {
            LoadByteArraysIntoList();
            StartCoroutine(SinglePhotoListInitialization());
          }
    }

    public void ServerToImagePipelinev2()
    {
        /// <summary>
        /// The production ServerToImagePipeline. Utilizes the JSON parser and
        /// webrequests to pull all pertinent information from the server, as
        /// well as image files. 
        /// </summary>
        
        RequestTrialID(userPin);
        RequestGridView(trialID, userPin);
        string serverPayload = RequestPhotoSet(trialID);
        UnpackServerPayload(serverPayload);

        if ( gridView)
        {
            GridViewListInitialization();
        } else
        {
            SinglePhotoListInitialization();
        }
        
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = =  End pipeline Related Functions  = = = = = = = = = = \\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + + Begin Server Related Functions + + + + + + + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    //  Should contain all functions that directly interface with the server  \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    // Section Note: I'm not sure if the first two functions are necessary, or
    // if they'll be included within another call. We need to get a finalized
    // set of documentation for the API from Sarah. Regardless, wherever those
    // two variables are sourced from, they just need to be assigned the script-
    // scope variables upon assignment - SA 

    public void RequestTrialID( string userPin )
    {
      /// <summary>
      /// Sends a request to the web server containing the UserPin as a Parameter
      /// and receives a trialID in return
      /// </summary>
      /// <remarks>
      /// Unsure if needed. Need API documentation for this. If it exists, make
      /// sure to assign the TrialID to the script-scope variable above
      /// </remarks>

      //          Steps to implement | Delete Comment Once Complete           //
      // If this needs to be included, speak with sarah for specifics
    }

    public void RequestGridView(string trialID, string userPin)
    {
        /// <summary>
        /// Sends a request to the web server containing the UserPin or TrialID
        /// and receives a boolean value indicating whether or not to deploy the
        /// Grid View
        /// </summary>
        /// <remarks>
        /// Unsure if needed. Need API documentation for this. If it exists, make
        /// sure to assign the gridView to the script-scope variable above
        /// </remarks>
    }

    public void SetUserPin( string passedUserPin)
    {
        userPin = passedUserPin;
    }

    public string RequestPhotoSet( string trialID )
    {
      /// <summary>
      /// Sends a request to the web server containing the TrialID as a parameter
      /// Expects a collection of images, packaged as JSON in return
      /// </summary>
      /// <remarks>
      /// I'm not entirely sure, but image files may be encoded as base64. If
      /// that is the case it may be worthwhile to unencode them in place, in
      /// the returned JSON array. Rather then pass buck to the Parser
      ///
      /// A ImageTask --> Node stuffing example is located on:
      /// ServerToLinkedListPipeline.LoadByteArraysIntoList()
      /// </remarks>

      //          Steps to implement | Delete Comment Once Complete           //
      // 1. Generate a UnityWebRequest.Post Object
      // 2. Generate the body in some way (either WWWForm data or a self made
      // JSON Array) and package the TrialID Parameter in it
      // 3. Wait for the response and generate an error loop for the event in
      // in which the request fails
      // 4. Once the JSON is in hand, make any potential adjustments (see
      // reference section in DocString)
      // 5. Pass JSON array to parser function

      String serverPayload = "";
      return serverPayload;

    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = = End Server Related Functions = = = = = = = = = = = \\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\


    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + +  Begin JSON Related Functions  + + + + + + + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + Should contain all functions that interface with the JSON Parser + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    public void UnpackServerPayload( string serverPayload )
    {
      /// <summary>
      /// Utilizing the JSON Parser, this function unpackages the payload
      /// received from the above web request, assigns the tag to the private
      /// variable above, and the packages the MyComponent.Nodes into the
      /// serverImageNodes list
      /// </summary>
      /// <param name="serverPayload"> JSON array from server </param>
      /// <remarks>
      /// </remarks>
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = =  End JSON Related Functions  = = = = = = = = = = = \\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + Begin LinkedList Related Functions + + + + + + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + Should contain all functions that interface with the LinkedList  + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    private IEnumerator SinglePhotoListInitialization()
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
      /// <remarks>
      /// This function is more or less a mirror image of GridViewListInitialization,
      /// with the key difference being the number of textures intially loaded.
      /// In this version, only the first 6 images have their texture loaded,
      /// as the progression speed is suitable for loading on the fly without
      /// frame drops
      /// </remarks>

      // Variable Toolbox
      List<MyComponent.Node> workingList = serverImageNodes;

      int imageCount = serverImageNodes.Count;
      rndImageList.len = serverImageNodes.Count;

      /// Randomly pop images out of the Node List onto the LinkedList
      for (int i = 0; i < imageCount; i++)
      {
          rndImageList.Add(ListPop(indexPicker.Next(0, workingList.Count), workingList));
      }

      // First 5 Image Generation
      MyComponent.Node currentNode = rndImageList.getHead();

      for (int i = 0; i < 6; i++)
      {
          currentNode.forceLoadTexture();
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
        }

        yield return null;
    }

    private IEnumerator GridViewListInitialization()
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
      /// <remarks>
      /// This function generates the textures for all 50 images at once
      /// </remarks>

      // Variable Toolbox
      List<MyComponent.Node> workingList = serverImageNodes;
      int imageCount = serverImageNodes.Count;
      rndImageList.len = serverImageNodes.Count;

      // Begin Randomization
      for (int i = 0; i < imageCount; i++)
      {
          rndImageList.Add(ListPop(indexPicker.Next(0, workingList.Count), workingList));
      }

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
        }

        yield return null;
    }

    // DEPRECATION WARNING: Will be deprecated after server connection is complete
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

        foreach (string path in dummyPhotoPathArray)
        {
            if (!path.EndsWith(metaComparison))
            {
                int booleanDecider = indexPicker.Next(0, 2);
                bool dummyTaskCorrect;
                if (booleanDecider % 2 == 1)
                {
                    dummyTaskCorrect = true;
                } else {
                    dummyTaskCorrect = false;
                }
                byte[] tempPhoto = File.ReadAllBytes(path);

                // ImageTask --> Node stuffing example
                MyComponent.Node tempImageTask = new MyComponent.Node(null, tempPhoto, dummyTaskCorrect);
                serverImageNodes.Add(tempImageTask);
                
            }
        }

    

    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = End LinkedList Related Functions = = = = = = = = = = \\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + + Begin Helper Related Functions + + + + + + + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + Should contain all functions that assist larger groups + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

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

    public string GetTag()
    {
        return trialTag;
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = = End Helper Related Functions = = = = = = = = = = = \\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\


}


// ========================================================================== //
// ============================ Code Graveyard ============================== //
// =================== Here lies the remains of functions past ============== //
// ========================================================================== //
//
// private IEnumerator ListInitialization(List<MyComponent.Node> orderedImages)
// {
//     /// <summary>
//     /// Begins by randomly popping ImageTasks from the passed list, in order
//     /// to generate a randomized LinkedList in the static location pointed
//     /// to by rndImageList. Following rndImageList generation the first 6
//     /// nodes have their .LoadTexture() function invoked to prepare for the
//     /// experiment. Within rndImageList: PEN, REN, and CDP nodes are properly
//     /// assigned, and RawImage Frame dimensions are derived for each of the
//     /// generated textures.
//     /// </summary>
//     /// <param name="orderedImages">List of ImageTasks for assignment</param>
//     /// <remarks>
//     /// This function is currently built to initialize the single user
//     /// experiment. For this reason, most of the values are hardcoded
//     /// (i.e. generating the first 6). In the future, this could be
//     /// minorily rewritten to initialize both the single-user experiment
//     /// and the grid experiment
//     /// </remarks>
//
//     // Variable Toolbox
//     List<MyComponent.Node> workingList = orderedImages;
//     System.Random rnd = new System.Random();
//     int imageCount = orderedImages.Count;
//     rndImageList.len = orderedImages.Count;
//     loadingSteps = rndImageList.len + 6;
//
//     // Randomization Loop - A fail safe in case the task order is not
//     // delivered pre-randomized
//     if (!GridView)
//     {
//         for (int i = 0; i < imageCount; i++)
//         {
//             rndImageList.Add(ListPop(rnd.Next(0, workingList.Count), workingList));
//             loadingProgress++;
//         }
//         rndImageList.len = loadingProgress;
//
//         // First 5 Image Generation
//         MyComponent.Node currentNode = rndImageList.getHead();
//
//         for (int i = 0; i < 6; i++)
//         {
//             currentNode.forceLoadTexture();
//             //Vector2 origDimensions = new Vector2(
//             //    currentNode.taskImage.width,
//             //    currentNode.taskImage.height
//             //    );
//             currentNode.GenerateResizedRawImageDimensions();
//
//             if (i == 0)
//             {
//                 rndImageList.CDP = currentNode;
//             }
//
//             if (i == 4)
//             {
//                 rndImageList.REN = currentNode;
//                 currentNode = currentNode.getNext();
//                 currentNode = currentNode.getNext();
//                 rndImageList.PEN = currentNode;
//                 currentNode = rndImageList.REN;
//             }
//
//             currentNode = currentNode.getNext();
//             loadingProgress++;
//         }
//     } else
//     {
//         for (int i = 0; i < imageCount; i++)
//         {
//             rndImageList.Add(ListPop(rnd.Next(0, workingList.Count), workingList));
//             loadingProgress++;
//         }
//         rndImageList.len = loadingProgress;
//
//         // Load entire board
//         MyComponent.Node currentNode = rndImageList.getHead();
//
//         for (int i = 0; i < rndImageList.len; i++)
//         {
//             currentNode.forceLoadTexture();
//             currentNode.GenerateResizedRawImageDimensions();
//
//             if (i == 0)
//             {
//                 rndImageList.CDP = currentNode;
//             }
//
//             if (i == 9)
//             {
//                 rndImageList.REN = currentNode;
//             }
//
//             currentNode = currentNode.getNext();
//             loadingProgress++;
//         }
//     }
//
//
//     whichView();
//     if (GridView){
//       GridInitializer.StepGridViewFoward();
//     }
//     yield return null;
// }
//
// Texture2D ByteArrayToTexture2D(byte[] byteArrayToConvert)
// {
//     Texture2D imgText = new Texture2D(2,2 );
//     imgText.LoadImage(byteArrayToConvert);
//     UnityEngine.Debug.Log(imgText.width);
//     imgText.Apply();
//
//     return imgText;
// }
//
//
// byte[] BytePop(List<byte[]> imageArray, int index)
// {
//     byte[] desiredArray = imageArray[index];
//     imageArray.RemoveAt(index);
//
//     return desiredArray;
// }
// List<List<byte[]>> RandomizedTaskAssignment()
// {
//     // DATA LOSS POTENTIAL!! This script uses a direct reference to the
//     // original imageByteArrays list. This should be fine. But check here
//     // if you're experienceing data loss issues.
//
//     List<List<byte[]>> TaskSets = new List<List<byte[]>>();
//
//     for(int i = 0; i < 5; i++)
//     {
//         List<byte[]> currentTaskSet = new List<byte[]>();
//
//         for (int j = 0; j <taskSize; j++)
//         {
//             int newIndex = indexPicker.Next(0, imageByteArrays.Count);
//             currentTaskSet.Add(BytePop(imageByteArrays, newIndex));
//         }
//         TaskSets.Add(currentTaskSet);
//     }
//
//     return TaskSets;
// }
// void LoadTextures(List<byte[]> workingArray)
// {
//     /// <summary>
//     /// Iterates over all images in a directory and converts them to textures
//     /// </summary>
//     /// <param name="workingArray">The container for our textures</param>
//     /// <remarks>
//     /// Currently optimized to work with local storage, can be gutted and
//     /// rewritted to work with delivery over air
//     /// </remarks>
//
//     Stopwatch LTstop = new Stopwatch();
//     LTstop.Start();
//     // Variable declarations (Tempo paths, future version would use WebRequests)
//     string dummyPhotoPath = Environment.CurrentDirectory + "/Assets/DummyPhotos";
//     string[] dummyPhotoPathArray = Directory.GetFiles(dummyPhotoPath);
//     String metaComparison = ".meta";
//     foreach (string path in dummyPhotoPathArray)
//     {
//
//         if (!path.EndsWith(metaComparison))
//         {
//             byte[] currentPhoto = File.ReadAllBytes(path);
//             workingArray.Add(currentPhoto);
//         }
//
//     }
//     LTstop.Stop();
//     TimeSpan ts = LTstop.Elapsed;
//     string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
//         ts.Hours, ts.Minutes, ts.Seconds,
//         ts.Milliseconds / 10);
//     UnityEngine.Debug.Log($"Load Textures Runtime: {elapsedTime}");
//
//
// }
