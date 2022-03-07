using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// Download GIF from WWW, decode and play example.
	/// </summary>
	public class DownloadExample : ExampleBase
	{
		public string Url;
		public AnimatedImage AnimatedImage;
		public Image ProgressFill;
		
		public IEnumerator Start()
		{
			if (!Regex.IsMatch(Url, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
			{
				throw new ArgumentException("Wrong URL!");
			}

			var www = UnityWebRequest.Get(Url);

            www.SendWebRequest();

			while (!www.isDone)
			{
				ProgressFill.fillAmount = www.downloadProgress;
				yield return null;
			}

			if (www.error != null)
			{
				throw new Exception(www.error);
			}

			var iterator = Gif.DecodeIterator(www.downloadHandler.data);
			var iteratorSize = Gif.GetDecodeIteratorSize(www.downloadHandler.data);
			var frames = new List<GifFrame>();
			var index = 0f;

			foreach (var frame in iterator)
			{
				frames.Add(frame);
				ProgressFill.fillAmount = ++index / iteratorSize;
				yield return null;
			}

			var gif = new Gif(frames);

			AnimatedImage.Play(gif);
		}
	}
}