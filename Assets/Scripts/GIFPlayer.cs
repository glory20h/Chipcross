using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIFPlayer : MonoBehaviour
{
    public string GIFPath;
    public float FrameInterval = 0.1f;
    private GIFLoader loader;
    private IEnumerator<Texture2D> frames;
    private float timeSinceLastFrame;
    private bool isPlaying;

    private void Start()
    {
        // Create a new GIFLoader instance
        loader = new GIFLoader();

        // Load the GIF file and get the frames
        frames = loader.Load(GIFPath);

        // Set the initial time
        timeSinceLastFrame = Time.time;

        // Set isPlaying to true
        isPlaying = true;
    }

    private void Update()
    {
        if (isPlaying)
        {
            // Check if it's time to show the next frame
            if (Time.time - timeSinceLastFrame >= FrameInterval)
            {
                // Advance to the next frame
                if (frames.MoveNext())
                {
                    // Get the current frame
                    Texture2D frame = frames.Current;

                    // Set the frame as the texture of the game object
                    GetComponent<Renderer>().material.mainTexture = frame;

                    // Update the time
                    timeSinceLastFrame = Time.time;
                }
                else
                {
                    // If there are no more frames, stop playing
                    isPlaying = false;
                }
            }
        }
    }

}
