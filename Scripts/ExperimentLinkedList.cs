using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentLinkedList : MonoBehaviour
{
    // = = = = = = = = = = Project-Wide Static Variable = = = = = = = = = = = \\
    public static MyComponent.LinkedList photoProgressionOrder;

    private void Awake()
    {
        // Initializes the Static LinkedList component before the rest of the
        // project's functionality is called
        photoProgressionOrder = new MyComponent.LinkedList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


}
