using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhotoResizer : MonoBehaviour
{
    List<byte[]> TextureArray = new List<byte[]>();

    public RawImage TheImage;
    public RectTransform PolaroidFrame;

    // Start is called before the first frame update
    void Start()
    {
        TexturePipeline(TextureArray);
        StartCoroutine(IterateOverPhotos(TextureArray));
    }

    // Update is called once per frame
    void Update()
    {

    }

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

        // Variable declarations (Tempo paths, future version would use WebRequests)
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

    Vector2 GenerateResizedRawImageDimensions(Vector2 nativeDimensions)
    {
        /// <summary>
        /// Finds the larger edge of the image, calculates the necessary scaling val
        /// </summary>
        /// <param name="nativeDimensions">Original image dimensions</param>
        /// <return>
        /// Vector2 containing downscaled image dimensions
        /// </return>

        float scale = Math.Min((PolaroidFrame.rect.height / nativeDimensions.y), (PolaroidFrame.rect.width / nativeDimensions.x));
        return new Vector2(nativeDimensions.x * scale, nativeDimensions.y * scale);

    }

}
