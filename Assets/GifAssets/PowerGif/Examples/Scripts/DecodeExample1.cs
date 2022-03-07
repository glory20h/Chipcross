using System.IO;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// Decoding GIF example.
	/// </summary>
	public class DecodeExample1 : ExampleBase
	{
		public AnimatedImage AnimatedImage;

		public void Start()
		{
			var bytes = File.ReadAllBytes(LargeSample1);
			var gif = Gif.Decode(bytes);

			AnimatedImage.Play(gif);
		}
	}
}