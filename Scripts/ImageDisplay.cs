using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplay : MonoBehaviour
{
    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public RawImage CDP_RawImage;
    public RawImage LDP_RawImage;
    public GameObject linkedListContainer;

    // = = = = = = Button Image Attachment Points = = = = = = \\
    public GameObject ButtonContainer;
    RawImage[] taggedImages;

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private MyComponent.LinkedList rndImageList;
    private bool GridView;

    private void Awake()
    {
        taggedImages = ButtonContainer.GetComponentsInChildren<RawImage>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Attach rndImageList to static pointer
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
        GridView = GridViewIdentifier.GridView;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks for project progression before each frame, and updates the
        // RawImage containers to reflect user decisions
        if (rndImageList.CDP != null)
        {
            CDP_RawImage.rectTransform.sizeDelta = rndImageList.CDP.taskDimensions;
            CDP_RawImage.texture = rndImageList.CDP.taskImage;
            try
            {
                LDP_RawImage.color = new Color((float)0.5, (float)0.5, (float)0.5, (float)0.75);
                LDP_RawImage.texture = rndImageList.LDP.taskImage;
            }
            catch
            {
                LDP_RawImage.color = Color.clear;
                LDP_RawImage.texture = null;
            }
        }

    }


    // Public Exposed Functions

    public void StepGridViewFoward() {
      GridViewImageGridUpdate();
    }


    // Actual Manipulation Functions

    private void GridViewImageGridUpdate()
    {
        MyComponent.Node tempNode = rndImageList.CDP;
        int buttonListIterator = 0;

        while (tempNode != rndImageList.REN.next)
        {
            taggedImages[buttonListIterator].texture = tempNode.taskImage;
            taggedImages[buttonListIterator].tag = "true";
            taggedImages[buttonListIterator].transform.parent.GetComponentInChildren<Image>().color = new Color((float)1, (float)1, (float)1, (float)0);
            buttonListIterator++;
            tempNode = tempNode.next;
        }
    }

}
