﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour {

	public float sensitivityX{ get; set;}
	public bool birdsEye { get; private set;}

	private GameObject player;
	private Vector3 originalPosition;
	private Quaternion originalRotation;
	private PlayerController playerScript;

	void Start ()
	{	
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<PlayerController> ();
		birdsEye = false;
		sensitivityX = 3F;
	}

	void Update ()
	{
		if (playerScript.paused == false && Application.loadedLevelName != "Main_Menu") {

			if(!birdsEye){
				if (Input.GetMouseButton (1) && Input.GetAxis ("Mouse X") < 0)//right button held and mouse moved left
				{
					//Rotate Around the player - no player gameObject Rotation
					/*Transform player = GameObject.FindWithTag("Player").transform;
					transform.RotateAround(player.transform.position, player.up, Input.GetAxis ("Mouse X") * sensitivityX);*/

					//Rotate Around the player - player gameObject Rotation
					player.transform.RotateAround(player.transform.position, player.transform.up, Input.GetAxis ("Mouse X") * sensitivityX);
					GameObject.Find("Character_Final").GetComponent<Animator>().SetFloat("Turning", -1f);
				} else if (Input.GetMouseButton (1) && Input.GetAxis ("Mouse X") > 0) {
					player.transform.RotateAround(player.transform.position, player.transform.up, Input.GetAxis ("Mouse X") * sensitivityX);
					GameObject.Find("Character_Final").GetComponent<Animator>().SetFloat("Turning", 1f);
				} else {
					GameObject.Find("Character_Final").GetComponent<Animator>().SetFloat("Turning", 0f);
				}
			}

			if(Input.GetMouseButtonUp(1)){
				GameObject.Find("Character_Final").GetComponent<Animator>().SetFloat("Turning", 0f);
			}
			//Zoom
			if(!birdsEye){
			//	transform.Translate (Vector3.forward * Input.GetAxis ("Mouse ScrollWheel"));
			}
	
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
				if(player.GetComponent<PlayerController>().moveDir != Vector3.zero){
					player.GetComponent<PlayerController>().moveDir = Vector3.zero;
				}

				originalPosition = this.transform.position;
				originalRotation = this.transform.rotation;

				this.transform.position = GameObject.Find("Player").transform.position + (GameObject.Find("Player").transform.up * 25);
				this.transform.LookAt(GameObject.Find("Player").transform);

				birdsEye = true;
			}

			if(birdsEye){
				GameObject.Find("BirdsEye").GetComponent<Canvas>().enabled = true;

				string stats = "";

				if(Application.loadedLevelName != "Tutorial"){
					stats += GameObject.Find ("Planet").GetComponent<EnemySpawner>().enemiesStats();
				} else {
					stats += GameObject.Find ("Planet").GetComponent<TutorialSpawner>().enemiesStats();
				}

				GameObject.Find("BirdsEye").transform.FindChild("Background").FindChild("EnemyStats").GetComponent<Text>().text = stats;
			} else {
				GameObject.Find("BirdsEye").GetComponent<Canvas>().enabled = false;
			}

			if(Input.GetMouseButton(2) && Application.loadedLevelName != "SaveSpot"){
				if(Input.GetAxis("Mouse X") != 0){
					this.transform.RotateAround(new Vector3(0,0,0), -player.transform.forward, Input.GetAxis ("Mouse X") * sensitivityX);
				} 
				if(Input.GetAxis("Mouse Y") != 0){
					this.transform.RotateAround(new Vector3(0,0,0), -player.transform.right, Input.GetAxis ("Mouse Y") * sensitivityX);
				}
			}

			//Release middle mouse button
			if(Input.GetMouseButtonUp(2) && Application.loadedLevelName != "SaveSpot"){
				this.transform.position = originalPosition;
				this.transform.rotation = originalRotation;

				//BirdsEye is disabled in the else above
				birdsEye = false;
			}
		}
	}
}