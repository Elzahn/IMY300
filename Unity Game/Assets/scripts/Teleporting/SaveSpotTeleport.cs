using UnityEngine;
using System.Collections;

public class SaveSpotTeleport : MonoBehaviour {

	private bool showExitConfirmation, showEntranceConfirmation, showNoEntry;
	public static bool canEnterSaveSpot = false;
	private PlayerAttributes attributesScript;
	private Sounds sound;

	// Use this for initialization
	void Start () {
		showExitConfirmation = false;
		showNoEntry = false;
		showEntranceConfirmation = false;
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		sound = GameObject.Find ("Player").GetComponent<Sounds> ();
	}

	public void setEnterSaveSpot(){
		canEnterSaveSpot = true;
	}

	void OnTriggerEnter(Collider col){
		if (col.name == "ExitPlane") {
			showExitConfirmation = true;
		} else if (col.name == "EntrancePlane" && canEnterSaveSpot) {
			showEntranceConfirmation = true;
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
			attributesScript.saveInventoryAndStorage ();
			Application.LoadLevel ("Scene");
			PlayerAttributes playerAttributes = GameObject.Find ("Player").GetComponent<PlayerAttributes>();
			playerAttributes.stamina = playerAttributes.maxStamina ();
		} else if (showEntranceConfirmation && Input.GetKeyDown (KeyCode.E)) {
			showEntranceConfirmation = false;
			attributesScript.restoreHealthToFull();
			attributesScript.restoreStaminaToFull();
			canEnterSaveSpot = false;
			this.GetComponent<Rigidbody>().mass = 100;
			this.transform.rotation = new Quaternion(0, 0.7f, 0, -0.7f);
			
			sound.playWorldSound(3);
			Application.LoadLevel ("SaveSpot");
		}
	}

	void OnGUI(){
		if (showNoEntry) {
			GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Kill the boss to go back."));
		} else if (showExitConfirmation) {
			GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Press E to go outside. Remember you can only come back once the level has been cleared."));
		} else if (showEntranceConfirmation) {
			GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),("Press E to enter. Remember once you have entered coming back starts the next level."));
		}
	}
}
