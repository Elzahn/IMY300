using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class LevelSelect : MonoBehaviour {

	public List<Material> materials;
	public static LevelSelect instance;

	PlayerAttributes attrs;

	public Renderer myRenderer{ get; set;}
	public bool spawnedLevel{ get; set;}

	private GameObject planet, fireMist, rain, snow, desert;

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
		fireMist = null;
		rain = null;
		snow = null;
		desert = null;
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
	}

	void Update () {
		if (Application.loadedLevelName == "Scene") {
			if (fireMist == null) {
				fireMist = GameObject.Find ("Fire Mist");

				rain = GameObject.Find ("Rain");

				snow = GameObject.Find ("Snow");

				desert = GameObject.Find ("Desert");

				clearParticles();
			}
		}

		if (myRenderer != null) {	//You are on a planet and now will customise the level
			myRenderer.material = GetMaterial (currentLevel);
			if(Application.loadedLevelName != "Tutorial"){
				switch (currentLevel) {
					case 1:
					{
						if(!spawnedLevel)
						{
							clearParticles();
							fireMist.SetActive(true);
							fireMist.GetComponent<ParticleSystem>().enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(20);
							//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
							planet.GetComponent<SpawnTrees>().spawnTrees(110, 0, 60, 0, 50);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(10);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_1_AMBIENCE);
							this.GetComponent<Sounds>().pauseSound("ambience");
							spawnedLevel = true;
						}
						break;
					}
					case 2:
					{
						if(!spawnedLevel)
						{
							clearParticles();
							rain.SetActive(true);
							rain.GetComponent<ParticleSystem>().enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(35);
							//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
							planet.GetComponent<SpawnTrees>().spawnTrees(0, 95, 0, 0, 40);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(12);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_2_AMBIENCE);
							this.GetComponent<Sounds>().pauseSound("ambience");
							spawnedLevel = true;
						}
						break;
					}
					case 3:
					{
						if(!spawnedLevel)
						{
							clearParticles();
							snow.SetActive(true);
							snow.GetComponent<ParticleSystem>().enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(10);
							//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
							planet.GetComponent<SpawnTrees>().spawnTrees(0, 10, 120, 0, 50);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(2);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_3_AMBIENCE);
							this.GetComponent<Sounds>().pauseSound("ambience");
							spawnedLevel = true;
						}
						break;
					}
					case 4:
					{
						if(!spawnedLevel)
						{
							clearParticles();
							desert.SetActive(true);
							desert.GetComponent<ParticleSystem>().enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(40);
							//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
							planet.GetComponent<SpawnTrees>().spawnTrees(0, 0, 0, 75, 35);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(15);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_4_AMBIENCE);
							this.GetComponent<Sounds>().pauseSound("ambience");
							spawnedLevel = true;
						}
						break;
					}
					case 5:
					{
						if(!spawnedLevel)
						{
							clearParticles();
							desert.SetActive(true);
							desert.GetComponent<ParticleSystem>().enableEmission = true;
							rain.SetActive(true);
							rain.GetComponent<ParticleSystem>().enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(50);
							//Burning Tree, Green Tree, Big Tree, Bare Tree, Shrub
							planet.GetComponent<SpawnTrees>().spawnTrees(8, 8, 8, 8, 40);
							planet.GetComponent<SpawnHealthPacks>().spawnHealth(20);
							this.GetComponent<Sounds>().playAmbienceSound(Sounds.PLANET_5_AMBIENCE);
							this.GetComponent<Sounds>().pauseSound("ambience");
							spawnedLevel = true;
						}
						break;
					}
					default:
					{
						GameObject.Find("Player").GetComponent<Rigidbody>().useGravity = true;
						GameObject.Find("Player").GetComponent<FauxGravityBody>().attractor = null;
						GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(Sounds.FINISHED_GAME);
						Application.LoadLevel("EndOfGame");
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