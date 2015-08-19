using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public Texture2D Loading_Screen;
	public Texture2D PlantTrees;
	public Texture2D TrainMonsters;
	public Texture2D HideHealth;
	public Texture2D CreateDisasters;
	public Texture2D WarpPlayer;

	public bool loading { get; set; }
	private bool notYetPlaced = true;
	private float changeLoading, delay, removeLoadingScreen = 0.0f;
	private int index = 1;
	private Texture2D texture;

	// Use this for initialization
	void Start () {
		loading = true;
		if (Application.loadedLevelName == "Scene") {
			GameObject.Find("Player").transform.position = new Vector3(-4.17f, 91.45f, 2.17f);
			GameObject.Find("Player").GetComponent<Rigidbody>().isKinematic = true;
			delay = 3;
			changeLoading = Time.time + delay;
			texture = Loading_Screen;
		}
	}

	// Update is called once per frame
	void Update () {

		GameObject planet = GameObject.Find ("Planet");
		if (notYetPlaced && planet.GetComponent<SpawnTrees> ().isTreesPlanted () && planet.GetComponent<SpawnHealthPacks> ().hasHealthLanded () && planet.GetComponent<EnemySpawner> ().hasEnemiesLanded () && planet.GetComponent<SpawnWarpPoints> ().wasPlaced ()) {
			GameObject.Find ("Player").GetComponent<Rigidbody> ().isKinematic = false;
			GameObject.Find ("Player").transform.position = new Vector3 (-4.17f, 78.85f, 2.17f);

			if (removeLoadingScreen == 0) {
				removeLoadingScreen = Time.time;
			}
			if (Time.time >= removeLoadingScreen + delay) {
				loading = false;
				notYetPlaced = false;
			}
		} /*else {
			GameObject.Find ("Player").transform.position = new Vector3(0.32f, 80.37f, 032f);
		}*/
	}

	public void OnGUI(){
		if (loading) {

			if(Time.time >= changeLoading){
				if(index >= 5){
					index = -1;
				}

				index++;

				switch(index){
				case 0: {
					texture = Loading_Screen;
					break;
				}
				case 1:{
					texture = PlantTrees;
					break;
				}
				case 2:{
					texture = TrainMonsters;
					break;
				}
				case 3:{
					texture = HideHealth;
					break;
				}
				case 4:{
					texture = CreateDisasters;
					break;
				}
				case 5:{
					texture = WarpPlayer;
					break;
				}
				}
				changeLoading = Time.time + delay;
			}

			GUI.depth = 0;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), texture);
		}
	}
}
