using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public GameObject bossEnemy;

	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject enemy4;
	// Use this for initialization
	
	const int ENEM_COUNT = 20;
	const int NORMAL_ENEMY_TYPES = 4;

	FauxGravityAttractor planet;
	int playerLevel;

	LinkedList<GameObject> enemies = new LinkedList <GameObject> ();

	void Start () {
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
			case 3 : 
				return enemy4;
			default : 
				return bossEnemy;
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
	}


}
