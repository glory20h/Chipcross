using UnityEngine;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	public abstract class ExampleBase : MonoBehaviour
	{
		protected const string SmallSample1 = "Assets/GifAssets/PowerGif/Examples/Samples/tt1.gif";
		protected const string SmallSample2 = "Assets/GifAssets/PowerGif/Examples/Samples/tt2.gif";
		protected const string SmallSample3 = "Assets/GifAssets/PowerGif/Examples/Samples/tt3.gif";
		protected const string LargeSample = "Assets/GifAssets/PowerGif/Examples/Samples/tt1.gif";
		protected const string LargeSample1 = "Assets/GifAssets/PowerGif/Examples/Samples/tt2.gif";
		protected const string LargeSample2 = "Assets/GifAssets/PowerGif/Examples/Samples/tt3.gif";
		protected const string SampleFolder = "Assets/GifAssets/PowerGif/Examples/Samples";

		public void Review()
		{
			Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/120039");
		}
	}
}