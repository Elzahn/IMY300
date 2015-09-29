using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveSpotTeleport : MonoBehaviour {
	/*bool _d = true;
	public bool canEnterSaveSpot {
		get{ return _d; }
		set{ _d = value;
			print (_d); }
	}*/
	public bool canEnterSaveSpot{ get; set; }
	public bool loadTutorial {get; set;}
	public bool notInUse {get; set;}
	public bool showedHealthHint {get; set;}

	private bool showExitConfirmation, showEntranceConfirmation, showNoEntry, justWarped;
	private PlayerAttributes attributesScript;
	private Sounds sound;

	public Sprite pressE;
	public Sprite noEntry;

	private Image interaction;
	private Image hint;

	private HUD Hud;

	//Used only for the cheat
	public void setExitConf(bool val){
		showExitConfirmation = val;
		hint.fillAmount = 0;
		interaction.fillAmount = 0;
	}

	// Use this for initialization
	void Start () {
		showedHealthHint = false;
		justWarped = false;
		notInUse = true;
		Hud = Camera.main.GetComponent<HUD> ();
		hint = GameObject.Find ("Hint").GetComponent<Image> ();
		interaction = GameObject.Find ("Interaction").GetComponent<Image> ();
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
		if (Application.loadedLevelName == "SaveSpot" && this.GetComponent<Tutorial> ().startTutorial) {
//			canEnterSaveSpot = false;
			GameObject.Find ("Tech Light").GetComponent<Light> ().enabled = true;
			GameObject.Find ("Console Light").GetComponent<Light> ().enabled = true;
			GameObject.Find ("Bedroom Light").GetComponent<Light> ().enabled = true;
		} else if(Application.loadedLevelName == "SaveSpot" && !this.GetComponent<Tutorial> ().startTutorial){
			GameObject.Find ("Tech Light").GetComponent<Light> ().enabled = false;
			GameObject.Find ("Console Light").GetComponent<Light> ().enabled = false;
			GameObject.Find ("Bedroom Light").GetComponent<Light> ().enabled = false;
		}

		//Position player when warping into the ship
		if(Application.loadedLevelName == "SaveSpot" && this.GetComponent<Tutorial>().tutorialDone && !justWarped){
			justWarped = true;
			this.transform.rotation = Quaternion.Euler(351.66f, 179.447f, 358.8f);
			this.transform.up = Vector3.up;
			this.transform.position = new Vector3 (12.99f, 81.45f, 15.25f);
		}

		if (showExitConfirmation && loadTutorial) {
			notInUse = false;
			Hud.makeInteractionHint ("Press E to start the tutorial", pressE);
		} else if (showNoEntry && !loadTutorial && showedHealthHint) {
			notInUse = false;
			Hud.makeInteractionHint ("Kill the boss and take his loot to go back to the ship.", noEntry);
		} else if (showEntranceConfirmation) {
			notInUse = false;
			Hud.makeInteractionHint ("Press E to teleport. Remember coming back starts the next level.", pressE);
		} else if(showExitConfirmation){
			notInUse = false;
			Hud.makeInteractionHint ("Press E to teleport. Remember you can only return upon killing the boss.", pressE);
		} else if(!showExitConfirmation && !showNoEntry && !showExitConfirmation && !showEntranceConfirmation) {
			notInUse = true;
		}

		if (Application.loadedLevelName == "Tutorial" && showEntranceConfirmation && Input.GetKeyDown (KeyCode.E) && Loot.gotPowerCore == true) {
			canEnterSaveSpot = false;
			GameObject.Find("Player").GetComponent<PlayerAttributes>().storage.AddLast(new Cupcake());
			showEntranceConfirmation = false;
			sound.playWorldSound(Sounds.TELEPORTING);
			attributesScript.restoreHealthToFull();
			attributesScript.restoreStaminaToFull();
			this.GetComponent<Rigidbody>().mass = 100;

			this.GetComponent<LevelSelect>().currentLevel++;
			interaction.fillAmount = 0;
			Application.LoadLevel ("SaveSpot");
			Resources.UnloadUnusedAssets();
		} else if (showExitConfirmation && Input.GetKeyDown (KeyCode.E) && !loadTutorial) {
			canEnterSaveSpot = false;
			showExitConfirmation = false;
			this.GetComponent<Rigidbody> ().mass = 0.1f;
			sound.playWorldSound (Sounds.TELEPORTING);
			attributesScript.saveInventoryAndStorage ();
			this.GetComponent<Tutorial>().stopTutorial();
			interaction.fillAmount = 0;
			this.GetComponent<Tutorial> ().startTutorial = false;
			justWarped = false;
			this.GetComponent<Rigidbody>().isKinematic = true;
			//this.transform.rotation = Quaternion.Euler(0f, -95.3399f, 0f);
			//this.transform.position = new Vector3 (-1.651f, 80.82f, 0.84f);

			Application.LoadLevel ("Scene");
			Resources.UnloadUnusedAssets();
		} else if(showExitConfirmation && Input.GetKeyDown(KeyCode.E) && loadTutorial){
			canEnterSaveSpot = false;

			this.GetComponent<Rigidbody> ().mass = 0.1f;
			sound.playWorldSound (Sounds.TELEPORTING);
			attributesScript.saveInventoryAndStorage ();
			interaction.fillAmount = 0;
			sound.stopSound("computer");
			this.GetComponent<LevelSelect>().currentLevel = 0;
			interaction.fillAmount = 0;
			justWarped = false;
			Application.LoadLevel("Tutorial");
			showExitConfirmation = false;
			this.transform.rotation = Quaternion.Euler(0f, 91.60388f, 0f);
			this.transform.position = new Vector3 (0.26f, 16.06f, 0.316f);
			Resources.UnloadUnusedAssets();
		}else if (showEntranceConfirmation && Input.GetKeyDown (KeyCode.E) && Application.loadedLevelName != "Tutorial") {
			showEntranceConfirmation = false;
			attributesScript.restoreHealthToFull();
			attributesScript.restoreStaminaToFull();
			this.GetComponent<Rigidbody>().mass = 100;

			this.GetComponent<LevelSelect>().currentLevel++;
			this.GetComponent<LevelSelect>().spawnedLevel = false;
			this.GetComponent<LevelSelect>().myRenderer = null;
			interaction.fillAmount = 0;
			this.GetComponent<Tutorial> ().startTutorial = false;
			Application.LoadLevel ("SaveSpot");
			sound.playWorldSound(Sounds.TELEPORTING);
			Resources.UnloadUnusedAssets();
		}
	}
}
