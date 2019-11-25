using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplay : MonoBehaviour
{
    public RawImage CDP_RawImage;
    public RawImage LDP_RawImage;
    public GameObject linkedListContainer;
    private MyComponent.LinkedList rndImageList;

    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
        //GetComponentInParent<ExperimentLinkedList>().photoProgressionOrder;
        //
    }

    // Update is called once per frame
    void Update()
    {
        CDP_RawImage.rectTransform.sizeDelta = rndImageList.CDP.nodeImage.taskDimensions;
        CDP_RawImage.texture = rndImageList.CDP.nodeImage.taskImage;
        try
        {
            LDP_RawImage.color = new Color((float)0.5,(float)0.5,(float)0.5,(float)0.75);
            LDP_RawImage.texture = rndImageList.LDP.nodeImage.taskImage;
        } catch {
            LDP_RawImage.color = Color.clear;
            LDP_RawImage.texture = null;
        }
    }

    void newPhoto()
    {
        try
        {

        }
        catch
        {
            
        }
    }
}
