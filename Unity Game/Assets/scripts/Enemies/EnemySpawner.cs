﻿using UnityEngine;
using UnityEngine.UI;
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

	private string EnemyName;
	private int numLoot;
	private Text hudText;
	private Accessory accessoryScript;

	public int ENEM_COUNT{ get; set; }
	const int NORMAL_ENEMY_TYPES = 4;

	FauxGravityAttractor planet;
	int playerLevel;

	LinkedList<GameObject> enemies = new LinkedList <GameObject> ();

	void Start(){
		hudText = GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ();
	}

	public string enemiesStats(){
		string temp = "Enemies left: " + enemies.Count ();
		temp += "\nEnemies dead: " + (ENEM_COUNT - enemies.Count ()) + "\n";

		temp += "\n";//for split between monster and player stats

		return temp;
	}

	// Use this for initialization
	public void spawnEnemies (int enemy_count) {	//Previously know as Start
		ENEM_COUNT = enemy_count;
		clearLoot ();

		planet = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
		playerLevel = GameObject.Find("Player").GetComponent<PlayerAttributes>().level;
		GameObject enemy;

		//Spawn Normal Enemies
		for (int i = 0; i < enemy_count -1; ++i) {
			int index = Mathf.RoundToInt(Random.value * NORMAL_ENEMY_TYPES);
			enemy = chooseEnemy(index);
			addEnemy(enemy);
		}

		//Spawn boss.
		addEnemy(bossEnemy);	
	}

	public void resumeEnemySound(){
		Enemy enemy;
		foreach(GameObject monster in enemies){
			enemy = monster.GetComponent<Enemy>();
			GameObject.Find("Player").GetComponent<Sounds>().resumeMonsterSound(enemy);
		}
	}

	public void pauseEnemySound(){
		Enemy enemy;
		foreach(GameObject monster in enemies){
			enemy = monster.GetComponent<Enemy>();
			GameObject.Find("Player").GetComponent<Sounds>().pauseMonsterSound(enemy);
		}
	}

	public void stopEnemiesSound(){
		Enemy enemy;
		foreach(GameObject monster in enemies){
			enemy = monster.GetComponent<Enemy>();
			GameObject.Find("Player").GetComponent<Sounds>().stopMonsterSound(enemy);
		}
	}

	public bool hasEnemiesLanded(){
		bool done = true;

		for (int i = 0; i < 3; i++) {
			if (amountEnemiesLanded () == ENEM_COUNT && done == true) {
				done = true;
			} else {
				done = false;
			}
		}

		return done;
	}

	public int amountEnemiesLanded(){
		int done = 0;
		for (int i=0; i < 5; i++) {
			done = 0;
			foreach (GameObject enemy in enemies) {
				if (enemy.GetComponent<PositionMe> ().touching == true) {
					enemy.GetComponent<PositionMe>().checkMyPosition = false;
					done++;
				}
			}
		}

		return done;
	}

	void Update () {
		foreach (GameObject go in enemies.ToList()) {			
			Enemy enemy = go.GetComponent<Enemy>();	

			Rigidbody rigidbody = go.GetComponent<Rigidbody> ();
			if (enemy.isDead()) {
				if(enemy.typeID == "BossAlien")
				{
					if(GameObject.Find("Player").GetComponent<FallThroughPlanet>().fallThroughPlanetUnlocked && GameObject.Find("Player").GetComponent<PlayerAttributes>().fallFirst){

						GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_FALL);
						if(GameObject.Find("Player").GetComponent<Sounds>().computerAudio.isPlaying){
							GameObject.Find("Player").GetComponent<FallThroughPlanet>().fallNow();
							GameObject.Find("Player").GetComponent<PlayerAttributes>().fallFirst = false;
							Camera.main.GetComponent<HUD>().setLight("fall");
							GameObject.Find("Player").GetComponent<Tutorial>().makeHint("You can use this ability by pressing F. It has a 10s cool down time.", GameObject.Find("Player").GetComponent<Tutorial>().Warp);
						}

						GameObject.Find("Player").GetComponent<FallThroughPlanet>().fallThroughPlanetUnlocked = true;

						hudText.text = "What does this button do? I probably shouldn’t, oh well whatever I’ll press it anyway. Oh shit! You seem to have fallen through the planet. That could be useful. \n\n";
						Canvas.ForceUpdateCanvases();
						Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
						scrollbar.value = 0f;
						

					}
					GameObject.Find("Player").GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
				}
				dropLoot(enemy, rigidbody.position);
				enemies.Remove(go);
				Destroy(go);
			}
		}
		hasEnemiesLanded ();
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
			if (playerLevel - 1 <= 0) {
				return 1;
			} else {
				return playerLevel - 1;
			}
		else if (r<= 0.55)
			return playerLevel;
		else if (r<= 0.8)
			return playerLevel + 1;
		else 
			return playerLevel + 2;
	}

	public void position(GameObject go){
		go.GetComponent<PositionMe> ().touching = false;
		go.GetComponent<PositionMe>().checkMyPosition = false;
		Vector3 position;
		bool landed = false;
		
		while (!landed) {
			
			position = Random.onUnitSphere * (GameObject.Find("Planet").GetComponent<SphereCollider>().radius * GameObject.Find("Planet").transform.lossyScale.x);
			
			Collider[] collidedItems = Physics.OverlapSphere(position, 0.5f);//can try with 20
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
		//if (go.name == "ApeMonster(Clone)") {
			//go.GetComponent<TrackEnemyHealth>().canvas = GameObject.Find("Monster_Canvas").GetComponent<Canvas>();
			go.GetComponent<EnemyHealth>().canvas = go.GetComponentInChildren<Canvas>();
			//go.GetComponentInChildren<Canvas>().eventCamera = Camera.main;
	//	}
		go.GetComponent<FauxGravityBody>().attractor = planet;
		go.tag = "Monster";

		Enemy enemyComponent = go.GetComponent<Enemy>();
		enemyComponent.level = chooseLevel();
		enemyComponent.init();
		enemies.AddLast(go);

		position (go);
	}
	
	void dropLoot(Enemy enemy, Vector3 position) {

		GameObject.Find("Player").GetComponent<Sounds>().playDeathSound(Sounds.DEAD_MONSTER);
		LinkedList<InventoryItem> tempLoot = new LinkedList<InventoryItem> ();;
		InventoryItem tempItem = null;
		int lootCount = 0;
		EnemyName = enemy.typeID;

		for (int i = 0; i < enemy.maxLoot; i++) {
			float chance = Random.value;
			if (chance <= enemy.lootChance) {
				++lootCount;

				if (chance <= enemy.lootChance/2) {
					int commonItemType = Random.Range(1, 8);
					int uncommonItemType = Random.Range(1, 10);
					int rareItemType = Random.Range(1, 7);
					switch(playerLevel){
						case 1:
						case 2: {
							chance = Random.Range (1, 101);
							if(chance <= 10){
								tempItem = new UncommonAccessory(uncommonItemType);
							} else{
								tempItem = new CommonAccessory(commonItemType);
							}
							break;
						}
						case 3:
						case 4:{
							chance = Random.Range (1, 101);
							if(chance <= 5){
								tempItem = new RareAccessory(rareItemType);
							} else if(chance <= 20){
								tempItem = new UncommonAccessory(uncommonItemType);
							} else{
								tempItem = new CommonAccessory(commonItemType);
							}
							break;
						}
						case 5:
						case 6:{
							chance = Random.Range (1, 101);
							if(chance <= 10){
								tempItem = new RareAccessory(rareItemType);
							} else if(chance <= 30){
								tempItem = new UncommonAccessory(uncommonItemType);
							} else{
								tempItem = new CommonAccessory(commonItemType);
							}
							break;
						}
						case 7:
						case 8:{
							chance = Random.Range (1, 101);
							if(chance <= 15){
								tempItem = new RareAccessory(rareItemType);
							} else if(chance <= 40){
								tempItem = new UncommonAccessory(uncommonItemType);
							} else{
								tempItem = new CommonAccessory(commonItemType);
							}
							break;
						}
						case 9:
						case 10:{
							chance = Random.Range (1, 101);
							if(chance <= 20){
								tempItem = new RareAccessory(rareItemType);
							} else if(chance <= 30){
								tempItem = new CommonAccessory(commonItemType);
							} else{
								tempItem = new UncommonAccessory(uncommonItemType);
							}
							break;
						}
						case 11:
						case 12:
						default: {
							chance = Random.Range (1, 101);
							if(chance <= 15){
								tempItem = new CommonAccessory(commonItemType);
							} else if(chance <= 25){
								tempItem = new RareAccessory(rareItemType);
							} else{
								tempItem = new UncommonAccessory(uncommonItemType);
							}
							break;
						}
					}
					tempLoot.AddLast(tempItem); 
				} 
				else {
					int weaponType = Random.Range (1, 4);	//choose between available weapons
					switch (weaponType) {
					case 1: {
							if (playerLevel < 5) {
								tempItem = new ButterKnife (5);								
							} else {
								tempItem = new ButterKnife (playerLevel);
							}
							break;
						}
					case 2: {
							tempItem = new Longsword (playerLevel);
							break;
						}
					case 3: {
							tempItem = new Warhammer (playerLevel);							
							break;
						}
					}
					tempLoot.AddLast(tempItem);
				}
			}
		}

		if (lootCount > 0) {
		GameObject deadEnemy = Instantiate(loot);
		deadEnemy.transform.position = position;
		deadEnemy.tag = "Loot";
		deadEnemy.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		Loot lootComponent = deadEnemy.GetComponentInChildren<Loot> ();
		lootComponent.storeLoot (tempLoot, "Dead " + EnemyName);
		tempLoot.Clear ();
		}
	}

	public void clearLoot(){
		GameObject[] gameObjectsToDelete =  GameObject.FindGameObjectsWithTag ("Loot");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}
	}
}