using System.Diagnostics;
using System.IO;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// This example shows how to check encoding/decoding operations performance.
	/// </summary>
	public class PerformanceExample : ExampleBase
	{
		public AnimatedImage AnimatedImage1;
		public AnimatedImage AnimatedImage2;
		public AnimatedImage AnimatedImage3;

		private readonly Stopwatch _stopwatch = new Stopwatch();

		public void Start()
		{
			var bytes1 = File.ReadAllBytes(SmallSample1);
			var bytes2 = File.ReadAllBytes(SmallSample2);
			var bytes3 = File.ReadAllBytes(SmallSample3);

			_stopwatch.Reset();
			_stopwatch.Start();

			var gif1 = Gif.Decode(bytes1);
			var gif2 = Gif.Decode(bytes2);
			var gif3 = Gif.Decode(bytes3);


			UnityEngine.Debug.LogFormat("Decoded in {0:N2}s", _stopwatch.Elapsed.TotalSeconds);

			_stopwatch.Reset();
			_stopwatch.Start();

			gif1.Encode();
			gif2.Encode();
			gif3.Encode();

			UnityEngine.Debug.LogFormat("Encoded in {0:N2}s", _stopwatch.Elapsed.TotalSeconds);

			AnimatedImage1.Play(gif1);
			AnimatedImage2.Play(gif2);
			AnimatedImage3.Play(gif3);
		}
	}
}