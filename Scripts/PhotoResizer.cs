using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotoResizer : MonoBehaviour
{
    static string DummyPhotoPath = Environment.CurrentDirectory + "/Assets/DummyPhotos";
    string[] DummyPhotoPathArray = Directory.GetFiles(DummyPhotoPath);

    // Start is called before the first frame update
    void Start()
    {
        foreach(String PhotoPath in DummyPhotoPathArray) { Debug.Log(PhotoPath); }
        Debug.Log("Dummy Photo Path: " + DummyPhotoPath);
        Debug.Log("Current Working Directory: " + Environment.CurrentDirectory );

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
