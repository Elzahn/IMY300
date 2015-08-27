using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public Texture2D PlantTrees;
	public Texture2D TrainMonsters;
	public Texture2D HideHealth;
	public Texture2D CreateDisasters;
	public Texture2D WarpPlayer;
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;

	public bool loading { get; set; }

	private float delay;
	private float removeLoadingScreen;
	private float loaderPos;
	private int actualLoaderPosValue;
	private Texture2D texture;
	private bool monstersLoaded;
	private bool treesLoaded;
	private bool healthLoaded;
	private bool warpPointsLoaded;
	private Vector2 pos = new Vector2(Screen.width/2 - 170, Screen.height/2);
	private Vector2 size = new Vector2(300, 40);
	private int temp;

	// Use this for initialization
	void Start () {
		temp = 0;
		loading = true;
		loaderPos = 0;
		monstersLoaded = false;
		treesLoaded = false;
		healthLoaded = false;
		warpPointsLoaded = false;

		if (Application.loadedLevelName == "Scene") {
			GameObject.Find ("Player").transform.position = new Vector3 (-4.17f, 91.45f, 2.17f);
			GameObject.Find ("Player").GetComponent<Rigidbody> ().isKinematic = true;
			delay = 3;
		}
	}

	// Update is called once per frame
	void Update () {

		if (Application.loadedLevelName == "Scene") {

			GameObject planet = GameObject.Find ("Planet");


			if(planet.GetComponent<EnemySpawner> ().hasEnemiesLanded() == false && !monstersLoaded){
				texture = TrainMonsters;
				temp = planet.GetComponent<EnemySpawner> ().amountEnemiesLanded();
			
				if(temp > actualLoaderPosValue){
					actualLoaderPosValue = temp;
					loaderPos = temp * 0.05f;
				}
			} else if(planet.GetComponent<EnemySpawner> ().hasEnemiesLanded() == true && !monstersLoaded){
				loaderPos = 0;
				actualLoaderPosValue = 0;
				monstersLoaded = true;
				temp = 0;
			} else if(planet.GetComponent<SpawnTrees> ().isTreesPlanted () == false && !treesLoaded){
				texture = PlantTrees;
				temp = planet.GetComponent<SpawnTrees> ().amountTreesLanded ();

				if(temp > actualLoaderPosValue){
					actualLoaderPosValue = temp;
					loaderPos = temp * 0.0033f;
				}
			} else if(planet.GetComponent<SpawnTrees> ().isTreesPlanted () == true && !treesLoaded){
				loaderPos = 0;
				actualLoaderPosValue = 0;
				treesLoaded = true;
				temp = 0;
			} else if(planet.GetComponent<SpawnHealthPacks> ().hasHealthLanded () == false && !healthLoaded){
				texture = HideHealth;
				temp = planet.GetComponent<SpawnHealthPacks> ().amountHealthLanded ();

				if(temp > actualLoaderPosValue){
					actualLoaderPosValue = temp;
					loaderPos = temp * 0.1f;
				}
			} else if(planet.GetComponent<SpawnHealthPacks> ().hasHealthLanded () == true && !healthLoaded){
				loaderPos = 0;
				actualLoaderPosValue = 0;
				healthLoaded = true;
				temp = 0;
			} else if(planet.GetComponent<SpawnWarpPoints> ().wasPlaced () == false && !warpPointsLoaded){
				texture = CreateDisasters;//dont have a warppoint screen :(
				temp = planet.GetComponent<SpawnWarpPoints> ().amountWarpsPlaced ();

				if(temp > actualLoaderPosValue){
					actualLoaderPosValue = temp;
					loaderPos = temp * 0.2f;
				}
			} else if(planet.GetComponent<SpawnWarpPoints> ().wasPlaced () == true && !warpPointsLoaded){
				loaderPos = 0;
				warpPointsLoaded = true;
				removeLoadingScreen = Time.time + delay;
			} else if(Time.time > removeLoadingScreen && loading){
				loading = false;
				GameObject.Find ("Player").GetComponent<Rigidbody> ().isKinematic = false;
				GameObject.Find ("Player").transform.position = new Vector3 (0.304f, 80.394f, 0.207f);//(-4.17f, 78.85f, 2.17f);
			} else {
				texture = WarpPlayer;
				loaderPos = Time.time * 0.035f;
			}
		}
	}

	public void OnGUI(){

		if (loading) {

			GUI.depth = 0;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), texture);

			// draw the background:
			GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
			GUI.DrawTexture (new Rect (0,0, size.x, size.y), progressBarEmpty);
			
			// draw the filled-in part:
			GUI.BeginGroup (new Rect (0, 0, size.x, size.y));
			GUI.DrawTexture (new Rect (0,0, size.x * loaderPos, size.y), progressBarFull);
			GUI.EndGroup ();
			
			GUI.EndGroup ();
		}
	}
}
