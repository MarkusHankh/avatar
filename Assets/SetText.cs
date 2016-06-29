using UnityEngine;
using System.Collections;

public class SetText : MonoBehaviour
{
		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
				GetComponent<GUIText>().text = Settings.GetInstance ().Button1Text;
		}
}
