using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {

	private PlayerAttributes playerAttributesScript;
	private PlayerController playerScript;
	public bool showLootConfirmation{ get; private set; }
	private GameObject collider;

	void Start(){
		playerAttributesScript = this.GetComponent<PlayerAttributes> ();
		playerScript = this.GetComponent<PlayerController> ();
	}



	void OnCollisionEnter (Collision col){	
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (col.collider.tag == "Loot") {
			showLootConfirmation = true;
			collider = col.collider.gameObject;
		} else {
			showLootConfirmation = false;
		}
		if (col.collider.tag == "WorldObject" && NaturalDisasters.shake > 0) {//Lose health only when an Earthquake hits
			int healthToLose = (int)(playerAttributesScript.currentHealth () * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when hit
			PlayerLog.addStat ("You lose " + healthToLose + " health");
		} else if (col.collider.tag == "MediumHealthPack") {
			if (playerAttributesScript.inventory.Count < playerAttributesScript.getMaxInventory ()) {
				playerAttributesScript.addToInventory (new MediumHealthPack ());
				Destroy (GameObject.Find (col.collider.transform.parent.name));
				this.GetComponent<Sounds> ().playWorldSound (4);
			}
		} else if (col.collider.tag == "LargeHealthPack") {
			if (playerAttributesScript.inventory.Count < playerAttributesScript.getMaxInventory ()) {
				playerAttributesScript.addToInventory (new LargeHealthPack ());
				Destroy (GameObject.Find (col.collider.transform.parent.name));
				this.GetComponent<Sounds> ().playWorldSound (4);
			}
		} else if (playerScript.run && col.collider.name != "Storage" && col.collider.name != "ExitPlane" && col.collider.name != "EntrancePlane" && col.collider.name != "Platform" && col.collider.name != "Planet" && col.collider.tag != "Monster") {
			int healthToLose = (int)(playerAttributesScript.currentHealth () * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when running into something
			PlayerLog.addStat ("You lose " + healthToLose + " health by running into something");
			if (playerAttributesScript.getGender () == 'f') {
				this.GetComponent<Sounds> ().playCharacterSound (6);
			} else {
				this.GetComponent<Sounds> ().playCharacterSound (5);
			}
		} else if((col.collider.name == "Platform" || col.collider.name == "Planet") && playerScript.getJumping() == true) {
			this.GetComponent<Sounds> ().playCharacterSound (4);
			playerScript.setJumping();
		} 
	}

	void OnGUI(){
		int boxWidth = 150;
		int boxHeight = 150;
		int width = boxWidth/2;
		int left = Screen.width/2 - boxWidth/2;
		int top = Screen.height/2 - boxHeight/2;
		int buttonWidth = 30;
		int itemHeight = 30;

		if (showLootConfirmation) {
			GUI.Box (new Rect (left, top, boxWidth, boxHeight), "Press E to loot");
			if (GUI.Button (new Rect (left + width-(buttonWidth*2), top + 80, buttonWidth, itemHeight), "E")) {
				collider.GetComponent<Loot>().showMyLoot();
			}
		}
	}
}
