using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BehaviourTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WillItHalt( 50 ));
        UnityEngine.Debug.Log("00000000000000000000000 - HERE I AM - 000000000000000000000");
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

}
