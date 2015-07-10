using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {
	
	private NaturalDisasters naturalDisastersScript;
	private PlayerAttributes playerAttributesScript;
	private PlayerController playerScript;

	void Start(){
		naturalDisastersScript = GameObject.Find ("Planet").GetComponent<NaturalDisasters> ();
		playerAttributesScript = GameObject.Find ("Persist").GetComponent<PlayerAttributes> ();
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}

	void OnCollisionEnter (Collision col){	//Lose health only when an Earthquake hits
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (col.collider.tag == "WorldObject" && naturalDisastersScript.shake > 0) {
			int healthToLose = (int)(playerAttributesScript.currentHealth () * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when hit
			print ("You lose " + healthToLose + " health");
		} else if (playerScript.run) {
			int healthToLose = (int)(playerAttributesScript.currentHealth () * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when running into something
			print ("You lose " + healthToLose + " health by running into something");
		}
		else if (col.collider.tag == "MediumHealthPack") {
			if (playerAttributesScript.inventory.Count < playerAttributesScript.getMaxInventory ()) {
				playerAttributesScript.addToInventory (new MediumHealthPack ());
				Destroy (GameObject.Find (col.collider.name));
			}
		}
		else if (col.collider.tag == "LargeHealthPack") {
			if (playerAttributesScript.inventory.Count < playerAttributesScript.getMaxInventory ()) {
				playerAttributesScript.addToInventory (new LargeHealthPack ());
				Destroy (GameObject.Find (col.collider.name));
			}
		}
	}
}
