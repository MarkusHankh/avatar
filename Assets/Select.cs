using UnityEngine;
using System.Collections;

public class Select : MonoBehaviour
{
		public int VideoNumber;
		public GameObject AnimationController;
	
		private string VideoName;
		private string ButtonText;
		private Material ColorMaterial;
		private float red = 223.0F / 255.0F;
		private float green = 0.0F;
		private float blue = 0.0F;
		private float alpha = 1.0F;

		void Start ()
		{
				ColorMaterial = GetComponent<Renderer>().materials [1];
				ColorMaterial.color = new Color (red, green, blue, alpha);
		}
		
		void Update ()
		{
				switch (VideoNumber) {
				case 1:
						VideoName = Settings.GetInstance ().Video1;
						ButtonText = Settings.GetInstance ().Button1Text;
						break;
				case 2:
						VideoName = Settings.GetInstance ().Video2;
						ButtonText = Settings.GetInstance ().Button2Text;
						break;
				case 3:
						VideoName = Settings.GetInstance ().Video3;
						ButtonText = Settings.GetInstance ().Button3Text;
						break;
				case 4:
						VideoName = Settings.GetInstance ().Video4;
						ButtonText = Settings.GetInstance ().Button4Text;
						break;
			
				default:
						break;
				}
				
				GetComponentInChildren<GUIText> ().text = ButtonText;
		}
		
		void OnTriggerEnter ()
		{
				red = 223.0F / 255.0F;
				ColorMaterial.color = new Color (red, green, blue, alpha);
		}

		void OnTriggerStay ()
		{
				if (red >= (100.0F / 255.0F)) {
						red -= 0.005F;
				} else {
						red = 223.0F / 255.0F;
						SceneController sa = AnimationController.GetComponent<SceneController> ();
						sa.playVideo (VideoName);
				}
				ColorMaterial.color = new Color (red, green, blue, alpha);
		}

		void OnTriggerExit (Collider collider)
		{
				red = 223.0F / 255.0F;
				ColorMaterial.color = new Color (red, green, blue, alpha);
		}
}
