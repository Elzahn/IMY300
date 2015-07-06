using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {
	
	private NaturalDisasters naturalDisastersScript;

	void Start(){
		naturalDisastersScript = GameObject.Find ("Planet").GetComponent<NaturalDisasters> ();
	}

	void OnTriggerEnter(Collider collider){	//Lose health only when an Earthquake hits
		if (collider.tag == "WorldObject" && naturalDisastersScript.shake > 0) {
			PlayerAttributes playerAttributesScript = this.GetComponent<PlayerAttributes>();
			int healthToLose = (int)(playerAttributesScript.currentHealth() * 0.02);
			playerAttributesScript.loseHP(healthToLose);//loses 5% health when warping
			print ("You lose " + healthToLose + " health");
		}
	}
}
