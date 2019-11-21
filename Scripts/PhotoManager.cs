using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private MyComponent.LinkedList rndImageList = new MyComponent.LinkedList();

    // Start is called before the first frame update
    void Start()
    {
        MyComponent.LinkedList rndImageList = GameObject.FindGameObjectWithTag("LinkedList").GetComponent<ExperimentLinkedList>().photoProgressionOrder;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // = = = = = = = = = Begin Photo Managment Functions == = = = = = = = = = \\

    private MyComponent.ImageTask ListPop(int index, List<MyComponent.ImageTask> prePopList)
    {
        MyComponent.ImageTask temp = prePopList[index];
        prePopList.RemoveAt(index);

        return temp;
    }

    private IEnumerator ListInitialization(List<MyComponent.ImageTask> orderedImages)
    {
        // Variable Toolbox
        List<MyComponent.ImageTask> workingList = orderedImages;
        System.Random rnd = new System.Random();
        rndImageList = new MyComponent.LinkedList();
        int imageCount = orderedImages.Count;
        int loadingIncrement = 0;

        // Randomization Loop
        for (int i = 0; i < imageCount; i++)
        {
            rndImageList.Add(ListPop(rnd.Next(0, workingList.Count), workingList));
            loadingIncrement++;
        }

        // First 5 Image Generation
        MyComponent.Node currentNode = rndImageList.getHead();

        for (int i = 0; i < 5; i++)
        {
            currentNode.nodeImage.LoadTexture();

            if (i == 0)
            {
                rndImageList.CDP = currentNode;
            }

            if (i == 5)
            {
                rndImageList.REN = currentNode;
                currentNode = currentNode.getNext();
                currentNode = currentNode.getNext();
                rndImageList.PEN = currentNode;
            }

            currentNode = currentNode.getNext();
            loadingIncrement++;
        }

        yield return null;
    }

    // = = = = = = = = = = End Photo Managment Functions = = = = = = = = = =  \\

    // = = = = = = = = = = Begin Button Click Functions = = = = = = = = = = = \\

    public void OnTrueClick()
    {
        String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
        Debug.Log("true + " + timeOfDecision);
    }

    public void OnFalseClick()
    {
        String timeOfDecision = DateTime.Now.ToString("yyyy'-'mm'-'dd'-'HH':'mm':'ss':'fff");
        Debug.Log("false + " + timeOfDecision);
    }
}
