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

	//public bool showInventory { get; set; }

	private int numLoot;
//	private PlayerController playerScript;
	private Accessory accessoryScript;
	private LinkedList<InventoryItem> tempLoot;
	private InventoryItem tempItem;
	private bool monsterDead = false;
	private int deadEnemies = 0;

	LinkedList<GameObject> enemies = new LinkedList <GameObject> ();

	// Use this for initialization
	void Start () {
		//showInventory = false;
		tempLoot = new LinkedList<InventoryItem> ();
		
	//	playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();

		addEnemy (enemy1, new Vector3(-9.794984f, 8.99264f, 7.973226f));
		addEnemy (enemy2, new Vector3(13.43f, 4.87f, -6.02f));
	}

	public string enemiesStats(){
		string temp = "Enemies left: " + enemies.Count ();

		temp += "\nEnemies dead: " + deadEnemies + "\n";
		
		temp += "\n";//for split between monster and player stats
		
		return temp;
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

	void addEnemy(GameObject enemy, Vector3 position) {	
		
		GameObject go = Instantiate(enemy);
		go.GetComponent<FauxGravityBody>().attractor = this.GetComponent<FauxGravityAttractor>();
		go.tag = "Monster";

		Enemy enemyComponent = go.GetComponent<Enemy>();
		enemyComponent.level = 1;
		enemyComponent.init();
		enemies.AddLast(go);

		go.transform.position = position;
		//position (go);
	}

	void Update () {
		foreach(GameObject monster in enemies.ToList()){

			/*float distance = Vector3.Distance (monster.GetComponent<Rigidbody> ().position, GameObject.Find("Player").GetComponent<Rigidbody> ().position);
			if(distance < 6){
				GameObject.Find("Player").GetComponent<Tutorial>().setupVisuals();
				GameObject.Find("Player").GetComponent<Tutorial>().showAttack = true;
			} else {
				GameObject.Find("Player").GetComponent<Tutorial>().showAttack = false;
			}*/

			Enemy enemy = monster.GetComponent<Enemy>();	

		//	Rigidbody rigidbody = monster.GetComponent<Rigidbody> ();
			if (enemy.isDead()) {
				//GameObject.Find("Player").GetComponent<Tutorial>().showAttack = false;
				if(enemy.typeID == "BossAlien")
				{
					deadEnemies++;
					tempLoot.AddLast(new PowerCore());
					GameObject deadEnemy = Instantiate(loot);
					deadEnemy.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
					deadEnemy.transform.position = (monster.GetComponent<Rigidbody>().position - new Vector3(0, -1, 0));
					deadEnemy.tag = "Loot";

					Loot lootComponent = deadEnemy.GetComponentInChildren<Loot> ();
					lootComponent.storeLoot (tempLoot, "Dead " + enemy.typeID);
					tempLoot.Clear ();

					GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_GOBACK);
					//GameObject.Find("Player").GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
					GameObject.Find("Player").GetComponent<Tutorial>().tutorialDone = true;
				} else if (!monsterDead){
					deadEnemies++;
					GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_INVENTORY);
					//GameObject.Find("Player").GetComponent<Tutorial>().setupVisuals();
					//showInventory = true;
					Loot.showInventoryHint = true;
					//showed Inventory hint here GameObject.Find("Player").GetComponent<Tutorial>().makeHint(inventoryHintText, PressI);
					monsterDead = true;
					GameObject.Find("Player").GetComponent<Tutorial>().teachInventory = true;

					tempLoot.AddLast(new Longsword(1));
					GameObject deadEnemy = Instantiate(loot);
					deadEnemy.transform.position = monster.GetComponent<Rigidbody>().position;
					deadEnemy.tag = "Loot";
					deadEnemy.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
					Loot lootComponent = deadEnemy.GetComponentInChildren<Loot> ();
					lootComponent.storeLoot (tempLoot, "Dead " + enemy.typeID);
					tempLoot.Clear ();
					//dropLoot(enemy, rigidbody.position);
				} else {
					deadEnemies++;
					this.GetComponent<NaturalDisasters>().makeEarthQuakeHappen();
					while(this.GetComponent<NaturalDisasters>().isShaking() == true){}
					GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_DISASTERD);
					addEnemy (boss, new Vector3(-0.04f, -15.52f, 0.15f));
				}
				enemies.Remove(monster);
				Destroy(monster);
			}
		}
	}

	/*public void OnGUI(){
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
	}*/
}