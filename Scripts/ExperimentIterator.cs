using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentIterator : MonoBehaviour
{
    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private MyComponent.LinkedList rndImageList;
    public DynamicResizer vectorGenerator;

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public Image loadingBar;
    public Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
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

        if ( rndImageList.LDP != null)
        {
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            rndImageList.currentPosition++;
            StartCoroutine(LoadTextureAndGenerateVector(rndImageList.PEN));
            rndImageList.stepForward();
            //TODO: Ensure that the first selection is being properly stored
            rndImageList.StoreUserInput(true, timeOfDecision);
            float progress = (float) rndImageList.currentPosition / (float) rndImageList.len;
            loadingBar.fillAmount = progress;
        }
        else
        {
            //TODO: Write helper function in ImageTask.LinkedList to cover this
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            rndImageList.currentPosition++;
            StartCoroutine(LoadTextureAndGenerateVector(rndImageList.PEN));
            rndImageList.LDP = rndImageList.CDP;
            rndImageList.CDP = rndImageList.CDP.next;
            rndImageList.REN = rndImageList.REN.next;
            rndImageList.PEN = rndImageList.PEN.next;
            rndImageList.StoreUserInput(true, timeOfDecision);
            float progress = (float)rndImageList.currentPosition / (float)rndImageList.len;
            loadingBar.fillAmount = progress;
        }
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

        if (rndImageList.LDP != null)
        {
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            rndImageList.currentPosition++;
            StartCoroutine(LoadTextureAndGenerateVector(rndImageList.PEN));
            rndImageList.stepForward();
            //TODO: Pretty sure this is not tracking properly
            rndImageList.StoreUserInput(false, timeOfDecision);
            float progress = (float)rndImageList.currentPosition / (float)rndImageList.len;
            loadingBar.fillAmount = progress;
        }
        else
        {
            //TODO: Write helper function in ImageTask.LinkedList to cover this
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            rndImageList.currentPosition++;
            StartCoroutine(LoadTextureAndGenerateVector(rndImageList.PEN));
            rndImageList.LDP = rndImageList.CDP;
            rndImageList.CDP = rndImageList.CDP.next;
            rndImageList.REN = rndImageList.REN.next;
            rndImageList.PEN = rndImageList.PEN.next;
            rndImageList.StoreUserInput(false, timeOfDecision);
            float progress = (float)rndImageList.currentPosition / (float)rndImageList.len;
            loadingBar.fillAmount = progress;
        }
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
            loadingBar.fillAmount = progress;
        }
    }
    // = = = = = = = = = = End Button Click Functions == = = = = = = = = = =  \\

    public IEnumerator LoadTextureAndGenerateVector(MyComponent.Node nodeToOperateOn)
    {
        /// <summary>
        /// Listens for clicks on ButtonCorrect.button. onClick, logs currrent 
        /// time and "false" boolean value to the currently displayed image.
        /// </summary>
        /// <param name="nodeToOperateOn">
        /// The node containing our targeted byte[] to generate a texture from
        /// </param>
        /// <remarks>
        /// Function waits until the texture has ben generated to attempt to 
        /// parse its dimensions for RawImage frame sizing
        /// </remarks>

        yield return StartCoroutine(rndImageList.PEN.nodeImage.loadTexture());
        Vector2 origDimensions = new Vector2(
                nodeToOperateOn.nodeImage.taskImage.width,
                nodeToOperateOn.nodeImage.taskImage.height
                );
        nodeToOperateOn.nodeImage.taskDimensions = vectorGenerator.GenerateResizedRawImageDimensions(origDimensions);
    }
}
