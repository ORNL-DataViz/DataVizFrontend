using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MyComponent : MonoBehaviour
{
    [System.Serializable]
    public class ImageTask
    {
        public int photoID;
        public Texture2D taskImage;
        public Vector2 taskDimensions;
        public byte[] serializedPhoto;
        public bool taskCorrect;
        public bool userCorrect;
        public bool userUndo = false;
        public Dictionary<String, DateTime> userDecisionPoints;

        public void UnloadTexture()
        {
            taskImage = null;
        }

        public void LoadTexture()
        {
            taskImage = new Texture2D(2, 2);
            taskImage.LoadImage(serializedPhoto);
            taskImage.Apply();
        }
    }

}
