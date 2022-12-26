using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class GifInputer : MonoBehaviour
{
    // The path to the gif file in the asset folder
    public string gifPath;

    // A list of sprites that represent the gif frames
    private List<Sprite> frames;

    // The current frame index
    private int frameIndex;

    // The delay between each frame in seconds
    public float frameDelay;

    // A reference to the SpriteRenderer component that will display the gif
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Load the gif frames as sprites
        Sprite[] sprites = Resources.LoadAll<Sprite>(gifPath);

        // Add the sprites to the list
        frames = new List<Sprite>(sprites);

        for (int i = 0; i < sprites.Length; i++)
        {
            frames[i] = sprites[i];
        }

        // Get a reference to the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial frame
        spriteRenderer.sprite = frames[0];

        // Start the animation coroutine
        StartCoroutine(AnimateGif());
    }

    IEnumerator AnimateGif()
    {
        while (true)
        {
            // Wait for the frame delay
            yield return new WaitForSeconds(frameDelay);

            // Increment the frame index and wrap around if necessary
            frameIndex = (frameIndex + 1) % frames.Count;

            // Set the current frame
            spriteRenderer.sprite = frames[frameIndex];
        }
    }
}
