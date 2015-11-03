using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public static bool loading { get; set; }

	private float delay;
	private float removeLoadingScreen;
	private float loaderPos;
	private int actualLoaderPosValue;

	public Sprite LoadingTrees;
	public Sprite LoadingWarps;
	public Sprite LoadingHealth;
	public Sprite LoadingMonsters;
	public Sprite LoadingPlayer;

	private bool monstersLoaded;
	private bool treesLoaded;
	private bool healthLoaded;
	private bool warpPointsLoaded;

	private Image loadingBar;
	private Image background;

	// Use this for initialization
	void Start () {
		background = GameObject.Find ("Loading Image").GetComponent<Image> ();
		loadingBar = GameObject.Find ("Loading Bar").GetComponent<Image> ();
		GameObject.Find ("Loading Screen").GetComponent<Canvas> ().enabled = true;
		removeLoadingScreen = Time.time + 4;
		loading = true;
		monstersLoaded = false;
		treesLoaded = false;
		healthLoaded = false;
		warpPointsLoaded = false;

		if (Application.loadedLevelName == "Scene") {
			GameObject.Find ("Player").transform.position = new Vector3 (-4.17f, 91.45f, 2.17f);
			GameObject.Find ("Player").GetComponent<Rigidbody> ().isKinematic = true;
			delay = 4;
		}
	}

	// Update is called once per frame
	void Update () {

		if (Application.loadedLevelName == "Scene") {

			GameObject planet = GameObject.Find ("Planet");

			if(planet.GetComponent<EnemySpawner> ().hasEnemiesLanded() == false && !monstersLoaded){
				background.sprite = LoadingMonsters;
				if(loadingBar.fillAmount < (float)planet.GetComponent<EnemySpawner> ().amountEnemiesLanded() / (float)planet.GetComponent<EnemySpawner> ().ENEM_COUNT){
					if(loadingBar){
						loadingBar.fillAmount = (float)planet.GetComponent<EnemySpawner> ().amountEnemiesLanded() / (float)planet.GetComponent<EnemySpawner> ().ENEM_COUNT;
					}
				}
			} else if(planet.GetComponent<EnemySpawner> ().hasEnemiesLanded() == true && !monstersLoaded){
				monstersLoaded = true;
				loadingBar.fillAmount = 0;
			} else if(planet.GetComponent<SpawnTrees> ().isTreesPlanted () == false && !treesLoaded){
				background.sprite = LoadingTrees;
				if(loadingBar && loadingBar.fillAmount < (float)planet.GetComponent<SpawnTrees> ().amountTreesLanded() / (float)planet.GetComponent<SpawnTrees> ().num_Trees){
					loadingBar.fillAmount = (float)planet.GetComponent<SpawnTrees> ().amountTreesLanded() / (float)planet.GetComponent<SpawnTrees> ().num_Trees;
				}
			} else if(planet.GetComponent<SpawnTrees> ().isTreesPlanted () == true && !treesLoaded){
				treesLoaded = true;
				loadingBar.fillAmount = 0;
			} else if(planet.GetComponent<SpawnHealthPacks> ().hasHealthLanded () == false && !healthLoaded){
				background.sprite = LoadingHealth;
				if(loadingBar.fillAmount < (float)planet.GetComponent<SpawnHealthPacks> ().amountHealthLanded() / (float)planet.GetComponent<SpawnHealthPacks> ().TOTAL_HEALTH){
					loadingBar.fillAmount = (float)planet.GetComponent<SpawnHealthPacks> ().amountHealthLanded() / (float)planet.GetComponent<SpawnHealthPacks> ().TOTAL_HEALTH;
				}
			} else if(planet.GetComponent<SpawnHealthPacks> ().hasHealthLanded () == true && !healthLoaded){
				healthLoaded = true;
				loadingBar.fillAmount = 0;
			} else if(planet.GetComponent<SpawnWarpPoints> ().wasPlaced () == false && !warpPointsLoaded){
				background.sprite = LoadingWarps;
				if(loadingBar.fillAmount < (float)planet.GetComponent<SpawnWarpPoints> ().amountWarpsPlaced() / (float)SpawnWarpPoints.TOTAL_WARPS){
					loadingBar.fillAmount = (float)planet.GetComponent<SpawnWarpPoints> ().amountWarpsPlaced() / (float)SpawnWarpPoints.TOTAL_WARPS;
				}
			} else if(planet.GetComponent<SpawnWarpPoints> ().wasPlaced () == true && !warpPointsLoaded){
				warpPointsLoaded = true;
				loadingBar.fillAmount = 0;
				removeLoadingScreen = Time.time + delay;
			} else if(Time.time > removeLoadingScreen && loading){
				loading = false;
				GameObject.Find ("Loading Screen").GetComponent<Canvas> ().enabled = false;
				GameObject.Find("Player").GetComponent<Sounds>().resumeSound("ambience");
				if(GameObject.Find("Player").GetComponent<LevelSelect>().currentLevel == 1 && !loading){
					//GameObject.Find("Player").GetComponent<SaveSpotTeleport>().showedHealthHint = true;
					GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_PLANET_HINT);
					GameObject.Find("Player").GetComponent<Tutorial>().makeHint("Need a health pack? Look out for these flowers.", GameObject.Find("Player").GetComponent<Tutorial>().Health);
					GameObject.Find("Player").GetComponent<Tutorial>().hudText.text += "\nNeed a health pack? Look out for these flowers.\n\n";
					GameObject.Find("Player").GetComponent<Tutorial>().attribteScript.narrativeSoFar += "\nNeed a health pack? Look out for these flowers.\n\n";
				}
				GameObject.Find ("Player").GetComponent<Rigidbody> ().isKinematic = false;
				GameObject.Find ("Player").transform.rotation = Quaternion.Euler(0f, -95.3399f, 0f);
				GameObject.Find ("Player").transform.position = new Vector3 (-1.651f, 80.82f, 0.84f);
			} else {
				background.sprite = LoadingPlayer;
				loadingBar.fillAmount = loadingBar.fillAmount + 0.25f;
			}
		}
	}
}
