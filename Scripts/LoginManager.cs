using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoginManager : MonoBehaviour
{

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
 
    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public InputField pinInput;
    public GameObject trialManager;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Currently requires two "Enter" keystrokes, need to adjust
        if (pinInput.isFocused && pinInput.text.Length == 4 && Input.GetKey(KeyCode.Return))
        {
            ReadUserPin();
            
        }
    }

    public void ReadUserPin()
    {
        /// <summary>
        /// Waits for [on-submission] event and then sends User PIN to the
        /// server. Waits for task assignment and name, and then invokes 
        /// </summary>
        Debug.Log($"The user pin inputted is: {pinInput.text}");
        StartCoroutine(trialManager.GetComponent<ExperimentInitializer>().LoadTheProgram(pinInput.text));
            

    }

    public void VerifyUserInput(string userName)
    {
        /*
         * Steps to enact this function:
         * 1. Add a "confirmation button" to the screen that appears after the
         * user submits a pin
         * 2. Pass the value that the server returns to this function
         * 3a. If server returns "PIN Not Found" throw an error message and ask
         * the user to resubmit their pin
         * 3b. If the server returns anything else. Display the name on screen
         * with a message asking the user to confirm that that is their name
         * 4a. If User says yes --> start the LoadTheProgram Coroutine
         * 4b. If User says no --> Wipe the previous input, wait for new PIN
         */
    }
}
