using System.IO;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// Decoding GIF example.
	/// </summary>
	public class DecodeExample2 : ExampleBase
	{
		public AnimatedImage AnimatedImage;

		public void Start()
		{
			var bytes = File.ReadAllBytes(LargeSample2);
			var gif = Gif.Decode(bytes);

			AnimatedImage.Play(gif);
		}
	}
}