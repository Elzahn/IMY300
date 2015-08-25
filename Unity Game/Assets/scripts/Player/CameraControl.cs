
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour {

	private PlayerController playerScript;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 5F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationX = 0F;
	
	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	

	public float frameCounter = 20;

	Vector3 originalPosition;

	void Update ()
	{
		if (playerScript.paused == false) {

			if (Input.GetMouseButton (1))//right
			{
				//Rotate Around the player - no player gameObject Rotation
				/*Transform player = GameObject.FindWithTag("Player").transform;
				transform.RotateAround(player.transform.position, player.up, Input.GetAxis ("Mouse X") * sensitivityX);*/

				//Rotate Around the player - player gameObject Rotation
				GameObject player = GameObject.FindWithTag("Player");
				player.transform.RotateAround(player.transform.position, player.transform.up, Input.GetAxis ("Mouse X") * sensitivityX);
				player.GetComponent<Animator>().SetFloat("Turning", 1f);
			}

			//Zoom
			transform.Translate (Vector3.forward * Input.GetAxis ("Mouse ScrollWheel"));
	
		/*	//Move
			if (Input.GetMouseButton (2)) {		//middle
				if (Input.GetAxis ("Mouse X") != 0) {
					transform.Translate (Vector3.right * Input.GetAxis ("Mouse X"));
				}

				if (Input.GetAxis ("Mouse Y") != 0) {
					transform.Translate (Vector3.up * Input.GetAxis ("Mouse Y"));
				}
			}*/
		}
	}
	
	void Start ()
	{			
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}
}