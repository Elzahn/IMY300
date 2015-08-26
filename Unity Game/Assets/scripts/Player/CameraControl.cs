using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour {

	private PlayerController playerScript;

	public float sensitivityX = 5F;

	GameObject player;
	Vector3 originalPosition;
	Quaternion originalRotation;

	void Update ()
	{
		if (playerScript.paused == false) {

			if (Input.GetMouseButton (1) && Input.GetAxis ("Mouse X") < 0)//right button held and mouse moved left
			{
				//Rotate Around the player - no player gameObject Rotation
				/*Transform player = GameObject.FindWithTag("Player").transform;
				transform.RotateAround(player.transform.position, player.up, Input.GetAxis ("Mouse X") * sensitivityX);*/

				//Rotate Around the player - player gameObject Rotation
				player.transform.RotateAround(player.transform.position, player.transform.up, Input.GetAxis ("Mouse X") * sensitivityX);
				GameObject.Find("Player").GetComponent<Animator>().SetFloat("Turning", -1f);
			} else if (Input.GetMouseButton (1) && Input.GetAxis ("Mouse X") > 0) {
				player.transform.RotateAround(player.transform.position, player.transform.up, Input.GetAxis ("Mouse X") * sensitivityX);
				GameObject.Find("Player").GetComponent<Animator>().SetFloat("Turning", 1f);
			}

			//Zoom
			transform.Translate (Vector3.forward * Input.GetAxis ("Mouse ScrollWheel"));
	
			/*//Move
			if (Input.GetMouseButton (2) && Input.GetMouseButton (1)) {		//middle and right
				if (Input.GetAxis ("Mouse X") != 0) {
					transform.Translate (Vector3.right * Input.GetAxis ("Mouse X"));
				}

				if (Input.GetAxis ("Mouse Y") != 0) {
					transform.Translate (Vector3.up * Input.GetAxis ("Mouse Y"));
				}
			}*/

			
			//Seek on planet
			if (Input.GetMouseButtonDown (2) && Application.loadedLevelName != "SaveSpot") {		//middle  
				originalPosition = this.transform.position;
				originalRotation = this.transform.rotation;

				this.transform.position = GameObject.Find("Player").transform.position + (GameObject.Find("Player").transform.up * 25);
				this.transform.LookAt(GameObject.Find("Player").transform);
			}

			if(Input.GetMouseButton(2) && Application.loadedLevelName != "SaveSpot"){
				if(Input.GetAxis("Mouse X") != 0){
					this.transform.RotateAround(new Vector3(0,0,0), -player.transform.right, Input.GetAxis ("Mouse X") * sensitivityX);
				} 
				if(Input.GetAxis("Mouse Y") != 0){
					this.transform.RotateAround(new Vector3(0,0,0), -player.transform.forward, Input.GetAxis ("Mouse Y") * sensitivityX);
				}
			}

			//Release middle mouse button
			if(Input.GetMouseButtonUp(2) && Application.loadedLevelName != "SaveSpot"){
				this.transform.position = originalPosition;
				this.transform.rotation = originalRotation;
			}
		}
	}
	
	void Start ()
	{			
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		player = GameObject.Find ("Player");
	}
}