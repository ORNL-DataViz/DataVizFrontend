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
    public DynamicResizer vectorGenerator;

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public Image loadingBar;
    public Text loadingText;
    public Button correctButton;
    public Button incorrectButton;
    public GameObject ScoreAndInstruct;
    public Image incorrectOverlay;
    public ImageDisplay GridViewStepper;
    public GameObject ImageCollection;
    private RawImage[] ImagesContainer;

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

    // = = = = = = = = = = Begin Button Click Functions = = = = = = = = = = = \\
    // The True/False values are derived by hard coding their values into
    // appropriately named functions, which are then attached to the buttons.
    // In the future we could potentially have only one onClick function that
    // pulls the True/False value from an attribute of the button itself. but
    // this seemed simpler and less bug prone in the short term

    public void OnTrueClick()
    {
        /// <summary>
        /// Listens for clicks on ButtonCorrect.button. onClick, logs currrent
        /// time and "true" boolean value to the currently displayed image.
        /// </summary>
        /// <remarks>
        /// if-else loop is a patch to step the list forward, because
        /// LinkedList.StoreUserInput utilizes the LDP node, which isn't
        /// generated until we've made our initial decision
        /// </remarks>
        ///
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            rndImageList.currentPosition++;
            stepFoward();
            //TODO: Ensure that the first selection is being properly stored
            rndImageList.StoreUserInput(true, timeOfDecision);
            float progress = rndImageList.currentPosition / rndImageList.len;
            loadingText.text = $"{((int)Math.Round(progress*100)).ToString()}%";
            loadingBar.fillAmount = progress;

    }

    public void OnFalseClick()
    {
        /// <summary>
        /// Listens for clicks on ButtonIncorrect.button. onClick, logs currrent
        /// time and "false" boolean value to the currently displayed image.
        /// </summary>
        /// <remarks>
        /// if-else loop is a patch to step the list forward, because
        /// LinkedList.StoreUserInput utilizes the LDP node, which isn't
        /// generated until we've made our initial decision
        /// </remarks>

        String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
        rndImageList.currentPosition++;
        stepFoward();
        //TODO: Pretty sure this is not tracking properly
        rndImageList.StoreUserInput(false, timeOfDecision);
        float progress = (float)rndImageList.currentPosition / (float)rndImageList.len;
        loadingText.text = $"{((int)Math.Round(progress * 100)).ToString()}%";
        loadingBar.fillAmount = progress;
    }

    public void OnUndoClick()
    {
        /// <summary>
        /// Listens for clicks on LDP_Button.button. onClick, logs currrent
        /// time and "undo" string value to the currently displayed image.
        /// </summary>

        if (rndImageList.LDP != null)
        {
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            rndImageList.currentPosition--;
            rndImageList.StoreUserUndo(timeOfDecision);
            rndImageList.CDP = rndImageList.LDP;
            rndImageList.LDP = null;
            float progress = (float)rndImageList.currentPosition / (float)rndImageList.len;
            loadingText.text = $"{((int)Math.Round(progress * 100)).ToString()}%";
            loadingBar.fillAmount = progress;
        }
    }

    public void OnGridClick()
    {
      Debug.Log("Clicked Me!");
      GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
      Debug.Log(selectedButton);

      Image incorrectOverlay = selectedButton.GetComponent<Image>();
      incorrectOverlay.color = new Color((float)1, (float)1, (float)1, (float)0.75);

      selectedButton.transform.parent.GetComponentInChildren<RawImage>().tag = "false";
    }

    public void OnCompleteClick()
    {
      if (rndImageList.REN.next != null)
      {
        int rawImageArrayIterator = 0;
        String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
        MyComponent.Node tempNode = rndImageList.CDP;
        while(tempNode != rndImageList.REN.next)
        {
            if ( ImagesContainer[rawImageArrayIterator].tag == "false" ){
              tempNode.userCorrect = (tempNode.taskCorrect == false);
              tempNode.userDecisionPoints[timeOfDecision] = "false";
            } else {
              tempNode.userCorrect = (tempNode.taskCorrect == true);
              tempNode.userDecisionPoints[timeOfDecision] = "true";
            }
            rawImageArrayIterator++;
            tempNode = tempNode.next;
        }
        for(int i = 0; i < 10; i++){
          rndImageList.CDP = rndImageList.CDP.next;
          rndImageList.REN = rndImageList.REN.next;
        }
        GridViewStepper.StepGridViewFoward();
      } else {
        int rawImageArrayIterator = 0;
        String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
        MyComponent.Node tempNode = rndImageList.CDP;
        while(tempNode != rndImageList.REN.next)
        {
            if ( ImagesContainer[rawImageArrayIterator].tag == "false" ){
              tempNode.userCorrect = (tempNode.taskCorrect == false);
              tempNode.userDecisionPoints[timeOfDecision] = "false";
            } else {
              tempNode.userCorrect = (tempNode.taskCorrect == true);
              tempNode.userDecisionPoints[timeOfDecision] = "true";
            }
            rawImageArrayIterator++;
            tempNode = tempNode.next;
        }
        StartCoroutine(ScoreAndInstruct.GetComponent<ScoreAndInstruct>().ScoreAndSend());
      }
    }
    // = = = = = = = = = = End Button Click Functions = = = = = = = = = = = = \\

    // = = = = = = = = = Begin List Traversal Functions = = = = = = = = = = = \\

    public void stepFoward()
    {
        if ( rndImageList.LDP != null)
        {
            if ( rndImageList.PEN != null)
            {
                MyComponent.Node tempNode = rndImageList.LDP;
                rndImageList.LDP = rndImageList.CDP;
                rndImageList.CDP = rndImageList.CDP.next;
                tempNode.unloadTexture();
                rndImageList.REN = rndImageList.REN.next;
                StartCoroutine(rndImageList.PEN.loadTexture());
                rndImageList.PEN = rndImageList.PEN.next;
            }
            else
            {
                if (rndImageList.REN != null )
                {
                    MyComponent.Node tempNode = rndImageList.LDP;
                    rndImageList.LDP = rndImageList.CDP;
                    rndImageList.CDP = rndImageList.CDP.next;
                    rndImageList.REN = rndImageList.REN.next;
                    tempNode.unloadTexture();
                }
                else
                {
                    if ( rndImageList.CDP.next != null )
                    {
                        rndImageList.LDP = rndImageList.CDP;
                        rndImageList.CDP = rndImageList.CDP.next;

                    }
                    else
                    {
                        correctButton.GetComponent<Button>().enabled = false;
                        incorrectButton.GetComponent<Button>().enabled = false;
                        StartCoroutine(ScoreAndInstruct.GetComponent<ScoreAndInstruct>().ScoreAndSend());
                    }
                }
            }

        }
        else
        {
            rndImageList.LDP = rndImageList.CDP;
            rndImageList.CDP = rndImageList.CDP.next;
            rndImageList.REN = rndImageList.REN.next;
            StartCoroutine(rndImageList.PEN.loadTexture());
            rndImageList.PEN = rndImageList.PEN.next;
        }

    }
}
