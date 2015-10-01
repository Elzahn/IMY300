using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//Code kan nog geoptimize word was net te moeg om dit dadelik te doen

public class TutorialSpawner : MonoBehaviour {
	
	public GameObject loot;
	public GameObject boss;
	public GameObject enemy1;
	public GameObject enemy2;

	public static PowerCore bossPowerCore;

	private Text hudText;
	private int numLoot;
	private Accessory accessoryScript;
	private LinkedList<InventoryItem> tempLoot;
	private InventoryItem tempItem;
	private bool monsterDead = false;
	private int deadEnemies = 0;

	LinkedList<GameObject> enemies = new LinkedList <GameObject> ();

	// Use this for initialization
	void Start () {
		bossPowerCore = new PowerCore ();
		tempLoot = new LinkedList<InventoryItem> ();

		hudText = GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ();

		addEnemy (enemy1, new Vector3(-9.794984f, 8.99264f, 7.973226f));
		addEnemy (enemy2, new Vector3(13.43f, 4.87f, -6.02f));

		GameObject.Find ("Player").GetComponent<Sounds> ().playAmbienceSound (Sounds.TUTORIAL_AMBIENCE);
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
	}

	void Update () {
		foreach(GameObject monster in enemies.ToList()){

			Enemy enemy = monster.GetComponent<Enemy>();	

			if (enemy.isDead()) {
				if(enemy.typeID == "BossAlien")
				{
					GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_GOBACK);
					hudText.text += "I didn’t think you’d survive that… Oh well, return the Power Core to the ship immediately.\n\n";
					Canvas.ForceUpdateCanvases();
					Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
					scrollbar.value = 0f;

					deadEnemies++;
					tempLoot.AddLast(bossPowerCore);
					GameObject deadEnemy = Instantiate(loot);
					deadEnemy.GetComponent<FauxGravityBody>().attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
					deadEnemy.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
					deadEnemy.transform.position = monster.GetComponent<Rigidbody>().position;
					deadEnemy.tag = "Loot";
					Loot lootComponent = deadEnemy.GetComponentInChildren<Loot> ();
					lootComponent.storeLoot (tempLoot, "Dead " + enemy.typeID);
					tempLoot.Clear ();
					GameObject.Find("Player").GetComponent<Tutorial>().tutorialDone = true;
				} else if (!monsterDead){
					deadEnemies++;
					Loot.showInventoryHint = true;	//shows hint and add HUD text
					monsterDead = true;
					GameObject.Find("Player").GetComponent<Tutorial>().teachInventory = true;

					tempLoot.AddLast(new Longsword(1));
					GameObject deadEnemy = Instantiate(loot);
					deadEnemy.GetComponent<FauxGravityBody>().attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
					deadEnemy.transform.position = monster.GetComponent<Rigidbody>().position;
					deadEnemy.tag = "Loot";
					deadEnemy.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
					Loot lootComponent = deadEnemy.GetComponentInChildren<Loot> ();
					lootComponent.storeLoot (tempLoot, "Dead " + enemy.typeID);
					lootComponent.showMyLoot();
					tempLoot.Clear ();
				} else {
					deadEnemies++;
					this.GetComponent<NaturalDisasters>().makeEarthQuakeHappen();
					while(this.GetComponent<NaturalDisasters>().isShaking() == true){}
					GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_DISASTERD);
					hudText.text += "Oh fuck, that earthquake scattered some of the essential pieces of the spacecraft across the solar system.\n\n";
					Canvas.ForceUpdateCanvases();
					Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
					scrollbar.value = 0f;
					addEnemy (boss, new Vector3(-0.04f, -15.52f, 0.15f));
				}
				enemies.Remove(monster);
				Destroy(monster);
			}
		}
	}
}