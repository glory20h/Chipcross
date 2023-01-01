using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class GIFLoader : MonoBehaviour
{
    public IEnumerator<Texture2D> Load(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            yield break;
        }

        // Read the GIF file into a byte array
        byte[] bytes = File.ReadAllBytes(path);

        // Create a new Texture2D and load the pixel data into it
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);

        // Split the texture into a series of frames
        int frameCount = texture.width / texture.height;
        for (int i = 0; i < frameCount; i++)
        {
            // Create a new Texture2D and copy the pixel data into it
            Texture2D frame = new Texture2D(texture.height, texture.height);
            frame.SetPixels(texture.GetPixels(i * texture.height, 0, texture.height, texture.height));
            frame.Apply();

            // Yield the frame
            yield return frame;
        }
    }


}
