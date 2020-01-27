using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebRequestTest : MonoBehaviour
{

    // // GameObject Connection Points
    // public RawImage testImage;
    // public Button NewRequestButton;
    // public Button NewPhotoButton;
    // public Text TagText;
    // public Text BooleanText;
    // public Text PhotoIDText;
    //
    // // private script variables
    // private string URL = "https://vr.uncw.edu/data_collection/sendTaskData";
    // private string TaskID = "";
    // private string APIKey = "";

    // Unity Target Classes

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // public IEnumerator GetPhotoSet()
    // {
    //     WWWForm webRequest = new WebForm();
    //     webrequest.AddField("token", APIKey);
    //     webRequest.AddField("TaskID", TaskID);
    //
    //     using (UnityWebRequest www = UnityWebRequest.Get(URL, webRequest))
    //     {
    //       yield return www.SendWebRequest();
    //
    //       if (www.isNetworkError || www.isHttpError )
    //       {
    //         Debug.Log(www.error);
    //       } else {
    //         Debug.Log($"returning: {www.downladHandler.text}");
    //       }
    //     }
    // }
}
