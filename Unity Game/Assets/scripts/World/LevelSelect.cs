using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Renderer))]
public class LevelSelect : MonoBehaviour {

	public List<Material> materials;

	private PlayerAttributes attrs;
	private Light sun;

	public Renderer myRenderer{ get; set;}
	private Renderer monsterRenderer;

	public bool spawnedLevel{ get; set;}

	private static GameObject planet, fireMist, rain, snow, desert, candy;

	private LinkedList<GameObject> enemies;

	private const int BOSS_BODY_LEVEL1 = 6;
	private const int BOSS_EYE_LEVEL1 = 7;
	private const int BOSS_BODY_LEVEL2 = 8;
	private const int BOSS_EYE_LEVEL2 = 9;
	private const int BOSS_BODY_LEVEL3 = 10;
	private const int BOSS_EYE_LEVEL3 = 11;
	private const int BOSS_BODY_LEVEL4 = 12;
	private const int BOSS_EYE_LEVEL4 = 13;
	private const int BOSS_BODY_LEVEL5 = 14;
	private const int BOSS_EYE_LEVEL5 = 15;
	/**
	 * References Attributes variable to be able to save it easily.
	 * */
	public int currentLevel { get {
			return attrs.CurrentLevel;
		} 
		set {
			attrs.CurrentLevel = value;
		} 
	}

	void Start(){
		spawnedLevel = false;
		attrs = this.GetComponent<PlayerAttributes>();
	}

	void clearParticles(){
		fireMist.GetComponent<ParticleSystem>().enableEmission = false;
		fireMist.SetActive (false);
		rain.GetComponent<ParticleSystem>().enableEmission = false;
		rain.SetActive (false);
		snow.GetComponent<ParticleSystem>().enableEmission = false;
		snow.SetActive (false);
		desert.GetComponent<ParticleSystem>().enableEmission = false;
		desert.SetActive (false);
		candy.GetComponent<ParticleSystem>().enableEmission = false;
		candy.SetActive (false);
		spawnedLevel = false;
	}

	void Update () {

		if (Application.loadedLevelName == "Scene" && fireMist == null) {
			sun = GameObject.Find("Sun").GetComponent<Light>();

			fireMist = GameObject.Find ("Fire Mist");
			
			rain = GameObject.Find ("Rain");
			
			snow = GameObject.Find ("Snow");
			
			desert = GameObject.Find ("Desert");

			candy = GameObject.Find ("CandyFlos");

			clearParticles ();
		} else if (fireMist != null) {

			if (myRenderer != null) {	//You are on a planet and now will customise the level
				myRenderer.material = GetMaterial (currentLevel);
				if (Application.loadedLevelName != "Tutorial") {
					switch (currentLevel) {
					case 1:
						{
							if (!spawnedLevel) {
								clearParticles ();
								fireMist.SetActive (true);
								fireMist.GetComponent<ParticleSystem> ().enableEmission = true;
								planet.GetComponent<EnemySpawner> ().spawnEnemies (20);
								//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
								planet.GetComponent<SpawnTrees> ().spawnTrees (110, 0, 60, 0, 50);
								planet.GetComponent<SpawnHealthPacks> ().spawnHealth (10);
								this.GetComponent<Sounds> ().playAmbienceSound (Sounds.PLANET_1_AMBIENCE);
								this.GetComponent<Sounds> ().pauseSound ("ambience");
								spawnedLevel = true;
								sun.intensity = 4;
								textureMonsters();
							}
							break;
						}
					case 2:
						{
							if (!spawnedLevel) {
								clearParticles ();
								rain.SetActive (true);
								rain.GetComponent<ParticleSystem> ().enableEmission = true;
								planet.GetComponent<EnemySpawner> ().spawnEnemies (35);
								//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
								planet.GetComponent<SpawnTrees> ().spawnTrees (0, 95, 0, 0, 40);
								planet.GetComponent<SpawnHealthPacks> ().spawnHealth (12);
								this.GetComponent<Sounds> ().playAmbienceSound (Sounds.PLANET_2_AMBIENCE);
								this.GetComponent<Sounds> ().pauseSound ("ambience");
								spawnedLevel = true;
								sun.intensity = 1.2f;
								textureMonsters();
							}
							break;
						}
					case 3:
						{
							if (!spawnedLevel) {
								clearParticles ();
								snow.SetActive (true);
								snow.GetComponent<ParticleSystem> ().enableEmission = true;
								planet.GetComponent<EnemySpawner> ().spawnEnemies (10);
								//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
								planet.GetComponent<SpawnTrees> ().spawnTrees (0, 10, 120, 0, 50);
								planet.GetComponent<SpawnHealthPacks> ().spawnHealth (2);
								this.GetComponent<Sounds> ().playAmbienceSound (Sounds.PLANET_3_AMBIENCE);
								this.GetComponent<Sounds> ().pauseSound ("ambience");
								spawnedLevel = true;
								sun.intensity = 1.2f;
								textureMonsters();
							}
							break;
						}
					case 4:
						{
							if (!spawnedLevel) {
								clearParticles ();
								desert.SetActive (true);
								desert.GetComponent<ParticleSystem> ().enableEmission = true;
								planet.GetComponent<EnemySpawner> ().spawnEnemies (40);
								//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
								planet.GetComponent<SpawnTrees> ().spawnTrees (0, 0, 0, 75, 35);
								planet.GetComponent<SpawnHealthPacks> ().spawnHealth (15);
								this.GetComponent<Sounds> ().playAmbienceSound (Sounds.PLANET_4_AMBIENCE);
								this.GetComponent<Sounds> ().pauseSound ("ambience");
								spawnedLevel = true;
								sun.intensity = 1.3f;
								textureMonsters();
							}
							break;
						}
					case 5:
						{
							if (!spawnedLevel) {
								clearParticles ();
								candy.SetActive (true);
								candy.GetComponent<ParticleSystem> ().enableEmission = true;
								planet.GetComponent<EnemySpawner> ().spawnEnemies (50);
								//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
								planet.GetComponent<SpawnTrees> ().spawnTrees (0, 16, 16, 0, 40);
								planet.GetComponent<SpawnHealthPacks> ().spawnHealth (20);
								this.GetComponent<Sounds> ().playAmbienceSound (Sounds.PLANET_5_AMBIENCE);
								this.GetComponent<Sounds> ().pauseSound ("ambience");
								spawnedLevel = true;
								sun.intensity = 1.2f;
								textureMonsters();
							}
							break;
						}
					/*default:
						{
							GameObject.Find ("Player").GetComponent<Rigidbody> ().useGravity = true;
							GameObject.Find ("Player").GetComponent<FauxGravityBody> ().attractor = null;
							GameObject.Find ("Player").GetComponent<Sounds> ().playWorldSound (Sounds.FINISHED_GAME);
							Application.LoadLevel ("EndOfGame");
							break;
						}*/
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
	}
	
	public Material GetMaterial(int level){
		if(materials != null && materials.Count > 0 && level >= 0 && level < materials.Count){
			return materials[level];
		}
		return null;
	}

	void textureMonsters(){
		enemies = GameObject.Find("Planet").GetComponent<EnemySpawner>().getEnemies();

		foreach(GameObject alien in enemies.ToList()){
			if(alien.GetComponent<Enemy>().typeID == "BossAlien"){
				textureBoss(alien);
			}
		}
	}

	void textureBoss(GameObject alien){
		switch (currentLevel) {
		case 1:{
			monsterRenderer = alien.transform.FindChild("MonsterBody").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_BODY_LEVEL1);
			monsterRenderer = alien.transform.FindChild("Eye_001").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL1);
			monsterRenderer = alien.transform.FindChild("Eye_002").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL1);
			break;
		}
		case 2:{
			monsterRenderer = alien.transform.FindChild("MonsterBody").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_BODY_LEVEL2);
			monsterRenderer = alien.transform.FindChild("Eye_001").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL2);
			monsterRenderer = alien.transform.FindChild("Eye_002").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL2);
			break;
		}
		case 3:{
			monsterRenderer = alien.transform.FindChild("MonsterBody").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_BODY_LEVEL3);
			monsterRenderer = alien.transform.FindChild("Eye_001").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL3);
			monsterRenderer = alien.transform.FindChild("Eye_002").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL3);
			break;
		}
		case 4:{
			monsterRenderer = alien.transform.FindChild("MonsterBody").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_BODY_LEVEL4);
			monsterRenderer = alien.transform.FindChild("Eye_001").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL4);
			monsterRenderer = alien.transform.FindChild("Eye_002").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL4);
			break;
		}
		case 5:{
			monsterRenderer = alien.transform.FindChild("MonsterBody").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_BODY_LEVEL5);
			monsterRenderer = alien.transform.FindChild("Eye_001").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL5);
			monsterRenderer = alien.transform.FindChild("Eye_002").GetComponent<Renderer>();
			monsterRenderer.material = GetMaterial(BOSS_EYE_LEVEL5);
			break;
		}
		}
	}
}