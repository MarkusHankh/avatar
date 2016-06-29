using System;
using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class SceneController : MonoBehaviour
{
		public GameObject CharacterController;
		public GameObject Menu;
		public GameObject AttractionLoop;
		public GameObject VideoLayer;
		public GameObject BackgroundImage;
		public GameObject WorldCenterMark;
		public GameObject Floor;
		public bool RealMirror;

		private Animator animator;
		private KinectManager kinectManager;
		
		private bool animationPlaying = false;
		private bool characterControlled = false;
		private float TimeThreshold;
	
		private Vector3 CharacterInitialPosition;
		private Vector3 Menu1InitialPosition;
		private Vector3 Menu2InitialPosition;
		private Vector3 Menu3InitialPosition;
		private Vector3 Menu4InitialPosition;

		void Start ()
		{
				animator = CharacterController.GetComponent<Animator> ();
		
				Menu1InitialPosition = GameObject.Find ("Menu1").transform.position;
				Menu2InitialPosition = GameObject.Find ("Menu2").transform.position;
				Menu3InitialPosition = GameObject.Find ("Menu3").transform.position;
				Menu.SetActive (false);
		
				CharacterInitialPosition = transform.position;
				
				if (RealMirror) {
						AttractionLoop.GetComponent<GUITexture>().enabled = false;
						BackgroundImage.GetComponent<ImageController> ().enabled = false;
						BackgroundImage.GetComponent<ColorSourceTextureView> ().enabled = true;
				} else {
						BackgroundImage.GetComponent<ImageController> ().enabled = true;
						BackgroundImage.GetComponent<ColorSourceTextureView> ().enabled = false;
				}
		}

		void Update ()
		{
				// Get the KinectManager instance
				if (kinectManager == null) {
						kinectManager = KinectManager.Instance;
				}

				// User detected - start attraction routine
				if (kinectManager.IsUserDetected () && kinectManager.IsUserCalibrated (kinectManager.GetPrimaryUserID ()) && !characterControlled && !animationPlaying) {
						StartCoroutine (playWelcomeAnimation ());
				}

				// User detected and controlled - this is the normal behaviour
				// when the character is controlled by a user
				if (kinectManager.IsUserDetected () && kinectManager.IsUserCalibrated (kinectManager.GetPrimaryUserID ()) && characterControlled && !animationPlaying) {
						TimeThreshold = 0;

						animator.SetBool ("UserActive", true);
						animator.enabled = false;

						GameObject.Find ("MessageBox").GetComponent<GUIText>().text =
				"Ich mache dir alles nach!" + "\n" +
								"Tippe die Kugeln oder Schalter an," + "\n" +
								"um zu navigieren.";
				}

				// No user detected, but character still in control mode - 
				// reset character to center of the view and move
				// him out of the view if there is no user detected for a
				// certain time
				if (!kinectManager.IsUserDetected () && characterControlled && !animationPlaying) {
						TimeThreshold++;

						CharacterController.transform.position = WorldCenterMark.transform.position;
						animator.enabled = true;
						animator.SetBool ("UserActive", false);

						GameObject.Find ("MessageBox").GetComponent<GUIText>().text = 
				"Bist du noch da?";

						// Idle time in seconds * 30 fps
						int seconds = 7;
						if (TimeThreshold > (seconds * 30)) {
								TimeThreshold = 0;
								StartCoroutine (playLeaveAnimation ());
						}
				}

				if (animator.enabled) {
						// Update avatar distance to ground
						animator.SetFloat ("Height", CharacterController.transform.position.y - Floor.transform.position.y); // TODO Raycast
				}
		}

		IEnumerator playWelcomeAnimation ()
		{
				animationPlaying = true;

				AttractionLoop.GetComponent<GUITexture>().enabled = false;
				BackgroundImage.GetComponent<GUITexture>().enabled = true;
		
				animator.SetTrigger ("UserDetected");
				CharacterController.GetComponent<Rigidbody>().drag = 0;
				yield return new WaitForSeconds (3F);

				GameObject.Find ("MessageBox").GetComponent<GUIText>().text = 
			"Hey, was machst du da?";
		
				yield return new WaitForSeconds (3F);
		
				CharacterController.transform.position = WorldCenterMark.transform.position;
				Menu.SetActive (true);

				characterControlled = true;
				animationPlaying = false;
		}

		IEnumerator playLeaveAnimation ()
		{
				animationPlaying = true;
				characterControlled = false;
				Menu.SetActive (false);
		
				GameObject.Find ("MessageBox").GetComponent<GUIText>().text = 
			"Wenn du nicht willst, gehe ich eben wieder.";

				animator.SetTrigger ("UserLeft");
				animator.SetFloat ("Walk", 1);
				animator.SetFloat ("Direction", 1);

				yield return new WaitForSeconds (7F);

				ResetCharacter ();
				ResetCharacter ();
				ResetMenu ();
				animationPlaying = false;
		}
		
		public void playVideo (String videoName)
		{
				animationPlaying = true;
				animator.enabled = true;
				
				
				// Hide everything else
				CenterCharacter ();
				CharacterController.SetActive (false);
				ResetMenu ();
		
				VideoLayer.GetComponent<MovieController> ().ChangeVideo (videoName);
				AttractionLoop.GetComponent<GUITexture>().enabled = false;
				
				StartCoroutine (endVideo ());
		}
		
		IEnumerator endVideo ()
		{
				yield return new WaitForSeconds (10F);
				BackgroundImage.GetComponent<GUITexture>().enabled = true;
				VideoLayer.GetComponent<GUITexture>().enabled = false;
				animationPlaying = false;
				
				CharacterController.SetActive (true);
				ResetMenu (true);
				//CharacterController.rigidbody.drag = 0F;
		}

		public void ResetCharacter ()
		{
				// Bring character to original position
				CharacterController.transform.position = CharacterInitialPosition;
				CharacterController.transform.rotation = new Quaternion (0, 180, 0, 0);
				animator.SetFloat ("Direction", 0);
				animator.SetFloat ("Walk", 0);
				animator.SetBool ("UserActive", false);
				CharacterController.GetComponent<Rigidbody>().drag = 9999F;
		}
		
		public void CenterCharacter ()
		{
				CharacterController.transform.position = WorldCenterMark.transform.position;
		}
	
		public void ResetScene ()
		{
				if (RealMirror) {
						BackgroundImage.GetComponent<GUITexture>().enabled = true;
						VideoLayer.GetComponent<GUITexture>().enabled = false;
						AttractionLoop.GetComponent<GUITexture>().enabled = false;
				} else {
						BackgroundImage.GetComponent<GUITexture>().enabled = false;
						VideoLayer.GetComponent<GUITexture>().enabled = false;
						AttractionLoop.GetComponent<GUITexture>().enabled = true;
				}
		}

		public void ResetMenu (bool active = false)
		{
				// Reset Menu
				Menu.SetActive (true);
				Quaternion noRotation = new Quaternion (0, 0, 180, 0);
				GameObject.Find ("Menu1").transform.position = Menu1InitialPosition;
				GameObject.Find ("Menu1").transform.rotation = noRotation;
				GameObject.Find ("Menu2").transform.position = Menu2InitialPosition;
				GameObject.Find ("Menu2").transform.rotation = noRotation;
				GameObject.Find ("Menu3").transform.position = Menu3InitialPosition;
				GameObject.Find ("Menu3").transform.rotation = noRotation;
				GameObject.Find ("Scheibe1").GetComponent<Renderer>().materials [1].color = new Color (0.87F, 0, 0, 1.0F);
				GameObject.Find ("Scheibe2").GetComponent<Renderer>().materials [1].color = new Color (0.87F, 0, 0, 1.0F);
				GameObject.Find ("Scheibe3").GetComponent<Renderer>().materials [1].color = new Color (0.87F, 0, 0, 1.0F);
				GameObject.Find ("Scheibe4").GetComponent<Renderer>().materials [1].color = new Color (0.87F, 0, 0, 1.0F);
				Menu.SetActive (active);

				// Reset Messagebox
				GameObject.Find ("MessageBox").GetComponent<GUIText>().text = "";
		}
}