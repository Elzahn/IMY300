using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public GameObject bossEnemy;

	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject enemy4;

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
		tempLoot = new LinkedList<InventoryItem> ();;

		if (GameObject.Find ("Persist") != null) {
			attributesScript = GameObject.Find ("Persist").GetComponent<PlayerAttributes> ();
		} else {
			attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		}
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
		foreach (GameObject go in enemies) {			
			Enemy enemy = go.GetComponent<Enemy>();			
			Rigidbody rigidbody = go.GetComponent<Rigidbody> ();
			if (enemy.isDead()) {
				dropLoot(enemy, rigidbody.position);
				enemies.Remove(go);
				Destroy(go);
				//Added XP here
				attributesScript.addXP(attributesScript.getLevel() * 20);
			}
			
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
			default : //case 3 : 
				return enemy4;
			/*default : 
				return bossEnemy;*/
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

	void addEnemy(GameObject enemy) {		
		GameObject go = Instantiate(enemy);
		go.GetComponent<FauxGravityBody>().attractor = planet;
		go.tag = "Monster";

		//go.transform.localScale = new Vector3(2, 3, 2);	actual scale of the monsters

		Enemy enemyComponent = go.GetComponent<Enemy>();
		enemyComponent.level = chooseLevel();
		enemyComponent.init();
		enemies.AddLast(go);

		Mesh mesh = go.GetComponent<MeshFilter>().mesh;
		//TODO Position correctly
		
		Vector3 position = Random.onUnitSphere * (20 + mesh.bounds.size.y/2);

		Rigidbody rigid = go.GetComponent<Rigidbody> () ;
		rigid.position = position;
	}

	void dropLoot(Enemy enemy, Vector3 position) {
		//TODO Implement 
		for (int i = 0; i < enemy.maxLoot; i++) {
			int chance = Random.Range (0, 101);
			if (chance >= enemy.lootChance) {
				giveLoot = true;
				playerScript.setPaused (true);
				EnemyName = enemy.typeID;

				chance = Random.Range (0, 101);

				if (chance <= 25) {
					tempItem = new NullAccessory ();
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
	}

	void OnGUI() {
		if (giveLoot) {

			int top = 30;
			int left = 200;

			GUI.Box (new Rect (left, top, 400, 250), EnemyName);

			foreach(InventoryItem item in tempLoot)
			{
				GUI.Label(new Rect (left+30, top+40, 300, 30), item.typeID);
				if (GUI.Button(new Rect(left+270, top+40, 100, 30), "Take it")){
					attributesScript.addToInventory(item);
					tempLoot.Remove(item);
					if(tempLoot.Count == 0)
					{
						giveLoot = false;
						playerScript.setPaused (false);
					}
				}
				top += 30;
			}
			if (GUI.Button(new Rect(left+270, 230, 100, 30), "Close")){
				giveLoot = false;
				playerScript.setPaused (false);
			}
		}
	}

}