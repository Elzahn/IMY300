using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class LevelSelect : MonoBehaviour {

	public List<Material> materials;
	public static LevelSelect instance;

	public Renderer myRenderer{ get; set;}
	public bool spawnedLevel{ get; set;}

	private GameObject planet;

	public int currentLevel { get; set; }

	void Start(){
		spawnedLevel = false;
	}

	void Update () {
		if (myRenderer != null) {	//You are on a planet and now will customise the level
			myRenderer.material = GetMaterial (currentLevel);
			if(Application.loadedLevelName != "Tutorial"){
				switch (currentLevel) {
					case 1:
					{
						if(!spawnedLevel)
						{
							planet.GetComponent<EnemySpawner>().spawnEnemies(20);
							planet.GetComponent<SpawnTrees>().spawnTrees(299);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(10);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_1_AMBIENCE);
							spawnedLevel = true;
						}
						break;
					}
					case 2:
					{
						if(!spawnedLevel)
						{
							planet.GetComponent<EnemySpawner>().spawnEnemies(35);
							planet.GetComponent<SpawnTrees>().spawnTrees(190);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(12);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_2_AMBIENCE);
							spawnedLevel = true;
						}
						break;
					}
					case 3:
					{
						if(!spawnedLevel)
						{
							planet.GetComponent<EnemySpawner>().spawnEnemies(10);
							planet.GetComponent<SpawnTrees>().spawnTrees(350);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(2);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_3_AMBIENCE);
							spawnedLevel = true;
						}
						break;
					}
					case 4:
					{
						if(!spawnedLevel)
						{
							planet.GetComponent<EnemySpawner>().spawnEnemies(40);
							planet.GetComponent<SpawnTrees>().spawnTrees(80);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(15);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_4_AMBIENCE);
							spawnedLevel = true;
						}
						break;
					}
					case 5:
					{
						if(!spawnedLevel)
						{
							planet.GetComponent<EnemySpawner>().spawnEnemies(50);
							planet.GetComponent<SpawnTrees>().spawnTrees(20);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(20);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_5_AMBIENCE);
							spawnedLevel = true;
						}
						break;
					}
				}
			}
		} else {
			planet = GameObject.Find ("Planet");
			if (planet != null) {	//You are on a planet so get the rendered of the planet
				myRenderer = planet.GetComponent<Renderer> ();
			} else {	//You are in the space ship
				myRenderer = null;
			}
		}
	}

	// Use this for initialization
	public void Awake() {
		if(instance == null){
			instance = this; 
		}
	}
	
	public Material GetMaterial(int level){
		if(materials != null && materials.Count > 0 && level >= 0 && level < materials.Count){
			return materials[level];
		}
		return null;
	}
}