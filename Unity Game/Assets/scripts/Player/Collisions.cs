using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {

	private PlayerAttributes playerAttributesScript;
	private PlayerController playerScript;
	public bool showLootConfirmation{ get; private set; }
	private GameObject colObj;

	public void setLootConf(){
		showLootConfirmation = false;
	}

	void Start(){
		playerAttributesScript = this.GetComponent<PlayerAttributes> ();
		playerScript = this.GetComponent<PlayerController> ();
	}

	void Update () {
		if(showLootConfirmation && Input.GetKeyDown(KeyCode.E)){
			colObj.GetComponent<Loot>().showMyLoot();
		}
	}
	
	void OnTriggerEnter(Collider col){
		if (col.tag == "Loot") {
			showLootConfirmation = true;
			colObj = col.gameObject;
		}
	}
	
	void OnTriggerExit(Collider col){
		showLootConfirmation = false;
	}

	void OnCollisionEnter (Collision col){	
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();

		//Lose health only when an Earthquake hits
		if (col.collider.tag == "WorldObject" && NaturalDisasters.shake > 0) {
			int healthToLose = (int)(playerAttributesScript.hp * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when hit
			PlayerLog.addStat ("You lose " + healthToLose + " health");
		} else if (col.collider.tag == "MediumHealthPack") {
			if (playerAttributesScript.inventory.Count < playerAttributesScript.inventorySize) {
				playerAttributesScript.addToInventory (new MediumHealthPack ());
				Destroy (GameObject.Find (col.collider.transform.parent.name));
				this.GetComponent<Sounds> ().playWorldSound (4);
				//GameObject.Find("Planet").GetComponent<SpawnHealthPacks>().healthPacks.Remove(col.collider.gameObject);
			}
		} else if (col.collider.tag == "LargeHealthPack") {
			if (playerAttributesScript.inventory.Count < playerAttributesScript.inventorySize) {
				playerAttributesScript.addToInventory (new LargeHealthPack ());
				Destroy (GameObject.Find (col.collider.transform.parent.name));
				this.GetComponent<Sounds> ().playWorldSound (4);
				//GameObject.Find("Planet").GetComponent<SpawnHealthPacks>().healthPacks.Remove(col.collider.gameObject);
			}
		} else  //Lose health if you run into something that you can't interact with, walk/run on and isn't a monster
		if (playerScript.run && col.collider.name != "Storage" && col.collider.name != "ExitPlane" && col.collider.name != "EntrancePlane" && col.collider.name != "Platform" && col.collider.name != "Planet" && col.collider.tag != "Monster" && col.collider.tag != "WarpPoint") {
			int healthToLose = (int)(playerAttributesScript.hp * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when running into something
			PlayerLog.addStat ("You lose " + healthToLose + " health by running into something");
			if (playerAttributesScript.gender == 'f') {
				this.GetComponent<Sounds> ().playCharacterSound (6);
			} else {
				this.GetComponent<Sounds> ().playCharacterSound (5);
			}
		} else //play landing sound
		if((col.collider.name == "Platform" || col.collider.name == "Planet") && playerScript.jumping == true) {
			this.GetComponent<Sounds> ().playCharacterSound (4);
			playerScript.jumping = false;
		} 
	}

	void OnGUI(){
		if (showLootConfirmation == true){    
			GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),("Press E to loot"));
		}
	}
}
