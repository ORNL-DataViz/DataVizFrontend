using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplay : MonoBehaviour
{
    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public RawImage CDP_RawImage;
    public RawImage LDP_RawImage;
    //public GameObject linkedListContainer;

    // = = = = = = Button Image Attachment Points = = = = = = \\
    public GameObject ButtonContainer;
    RawImage[] taggedImages;

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    private MyComponent.LinkedList rndImageList;
    private bool GridView;
    private bool undoState;

    // = = = = = = = = = External Script Attachment Points  = = = = = = = = = \\
    public ButtonListeners buttonManager;

    private void Awake()
    {
        taggedImages = ButtonContainer.GetComponentsInChildren<RawImage>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Attach static pointers to 
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
        GridView = GridViewIdentifier.GridView;
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + + Begin Public Update Functions + + + + + + + + + + +\\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + Should contain all publicly reachable screen refresh functions + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    public void UpdateGridView()
    {
      GridViewImageUpdate();
    }

    public void UpdateSingleView()
    {
        SinglePhotoImageUpdate();
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = = End Public Update Functions = = = = = = = = = = = =\\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\

    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + + + + + + Begin Screen Refresh Functions + + + + + + + + + + \\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\
    // + + + + + Should contain all private screen refresh functions + + + + +\\
    // + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + +\\

    private void GridViewImageUpdate()
    {
        /// <summary>
        /// Determines the undoState, Iterates over the 5x2 rawImage grid, and
        /// appropriately prepares the board for the next user action.
        /// </summary>

        // Assigns tempNode to the current CDP
        MyComponent.Node tempNode = rndImageList.CDP;
        int buttonListIterator = 0;

        undoState = buttonManager.getUndoState();

        // Checks whether we are currently in an Undo State
        if (!undoState)
        {
            // If not in Undo State, wipes the board clean of overlays and
            // sets all of the tags to "true"
            while (tempNode != rndImageList.REN.next)
            {
                taggedImages[buttonListIterator].texture = tempNode.taskImage;
                taggedImages[buttonListIterator].tag = "true";
                taggedImages[buttonListIterator].transform.parent.GetComponentInChildren<Image>().color = new Color((float)1, (float)1, (float)1, (float)0);
                buttonListIterator++;
                tempNode = tempNode.next;
            }
        }
        else
        {
            // if in undo state, sets the board to reflect the previously
            // existing state
            while (tempNode != rndImageList.REN.next)
            {
                if (tempNode.userCorrect)
                {
                    // if the user was correct, than we can use the tempNode.taskCorrect
                    // value as the logical decision point
                    taggedImages[buttonListIterator].texture = tempNode.taskImage;

                    // sets the tag to be equal to the lowercase, string
                    // represenation of the taskCorrect bool value
                    taggedImages[buttonListIterator].tag = tempNode.taskCorrect.ToString().ToLower();

                    // either displays or hides the overlay depending on taskCorrect bool value
                    if (tempNode.taskCorrect)
                    {
                        taggedImages[buttonListIterator].transform.parent.GetComponentInChildren<Image>().color = new Color((float)1, (float)1, (float)1, (float)0);
                    }
                    else
                    {
                        taggedImages[buttonListIterator].transform.parent.GetComponentInChildren<Image>().color = new Color((float)1, (float)1, (float)1, (float)0.75);
                    }
                }
                else
                {
                    // if the user was incorrect, we derive a flipped boolean
                    // value of taskCorrect to determine what they had selected
                    // previously. We then treat that exactly as we treated
                    // the taskCorrect value above, to properly display what
                    // selections the user had made on the previous screen
                    taggedImages[buttonListIterator].texture = tempNode.taskImage;
                    bool flippedValue = !tempNode.taskCorrect;
                    taggedImages[buttonListIterator].tag = flippedValue.ToString().ToLower();
                    if (flippedValue)
                    {
                        taggedImages[buttonListIterator].transform.parent.GetComponentInChildren<Image>().color = new Color((float)1, (float)1, (float)1, (float)0);
                    }
                    else
                    {
                        taggedImages[buttonListIterator].transform.parent.GetComponentInChildren<Image>().color = new Color((float)1, (float)1, (float)1, (float)0.75);
                    }
                }

                buttonListIterator++;
                tempNode = tempNode.next;
            }
        }
        
    }

    private void SinglePhotoImageUpdate()
    {
        /// <summary>
        /// Updates the CDP_RawImage and LDP_RawImage to reflect the current
        /// state of the LinkedList
        /// </summary>

        CDP_RawImage.rectTransform.sizeDelta = rndImageList.CDP.taskDimensions;
        CDP_RawImage.texture = rndImageList.CDP.taskImage;

        if (LDP_RawImage.enabled)
        {
            LDP_RawImage.color = new Color((float)0.5, (float)0.5, (float)0.5, (float)0.75);
            LDP_RawImage.texture = rndImageList.LDP.taskImage;
        }
    }

    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\
    // = = = = = = = = = = End Screen Refresh Functions = = = = = = = = = = = \\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\


}
