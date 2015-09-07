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

	private bool showExitConfirmation, showEntranceConfirmation, showNoEntry;
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

		if (showExitConfirmation && loadTutorial) {
			notInUse = false;
			Hud.makeInteractionHint ("Press E to start the tutorial", pressE);
			//interaction_Button.sprite = pressE;
			//interaction_Button.GetComponent<Mask> ().enabled = false;
			/*if(!textAdded){
				expandText.text += "\nPress E to start the tutorial";
				Canvas.ForceUpdateCanvases();
				scrollbar.value = 0f;
				textAdded = true;
			}*/
		} else if (showNoEntry && !loadTutorial && !showedHealthHint) {
			notInUse = false;
			Hud.makeInteractionHint ("Kill the boss and take his loot to go back to the ship.", noEntry);
			/*interaction_Button.sprite = noEntry;
			interaction_Button.GetComponent<Mask> ().enabled = false;*/
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
			//showExitConfirmation = true;
			canEnterSaveSpot = false;
			GameObject.Find("Player").GetComponent<PlayerAttributes>().storage.AddLast(new Cupcake());
			showEntranceConfirmation = false;
			sound.playWorldSound(Sounds.TELEPORTING);
			attributesScript.restoreHealthToFull();
			attributesScript.restoreStaminaToFull();
			this.GetComponent<Rigidbody>().mass = 100;
			this.transform.rotation = new Quaternion(0, 0.7f, 0, 0.7f);
			//this.transform.position = new Vector3 (-27.01f, 79.65f, 1.93f);
			this.transform.position = new Vector3 (13.72f, 81.58f, 14.77f);//(9.4f, 81.38f, 6.62f);

			this.GetComponent<LevelSelect>().currentLevel++;
			interaction.fillAmount = 0;
			//this.transform.up = GameObject.Find("Floor").transform.up;
			Application.LoadLevel ("SaveSpot");
			Resources.UnloadUnusedAssets();
		} else if (showExitConfirmation && Input.GetKeyDown (KeyCode.E) && !loadTutorial) {
			canEnterSaveSpot = false;
			showExitConfirmation = false;
			this.GetComponent<Rigidbody> ().mass = 0.1f;
			sound.playWorldSound (Sounds.TELEPORTING);
			attributesScript.saveInventoryAndStorage ();
			//GameObject.Find("Player").transform.position = new Vector3(9.41f, 79.19f, 7.75f);
			this.GetComponent<Tutorial>().stopTutorial();
			interaction.fillAmount = 0;
			this.GetComponent<Tutorial> ().startTutorial = false;

			Application.LoadLevel ("Scene");
			Resources.UnloadUnusedAssets();
		} else if(showExitConfirmation && Input.GetKeyDown(KeyCode.E) && loadTutorial){
			canEnterSaveSpot = false;

			this.GetComponent<Rigidbody> ().mass = 0.1f;
			sound.playWorldSound (Sounds.TELEPORTING);
			attributesScript.saveInventoryAndStorage ();
			//this.transform.position = new Vector3(0f, 15.03f, 0);
			//this.transform.position = new Vector3(-0.01f, 16.149f, -0.27f);
			interaction.fillAmount = 0;
			sound.stopSound("computer");
			this.GetComponent<LevelSelect>().currentLevel = 0;
			interaction.fillAmount = 0;
			Application.LoadLevel("Tutorial");
			showExitConfirmation = false;
			//Set it once Run hint pasts	loadTutorial = false;
			this.transform.position = new Vector3(0, 16f, -0.327f);
			Resources.UnloadUnusedAssets();
		}else if (showEntranceConfirmation && Input.GetKeyDown (KeyCode.E) && Application.loadedLevelName != "Tutorial") {
			//showExitConfirmation = true;

			showEntranceConfirmation = false;
			attributesScript.restoreHealthToFull();
			attributesScript.restoreStaminaToFull();
			this.GetComponent<Rigidbody>().mass = 100;
			this.transform.rotation = new Quaternion(0, 0.7f, 0, -0.7f);
			//this.transform.position = new Vector3 (-27.01f, 79.65f, 1.93f);
			this.transform.position = new Vector3 (13.72f, 81.58f, 14.77f);//(9.4f, 81.38f, 6.62f);

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

	/*void OnGUI(){
		if (showNoEntry) {
			//GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Kill the boss and take his loot to go back."));
		} else if (showExitConfirmation && loadTutorial) {
			//GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Press E to start the tutorial"));
		} else if (showExitConfirmation){
			//GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Press E to go outside. Remember you can only come back once the level has been cleared."));
		} else if (showEntranceConfirmation) {
			//GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),("Press E to enter. Remember once you have entered coming back starts the next level."));
		}
	}*/
}
