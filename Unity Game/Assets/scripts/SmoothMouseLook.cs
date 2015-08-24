using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour {

	private PlayerController playerScript;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 5F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationX = 0F;
	//float rotationY = 0F;
	
	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	
	
//	private List<float> rotArrayY = new List<float>();
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
		if (playerScript.paused == false) {

		/*	//Rotate player
			if(Input.GetMouseButton (1)){
				print ("here");
				if(Input.GetAxisRaw("Horizontal") < 0){
					float tur = Input.GetAxisRaw("Horizontal");
					GameObject.Find("Player").GetComponent<Animator>().SetFloat("Turning", tur);
					transform.RotateAround(transform.localPosition, transform.up, Time.deltaTime * -10f);
				} else if(Input.GetAxisRaw("Horizontal") > 0){
					transform.RotateAround(transform.localPosition, transform.up, Time.deltaTime * 10f);
					GameObject.Find("Player").GetComponent<Animator>().SetFloat("Turning", Input.GetAxisRaw("Horizontal"));
				} else {
					float tur = Input.GetAxisRaw("Horizontal");
					GameObject.Find("Player").GetComponent<Animator>().SetFloat("Turning", tur);
				}
			}*/

			/*if(Input.GetKey (KeyCode.H))
			{
				Vector3 verticalaxis;

				float speedToRotate = 5F;

				verticalaxis = transform.TransformDirection(GameObject.Find("Player").transform.up);

				transform.RotateAround(GameObject.Find("Player").transform.position, verticalaxis, speedToRotate * Time.deltaTime); 

			}*/

			if (Input.GetMouseButton (1))//right
			{
				/*Transform player = GameObject.Find("Player").transform;
				Quaternion rotation = Quaternion.AngleAxis(90 * Time.deltaTime, player.up);
				transform.rotation *= rotation;

				Vector3 delta = transform.position - player.position;
				transform.position = player.position + rotation * delta;*/

				//Rotate Around the player - no player gameObject Rotation
				/*Transform player = GameObject.FindWithTag("Player").transform;
				transform.RotateAround(player.transform.position, player.up, Input.GetAxis ("Mouse X") * sensitivityX);*/


				//Rotate Around the player - player gameObject Rotation
				GameObject player = GameObject.FindWithTag("Player");
				player.transform.RotateAround(player.transform.position, player.transform.up, Input.GetAxis ("Mouse X") * sensitivityX);
				player.GetComponent<Animator>().SetFloat("Turning", 1f);
			}


			/*if (Input.GetMouseButton (1)) {		//right

			//	rotAverageY = 0f;
				rotAverageX = 0f;
			
				//rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
			
				//rotArrayY.Add (rotationY);
				rotArrayX.Add (rotationX);

				if (rotArrayX.Count >= frameCounter) {
					rotArrayX.RemoveAt (0);
				}

				for (int i = 0; i < rotArrayX.Count; i++) {
					rotAverageX += rotArrayX [i];
				}
			
			//	rotAverageY /= rotArrayY.Count;
				rotAverageX /= rotArrayX.Count;

				//rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
				rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);
			
				//Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
				Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);//GameObject.Find("Player").GetComponent<Transform>().up);

				//print(GameObject.Find("Player").transform.up);

				GameObject.Find("Player").transform.rotation = player.up * xQuaternion;// * yQuaternion;
				//transform.localRotation = originalRotation * yQuaternion;


				//transform.RotateAround(transform.position, player.up, originalRotation * xQuaternion);
			} 	*/

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