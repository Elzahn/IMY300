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
	private ParticleSystem fireMist, rain, snow, desert;

	public int currentLevel { get; set; }

	void Start(){
		spawnedLevel = false;
		fireMist = null;
		rain = null;
		snow = null;
		desert = null;
	}

	void clearParticles(){
		fireMist.enableEmission = false;
		fireMist.Clear ();
		rain.enableEmission = false;
		rain.Clear ();
		snow.enableEmission = false;
		snow.Clear ();
		desert.enableEmission = false;
		desert.Clear ();
	}

	void Update () {
		if (Application.loadedLevelName == "Scene") {
			if (fireMist == null) {
				fireMist = GameObject.Find ("Fire Mist").GetComponent<ParticleSystem> ();
				rain = GameObject.Find ("Rain").GetComponent<ParticleSystem> ();
				snow = GameObject.Find ("Snow").GetComponent<ParticleSystem> ();
				desert = GameObject.Find ("Desert").GetComponent<ParticleSystem> ();
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
							fireMist.enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(20);
							planet.GetComponent<SpawnTrees>().spawnTrees(299);
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
							rain.enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(35);
							planet.GetComponent<SpawnTrees>().spawnTrees(190);
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
							snow.enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(10);
							planet.GetComponent<SpawnTrees>().spawnTrees(350);
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
							desert.enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(40);
							planet.GetComponent<SpawnTrees>().spawnTrees(80);
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
							desert.enableEmission = true;
							planet.GetComponent<EnemySpawner>().spawnEnemies(50);
							planet.GetComponent<SpawnTrees>().spawnTrees(20);
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