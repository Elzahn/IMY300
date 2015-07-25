﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawner : MonoBehaviour {

	public GameObject bossEnemy;

	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject enemy4;

	public GameObject loot;

	private bool giveLoot = false;
	private string EnemyName;
	private int numLoot;
	private PlayerAttributes attributesScript;
	private PlayerController playerScript;
	private Accessory accessoryScript;
	private LinkedList<InventoryItem> tempLoot;
	private InventoryItem tempItem;
	// Use this for initialization

	const int ENEM_COUNT = 20;
	const int NORMAL_ENEMY_TYPES = 4;

	FauxGravityAttractor planet;
	int playerLevel;

	LinkedList<GameObject> enemies = new LinkedList <GameObject> ();

	void Start () {
		clearLoot ();
		tempLoot = new LinkedList<InventoryItem> ();
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();

		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		planet = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
		playerLevel = GameObject.Find("Player").GetComponent<PlayerAttributes>().level;
		GameObject enemy;

		//Spawn Normal Enemies
		for (int i=0; i<ENEM_COUNT -1; ++i) {
			int index = Mathf.RoundToInt(Random.value * NORMAL_ENEMY_TYPES);
			enemy = chooseEnemy(index);
			addEnemy(enemy);
		}

		//Spawn boss.
		addEnemy(bossEnemy);	
	}

	void Update () {
		foreach (GameObject go in enemies.ToList()) {			
			Enemy enemy = go.GetComponent<Enemy>();	

			Rigidbody rigidbody = go.GetComponent<Rigidbody> ();
			if (enemy.isDead()) {
				if(enemy.typeID == "BossAlien")
				{
					FallThroughPlanet.fallThroughPlanetUnlocked = true;
					SaveSpotTeleport.canEnterSaveSpot = true;
				}
				dropLoot(enemy, rigidbody.position);
				enemies.Remove(go);
				Destroy(go);
			}
		}
		bool done = true;
		foreach (GameObject enemy in enemies) {
			if(done == true && enemy.GetComponent<PositionMe>().touching == true){
				done = true;
			} else {
				done = false;
			}
		}
		if (done) {
			print ("Enemies landed");
		}
	}

	GameObject chooseEnemy(int i) {
		switch(i) {
			case 0 : 
				return enemy1;
			case 1 : 
				return enemy2;
			case 2 : 
				return enemy3;
			default : 
				return enemy4;
		}
	}

	int chooseLevel() {
		float r = Random.value;
		if (r <= 0.3)
			return playerLevel -1;
		else if (r<= 0.55)
			return playerLevel;
		else if (r<= 0.8)
			return playerLevel + 1;
		else 
			return playerLevel + 2;
	}

	public void position(GameObject go){
		go.GetComponent<PositionMe>().checkMyPosition = false;
		Vector3 position;
		bool landed = false;
		
		while (!landed) {
			
			position = Random.onUnitSphere * (GameObject.Find("Planet").GetComponent<SphereCollider>().radius * GameObject.Find("Planet").transform.lossyScale.x);
			
			Collider[] collidedItems = Physics.OverlapSphere(position, 1.5f);//can try with 20
			List<Collider> tempList = new List<Collider>();
			
			foreach(Collider col in collidedItems){
				if(col.name != "Planet" && col.transform != go.transform){
					tempList.Add(col);
				}
			}

			if(tempList.Count() == 0){
				go.GetComponent<Rigidbody> ().position = position;
				GameObject.Find(go.name).GetComponent<PositionMe>().timeToCheckMyPosition = Time.time;
				GameObject.Find(go.name).GetComponent<PositionMe>().checkMyPosition = true;
				return;
			}
		}
	}

	void addEnemy(GameObject enemy) {	

		GameObject go = Instantiate(enemy);
		go.GetComponent<FauxGravityBody>().attractor = planet;
		go.tag = "Monster";

		Enemy enemyComponent = go.GetComponent<Enemy>();
		enemyComponent.level = chooseLevel();
		enemyComponent.init();
		enemies.AddLast(go);

		position (go);
	}
	
	void dropLoot(Enemy enemy, Vector3 position) {

		GameObject.Find("Player").GetComponent<Sounds>().playDeathSound(0);
		for (int i = 0; i < enemy.maxLoot; i++) {
			int chance = Random.Range (0, 101);
			if (chance >= enemy.lootChance) {
				giveLoot = true;
				playerScript.paused = true;
				EnemyName = enemy.typeID;

				chance = Random.Range (0, 101);

				if (chance <= 25) {
					int commonItem = Random.Range(1, 8);
					int uncommonItem = Random.Range(1, 10);
					int rareItem = Random.Range(1, 7);
					switch(playerLevel){
					case 1:
					case 2: {
						chance = Random.Range (1, 101);
						if(chance <= 10){
							tempItem = new UncommonAccessory(uncommonItem);
						} else{
							tempItem = new CommonAccessory(commonItem);
						}
						break;
					}
					case 3:
					case 4:{
						chance = Random.Range (1, 101);
						if(chance <= 5){
							tempItem = new RareAccessory(rareItem);
						} else if(chance <= 20){
							tempItem = new UncommonAccessory(uncommonItem);
						} else{
							tempItem = new CommonAccessory(commonItem);
						}
						break;
					}
					case 5:
					case 6:{
						chance = Random.Range (1, 101);
						if(chance <= 10){
							tempItem = new RareAccessory(rareItem);
						} else if(chance <= 30){
							tempItem = new UncommonAccessory(uncommonItem);
						} else{
							tempItem = new CommonAccessory(commonItem);
						}
						break;
					}
					case 7:
					case 8:{
						chance = Random.Range (1, 101);
						if(chance <= 15){
							tempItem = new RareAccessory(rareItem);
						} else if(chance <= 40){
							tempItem = new UncommonAccessory(uncommonItem);
						} else{
							tempItem = new CommonAccessory(commonItem);
						}
						break;
					}
					case 9:
					case 10:{
						chance = Random.Range (1, 101);
						if(chance <= 20){
							tempItem = new RareAccessory(rareItem);
						} else if(chance <= 30){
							tempItem = new CommonAccessory(commonItem);
						} else{
							tempItem = new UncommonAccessory(uncommonItem);
						}
						break;
					}
					case 11:
					case 12:{
						chance = Random.Range (1, 101);
						if(chance <= 15){
							tempItem = new CommonAccessory(commonItem);
						} else if(chance <= 25){
							tempItem = new RareAccessory(rareItem);
						} else{
							tempItem = new UncommonAccessory(uncommonItem);
						}
						break;
					}
					}
					tempLoot.AddLast(tempItem); 
				} else {
					chance = Random.Range (1, 4);	//choose between available weapons
					switch (chance) {
					case 1:
						{
							if (playerLevel < 5) {
								tempItem = new ButterKnife (5);
								tempLoot.AddLast(tempItem);
							} else {
								tempItem = new ButterKnife (playerLevel);
								tempLoot.AddLast(tempItem);
							}
							break;
						}
					case 2:
						{
							tempItem = new Longsword (playerLevel);
							tempLoot.AddLast(tempItem);
							break;
						}
					case 3:
						{
							tempItem = new Warhammer (playerLevel);
							tempLoot.AddLast(tempItem);
							break;
						}
					}
				}
			}
		}

		GameObject deadEnemy = Instantiate(loot);
		deadEnemy.transform.position = position;
		deadEnemy.tag = "Loot";
		deadEnemy.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		Loot lootComponent = deadEnemy.GetComponent<Loot> ();
		lootComponent.storeLoot (tempLoot, "Dead " + enemy.typeID);
	}

	void OnGUI() {
		if (giveLoot) {

			int boxHeight = 250;
			int boxWidth = 400;
			int top = Screen.height/2-boxHeight/2;
			int left = Screen.width/2-boxWidth/2;
			int itemHeight = 30;
			int buttonWidth = 100;
			int closeTop = top;

			GUI.Box (new Rect (left, top, boxWidth, boxHeight), EnemyName);

			foreach (InventoryItem item in tempLoot.ToList()) {
				GUI.Label (new Rect (left + 30, top + 40, boxWidth-buttonWidth, itemHeight), item.typeID);
				if (GUI.Button (new Rect (left + 270, top + 40, buttonWidth, itemHeight), "Take it")) {
					attributesScript.addToInventory (item);
					tempLoot.Remove (item);
					GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(2);
					if (tempLoot.Count == 0) {
						giveLoot = false;
						playerScript.paused = false;
					}
				}
				top += 30;
			}
			if (GUI.Button (new Rect (left + 270, closeTop + (boxHeight - itemHeight), buttonWidth, itemHeight), "Close")) {
				giveLoot = false;
				playerScript.paused =(false);
				GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(2);
				tempLoot.Clear ();
			}
		}
	}

	public void clearLoot(){
		GameObject[] gameObjectsToDelete =  GameObject.FindGameObjectsWithTag ("Loot");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}
	}
}