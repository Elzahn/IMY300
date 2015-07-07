using UnityEngine;
using System.Collections;

public class SaveSpotTeleport : MonoBehaviour {

	private bool showExitConfirmation, showEntranceConfirmation, canEnterSaveSpot;
	private PlayerController playerScript;

	// Use this for initialization
	void Start () {
		showExitConfirmation = false;
		showEntranceConfirmation = false;
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}

	public void setEnterSaveSpot()
	{
		canEnterSaveSpot = true;
	}
	
	void OnTriggerEnter(Collider collider){
		if (collider.name == "ExitPlane") {
			playerScript.setPaused (true);	//Pause game
			showExitConfirmation = true;
		} else if (collider.name == "EntrancePlane" && canEnterSaveSpot) {
			playerScript.setPaused (true);	//Pause game
			showEntranceConfirmation = true;
		} else if (collider.name == "Storage") {
			print ("Storage");
		}
	}

	void resume(){
		playerScript.setPaused(false);	//Pause game
		showExitConfirmation = false;
		showEntranceConfirmation = false;
	}

	void OnGUI()
	{
		if (showExitConfirmation) {
			
			//x, y top, length, height
			GUI.Box (new Rect (200, 30, 400, 250), "All set to go outside? Remember you can only \ncome back once the level has been cleared.");
			
			if (GUI.Button (new Rect (320, 90, 150, 20), "Go outside")) {
				resume ();
				Application.LoadLevel ("Scene");
			}

			if (GUI.Button (new Rect (320, 120, 150, 20), "Stay here")) {
				resume ();
			}
		} else if (showEntranceConfirmation) {

			//x, y top, length, height
			GUI.Box (new Rect (200, 30, 400, 250), "All done exploring? Remember once you have entered \ncoming back starts the next level.");
			
			if (GUI.Button (new Rect (320, 90, 150, 20), "Go inside")) {
				resume ();
				canEnterSaveSpot = false;
				Application.LoadLevel ("SaveSpot");
			}
			
			if (GUI.Button (new Rect (320, 120, 150, 20), "Stay here")) {
				resume ();
			}
		}
	}
}
