using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour
{
		public float Speed = 0.03F;
		public float Height = 30;
		public bool Inverted = false;
		float Step;
		float Offset;

		// Use this for initialization
		void Start ()
		{
				//Store where we were placed in the editor
				//Vector3 InitialPosition = transform.position;
				//Create an offset based on our height
				Offset = transform.position.y + transform.localScale.y;
		}
	
		// Update is called once per frame
		void Update ()
		{
				Step += Speed;
				//Make sure Steps value never gets too out of hand 
				if (Step > 999999) {
						Step = 1;
				}
		
				//Float up and down along the y axis
				float amount;
				if (!Inverted) {
						amount = Mathf.Sin (Step);
				} else {
						amount = Mathf.Cos (Step);
				}
				Vector3 newPosition = new Vector3 (transform.position.x, (amount / Height) + Offset, transform.position.z);
				transform.position = newPosition;
		}
}
