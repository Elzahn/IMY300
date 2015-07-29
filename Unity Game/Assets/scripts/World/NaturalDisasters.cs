using UnityEngine;
using System.Collections;

public class NaturalDisasters : MonoBehaviour {
	
	private bool earthquakeNow = false;
	private bool spinNow = false;
	private PlayerController playerScript;
	private float nextDisaster, delay = 180, shakeAmount, dizzyWearOfNext, dizzyDelay = 10;	//decreaseFactor
	public static float shake, spin;	//how long the shake/spin lasts
	private Transform cameraTransform;
	private Vector3 originalCamPos;
	private Quaternion originalCamRotation;
	private bool spinningDone, earthquakeDone;	//set to ensure that game isn't resumed entire time

	public void makeEarthQuakeHappen(){
		earthquakeNow = true;
	}

	public void spinPlanetNow(){
		spinNow = true;
	}
	// Use this for initialization
	void Start () {
		//warpingScript = GameObject.Find ("Player").GetComponent<Warping>();
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController>();
		nextDisaster = Time.time + delay;
		dizzyWearOfNext = Time.time + dizzyDelay;
		cameraTransform = GameObject.Find ("Main Camera").transform;
		originalCamPos = cameraTransform.localPosition;
		shake = 0f;
		spin = 0f;
	//	decreaseFactor = 1.0f;
		shakeAmount = 0.7f;
		originalCamRotation = cameraTransform.localRotation;
		spinningDone = false;
		earthquakeDone = false;
		GameObject.Find ("Player").GetComponent<Sounds> ().playAmbienceSound (0);
	}
	
	// Update is called once per frame
	void Update () {

		var spawnTrees = GameObject.Find ("Planet").GetComponent<SpawnTrees> ();
		if (!spawnTrees.isTreesPlanted () && shake > 0) {
			cameraTransform.localPosition = originalCamPos + Random.insideUnitSphere * shakeAmount;
			//shake -= Time.deltaTime * decreaseFactor;
			nextDisaster = Time.time + delay;
		} else if(spawnTrees.isTreesPlanted ()){
			shake = -1;
		}else if ((!spawnTrees.isTreesPlanted () || !GameObject.Find("Planet").GetComponent<EnemySpawner>().hasEnemiesLanded()) && spin > 0) {
				cameraTransform.Rotate (5, 10, 5);
				//spin -= Time.deltaTime * decreaseFactor;
				nextDisaster = Time.time + delay;
		} else if(spawnTrees.isTreesPlanted () && GameObject.Find("Planet").GetComponent<EnemySpawner>().hasEnemiesLanded()){
			spin = -1;
		} else if((shake <= 0 && earthquakeDone) || (spin <= 0 && spinningDone)){
			GameObject.Find("Player").GetComponent<Sounds>().stopSound("world");
			spin = 0f;
			shake = 0f;	
			cameraTransform.localPosition = originalCamPos;
			playerScript.paused = false;
			cameraTransform.localRotation = originalCamRotation;
		}

		if (!playerScript.paused) {


			var playerAttributes = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
			if (playerAttributes.dizzy && Time.time >= dizzyWearOfNext) {
				playerAttributes.dizzy = false;
			}

			if (Time.time >= nextDisaster - 10) {
				GameObject.Find ("Player").GetComponent<Sounds> ().playAlarmSound (0);
			}
			if (Time.time >= nextDisaster - 5) {
				GameObject.Find ("Player").GetComponent<Sounds> ().stopAlarmSound (0);
			}

			if (Time.time >= nextDisaster || earthquakeNow || spinNow) {
				nextDisaster = Time.time + delay;
				int chance = Random.Range (0, 101);

				//moved chance to the back so cheats get preferance
				if (earthquakeNow || spinNow || chance <= 20) {	//Earthquake
					if (earthquakeNow || chance <= 10) {
						GameObject.Find ("Player").GetComponent<Sounds> ().playWorldSound (13);
						shake = 2f;
						playerScript.paused = true;
						SpawnWarpPoints.spawnNewTeleports ();

						GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("WorldObject");

						for (int i = 0; i < gameObjects.Length; i++) {
							if ((gameObjects [i].name != "Sphere001" && gameObjects [i].name != "Cylinder001") && (gameObjects [i].transform.localScale.y > gameObjects [i].transform.localScale.x) && (gameObjects [i].GetComponent<FauxGravityBody> ().getRotateMe () == true)) { //if it is taller than it is wide
								chance = Random.Range (0, 101);
								if (chance <= 30 || earthquakeNow) {	//chance of falling over
									gameObjects [i].GetComponent<FauxGravityBody> ().setRotateMe ();
									gameObjects [i].GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
									gameObjects [i].transform.Rotate (new Vector3 (gameObjects [i].transform.rotation.x + Random.Range (-90, 91), gameObjects [i].transform.rotation.y + Random.Range (-90, 91), gameObjects [i].transform.rotation.z + Random.Range (-90, 91)));	//fall over 
								}
							}
						}
						earthquakeDone = true;
						earthquakeNow = false;
						//checking for collision when falling in collision file
									
						print ("Earth quake!");
					} else if (spinNow || chance > 10) {	//Spin
						GameObject.Find ("Player").GetComponent<Sounds> ().playWorldSound (12);
						spin = 2f;

						if (!playerScript.jumping) {
							playerAttributes.dizzy = true;
							dizzyWearOfNext = Time.time + dizzyDelay;
						}

						GameObject[] objectsToBeMoved = GameObject.FindGameObjectsWithTag ("WorldObject");	
						GameObject[] enemiesToBeMoved = GameObject.FindGameObjectsWithTag ("Monster");	

						int moveDirection;	
						int index = Random.Range (0, objectsToBeMoved.Length / 2);

						for (int i = 0; i < objectsToBeMoved.Length/2; i++) {

							GameObject temp = null;

							if (objectsToBeMoved [index].name == "Cylinder001" || objectsToBeMoved [index].name == "Box012" || objectsToBeMoved [index].name == "Sphere001") {
								temp = objectsToBeMoved [index].transform.parent.gameObject;
							} else {
								temp = objectsToBeMoved [index].gameObject;
							}

							moveDirection = Random.Range (1, 21);
							temp.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
							temp.GetComponent<Rigidbody> ().position = new Vector3 (moveDirection, objectsToBeMoved [index].transform.position.y, moveDirection);
							index++;
						}

						index = Random.Range (0, enemiesToBeMoved.Length / 2);
						for (int i = 0; i < enemiesToBeMoved.Length/2; i++) {
							moveDirection = Random.Range (1, 21);	
							enemiesToBeMoved [index].transform.position = new Vector3 (moveDirection, enemiesToBeMoved [index].transform.position.y, moveDirection);
							index++;
						}
						
						playerScript.paused = true;

						spinningDone = true;
						spinNow = false;
						print ("Spinning around and around");
					}
				}
			}
		} else {
			nextDisaster = Time.time + delay;	//just so that natural disasters can't occure while game is paused
			dizzyWearOfNext = Time.time + dizzyDelay;
		}
	}
}
