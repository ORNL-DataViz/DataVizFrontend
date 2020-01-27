using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DynamicResizer : MonoBehaviour
{
    // = = = = = = = = = = = = = DEPRECATION WARNING = = = = = = = = = = = = =\\
    // = SCRIPT NO LONGER IN USE, WILL BE DEPRECATED IN NEXT VERSION RELEASE =\\
    // = = = = = ALL FUNCTIONALITY CAN BE FOUND WITHIN IMAGETASK.CS = = = = = \\
    // = = DO NO ACCESS FUNCTIONS THROUGH THIS SCRIPT BEGINNING 01.26.2019 = =\\
    // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =\\

    // = = = = = = = = = = = = Script-Scope Variables = = = = = = = = = = = = \\
    List<byte[]> TextureArray = new List<byte[]>();

    // = = = = = = = = = = GameObject Attachment Points = = = = = = = = = = = \\
    public RawImage TheImage;
    public RectTransform PolaroidFrame;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //TODO: Deprecate this once fully shifted over to ExperimentInitializer.CS
    void TexturePipeline(List<byte[]> workingArray)
    {
        /// <summary>
        /// Iterates over all images in a directory and converts them to textures
        /// </summary>
        /// <param name="workingArray">The container for our textures</param>
        /// <remarks>
        /// Currently optimized to work with local storage, can be gutted and 
        /// rewritted to work with delivery over air
        /// </remarks>

        // Variable declarations (Temp paths, future version would use WebRequests)
        string dummyPhotoPath = Environment.CurrentDirectory + "/Assets/DummyPhotos";
        string[] dummyPhotoPathArray = Directory.GetFiles(dummyPhotoPath);
        String metaComparison = ".meta";
        foreach (string path in dummyPhotoPathArray)
        {

            if (!path.EndsWith(metaComparison))
            {
                byte[] currentPhoto = File.ReadAllBytes(path);
                workingArray.Add(currentPhoto);
            }

        }

    }

    //TODO: Deprecate this once fully shifted over to ExperimentInitializer.CS
    IEnumerator IterateOverPhotos(List<byte[]> textureArray)
    {
        /// Temporary function until multithreading is built in.
        foreach (byte[] photo in textureArray)
        {
            Texture2D imgText = new Texture2D(2, 2);
            imgText.LoadImage(photo);
            Vector2 photoDimensions = new Vector2(imgText.width, imgText.height);
            TheImage.rectTransform.sizeDelta = GenerateResizedRawImageDimensions(photoDimensions);
            TheImage.texture = imgText;
            yield return new WaitForSeconds(1);
        }
    }

    public Vector2 GenerateResizedRawImageDimensions(Vector2 nativeDimensions)
    {
        /// <summary>
        /// Finds the larger edge of the image, calculates the necessary scaling val
        /// </summary>
        /// <param name="nativeDimensions">Original image dimensions</param>
        /// <return>
        /// Vector2 containing downscaled image dimensions
        /// </return>

        float targetedArea = PolaroidFrame.sizeDelta.x * PolaroidFrame.sizeDelta.y;
        if( nativeDimensions.x > nativeDimensions.y)
        {
            float scale = nativeDimensions.y / nativeDimensions.x;
            double newY = Math.Sqrt(targetedArea * scale);
            double newX = targetedArea / newY;
            return new Vector2((float)newX, (float)newY);
        }
        else
        {
            float scale = nativeDimensions.x / nativeDimensions.y;
            double newX = Math.Sqrt(targetedArea * scale);
            double newY = targetedArea / newX;
            return new Vector2((float)newX, (float)newY);
        }
    }
}
