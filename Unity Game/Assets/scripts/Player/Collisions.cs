using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {
	
	private NaturalDisasters naturalDisastersScript;
	private PlayerAttributes playerAttributesScript;
	void Start(){
		naturalDisastersScript = GameObject.Find ("Planet").GetComponent<NaturalDisasters> ();
		playerAttributesScript = GameObject.Find ("Persist").GetComponent<PlayerAttributes> ();
	}

	void OnTriggerEnter(Collider collider){	//Lose health only when an Earthquake hits
		if (collider.tag == "WorldObject" && naturalDisastersScript.shake > 0) {
			int healthToLose = (int)(playerAttributesScript.currentHealth () * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 5% health when warping
			print ("You lose " + healthToLose + " health");
		} else if (collider.tag == "MediumHealthPack") {
			if(playerAttributesScript.inventory.Count < playerAttributesScript.getMaxInventory()){
				playerAttributesScript.addToInventory(new MediumHealthPack());
				Destroy(GameObject.Find(collider.name));
			}
			/*int healthRegen = (int)(playerAttributesScript.maxHP() * 0.2);
			playerAttributesScript.addHealth(healthRegen);*/
		} else if (collider.tag == "LargeHealthPack") {
			if(playerAttributesScript.inventory.Count < playerAttributesScript.getMaxInventory()){
				playerAttributesScript.addToInventory(new LargeHealthPack());
				Destroy(GameObject.Find(collider.name));
			}
			/*int healthRegen = (int)(playerAttributesScript.maxHP() * 0.4);
			playerAttributesScript.addHealth(healthRegen);*/
			
		}
	}
}
