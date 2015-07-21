﻿using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {

	private PlayerAttributes playerAttributesScript;
	private PlayerController playerScript;

	void Start(){
		playerAttributesScript = this.GetComponent<PlayerAttributes> ();
		playerScript = this.GetComponent<PlayerController> ();
	}

	void OnCollisionEnter (Collision col){	//Lose health only when an Earthquake hits
		print (col.collider.name);
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (col.collider.tag == "Loot") {
			print (col.collider.name);
		} else if (col.collider.tag == "WorldObject" && NaturalDisasters.shake > 0) {
			int healthToLose = (int)(playerAttributesScript.currentHealth () * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when hit
			print ("You lose " + healthToLose + " health");
			PlayerLog.addStat ("You lose " + healthToLose + " health");
		} else if (col.collider.tag == "MediumHealthPack") {
			print (col.collider.name);
			if (playerAttributesScript.inventory.Count < playerAttributesScript.getMaxInventory ()) {
				playerAttributesScript.addToInventory (new MediumHealthPack ());
				Destroy (GameObject.Find (col.collider.name));
				this.GetComponent<Sounds> ().playWorldSound (4);
			}
		} else if (col.collider.tag == "LargeHealthPack") {
		
			if (playerAttributesScript.inventory.Count < playerAttributesScript.getMaxInventory ()) {
				playerAttributesScript.addToInventory (new LargeHealthPack ());
				Destroy (GameObject.Find (col.collider.name));
				this.GetComponent<Sounds> ().playWorldSound (4);
			}
		} else if (playerScript.run && col.collider.name != "Storage" && col.collider.name != "ExitPlane" && col.collider.name != "EntrancePlane" && col.collider.name != "Platform" && col.collider.name != "Planet" && col.collider.tag != "Monster") {
			int healthToLose = (int)(playerAttributesScript.currentHealth () * 0.02);
			playerAttributesScript.loseHP (healthToLose);//loses 2% health when running into something
			print ("You lose " + healthToLose + " health by running into something");
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
}
