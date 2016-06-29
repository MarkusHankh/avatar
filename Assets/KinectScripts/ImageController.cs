using UnityEngine;
using System.Collections;
using System.IO;

public class ImageController : MonoBehaviour
{
		public string FileName;
		private Texture imgTexture;

		// Use this for initialization
		void Start ()
		{
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				setViewSize (gameObject.GetComponent<GUITexture>());
		}

		private void setViewSize (GUITexture guitexture)
		{
				float W = Screen.width * 2;
				float H = 0;
		
				float InsetX = Screen.width / 2;
				float InsetY = Screen.height / 2;
		
				GetComponent<GUITexture>().pixelInset = new Rect (InsetX, InsetY, W, H);
		}
		
		public void LoadImage (string fileName)
		{
				FileName = fileName;
				StartCoroutine (loadImage ());
		}

		private IEnumerator loadImage ()
		{
				WWW data = new WWW ("file://" + Application.streamingAssetsPath + "/" + FileName);
				imgTexture = data.texture;
				gameObject.GetComponent<GUITexture>().texture = imgTexture;
				yield return data;
		}
}
