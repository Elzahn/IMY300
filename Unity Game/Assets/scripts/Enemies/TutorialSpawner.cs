using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//Code kan nog geoptimize word was net te moeg om dit dadelik te doen

public class TutorialSpawner : MonoBehaviour {
	
	public GameObject loot;
	public GameObject boss;
	public GameObject enemy1;
	public GameObject enemy2;

	public bool showInventory { get; set; }

	private string EnemyName;
	private int numLoot;
//	private PlayerController playerScript;
	private Accessory accessoryScript;
	private LinkedList<InventoryItem> tempLoot;
	private InventoryItem tempItem;
	private bool monsterDead = false;

	LinkedList<GameObject> enemies = new LinkedList <GameObject> ();

	int playerLevel;

	// Use this for initialization
	void Start () {
		showInventory = false;

		playerLevel = GameObject.Find("Player").GetComponent<PlayerAttributes>().level;
		tempLoot = new LinkedList<InventoryItem> ();
		
	//	playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();

		addEnemy (enemy1);
		addEnemy (enemy2);
	}


	public void resumeEnemySound(){
		Enemy enemy;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Monster");
		for (int i = 0; i < enemies.Length; i++) {
			enemy = enemies[i].GetComponent<Enemy>();
			GameObject.Find("Player").GetComponent<Sounds>().resumeMonsterSound(enemy);
		}
	}
	
	public void pauseEnemySound(){
		Enemy enemy;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Monster");
		for (int i = 0; i < enemies.Length; i++) {
			enemy = enemies[i].GetComponent<Enemy>();
			GameObject.Find("Player").GetComponent<Sounds>().pauseMonsterSound(enemy);
		}
	}
	
	public void stopEnemiesSound(){
		Enemy enemy;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Monster");
		for (int i = 0; i < enemies.Length; i++) {
			enemy = enemies[i].GetComponent<Enemy>();
			GameObject.Find("Player").GetComponent<Sounds>().stopMonsterSound(enemy);
		}
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
		go.GetComponent<FauxGravityBody>().attractor = this.GetComponent<FauxGravityAttractor>();
		go.tag = "Monster";

		Enemy enemyComponent = go.GetComponent<Enemy>();
		enemyComponent.level = 1;
		enemyComponent.init();
		enemies.AddLast(go);
		
		position (go);
	}

	void Update () {
		foreach(GameObject monster in enemies.ToList()){

			float distance = Vector3.Distance (monster.GetComponent<Rigidbody> ().position, GameObject.Find("Player").GetComponent<Rigidbody> ().position);
			if(distance < 6){
				GameObject.Find("Player").GetComponent<Tutorial>().setupVisuals();
				GameObject.Find("Player").GetComponent<Tutorial>().showAttack = true;
			}

			Enemy enemy = monster.GetComponent<Enemy>();	

			Rigidbody rigidbody = monster.GetComponent<Rigidbody> ();
			if (enemy.isDead()) {
				GameObject.Find("Player").GetComponent<Tutorial>().showAttack = false;
				if(enemy.typeID == "BossAlien")
				{
					tempLoot.AddLast(new PowerCore());
					GameObject deadEnemy = Instantiate(loot);
					deadEnemy.transform.position = monster.GetComponent<Rigidbody>().position;
					deadEnemy.tag = "Loot";
					deadEnemy.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
					Loot lootComponent = deadEnemy.GetComponentInChildren<Loot> ();
					lootComponent.storeLoot (tempLoot, "Dead " + EnemyName);
					tempLoot.Clear ();
					GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_GOBACK);
					GameObject.Find("Player").GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
					GameObject.Find("Player").GetComponent<Tutorial>().tutorialDone = true;
				} else if (!monsterDead){
					GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_INVENTORY);
					GameObject.Find("Player").GetComponent<Tutorial>().setupVisuals();
					showInventory = true;
					monsterDead = true;
					GameObject.Find("Player").GetComponent<Tutorial>().teachInventory = true;

					dropLoot(enemy, rigidbody.position);
				} else {
					this.GetComponent<NaturalDisasters>().makeEarthQuakeHappen();
					while(this.GetComponent<NaturalDisasters>().isShaking() == true){}
					GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_DISASTERD);
					addEnemy (boss);
				}
				enemies.Remove(monster);
				Destroy(monster);
			}
		}
	}

	void dropLoot(Enemy enemy, Vector3 position) {
		
		GameObject.Find("Player").GetComponent<Sounds>().playDeathSound(Sounds.DEAD_MONSTER);
		for (int i = 0; i < enemy.maxLoot; i++) {
			int chance = Random.Range (0, 101);
				//playerScript.paused = true;
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
		
		GameObject deadEnemy = Instantiate(loot);
		deadEnemy.transform.position = position;
		deadEnemy.tag = "Loot";
		deadEnemy.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		Loot lootComponent = deadEnemy.GetComponentInChildren<Loot> ();
		lootComponent.storeLoot (tempLoot, "Dead " + EnemyName);
		tempLoot.Clear ();
		//lootComponent.showMyLoot ();
	}
	
	public void clearLoot(){
		GameObject[] gameObjectsToDelete =  GameObject.FindGameObjectsWithTag ("Loot");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}
	}

	public void OnGUI(){
		if (GameObject.Find("Player").GetComponent<Tutorial>().startTutorial) {
			if(GameObject.Find("Player").GetComponent<Tutorial>().showVisuals){
				if (showInventory) {
					GUI.depth = 0;
					GUI.color = new Color32 (255, 255, 255, 50);
					GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), (""));
					GUI.color = new Color32 (255, 255, 255, 255);
					GUI.Label (new Rect (Screen.width/2-100, Screen.height - 35, Screen.width - 300, 120), ("Access Inventory by pressing I"));
					//GUI.DrawTexture (new Rect (Screen.width / 2 - Screen.width / 8, Screen.height / 2 - Screen.height / 3, Screen.width / 4, Screen.height / 4), WASD);
				}
			}
			if (Time.time >= GameObject.Find("Player").GetComponent<Tutorial>().showVisualQue) {
				GameObject.Find("Player").GetComponent<Tutorial>().showVisuals = false;
			}
		}
	}
}