using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collisions : MonoBehaviour {
	
	public bool showLootConfirmation{ get; private set; }
	public bool showRestore{ get; set; }
	public static bool showHealthConfirmation{ get; private set; }

	private HUD Hud;
	private Text hudText;
	private PlayerAttributes playerAttributesScript;
	private PlayerController playerScript;
	private GameObject colObj;

	public void setLootConf(){
		showLootConfirmation = false;
	}

	void Start(){
		showRestore = false;
		hudText = GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ();
		Hud = Camera.main.GetComponent<HUD> ();
		playerAttributesScript = this.GetComponent<PlayerAttributes> ();
		playerScript = this.GetComponent<PlayerController> ();
	}

	void Update () {
		if (Application.loadedLevelName == "SaveSpot" && Loot.gotPowerCore && colObj != null && colObj.name == "Console") {
			showRestore = true;
			Hud.makeInteractionHint ("Press E to replace the power core", this.GetComponent<Tutorial> ().PressE);
		}

		if (Application.loadedLevelName == "SaveSpot" && Loot.gotPowerCore && colObj != null && colObj.name == "Console" && Input.GetKeyDown (KeyCode.E)) {
			showRestore = false;
			this.GetComponent<Sounds>().playWorldSound(Sounds.POWER_ON);
			Loot.gotPowerCore = false;
			GameObject.Find ("Player").GetComponent<Sounds> ().playComputerSound (Sounds.COMPUTER_STORAGE);
			hudText.text += "That weapon looks heavy. You can use the closet in your living quarters as a storage area. \nI’ve tracked down the scattered pieces of the spacecraft on various planets of this solar system. With the return of my Power Core I have enough power to teleport you to these planets to retrieve them.\n\n";
			Canvas.ForceUpdateCanvases ();
			Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
			scrollbar.value = 0f;
			GameObject.Find ("Player").GetComponent<Tutorial> ().startTutorial = false;
			GameObject.Find ("Tech Light").GetComponent<Light> ().enabled = false;
			GameObject.Find ("Console Light").GetComponent<Light> ().enabled = false;
			GameObject.Find ("Bedroom Light").GetComponent<Light> ().enabled = false;
			GameObject.Find ("Player").GetComponent<PlayerAttributes> ().inventory.Remove (TutorialSpawner.bossPowerCore);
			this.GetComponent<SaveSpotTeleport> ().canEnterSaveSpot = true;
		}

		if (showLootConfirmation) {
			Hud.makeInteractionHint ("Press E to get loot", this.GetComponent<Tutorial> ().PressE);
		}

		if (showHealthConfirmation) {
			Hud.makeInteractionHint ("Press E to get a health pack", this.GetComponent<Tutorial> ().PressE);
		}

		if (showLootConfirmation && Input.GetKeyDown (KeyCode.E)) {
			colObj.GetComponent<Loot> ().showMyLoot ();
		} else if (showHealthConfirmation && Input.GetKeyDown(KeyCode.E) && colObj.tag == "MediumHealthPack") {
			if (playerAttributesScript.inventory.Count < playerAttributesScript.inventorySize) {
				playerAttributesScript.addToInventory (new MediumHealthPack ());
				this.GetComponent<Sounds> ().playWorldSound (Sounds.HEALTH_COLLECTION);
				showHealthConfirmation = false;
				Vector3 tempPos = colObj.transform.position;
				//Delete health shrub
				GameObject.Find("Planet").GetComponent<SpawnHealthPacks>().removeHealth(colObj);
				Destroy (colObj.transform.gameObject.gameObject);
				
				//Add shrub
				GameObject.Find("Planet").GetComponent<SpawnTrees>().replaceHealth(tempPos);			
			}
		} else if (showHealthConfirmation && Input.GetKeyDown(KeyCode.E) && colObj.tag == "LargeHealthPack") {
			if (playerAttributesScript.inventory.Count < playerAttributesScript.inventorySize) {
				playerAttributesScript.addToInventory (new LargeHealthPack ());
				this.GetComponent<Sounds> ().playWorldSound (Sounds.HEALTH_COLLECTION);
				showHealthConfirmation = false;
				Vector3 tempPos = colObj.transform.position;
//				print (tempPos);
				//Delete health shrub
				GameObject.Find("Planet").GetComponent<SpawnHealthPacks>().removeHealth(colObj);
				Destroy (colObj.transform.gameObject);
				
				//Add shrub
				GameObject.Find("Planet").GetComponent<SpawnTrees>().replaceHealth(tempPos);
			}
		}
	}
	
	void OnTriggerEnter(Collider col){
		if (col.tag == "Loot") {
			showLootConfirmation = true;
		} else if (col.tag == "MediumHealthPack" || col.tag == "LargeHealthPack") {
			showHealthConfirmation = true;
		}
		colObj = col.gameObject;
	}
	
	void OnTriggerExit(Collider col){
		showLootConfirmation = false;
		showHealthConfirmation = false;
	}

	void OnCollisionEnter (Collision col){	
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();

		//Lose health only when an Earthquake hits
		if (col.collider.tag == "WorldObject" && NaturalDisasters.shake > 0) {
			int healthToLose = (int)(playerAttributesScript.hp * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when hit
			//PlayerLog.addStat ("A tree fell on you. You lose " + healthToLose + " health");
		} else  //Lose health if you run into something that you can't interact with, walk/run on and isn't a monster
		if (playerScript.run && col.collider.name != "Storage" && col.collider.name != "ExitPlane" && col.collider.name != "EntrancePlane" && col.collider.name != "Ship_interior" && col.collider.name != "Planet" && col.collider.tag != "Monster" && col.collider.tag != "WarpPoint" && col.collider.tag != "MediumHealthPack" && col.collider.tag != "LargeHealthPack") {
			int healthToLose = (int)(playerAttributesScript.hp * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when running into something
			//PlayerLog.addStat ("You lose " + healthToLose + " health by running into something");
			if (playerAttributesScript.gender == 'f') {
				this.GetComponent<Sounds> ().playCharacterSound (Sounds.FEMALE_HURT);
			} else {
				this.GetComponent<Sounds> ().playCharacterSound (Sounds.MALE_HURT);
			}
		}
	}
}
