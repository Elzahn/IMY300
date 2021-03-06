﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnHealthPacks : MonoBehaviour {

	public GameObject medHealth;
	public GameObject largeHealth;

	public int TOTAL_HEALTH{ get; set; }// = 10;

	LinkedList<GameObject> healthPacks = new LinkedList <GameObject> ();

	// Use this for initialization
	/*void Start () {
		spawn ();
	}*/

	public int amountHealthLanded(){
		int done = 0;
		for (int i = 0; i < 5; i++) {
			done = 0;
			foreach (GameObject pack in healthPacks) {
				if (pack.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll) {
					done++;
				}
			}
		}
		
		return done;
	}

	public bool hasHealthLanded(){
		/*bool done = true;
		foreach (GameObject healthPack in healthPacks) {
			if (done && healthPack.GetComponent<Rigidbody> ().constraints == RigidbodyConstraints.FreezeAll) {
				done = true;
			} else {
				done = false;
			}
		}
		if (done) {
			print ("Healthpacks placed");
		}*/

		bool done = true;
		
		for (int i = 0; i < 3; i++) {
			if (amountHealthLanded () == TOTAL_HEALTH && done == true) {
				done = true;
			} else {
				done = false;
			}
		}

		return done;
	}

	public void removeHealth(GameObject health){
		healthPacks.Remove (health);
	}

	public void position(GameObject go){
		GameObject.Find(go.transform.parent.gameObject.name).GetComponent<PositionMe>().checkMyPosition = false;
		Vector3 position;
		bool placed = false;
		
		while (!placed) {
			
			position = Random.onUnitSphere * (GameObject.Find("Planet").GetComponent<SphereCollider>().radius * GameObject.Find("Planet").transform.lossyScale.x);
			
			Collider[] collidedItems = Physics.OverlapSphere(position, 0.5f);
			List<Collider> tempList = new List<Collider>();
			
			foreach(Collider col in collidedItems){
				if(col.name != "Planet" && col.transform != go.transform){
					tempList.Add(col);
				}
			}
			
			if(tempList.Count() == 0){
				go.transform.parent.gameObject.transform.GetComponent<Rigidbody> ().position = position;
				GameObject.Find(go.transform.parent.gameObject.name).GetComponent<PositionMe>().timeToCheckMyPosition = Time.time;
				GameObject.Find(go.transform.parent.gameObject.name).GetComponent<PositionMe>().checkMyPosition = true;
				return;
			}
		}
	}

	public void spawnHealth (int numHealth)
	{
		TOTAL_HEALTH = numHealth;

		GameObject[] gameObjectsToDelete =  GameObject.FindGameObjectsWithTag ("MediumHealthPack");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}

		gameObjectsToDelete =  GameObject.FindGameObjectsWithTag ("LargeHealthPack");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}

		GameObject planet = GameObject.Find("Planet");

		for (int i = 0; i < TOTAL_HEALTH; i++) {

			GameObject tempHealthPack;

			if(Random.Range(0,2) == 0){
				tempHealthPack = Instantiate(medHealth);
			} else {
				tempHealthPack = Instantiate(largeHealth);
			}

			position(tempHealthPack.transform.FindChild ("Box012").gameObject);//send child through

			tempHealthPack.GetComponent<FauxGravityBody>().attractor = planet.GetComponent<FauxGravityAttractor>();

			healthPacks.AddLast(tempHealthPack);
		}
	}
}
