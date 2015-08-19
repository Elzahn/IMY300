using UnityEngine;
using System.Collections;

public class SaveSpotTeleport : MonoBehaviour {

	private bool showExitConfirmation, showEntranceConfirmation, showNoEntry;
	public bool canEnterSaveSpot{ get; set; }
	public bool loadTutorial {get; set;}
	private PlayerAttributes attributesScript;
	private Sounds sound;

	//Used only for the cheat
	public void setExitConf(bool val){
		showExitConfirmation = val;
	}

	// Use this for initialization
	void Start () {
		loadTutorial = true;
		canEnterSaveSpot = false;
		showExitConfirmation = false;
		showNoEntry = false;
		showEntranceConfirmation = false;
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		sound = GameObject.Find ("Player").GetComponent<Sounds> ();
	}

	void OnTriggerEnter(Collider col){
		if (col.name == "ExitPlane"  && canEnterSaveSpot) {
			showExitConfirmation = true;
		} else if (col.name == "EntrancePlane" && canEnterSaveSpot) {
			showEntranceConfirmation = true;
		} else if (col.name == "ExitPlane"  && canEnterSaveSpot && loadTutorial) {

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
		if (showExitConfirmation && Input.GetKeyDown (KeyCode.E) && !loadTutorial) {
			canEnterSaveSpot = false;
			showExitConfirmation = false;
			this.GetComponent<Rigidbody> ().mass = 0.1f;
			sound.playWorldSound (Sounds.SHIP_DOOR);
			attributesScript.saveInventoryAndStorage ();
			//GameObject.Find("Player").transform.position = new Vector3(9.41f, 79.19f, 7.75f);
			Application.LoadLevel ("Scene");
			Resources.UnloadUnusedAssets();
		} else if(showExitConfirmation && Input.GetKeyDown(KeyCode.E) && loadTutorial){
			canEnterSaveSpot = false;
			showExitConfirmation = false;
			this.GetComponent<Rigidbody> ().mass = 0.1f;
			sound.playWorldSound (Sounds.SHIP_DOOR);
			attributesScript.saveInventoryAndStorage ();
			this.transform.position = new Vector3(0f, 15.03f, 0);
			Application.LoadLevel("Tutorial");
			Resources.UnloadUnusedAssets();
		}else if (showEntranceConfirmation && Input.GetKeyDown (KeyCode.E)) {
			//showExitConfirmation = true;
			showEntranceConfirmation = false;
			attributesScript.restoreHealthToFull();
			attributesScript.restoreStaminaToFull();
			this.GetComponent<Rigidbody>().mass = 100;
			this.transform.rotation = new Quaternion(0, 0.7f, 0, -0.7f);
			//this.transform.position = new Vector3 (-27.01f, 79.65f, 1.93f);
			this.transform.position = new Vector3 (9.4f, 81.38f, 6.62f);
			sound.playWorldSound(Sounds.SHIP_DOOR);
			Application.LoadLevel ("SaveSpot");
			Resources.UnloadUnusedAssets();
		}
	}

	void OnGUI(){
		if (showNoEntry) {
			GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Kill the boss to go back."));
		} else if (showExitConfirmation && loadTutorial) {
			GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Press E to start the tutorial"));
		} else if (showExitConfirmation){
			GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Press E to go outside. Remember you can only come back once the level has been cleared."));
		} else if (showEntranceConfirmation) {
			GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),("Press E to enter. Remember once you have entered coming back starts the next level."));
		}
	}
}
