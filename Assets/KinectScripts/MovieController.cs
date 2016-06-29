using UnityEngine;
using System.Collections;
using System.IO;

public class MovieController : MonoBehaviour
{
		private MovieTexture movTexture;
		public string FileName;
		public bool Vertical;
		public bool Loop;

		// Use this for initialization
		void Start ()
		{
				
		}
		
		public void ChangeVideo (string externalVideo)
		{
				FileName = externalVideo;
				GameObject.Find ("Background Image").GetComponent<GUITexture>().enabled = false;
				GetComponent<GUITexture>().enabled = true;
				StartCoroutine (loadMovie ());
		}
	
		// Update is called once per frame
		void Update ()
		{
				
		}
		
		public void LoadMovie (string fileName)
		{
				FileName = fileName;
				StartCoroutine (loadMovie ());
		}
		
		private IEnumerator loadMovie ()
		{
				WWW data = new WWW ("file://" + Application.streamingAssetsPath + "/" + FileName);
				movTexture = data.movie;
				gameObject.GetComponent<GUITexture>().texture = movTexture;
				movTexture.loop = Loop;
				yield return data;
		
				data.Dispose ();
				
				Resize ();
				movTexture.Play ();
		}
		
		private void Resize ()
		{
				if (Vertical) {
						float W = 0;
						float H = 0;
						float InsetX = Screen.width / 2;
						float InsetY = Screen.height / 2;
						GetComponent<GUITexture>().pixelInset = new Rect (InsetX, InsetY, W, H);
				} else {
						float W = Screen.width * 2;
						float H = 0;
						float InsetX = - (W / 4);
						float InsetY = Screen.height / 2;
						GetComponent<GUITexture>().pixelInset = new Rect (InsetX, InsetY, W, H);
				}
		}
}
