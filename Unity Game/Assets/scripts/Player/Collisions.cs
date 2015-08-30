using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {

	private PlayerAttributes playerAttributesScript;
	private PlayerController playerScript;
	public bool showLootConfirmation{ get; private set; }
	public bool showHealthConfirmation{ get; private set; }
	private GameObject colObj;

	public void setLootConf(){
		showLootConfirmation = false;
	}

	void Start(){
		playerAttributesScript = this.GetComponent<PlayerAttributes> ();
		playerScript = this.GetComponent<PlayerController> ();
	}

	void Update () {
		if (showLootConfirmation && Input.GetKeyDown (KeyCode.E)) {
			colObj.GetComponent<Loot> ().showMyLoot ();
		} else if (showHealthConfirmation && Input.GetKeyDown(KeyCode.E) && colObj.tag == "MediumHealthPack") {
			if (playerAttributesScript.inventory.Count < playerAttributesScript.inventorySize) {
				playerAttributesScript.addToInventory (new MediumHealthPack ());
				this.GetComponent<Sounds> ().playWorldSound (Sounds.HEALTH_COLLECTION);
				
				print (colObj);
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
				print (colObj);
				Vector3 tempPos = colObj.transform.position;
				print (tempPos);
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
			colObj = col.gameObject;
		} else if (col.tag == "MediumHealthPack" || col.tag == "LargeHealthPack") {
			showHealthConfirmation = true;
			colObj = col.gameObject;
		}
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
		}/* else //play landing sound
		if((col.collider.name == "Ship_interior" || col.collider.name == "Planet") && playerScript.jumping == true) {
			this.GetComponent<Sounds> ().playCharacterSound (Sounds.JUMP);
			playerScript.jumping = false;
		} */
	}

	void OnGUI(){
		if (showLootConfirmation) {    
			GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Press E to loot"));
		} else if (showHealthConfirmation) {
			GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), ("Press E for HealthPack"));
		}
	}
}
