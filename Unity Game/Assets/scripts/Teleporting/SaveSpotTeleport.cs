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
	private PlayerAttributes attributesComponent;
	private Sounds sound;
	private bool showEndGameSuccess;
	private bool showEndGameFail;

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
		attributesComponent = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		sound = GameObject.Find ("Player").GetComponent<Sounds> ();
	}

	void OnTriggerEnter(Collider col){
		if (col.name == "ExitPlane"  && canEnterSaveSpot) {
			if(attributesComponent.CurrentLevel < 6){
				showExitConfirmation = true;
			} else {
				if(this.GetComponent<Collisions>().totalPieces == 5){
					showEndGameSuccess = true;
				} else {
					showEndGameFail = true;
				}
			}
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
		showEndGameFail = false;
		showEndGameSuccess = false;
	}

	void Update () {
		if (!this.GetComponent<PlayerController> ().paused) {
			if (Application.loadedLevelName == "SaveSpot" && this.GetComponent<Tutorial> ().startTutorial) {
//			canEnterSaveSpot = false;
				GameObject.Find ("Tech Light").GetComponent<Light> ().enabled = true;
				GameObject.Find ("Console Light").GetComponent<Light> ().enabled = true;
				GameObject.Find ("Bedroom Light").GetComponent<Light> ().enabled = true;
			} else if (Application.loadedLevelName == "SaveSpot" && !this.GetComponent<Tutorial> ().startTutorial) {
				GameObject.Find ("Tech Light").GetComponent<Light> ().enabled = false;
				GameObject.Find ("Console Light").GetComponent<Light> ().enabled = false;
				GameObject.Find ("Bedroom Light").GetComponent<Light> ().enabled = false;
			}

			//Position player when warping into the ship
			if (Application.loadedLevelName == "SaveSpot" && this.GetComponent<Tutorial> ().tutorialDone && !justWarped) {
				justWarped = true;
				this.transform.rotation = Quaternion.Euler (351.66f, 179.447f, 358.8f);
				this.transform.up = Vector3.up;
				this.transform.position = new Vector3 (13.18f, 81.55f, 14.8f);
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
			} else if (showExitConfirmation) {
				notInUse = false;
				Hud.makeInteractionHint ("Press E to teleport. Remember you can only return upon killing the boss.", pressE);
			} else if (showEndGameSuccess){
				notInUse = false;
				Hud.makeInteractionHint ("Press E to go home.", pressE);
			} else if (showEndGameFail){
				notInUse = false;
				Hud.makeInteractionHint ("You must first fix the ship.", noEntry);
			} else if (!showExitConfirmation && !showNoEntry && !showExitConfirmation && !showEntranceConfirmation && !showEndGameFail && !showEndGameSuccess) {
				notInUse = true;
			}

	    
			if (Application.loadedLevelName == "Tutorial" && showEntranceConfirmation && Input.GetButtonDown ("Interact") && Loot.gotPowerCore == true) {
				canEnterSaveSpot = false;
				GameObject.Find ("Player").GetComponent<PlayerAttributes> ().storage.AddLast (new Cupcake ());
				showEntranceConfirmation = false;
				sound.playWorldSound (Sounds.TELEPORTING);
				attributesComponent.restoreHealthToFull ();
				attributesComponent.restoreStaminaToFull ();
				this.GetComponent<Rigidbody> ().mass = 100;
				
				sound.stopSound("computer");
				this.GetComponent<Tutorial>().sarcasticRemarks = -1;
				interaction.fillAmount = 0;
				attributesComponent.returnToSaveSpot ();
				unequipWeapon();
			
			} else if (showExitConfirmation && Input.GetButtonDown ("Interact") && !loadTutorial) {
				canEnterSaveSpot = false;
				showExitConfirmation = false;
				this.GetComponent<Rigidbody> ().mass = 0.1f;
				sound.playWorldSound (Sounds.TELEPORTING);
				attributesComponent.saveInventoryAndStorage ();
				this.GetComponent<Tutorial> ().stopTutorial ();
				interaction.fillAmount = 0;
				this.GetComponent<Tutorial> ().startTutorial = false;
				justWarped = false;
				this.GetComponent<Rigidbody> ().isKinematic = true;
				//this.transform.rotation = Quaternion.Euler(0f, -95.3399f, 0f);
				//this.transform.position = new Vector3 (-1.651f, 80.82f, 0.84f);

				Application.LoadLevel ("Scene");
				reEquipWeapon();
				Resources.UnloadUnusedAssets ();
			} else if (showExitConfirmation && Input.GetButtonDown ("Interact") && loadTutorial) {
				canEnterSaveSpot = false;

				this.GetComponent<Rigidbody> ().mass = 0.1f;
				sound.playWorldSound (Sounds.TELEPORTING);
				attributesComponent.saveInventoryAndStorage ();
				interaction.fillAmount = 0;
				sound.stopSound ("computer");
				this.GetComponent<LevelSelect> ().currentLevel = 0;
				interaction.fillAmount = 0;
				justWarped = false;
				Application.LoadLevel ("Tutorial");
				showExitConfirmation = false;
				this.transform.rotation = Quaternion.Euler (0f, 91.60388f, 0f);
				this.transform.position = new Vector3 (0.26f, 16.06f, 0.316f);
				Resources.UnloadUnusedAssets ();
			} else if(showEndGameSuccess && Input.GetButtonDown ("Interact")){
				this.GetComponent<Rigidbody> ().useGravity = true;
				this.GetComponent<FauxGravityBody> ().attractor = null;
				//sound.playWorldSound (Sounds.FINISHED_GAME);
				if(BonusObjectives.endGameKing){
					Application.LoadLevel("EndOfGameKing");
				} else if(BonusObjectives.endGamePlanet){
					Application.LoadLevel("EndOfGamePlanet");
				} else {
					Application.LoadLevel ("EndOfGame");
				}
			} else if (showEntranceConfirmation && Input.GetButtonDown ("Interact") && Application.loadedLevelName != "Tutorial") {
				showEntranceConfirmation = false;
				attributesComponent.restoreHealthToFull ();
				attributesComponent.restoreStaminaToFull ();
				this.GetComponent<Rigidbody> ().mass = 100;

				this.GetComponent<LevelSelect> ().currentLevel++;
				attributesComponent.save (0);
				this.GetComponent<LevelSelect> ().spawnedLevel = false;
				this.GetComponent<LevelSelect> ().myRenderer = null;
				interaction.fillAmount = 0;
				this.GetComponent<Tutorial> ().startTutorial = false;
				Application.LoadLevel ("SaveSpot");
				unequipWeapon();
				sound.playWorldSound (Sounds.TELEPORTING);
				Resources.UnloadUnusedAssets ();
			}
		}
	}

	void reEquipWeapon(){
		if(attributesComponent.weapon != null){
			GameObject.Find("Character_Final").GetComponent<Animator>().SetBool("Weapon", true);
			GameObject weaponPrefab = null;

			if(attributesComponent.weapon.typeID == "ButterKnife"){
				weaponPrefab = Instantiate(attributesComponent.butterKnife);
				weaponPrefab.transform.SetParent(GameObject.Find("mixamorig:RightHandMiddle1").transform);
				weaponPrefab.transform.localScale = new Vector3(10f, 10f, 7);
				weaponPrefab.transform.localPosition = new Vector3(3.4f, 1.11f, 1.02f);
				weaponPrefab.transform.localRotation = Quaternion.Euler(5.536556f, 265.9102f, 359.8019f);
			} else if(attributesComponent.weapon.typeID == "Longsword"){
				weaponPrefab = Instantiate(attributesComponent.longSword);
				weaponPrefab.transform.SetParent(GameObject.Find("mixamorig:RightHandMiddle1").transform);
				weaponPrefab.transform.localScale = new Vector3(10f, 10f, 20f);
				weaponPrefab.transform.localPosition = new Vector3(4.81f, 1.22f, 1.77f);
				weaponPrefab.transform.localRotation = Quaternion.Euler(2.872368f, 264.6271f, 359.299f);
			} else if(attributesComponent.weapon.typeID == "Warhammer"){
				weaponPrefab = Instantiate(attributesComponent.warHammer);
				weaponPrefab.transform.SetParent(GameObject.Find("mixamorig:RightHandMiddle1").transform);
				weaponPrefab.transform.localScale = new Vector3(15, 15, 20);
				weaponPrefab.transform.localPosition = new Vector3(4.149894f, 3.779963f, 3.159975f);
				weaponPrefab.transform.localRotation = Quaternion.Euler(24.22492f, 252.6553f, 311.9116f);
			} else if(attributesComponent.weapon.typeID == "BonusWeapon"){
				weaponPrefab = Instantiate(attributesComponent.longSword);
				weaponPrefab.transform.SetParent(GameObject.Find("mixamorig:RightHandMiddle1").transform);
				weaponPrefab.transform.localScale = new Vector3(10f, 10f, 20f);
				weaponPrefab.transform.localPosition = new Vector3(4.81f, 1.22f, 1.77f);
				weaponPrefab.transform.localRotation = Quaternion.Euler(2.872368f, 264.6271f, 359.299f);
			}
		}
	}
	
	void unequipWeapon(){
		if(attributesComponent.weapon != null){
			GameObject.Find("Character_Final").GetComponent<Animator>().SetBool("Weapon", false);
			
			print (attributesComponent.weapon.typeID);
			if(attributesComponent.weapon.typeID == "ButterKnife"){
				Destroy(GameObject.Find("ButterKnife(Clone)"));
			} else if(attributesComponent.weapon.typeID == "Longsword"){
				Destroy(GameObject.Find("LongSword(Clone)"));
			} else if(attributesComponent.weapon.typeID == "Warhammer"){
				Destroy(GameObject.Find("WarHammer(Clone)"));
			} else if(attributesComponent.weapon.typeID == "BonusWeapon"){
				Destroy(GameObject.Find("LongSword(Clone)"));
			}
		}
	}
}
