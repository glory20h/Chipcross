using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// Upload to GIPHY example.
	/// For more info visit: https://developers.giphy.com/docs/
	/// </summary>
	public class GiphyUploadExample : ExampleBase
	{
		public List<Texture2D> Frames;

		public void Start()
		{
			var frames = Frames.Select(f => new GifFrame(f, 0.1f)).ToList();
			var gif = new Gif(frames);
			var binary = gif.Encode();
			var giphy = new GiphyApi("USER_NAME", "API_KEY"); // Create an account on https://developers.giphy.com/ to get the API key.

			StartCoroutine(giphy.Upload("animation", binary, "animation,amazing,pixel", "https://assetstore.unity.com/packages/slug/120039", OnUploaded));
		}

		private static void OnUploaded(bool success, string result)
		{
			Debug.LogFormat("Success: {0}, Result: {1}", success, result);

			if (success)
			{
				var url = "https://giphy.com/gifs/" + result;

				Debug.LogFormat("Uploaded as: {0}", url);
			}
		}
	}
}