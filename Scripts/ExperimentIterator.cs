using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExperimentIterator : MonoBehaviour
{
    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private MyComponent.LinkedList rndImageList;
    private Image incorrectOverlay;

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public Image loadingBar;
    public Text loadingText;
    public Button correctButton;
    public Button incorrectButton;
    public GameObject ImageCollection;
    private RawImage[] ImagesContainer;
    public Button gridViewUndo;
    public GameObject singlePhotoUndo;

    // = = = = = = = = = External Script Attachment Points  = = = = = = = = = \\
    public ScoreAndInstruct trialScorer;
    public DynamicResizer vectorGenerator;

    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
        ImagesContainer = ImageCollection.GetComponentsInChildren<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + + Begin List Traversal Functions  + + + + + + + + + +\\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + Should contain the function necessary to initialize the experiment + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    public void StepSinglePhotoFoward()
    {
        /// <summary>
        /// Steps the LinkedList forward, while programatically loading and
        /// unloading images off of the GPU. Specifically for the single
        /// photo instance
        /// </summary>

        // enables the singlePhotoUndo once we have a photo capable of being
        // revisited
        if (!singlePhotoUndo.active)
        {
            singlePhotoUndo.active = true;
        }

        // Increments the rndImageList current position for the progress bar
        rndImageList.currentPosition++;

        // Checks to see if we are at the beginning of the experiment
        if (rndImageList.LDP != null)
        {
            // While we are more than 7 nodes away from the end of the list
            if (rndImageList.PEN != null)
            {
                // Saves the LDP for unloading
                MyComponent.Node tempNode = rndImageList.LDP;

                // Steps the list forward
                rndImageList.LDP = rndImageList.CDP;
                rndImageList.CDP = rndImageList.CDP.next;

                // Unloads the previous LDP's texture once it's no longer a candidate for display
                tempNode.unloadTexture();

                // Steps REN and PEN forward, begins to load the PEN to screen
                rndImageList.REN = rndImageList.REN.next;
                StartCoroutine(rndImageList.PEN.loadTexture());
                rndImageList.PEN = rndImageList.PEN.next;
            }
            else
            {
                // While we are more than 5 nodes away from the end of the list
                if (rndImageList.REN != null)
                {

                    // Saves the LDP for unloading
                    MyComponent.Node tempNode = rndImageList.LDP;
                    rndImageList.LDP = rndImageList.CDP;
                    rndImageList.CDP = rndImageList.CDP.next;
                    rndImageList.REN = rndImageList.REN.next;

                    // Unloads the previous LDP's texture once it's no longer a candidate for display
                    tempNode.unloadTexture();
                }
                else
                {
                    // While there is still at least one more image to view
                    if (rndImageList.CDP.next != null)
                    {

                        MyComponent.Node tempNode = rndImageList.LDP;

                        // steps CDP and LDP forward
                        rndImageList.LDP = rndImageList.CDP;
                        rndImageList.CDP = rndImageList.CDP.next;

                        tempNode.unloadTexture();
                    }
                    else
                    {
                        // Disconects the correct/incorrect buttons and initiates
                        // end of trial cleanup function
                        correctButton.GetComponent<Button>().enabled = false;
                        incorrectButton.GetComponent<Button>().enabled = false;
                        StartCoroutine(trialScorer.ScoreAndSend());
                    }
                }
            }

        }
        else
        {

            // Initializes the LDP and begins the list traversal process
            rndImageList.LDP = rndImageList.CDP;
            rndImageList.CDP = rndImageList.CDP.next;
            rndImageList.REN = rndImageList.REN.next;
            StartCoroutine(rndImageList.PEN.loadTexture());
            rndImageList.PEN = rndImageList.PEN.next;
        }

    }

    public void StepSinglePhotoBackwards()
    {

        /// <summary>
        /// Steps the LinkedList backwards and disables the undo button on screen
        /// </summary>

        // Disable Undo Button to prevent more than 1 consecutive undo actions
        singlePhotoUndo.active = false;
        rndImageList.currentPosition--;

        rndImageList.CDP = rndImageList.LDP;
    }

    public void StepGridViewForward()
    {
        /// <summary>
        /// Steps the LinkedList forward, and saves the state of the current
        /// CDP and LDP in the REN and PEN nodes for easy of step back. Unloads
        /// previous deck
        /// </summary>

        // Activate the GridView Undo button once we have a section capable of
        // being revisted
        gridViewUndo.enabled = true;

        if (rndImageList.LDP != null)
        {
            // Grabs current LDP before being updates
            MyComponent.Node unloadStartingPoint = rndImageList.LDP;
            StartCoroutine(unloadOldGridView(unloadStartingPoint));
        }
        

        // Save the location of the last CDP in the LDP, and the last REN in the PEN
        rndImageList.LDP = rndImageList.CDP;
        rndImageList.PEN = rndImageList.REN;

        if (rndImageList.LDP != null)
        {
            // unloads the old LDP from the GPU
            
        }
        

        // If there are more tasks to complete, step CDP and REN 10 nodes
        // forward, else, score the trial
        if (rndImageList.REN.next != null)
        {
            for (int i = 0; i < 10; i++)
            {
                rndImageList.currentPosition++;
                rndImageList.CDP = rndImageList.CDP.next;
                rndImageList.REN = rndImageList.REN.next;
            }
        }
        else
        {
            StartCoroutine(trialScorer.ScoreAndSend());
        }

    }

    public void StepGridViewBackwards()
    {
        /// <summary>
        /// Steps the LinkedList backwards and disables access to the undo button.
        /// </summary>

        // Disable the undo to prevent more than 1 consecutive undo actions
        gridViewUndo.enabled = false;

        rndImageList.currentPosition -= 10;
        // reset the CDP/REN to the prior views CDP and REN (saved LDP and PEN)
        rndImageList.CDP = rndImageList.LDP;
        rndImageList.REN = rndImageList.PEN;

    }

    public IEnumerator unloadOldGridView(MyComponent.Node oldLDP)
    {
        /// <summary>
        /// Iterates through the old grid section, unloading their textures from
        /// the GPU
        /// </summary>
        ///
        yield return new WaitForSeconds(5);

        for (int i = 0; i < 10; i++)
        {
            oldLDP.unloadTexture();
            oldLDP = oldLDP.next;
        }
        yield return null;
    }


    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = = End List Traversal Functions = = = = = = = = = = = \\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + + Begin List Based Update Functions  + + + + + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // Should contain all functions which utilize the list for screen updates \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    public void ProgressUpdate()
    {

        /// <summary>
        /// Calculates the trial completion status as a floating point value.
        /// Updates the loading bar and loading text to reflect that value.
        /// </summary>

        float progress = rndImageList.currentPosition / rndImageList.len;
        loadingText.text = $"{((int)Math.Round(progress * 100)).ToString()}%";
        loadingBar.fillAmount = progress;
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = = End List Based Update Functions = = = = = = = = = =\\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
}



// ========================================================================== //
// ============================ Code Graveyard ============================== //
// =================== Here lies the remains of functions past ============== //
// ========================================================================== //

// = = = = = = = = = = Begin Button Click Functions = = = = = = = = = = = \\
// The True/False values are derived by hard coding their values into
// appropriately named functions, which are then attached to the buttons.
// In the future we could potentially have only one onClick function that
// pulls the True/False value from an attribute of the button itself. but
// this seemed simpler and less bug prone in the short term

//public void OnTrueClick()
//{
//    /// <summary>
//    /// Listens for clicks on ButtonCorrect.button. onClick, logs currrent
//    /// time and "true" boolean value to the currently displayed image.
//    /// </summary>
//    /// <remarks>
//    /// if-else loop is a patch to step the list forward, because
//    /// LinkedList.StoreUserInput utilizes the LDP node, which isn't
//    /// generated until we've made our initial decision
//    /// </remarks>
//    ///
//        String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
//        rndImageList.currentPosition++;
//        stepFoward();
//        //TODO: Ensure that the first selection is being properly stored
//        rndImageList.StoreUserInput(true, timeOfDecision);
//        float progress = rndImageList.currentPosition / rndImageList.len;
//        loadingText.text = $"{((int)Math.Round(progress*100)).ToString()}%";
//        loadingBar.fillAmount = progress;

//}

//public void OnFalseClick()
//{
//    /// <summary>
//    /// Listens for clicks on ButtonIncorrect.button. onClick, logs currrent
//    /// time and "false" boolean value to the currently displayed image.
//    /// </summary>
//    /// <remarks>
//    /// if-else loop is a patch to step the list forward, because
//    /// LinkedList.StoreUserInput utilizes the LDP node, which isn't
//    /// generated until we've made our initial decision
//    /// </remarks>

//    String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
//    rndImageList.currentPosition++;
//    stepFoward();
//    //TODO: Pretty sure this is not tracking properly
//    rndImageList.StoreUserInput(false, timeOfDecision);
//    float progress = (float)rndImageList.currentPosition / (float)rndImageList.len;
//    loadingText.text = $"{((int)Math.Round(progress * 100)).ToString()}%";
//    loadingBar.fillAmount = progress;
//}

//public void OnUndoClick()
//{
//    /// <summary>
//    /// Listens for clicks on LDP_Button.button. onClick, logs currrent
//    /// time and "undo" string value to the currently displayed image.
//    /// </summary>

//    if (rndImageList.LDP != null)
//    {
//        String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
//        rndImageList.currentPosition--;
//        rndImageList.StoreUserUndo(timeOfDecision);
//        rndImageList.CDP = rndImageList.LDP;
//        rndImageList.LDP = null;
//        float progress = (float)rndImageList.currentPosition / (float)rndImageList.len;
//        loadingText.text = $"{((int)Math.Round(progress * 100)).ToString()}%";
//        loadingBar.fillAmount = progress;
//    }
//}

//public void OnGridClick()
//{
//  Debug.Log("Clicked Me!");
//  GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
//  Debug.Log(selectedButton);

//  Image incorrectOverlay = selectedButton.GetComponent<Image>();
//  incorrectOverlay.color = new Color((float)1, (float)1, (float)1, (float)0.75);

//  selectedButton.transform.parent.GetComponentInChildren<RawImage>().tag = "false";
//}

//public void OnCompleteClick()
//{
//  if (rndImageList.REN.next != null)
//  {
//    int rawImageArrayIterator = 0;
//    String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
//    MyComponent.Node tempNode = rndImageList.CDP;
//    while(tempNode != rndImageList.REN.next)
//    {
//        if ( ImagesContainer[rawImageArrayIterator].tag == "false" ){
//          tempNode.userCorrect = (tempNode.taskCorrect == false);
//          tempNode.userDecisionPoints[timeOfDecision] = "false";
//        } else {
//          tempNode.userCorrect = (tempNode.taskCorrect == true);
//          tempNode.userDecisionPoints[timeOfDecision] = "true";
//        }
//        rawImageArrayIterator++;
//        tempNode = tempNode.next;
//    }
//    for(int i = 0; i < 10; i++){
//      rndImageList.CDP = rndImageList.CDP.next;
//      rndImageList.REN = rndImageList.REN.next;
//    }
//    GridViewStepper.StepGridViewFoward();
//  } else {
//    int rawImageArrayIterator = 0;
//    String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
//    MyComponent.Node tempNode = rndImageList.CDP;
//    while(tempNode != rndImageList.REN.next)
//    {
//        if ( ImagesContainer[rawImageArrayIterator].tag == "false" ){
//          tempNode.userCorrect = (tempNode.taskCorrect == false);
//          tempNode.userDecisionPoints[timeOfDecision] = "false";
//        } else {
//          tempNode.userCorrect = (tempNode.taskCorrect == true);
//          tempNode.userDecisionPoints[timeOfDecision] = "true";
//        }
//        rawImageArrayIterator++;
//        tempNode = tempNode.next;
//    }
//    StartCoroutine(ScoreAndInstruct.GetComponent<ScoreAndInstruct>().ScoreAndSend());
//  }
//}
// = = = = = = = = = = End Button Click Functions = = = = = = = = = = = = \\
