﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawner : MonoBehaviour {

	public Sprite bossImage;

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
	public static LinkedList<InventoryItem> bossLoot = null;

	public int ENEM_COUNT{ get; set; }
	const int NORMAL_ENEMY_TYPES = 4;

	public static int ALL_ENEMIES{ get; set; }

	FauxGravityAttractor planet;
	bool noHint;

	LinkedList<GameObject> enemies = new LinkedList <GameObject> ();

	BonusObjectives bonusObjectives;

	public LinkedList<GameObject> getEnemies(){
		return enemies;
	}
	
	void Start(){
		//creates and stores ship pieces to be dropped
		noHint = true;
		ENEM_COUNT = 100;
		Collisions.backEngine = new BackEngine();
		Collisions.landingGear = new LandingGear();
		Collisions.tailFin = new TailFin();
		Collisions.leftWing = new LeftWing();
		Collisions.flightControl = new FlightControl();
		if (bossLoot == null) {
			bossLoot = new LinkedList<InventoryItem>();
			bossLoot.AddLast(Collisions.backEngine);

			bossLoot.AddLast(Collisions.landingGear);

			bossLoot.AddLast(Collisions.tailFin);
			bossLoot.AddLast(Collisions.leftWing);

			bossLoot.AddLast(Collisions.flightControl);
		}

		hudText = GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ();
		bonusObjectives = GameObject.Find ("Player").GetComponent<BonusObjectives> ();
		bonusObjectives.deadEnemiesOnLevel = 0;
	}

	public string enemiesStats(){
		string temp = "Enemies left: " + enemies.Count ();
		temp += "\nEnemies dead: " + (ENEM_COUNT - enemies.Count ()) + "\n";

		temp += "\n";//for split between monster and player stats

		return temp;
	}

	// Use this for initialization
	public void spawnEnemies (int enemy_count) {	//Previously know as Start
		var playerAttributes = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		ENEM_COUNT = (int)(enemy_count * playerAttributes.difficulty);
		ALL_ENEMIES = ENEM_COUNT;
		clearLoot ();

		planet = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
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
			if (amountEnemiesLanded () >= (ENEM_COUNT/2) && done == true) {
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

		if(enemies.Count == ENEM_COUNT){
			noHint = false;
		}

		if(enemies.Count <= 5){
			if (!noHint){
				GameObject.Find("Player").GetComponent<Tutorial>().makeHint("We're coming for you...",  bossImage);
				noHint = true;
			}

			foreach (GameObject go in enemies.ToList()) {			
				Enemy enemy = go.GetComponent<Enemy>();

				enemy.seekOutPlayer();
			}
		}
		foreach (GameObject go in enemies.ToList()) {		

			Enemy enemy = go.GetComponent<Enemy>();	
			if (enemy == null)
				continue;

			Rigidbody rigidbody = go.GetComponent<Rigidbody> ();

			if (enemy.isDead()) {
				try {
					bonusObjectives.deadEnemies++;
					bonusObjectives.deadEnemiesOnLevel++;
					if(enemy.typeID == "BossAlien")
					{
						//hudText.text = "You found a spaceship part! \n";
						var playerGO = GameObject.Find ("Player");
						var fallThroughPlanet = playerGO.GetComponent<FallThroughPlanet> ();

						if(fallThroughPlanet.fallThroughPlanetUnlocked && playerGO.GetComponent<PlayerAttributes> ().fallFirst){

							var sounds = playerGO.GetComponent<Sounds> ();

							sounds.playComputerSound (Sounds.COMPUTER_FALL);
							if(sounds.computerAudio.isPlaying){
								fallThroughPlanet.fallNow ();
								playerGO.GetComponent<PlayerAttributes> ().fallFirst = false;
								Camera.main.GetComponent<HUD>().setLight("fall");
								playerGO.GetComponent<Tutorial> ().makeHint("You can use this ability by pressing F. It has a 10s cool down time.", playerGO.GetComponent<Tutorial> ().Warp);
							}

							fallThroughPlanet.fallThroughPlanetUnlocked = true;

							hudText.text += "What does this button do? I probably shouldn’t, oh well whatever I’ll press it anyway. You seem to have fallen through the planet. That could be useful. \n\n";
							Canvas.ForceUpdateCanvases();
							Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
							scrollbar.value = 0f;
							

						}
					//	GameObject.Find("Player").GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
					}
					dropLoot(enemy, rigidbody.position);
					
				} finally {
						enemies.Remove(go);						
						Destroy(go);
				}
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
		int playerLevel = GameObject.Find("Player").GetComponent<PlayerAttributes>().level;
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
		if(go.GetComponentInChildren<ParticleSystem>()){
			go.GetComponentInChildren<ParticleSystem>().enableEmission = false;
		}

		Enemy enemyComponent = go.GetComponent<Enemy>();
		enemyComponent.level = chooseLevel();
		enemyComponent.init();
		enemies.AddLast(go);

		position (go);
	}
	
	void dropLoot(Enemy enemy, Vector3 position) {
		int playerLevel = GameObject.Find("Player").GetComponent<PlayerAttributes>().level;
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
					int weaponLevel = Random.Range(-1,1) + playerLevel;
					switch (weaponType) {
					case 1: {
							if (weaponLevel < 5) {
								tempItem = new ButterKnife (5);								
							} else {
								tempItem = new ButterKnife (weaponLevel);
							}
							break;
						}
					case 2: {
							tempItem = new Longsword (weaponLevel);
							break;
						}
					case 3: {
							tempItem = new Warhammer (weaponLevel);							
							break;
						}
					}
					tempLoot.AddLast(tempItem);
				}
			}
		}
		
		if (enemy.typeID == "BossAlien") {

			//GameObject.Find("Player").GetComponent<PlayerAttributes>().addToInventory(bossLoot.First());
			tempLoot.AddLast(bossLoot.First());
			lootCount++;
			bossLoot.RemoveFirst();
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