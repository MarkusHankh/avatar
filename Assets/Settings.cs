using UnityEngine;
using System.Collections;
using System.Xml;

public class Settings : MonoBehaviour
{
		public int ScreenWidth;
		public int ScreenHeight;
		public bool FullScreen;
		
		public string Button1Text;
		public string Button2Text;
		public string Button3Text;
		public string Button4Text;
		
		public string Video1;
		public string Video2;
		public string Video3;
		public string Video4;
		
		public string AttractionLoop;
		public string BackgroundImage;
		
		private XmlDocument xml;
		private XmlNode root;
		
		// Use this for initialization
		void Start ()
		{
				StartCoroutine (LoadConfig ());				
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
		
		public static Settings GetInstance ()
		{
				return GameObject.Find ("SceneController").GetComponent<Settings> ();
		}
		
		public IEnumerator LoadConfig ()
		{
				WWW www = new WWW ("file://" + Application.streamingAssetsPath + "/Config.xml");
				yield return www;
				
				string textXML = www.text;
				xml = new XmlDocument ();
				xml.LoadXml (textXML);
				
				www.Dispose ();
				www = null;
				
				// Read
				root = xml.FirstChild;
		
				// Debug
				/*foreach (XmlNode node in root.ChildNodes) {
						foreach (XmlNode childNode in node.ChildNodes) {
								Debug.Log (childNode.Name);
								foreach (XmlAttribute attribute in childNode.Attributes) {
										Debug.Log (attribute.Name + ": " + attribute.Value);
								}
						}
				}*/
				
				SetConfig ();	
		}
	
		public void SetConfig ()
		{
				// Set screen resolution
				ScreenWidth = int.Parse (root ["Display"] ["ScreenResolution"].Attributes ["x"].Value);
				ScreenHeight = int.Parse (root ["Display"] ["ScreenResolution"].Attributes ["y"].Value);
				FullScreen = bool.Parse (root ["Display"] ["FullScreen"].Attributes ["active"].Value);
		
				// Set button labels
				Button1Text = root ["Menu"] ["Button1"].Attributes ["Text"].Value;
				Button2Text = root ["Menu"] ["Button2"].Attributes ["Text"].Value;
				Button3Text = root ["Menu"] ["Button3"].Attributes ["Text"].Value;
				Button4Text = root ["Menu"] ["Button4"].Attributes ["Text"].Value;
		
				// Set video paths
				Video1 = root ["Sources"] ["Video1"].Attributes ["Path"].Value;
				Video2 = root ["Sources"] ["Video2"].Attributes ["Path"].Value;
				Video3 = root ["Sources"] ["Video3"].Attributes ["Path"].Value;
				Video4 = root ["Sources"] ["Video4"].Attributes ["Path"].Value;
				AttractionLoop = root ["Sources"] ["AttractionLoop"].Attributes ["Path"].Value;
				BackgroundImage = root ["Sources"] ["BackgroundImage"].Attributes ["Path"].Value;
		
				// Apply settings
				Screen.SetResolution (ScreenWidth, ScreenHeight, FullScreen);
				GameObject.Find ("Attraction Loop Video").GetComponent<MovieController> ().LoadMovie (AttractionLoop);
				GameObject.Find ("Background Image").GetComponent<ImageController> ().LoadImage (BackgroundImage);
		}
}
