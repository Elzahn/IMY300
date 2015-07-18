using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour {

	private PlayerController playerScript;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationX = 0F;
	//float rotationY = 0F;
	
	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	
	
	private List<float> rotArrayY = new List<float>();
	//float rotAverageY = 0F;
	
	public float frameCounter = 20;
	
	Quaternion originalRotation;

	Vector3 originalPosition;

	public void resetRotation(){
		//resets rotation
		/*transform.localRotation = originalRotation;
		rotationY = 0f;
		rotationX = 0f;
		rotArrayX.Clear ();
		rotArrayY.Clear ();*/

		//resets zooming
		GameObject.Find("Player").transform.localPosition = originalPosition;
	}

	void Update ()
	{
		if (playerScript.getPaused() == false) {
			if (Input.GetMouseButton (1)) {		//right

			//	rotAverageY = 0f;
				rotAverageX = 0f;
			
				//rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
			
				//rotArrayY.Add (rotationY);
				rotArrayX.Add (rotationX);
			
			/*	if (rotArrayY.Count >= frameCounter) {
					rotArrayY.RemoveAt (0);
				}*/
				if (rotArrayX.Count >= frameCounter) {
					rotArrayX.RemoveAt (0);
				}
			
		/*		for (int j = 0; j < rotArrayY.Count; j++) {
					rotAverageY += rotArrayY [j];
				}*/
				for (int i = 0; i < rotArrayX.Count; i++) {
					rotAverageX += rotArrayX [i];
				}
			
			//	rotAverageY /= rotArrayY.Count;
				rotAverageX /= rotArrayX.Count;

				//rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
				rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);
			
				//Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
				Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);//GameObject.Find("Player").GetComponent<Transform>().up);

				GameObject.Find("Player").transform.rotation = originalRotation * xQuaternion;// * yQuaternion;
				//transform.localRotation = originalRotation * yQuaternion;
			} 		

			//Zoom
			transform.Translate (Vector3.forward * Input.GetAxis ("Mouse ScrollWheel"));
	
			//Move
			if (Input.GetMouseButton (2)) {		//middle
				if (Input.GetAxis ("Mouse X") != 0) {
					transform.Translate (Vector3.right * Input.GetAxis ("Mouse X"));
				}

				if (Input.GetAxis ("Mouse Y") != 0) {
					transform.Translate (Vector3.up * Input.GetAxis ("Mouse Y"));
				}
			}
		}
	}
	
	void Start ()
	{			
		/*if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;*/
		originalRotation = GameObject.Find("Player").transform.rotation;
		//originalPosition = transform.localPosition;

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