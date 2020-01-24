using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndInstruct : MonoBehaviour
{

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private MyComponent.LinkedList rndImageList;

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public Text trialCompleteText;
    public Canvas completeCanvas;
    public Canvas binaryCanvas;


    public void Awake()
    {
        completeCanvas.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ScoreAndSend()
    {
        /*
         * Steps to enact this function:
         * 1. Run rndImageList.ScoreLinkedList() to quickly score the trial
         * 2. Send call to server to determine the users next task
         * 3. Set trialCompleteMessage.text to reflect user's score and
         * provide instructions as to what to do next
         * 4. Pass rndImageList to Blakes JSON parser
         * 5. Send returned JSON array to the server for storing
         */

        completeCanvas.enabled = true;
        binaryCanvas.enabled = false;
        
        string nextInstructions = "Please move to the tablet station for your" +
            "next trial";
        string userScore = ((int)Math.Round(rndImageList.scoreLinkedList() * 100)).ToString();
        string trialCompleteMessage = $"The previous trial is now complete. " +
            $"You correctly labeled {userScore}% of the images shown to you. " +
            $"{nextInstructions}";

        trialCompleteText.text = trialCompleteMessage;

        yield return null;
    }
}
