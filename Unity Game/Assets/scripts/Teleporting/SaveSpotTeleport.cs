using UnityEngine;
using System.Collections;

public class SaveSpotTeleport : MonoBehaviour {

	private bool showExitConfirmation, showEntranceConfirmation;
	public static bool canEnterSaveSpot = false;
	private PlayerController playerScript;
	private PlayerAttributes attributesScript;
	private Sounds sound;

	// Use this for initialization
	void Start () {
		showExitConfirmation = false;
		showEntranceConfirmation = false;
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		sound = GameObject.Find ("Player").GetComponent<Sounds> ();
	}

	public void setEnterSaveSpot()
	{
		canEnterSaveSpot = true;
	}
	
	void OnCollisionEnter (Collision col){
		if (col.collider.name == "ExitPlane") {
			playerScript.setPaused (true);	//Pause game
			showExitConfirmation = true;
		} else if (col.collider.name == "EntrancePlane" && canEnterSaveSpot) {
			playerScript.setPaused (true);	//Pause game
			showEntranceConfirmation = true;
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
			int boxWidth = 400;
			int boxHeight = 250;
			int left = (int)Screen.width/2 - boxWidth/2;//200;
			int top = (int)Screen.height/2 - boxHeight/2;//30;
			int itemHeight = 30;//20;
			int buttonWidth = 150;
			GUI.Box (new Rect (left, top, boxWidth, boxHeight), "All set to go outside? Remember you can only \ncome back once the level has been cleared.");
			
			if (GUI.Button (new Rect (left + boxWidth/2 - buttonWidth/2, top + boxHeight/2 - itemHeight, buttonWidth, itemHeight), "Go outside")) {
				resume ();
				this.transform.position = new Vector3(0.63f, 21.9f, 1.68f);
				this.transform.rotation = new Quaternion(4.336792f, -0.0001220703f, 0.3787689f, 1);
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
			}
		} else if (showEntranceConfirmation) {

			//x, y top, length, height
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
			}
		}
	}
}
