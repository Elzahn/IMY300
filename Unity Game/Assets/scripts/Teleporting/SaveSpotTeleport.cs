using UnityEngine;
using System.Collections;

public class SaveSpotTeleport : MonoBehaviour {

	private bool showExitConfirmation, showEntranceConfirmation, showNoEntry;
	public static bool canEnterSaveSpot = false;
	private PlayerController playerScript;
	private PlayerAttributes attributesScript;
	private Sounds sound;
	private GameObject colObj;

	// Use this for initialization
	void Start () {
		showExitConfirmation = false;
		showNoEntry = false;
		showEntranceConfirmation = false;
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		sound = GameObject.Find ("Player").GetComponent<Sounds> ();
	}

	public void setEnterSaveSpot(){
		canEnterSaveSpot = true;
	}

	void OnTriggerEnter(Collider col){
		if (col.name == "ExitPlane") {
			showExitConfirmation = true;
			colObj = col.gameObject;
		} else if (col.name == "EntrancePlane" && canEnterSaveSpot) {
			showEntranceConfirmation = true;
			colObj = col.gameObject;
		} else if (col.name == "EntrancePlane") {
			showNoEntry = true;
		}
	}
	
	void OnTriggerExit(Collider col){
		showExitConfirmation = false;
		showEntranceConfirmation = false;
		showNoEntry = false;
	}

	void Update () {
		if (showExitConfirmation && Input.GetKeyDown (KeyCode.E)) {
			showExitConfirmation = false;
			this.GetComponent<Rigidbody> ().mass = 0.8f;
			sound.playWorldSound (3);
			/*if (playerScript.run) {
				playerScript.moveSpeed = 10;
				playerScript.run = false;
			}*/
			attributesScript.saveInventoryAndStorage ();
			Application.LoadLevel ("Scene");
		} else if (showEntranceConfirmation && Input.GetKeyDown (KeyCode.E)) {
			showEntranceConfirmation = false;
			attributesScript.restoreHealthToFull();
			attributesScript.restoreStaminaToFull();
			canEnterSaveSpot = false;
			this.GetComponent<Rigidbody>().mass = 100;
			this.transform.rotation = new Quaternion(0, 0.7f, 0, -0.7f);
			
			sound.playWorldSound(3);
			/*if(playerScript.run){
				playerScript.moveSpeed = 10;
				playerScript.run = false;
			}*/
			Application.LoadLevel ("SaveSpot");
		}
	}
	/*void OnCollisionEnter (Collision col){
		if (col.collider.name == "ExitPlane") {
			playerScript.setPaused (true);	//Pause game
			showExitConfirmation = true;
		} else if (col.collider.name == "EntrancePlane" && canEnterSaveSpot) {
			playerScript.setPaused (true);	//Pause game
			showEntranceConfirmation = true;
		}
	}*/

/*	void resume(){
		playerScript.setPaused(false);	//Pause game
		showExitConfirmation = false;
		showEntranceConfirmation = false;
	}*/

	void OnGUI(){
		if (showNoEntry) {
			GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Kill the boss to go back."));
		} else if (showExitConfirmation) {
			GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Press E to go outside. Remember you can only come back once the level has been cleared."));
		
		/*	//x, y top, length, height
			int boxWidth = 400;
			int boxHeight = 250;
			int left = (int)Screen.width/2 - boxWidth/2;//200;
			int top = (int)Screen.height/2 - boxHeight/2;//30;
			int itemHeight = 30;//20;
			int buttonWidth = 150;
			GUI.Box (new Rect (left, top, boxWidth, boxHeight), "All set to go outside? Remember you can only \ncome back once the level has been cleared.");

			if (GUI.Button (new Rect (left + boxWidth/2 - buttonWidth/2, top + boxHeight/2 - itemHeight, buttonWidth, itemHeight), "Go outside")) {
				resume ();
				//this.transform.position = new Vector3(0.63f, 20.65f, 1.68f);
			//	this.transform.rotation = new Quaternion(4.336792f, -0.0001220703f, 0.3787689f, 1);
				this.GetComponent<Rigidbody>().mass = 0.8f;
				sound.playWorldSound(3);
				if(playerScript.run){
					playerScript.moveSpeed = 10;
					playerScript.run = false;
				}
				attributesScript.saveInventoryAndStorage();
				Application.LoadLevel ("Scene");
			}

			if (GUI.Button (new Rect (left + boxWidth/2 - buttonWidth/2, top + boxHeight/2 + itemHeight/2, buttonWidth, itemHeight), "Stay here")) {
				sound.playWorldSound(2);
				resume ();
			}*/
		} else if (showEntranceConfirmation) {
			GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),("Press E to enter. Remember once you have entered coming back starts the next level."));
		/*	//x, y top, length, height
			int boxWidth = 400;
			int boxHeight = 250;
			int left = (int)Screen.width/2 - boxWidth/2;//200;
			int top = (int)Screen.height/2 - boxHeight/2;//30;
			int itemHeight = 30;//20;
			int buttonWidth = 150;

			GUI.Box (new Rect (left, top, boxWidth, boxHeight), "All done exploring? Remember once you have entered \ncoming back starts the next level.");
			
			if (GUI.Button (new Rect (left + boxWidth/2 - buttonWidth/2, top + boxHeight/2 - itemHeight, buttonWidth, itemHeight), "Go inside")) {
				resume ();
				attributesScript.restoreHealthToFull();
				attributesScript.restoreStaminaToFull();
				canEnterSaveSpot = false;
				this.GetComponent<Rigidbody>().mass = 100;
				//this.transform.position = new Vector3(-132.81f, 35.4f, 2.79f);
				this.transform.rotation = new Quaternion(0, 0.7f, 0, -0.7f);

				sound.playWorldSound(3);
				if(playerScript.run){
					playerScript.moveSpeed = 10;
					playerScript.run = false;
				}
				Application.LoadLevel ("SaveSpot");
			}
			
			if (GUI.Button (new Rect (left + boxWidth/2 - buttonWidth/2, top + boxHeight/2 + itemHeight/2, buttonWidth, itemHeight), "Stay here")) {
				sound.playWorldSound(2);
				resume ();
			}*/
		//}
		}
	}
}
