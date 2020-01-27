using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonListeners : MonoBehaviour
{

    // = = = = = = = = = = = = Pertinent Script Notes = = = = = = = = = = = = \\
    // Unless otherwise specified any references to "true", "false", or
    // "incorrect" found in this script pertain to the user's response to the
    // onscreen prompt. As in the user seeing a photo, looking at the tag, and
    // saying "this image is incorrectly tagged" or "this image is correctly
    // tagged." The majority of all scoring pertaing to whether they correctly
    // made their decision is found in the ImageTask.cs script. 

    // = = = = = = = = = = = Static Undo State Variable = = = = = = = = = = = \\
    public static bool undoState = false;
    public static string undoDateTime = "";

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private MyComponent.LinkedList rndImageList;
    private Image incorrectOverlay;

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public GameObject imageCollection;
    private RawImage[] imagesContainer;

    // = = = = = = = = = External Script Attachment Points  = = = = = = = = = \\
    public ExperimentIterator trialManager;
    public ScoreAndInstruct trialScorer;
    public ImageDisplay ImageUpdate;

    // public DynamicResizer vectorGenerator; DEPRECATE ME ON FINAL LOOP THROUGH


    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
        imagesContainer = imageCollection.GetComponentsInChildren<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + +  Begin Single Photo Button Listeners + + + + + + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + Should contain all listeners related to the single photo view  + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    public void onSinglePhotoBinaryClick()
    {
        /// <summary>
        /// Listen for clicks on the single photo binary decision buttons,
        /// determines which one was clicked based off the UnityEventSystem, and
        /// then scores and iterates the list forward accordingly
        /// </summary>

        // Caputure DateTime First for accuracy
        String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");

        // Determine which button was clicked 
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        bool binaryType = bool.Parse(selectedButton.tag);

        // Steps the list forward and stores user input
        //rndImageList.currentPosition++;
        trialManager.StepSinglePhotoFoward();
        //TODO: Ensure that the last selection is being properly stored
        rndImageList.StoreUserInput(binaryType, timeOfDecision);
        trialManager.ProgressUpdate();
        ImageUpdate.UpdateSingleView();
    }

    public void OnUndoClick()
    {
        /// <summary>
        /// Listens for clicks on LDP_Button.button.onClick, logs currrent
        /// time and "undo" string value to the currently displayed image.
        /// </summary>

        // Redundant safety check, can probably be removed.
        // POTENTIALY DEPRECATE IF STATEMENT
        if (rndImageList.LDP != null)
        {
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            rndImageList.StoreUserUndo(timeOfDecision);
            trialManager.StepSinglePhotoBackwards();
            //rndImageList.LDP = null; DEPRECATE ME ON FINAL LOOP THROUGH
            trialManager.ProgressUpdate();
            ImageUpdate.UpdateSingleView();
        }
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = =  End Single Photo Listeners = = = = = = = = = = = =\\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + Begin GridView Button Listeners + + + + + + + + + + +\\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + Should contain all listeners related to the GridView  + + + +\\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    public void OnGridClick()
    {
        /// <summary>
        /// Listens for a click on each of the gridView images and the
        /// determines which was clicked based on the Unity EventSystem. Then
        /// checks whether the UndoState is currently active
        /// </summary>
        
        if (!undoState)
        {
            // Determine which button we are working with
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

            // Apply the Incorrect overlay to the selected button
            Image incorrectOverlay = selectedButton.GetComponent<Image>();
            incorrectOverlay.color = new Color((float)1, (float)1, (float)1, (float)0.75);

            // Switch the selected items tag to False
            selectedButton.transform.parent.GetComponentInChildren<RawImage>().tag = "false";
        } else
        {
            // Determine which button we are working with
            GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

            // Determine whether the button was previously selected or not
            // All images are by default "true" so returning a "false" tag implies
            // that the user previously selected this image and is reverting their decision
            bool revertedDecision = bool.Parse(
                selectedButton.transform.parent.GetComponentInChildren<RawImage>().tag
                );

            if (!revertedDecision)
            {
                // Switch off the incorrect overlay
                Image incorrectOverlay = selectedButton.GetComponent<Image>();
                incorrectOverlay.color = new Color((float)1, (float)1, (float)1, (float)0.0);

                // Switch tag to true
                selectedButton.transform.parent.GetComponentInChildren<RawImage>().tag = "true";
            } else
            {
                // Switch on the incorrect overlay
                Image incorrectOverlay = selectedButton.GetComponent<Image>();
                incorrectOverlay.color = new Color((float)1, (float)1, (float)1, (float)0.75);

                // Switch tag to false
                selectedButton.transform.parent.GetComponentInChildren<RawImage>().tag = "false";
            }
        }
        
    }

    public void OnCompleteClick()
    {

        /// <summary>
        /// Listens for a click on the complete button within the GridView
        /// interface, and then determines whether we are in an undoState or not.
        /// If not, simply compares the current state of the button against the
        /// imageTask attrubutes of the associated node. If in an undo state,
        /// checks the current state against the previous decision, and scores
        /// appropriately.
        /// </summary>

        if (!undoState)
        {
            if (rndImageList.REN.next != null)
            {
                // Captures the time of decision and intializes traversal pointers
                String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
                int rawImageArrayIterator = 0;
                MyComponent.Node tempNode = rndImageList.CDP;

                // iterates through grid, captures decision, and stores/scores user input
                while (tempNode != rndImageList.REN.next)
                {
                    bool userDecision = bool.Parse(imagesContainer[rawImageArrayIterator].tag);
                    tempNode.StoreUserInput(userDecision, timeOfDecision);
                    rawImageArrayIterator++;
                    tempNode = tempNode.next;
                }

                // Steps the LinkedList forward and then updates grid
                trialManager.StepGridViewForward();
                ImageUpdate.UpdateGridView();
            }
            else
            {
                // Captures the time of decision and intializes traversal pointers
                String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
                int rawImageArrayIterator = 0;
                MyComponent.Node tempNode = rndImageList.CDP;

                // iterates through grid, captures decision, and stores/scores user input
                while (tempNode != rndImageList.REN.next)
                {
                    bool userDecision = bool.Parse(imagesContainer[rawImageArrayIterator].tag);
                    tempNode.StoreUserInput(userDecision, timeOfDecision);
                    rawImageArrayIterator++;
                    tempNode = tempNode.next;
                }

                // Initiates the end of trial cleanup proces
                StartCoroutine(trialScorer.ScoreAndSend());
            }
        } else
        {
            // Captures the time of decision and intializes traversal pointers
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            int rawImageArrayIterator = 0;
            MyComponent.Node tempNode = rndImageList.CDP;

            // iterates through grid, captures decision, and stores/scores user input
            while (tempNode != rndImageList.REN.next)
            {
                // captures the current nodes new decision state
                bool userDecision = bool.Parse(imagesContainer[rawImageArrayIterator].tag);

                // compares this against the correct value from the nodes imageTask
                bool usersCurrentScoredDecisionState = (tempNode.taskCorrect == userDecision);

                // compares the new userScoreDecision state against the users
                // previous decision state to determine whether the user has
                // updated their choice or not.
                if (!usersCurrentScoredDecisionState == tempNode.userCorrect)
                {
                    // if they have, store an "undo" point for later analyisis
                    tempNode.userUndo = true;
                    tempNode.userDecisionPoints.Add(
                        new List<string>(2) { undoDateTime, "undo" }
                        );

                    // then score the decision as normal
                    tempNode.StoreUserInput(userDecision, timeOfDecision);
                } else
                {
                    // score the decision again, to simply append the decision time
                    tempNode.StoreUserInput(userDecision, timeOfDecision);
                }

                rawImageArrayIterator++;
                tempNode = tempNode.next;
            }

            // Switch the undo state back before progressing to next board
            undoState = false;
            trialManager.StepGridViewForward();
            ImageUpdate.UpdateGridView();
            trialManager.ProgressUpdate();

        }
        
    }

    public void OnGridUndoClick()
    {
        /// <summary>
        /// Listens for a click on the undo button within the GridView
        /// interface. Once selected, stores a new DateTime string representing
        /// the beginning of the "undo" state, as well as flipping the undoState
        /// to true. Then steps the grid backwards and updates the screen.
        /// </summary>

        // Generate/Update static undo variables
        undoDateTime = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
        undoState = true;

        trialManager.StepGridViewBackwards();
        ImageUpdate.UpdateGridView();
        trialManager.ProgressUpdate();

    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = =  End Single Photo Listeners = = = = = = = = = = = =\\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + + Begin Helper Related Functions + + + + + + + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + Should contain all functions that assist larger groups + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    public bool getUndoState()
    {
        /// <summary>
        /// Returns the undo state, because c# static abilities is really flakey
        /// for some reason
        /// </summary>
        
        return undoState;
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = = End Helper Related Functions = = = = = = = = = = = \\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
}

// ========================================================================== //
// ============================ Code Graveyard ============================== //
// =================== Here lies the remains of functions past ============== //
// ========================================================================== //
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
//    trialManager.stepFoward();
//    rndImageList.StoreUserInput(false, timeOfDecision);
//    float progress = (float)rndImageList.currentPosition / (float)rndImageList.len;
//    loadingText.text = $"{((int)Math.Round(progress * 100)).ToString()}%";
//    loadingBar.fillAmount = progress;
//}

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

//    // Caputure DateTime
//    String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
//    rndImageList.currentPosition++;
//    trialManager.stepFoward();
//    //TODO: Ensure that the last selection is being properly stored
//    rndImageList.StoreUserInput(true, timeOfDecision);
//    trialManager.progressUpdate();

//}