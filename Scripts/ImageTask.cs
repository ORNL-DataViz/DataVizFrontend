using UnityEngine;
using System.Collections;

public class MyComponent : MonoBehaviour
{
    [System.Serializable]
    public class ImageTask
    {
        public int photoID;
        public Texture2D taskImage;
        public Vector2 taskDimensions;
        public bool taskCorrect;
    }

}
