using UnityEngine;
using System.Collections;
using System.IO;

public class ColorSourceTextureView : MonoBehaviour
{
		public GameObject ColorSourceManager;
		private ColorSourceManager _ColorManager;
		private KinectManager kinectManager;
	
		private WebCamTexture webcamTexture;
		private bool imageLoaded = false;
		private int counter = 0;

		// Use this for initialization
		void Start ()
		{
				setViewSize (GetComponent<GUITexture>());
		}
	
		// Update is called once per frame
		void Update ()
		{
				// Get the KinectManager instance
				if (kinectManager == null) {
						kinectManager = KinectManager.Instance;
				}

				// Get the ColorManager instance
				if (ColorSourceManager != null) {
						_ColorManager = ColorSourceManager.GetComponent<ColorSourceManager> ();
				}


				// Save a snapshot when nobody is in the view every x seconds
				if (counter == 0 && kinectManager.IsUserDetected () == false) {
						StartCoroutine (saveSnapshot ());		
				}
		
				setBackground ();

				if (counter == 150) {
						counter = 0;
				} else {
						counter++;
				}
		}

		// Set background image depending on user detected or not
		private void setBackground ()
		{
				if (kinectManager.IsUserDetected () == false) {
						if (imageLoaded == true) {
								imageLoaded = false;
						}
						gameObject.GetComponent<GUITexture>().texture = _ColorManager.GetColorTexture ();
				} else if (imageLoaded == false) {
						byte[] textureData = File.ReadAllBytes (Application.streamingAssetsPath + "/texture.png");
						Texture2D backgroundTexture = new Texture2D (_ColorManager.GetColorTexture ().width, _ColorManager.GetColorTexture ().height);
						backgroundTexture.LoadImage (textureData);
						GetComponent<GUITexture>().texture = backgroundTexture;
						imageLoaded = true;
				}
		}

		private void setViewSize (GUITexture guitexture)
		{
				float W = Screen.width * 4;
				float H = Screen.height;
		
				float InsetX = -(W / 4);
				float InsetY = 0;
		
				GetComponent<GUITexture>().pixelInset = new Rect (InsetX, InsetY, W, H);
				transform.localScale = new Vector3 (0, -2, 0);
		}

		private IEnumerator saveSnapshot ()
		{
				yield return new WaitForEndOfFrame ();

				Texture2D snapshot = _ColorManager.GetColorTexture ();
				File.WriteAllBytes (Application.streamingAssetsPath + "/texture.png", snapshot.EncodeToPNG ());
		}
}