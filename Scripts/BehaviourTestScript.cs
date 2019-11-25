using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BehaviourTestScript : MonoBehaviour
{

    public GameObject linkedListContainer;
    private MyComponent.LinkedList rndImageList;
    // Start is called before the first frame update
    void Start()
    {
        rndImageList = ExperimentLinkedList.photoProgressionOrder;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator WillItHalt( int timeToPrint)
    {
        for( int i = 0; i < timeToPrint; i++)
        {
            UnityEngine.Debug.Log(i + 1);
        }
        yield return 5;
    }

    public void HowLong()
    {
        int len = 0;
        MyComponent.Node tempNode = rndImageList.getHead();

        while( tempNode.next != null)
        {
            len++;
            tempNode = tempNode.next;
        }

        UnityEngine.Debug.Log(len);
    }
}
