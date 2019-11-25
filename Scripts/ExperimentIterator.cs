using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentIterator : MonoBehaviour
{
    // Script Scope Variables
    private MyComponent.LinkedList rndImageList;
    public DynamicResizer vectorGenerator;
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

    public void OnTrueClick()
    {
        if( rndImageList.LDP != null)
        {
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            rndImageList.currentPosition++;
            StartCoroutine(LoadTextureAndGenerateVector(rndImageList.PEN));
            rndImageList.stepForward();
            //TODO: Pretty sure this is not tracking properly
            rndImageList.StoreUserInput(true, timeOfDecision);
            float progress = (float) rndImageList.currentPosition / (float) rndImageList.len;
            loadingBar.fillAmount = progress;
            loadingText.text = $"{(progress * 100).ToString("F2")}%";
        }
        else
        {
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
            loadingText.text = $"{(progress * 100).ToString("F2")}%";
        }
    }

    public void OnFalseClick()
    {
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
            loadingText.text = $"{(progress * 100).ToString("F2")}%";
        }
        else
        {
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
            loadingText.text = $"{(progress * 100).ToString("F2")}%";
        }
    }

    public void OnUndoClick()
    {
        if (rndImageList.LDP != null)
        {
            String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
            rndImageList.currentPosition--;
            rndImageList.StoreUserUndo(timeOfDecision);
            rndImageList.CDP = rndImageList.LDP;
            rndImageList.LDP = null;
            float progress = (float)rndImageList.currentPosition / (float)rndImageList.len;
            loadingBar.fillAmount = progress;
            loadingText.text = $"{(progress * 100).ToString("F2")}%";
        }
    }
    // = = = = = = = = = = End Button Click Functions == = = = = = = = = = =  \\

    public IEnumerator LoadTextureAndGenerateVector(MyComponent.Node nodeToOperateOn)
    {
        yield return StartCoroutine(rndImageList.PEN.nodeImage.loadTexture());
        Vector2 origDimensions = new Vector2(
                nodeToOperateOn.nodeImage.taskImage.width,
                nodeToOperateOn.nodeImage.taskImage.height
                );
        nodeToOperateOn.nodeImage.taskDimensions = vectorGenerator.GenerateResizedRawImageDimensions(origDimensions);
    }
}
