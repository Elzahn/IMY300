using UnityEngine;
using System.Collections;

public class NaturalDisasters : MonoBehaviour {
	
	private bool earthquakeNow = false;
	private bool spinNow = false;
	private PlayerController playerScript;
	private float nextDisaster, delay = 120, shakeAmount, dizzyWearOfNext, dizzyDelay = 10;	//decreaseFactor
	public static float shake, spin;	//how long the shake/spin lasts
	private Transform cameraTransform;
	private Vector3 originalCamPos;
	private Quaternion originalCamRotation;
	private bool spinningDone, earthquakeDone;	//set to ensure that game isn't resumed entire time
	private float tutorialShake, cheatSpin;
	private bool earthQuakeHappening, spinHappening;
	private Sounds soundScript;
	private float stopAlarm;
	private int chance;
	private Animator animator;

	public bool isShaking(){
		if (shake > 0) {
			return true;
		} else {
			return false;
		}
	}

	public void makeEarthQuakeHappen(){
		earthquakeNow = true;
	}

	public void spinPlanetNow(){
		spinNow = true;
	}

	// Use this for initialization
	void Start () {
		animator = GameObject.Find("Character_Final").GetComponent<Animator>();
		chance = 0;
		stopAlarm = 0f;
		earthQuakeHappening = false;
		spinHappening = false;
		soundScript = GameObject.Find("Player").GetComponent<Sounds>();
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController>();
		nextDisaster = Time.time + delay;
		dizzyWearOfNext = Time.time + dizzyDelay;
		cameraTransform = GameObject.Find ("Main Camera").transform;
		originalCamPos = cameraTransform.localPosition;
		shake = 0f;
		spin = 0f;
	
		shakeAmount = 0.7f;
		originalCamRotation = cameraTransform.localRotation;
		spinningDone = false;
		earthquakeDone = false;
		//GameObject.Find ("Player").GetComponent<Sounds> ().playAmbienceSound (Sounds.PLANET_AMBIENCE);
	}
	
	// Update is called once per frame
	void Update () {
		//var spawnTrees = GameObject.Find ("Planet").GetComponent<SpawnTrees> ();
		if (shake > 0) {
			cameraTransform.localPosition = originalCamPos + Random.insideUnitSphere * shakeAmount;
			//shake -= Time.deltaTime * decreaseFactor;
			nextDisaster = Time.time + delay;
			if (Application.loadedLevelName != "Tutorial" && Time.time >= tutorialShake) {//if (Application.loadedLevelName != "Tutorial" && spawnTrees.isTreesPlanted ()) {
				shake = -1;
				playerScript.paused = false;
			} else if(Time.time >= tutorialShake){
				shake -= 1;
				playerScript.paused = false;
			//	soundScript.playAmbienceSound(Sounds.TUTORIAL_AMBIENCE);
			}
		} else if (spin > 0) {
			//cameraTransform.Rotate(5, 10, 5);
			cameraTransform.RotateAround(new Vector3(0,0,0), new Vector3(1,2,3), 5);
			//spin -= Time.deltaTime * decreaseFactor;
			nextDisaster = Time.time + delay;
			if(cheatSpin == 0){
				cheatSpin = Time.time + 3;
			}

			if (Application.loadedLevelName == "Scene" && Time.time >= cheatSpin) {//if (Application.loadedLevelName == "Scene" && spawnTrees.isTreesPlanted () && GameObject.Find ("Planet").GetComponent<EnemySpawner> ().hasEnemiesLanded ()) {
				spin = -1;
				playerScript.paused = false;
			} else if(Application.loadedLevelName == "Tutorial"){
				if(Time.time >= cheatSpin)
				{
					spin = -1;
					playerScript.paused = false;
				}
			}
		} else if ((shake <= 0 && earthquakeDone) || (spin <= 0 && spinningDone)) {
			soundScript.stopSound ("world");
			if(shake < 0 || spin < 0)
			{
				spin = 0f;
				shake = 0f;	
				cheatSpin = 0f;

				cameraTransform.localPosition = originalCamPos;
				playerScript.paused = false;
				cameraTransform.localRotation = originalCamRotation;
			}
		}

		//if (spinNow || earthquakeNow) {
			if (!playerScript.paused && !Camera.main.GetComponent<CameraControl>().birdsEye) {
				if ((Application.loadedLevelName == "Scene" && !GameObject.Find("Planet").GetComponent<LoadingScreen>().loading) || Application.loadedLevelName != "Scene"){
					var playerAttributes = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();

					if (playerAttributes.dizzy && Time.time >= dizzyWearOfNext) {
						playerAttributes.dizzy = false;
						dizzyWearOfNext = 0;
					}

					/*if (Time.time >= nextDisaster - 10) {
						GameObject.Find ("Player").GetComponent<Sounds> ().playAlarmSound (Sounds.DISASTER_ALARM);
					}
					if (Time.time >= nextDisaster - 5) {
						GameObject.Find ("Player").GetComponent<Sounds> ().stopAlarmSound (Sounds.DISASTER_ALARM);
					}*/

					if ((Application.loadedLevelName != "Tutorial" && Time.time >= nextDisaster) || earthquakeNow || spinNow || spinHappening || earthQuakeHappening) {
						nextDisaster = Time.time + delay;
						
						if(!earthQuakeHappening && !spinHappening){
							chance = Random.Range (0, 100);
						}

						int prob = 40;
						//moved chance to the back so cheats get preferance
						if (earthquakeNow || spinNow || chance <= prob) {
							if (earthquakeNow || chance <= prob/2) {	//Earthquake
								earthQuakeHappening = true;

								if(Application.loadedLevelName != "Tutorial"){
									if(stopAlarm == 0){
										stopAlarm = Time.time + 3;
										soundScript.playAlarmSound (Sounds.DISASTER_ALARM);
									}
									
									if(Time.time >= stopAlarm)
									{
										soundScript.stopAlarmSound(Sounds.DISASTER_ALARM);
										stopAlarm = 0f;
									}
								}

								if(!soundScript.alarmAudio.isPlaying){
									earthQuakeHappening = false;
									soundScript.playWorldSound (Sounds.EARTHQUAKE);
									shake = 2f;
									tutorialShake = Time.time + 3;
									playerScript.paused = true;
									if(Application.loadedLevelName != "Tutorial"){
										SpawnWarpPoints.spawnNewTeleports ();
									}

									GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("WorldObject");

									for (int i = 0; i < gameObjects.Length; i++) {
										if ((gameObjects [i].name != "Sphere001" && gameObjects [i].name != "Cylinder001") && (gameObjects [i].transform.localScale.y > gameObjects [i].transform.localScale.x) && (gameObjects [i].GetComponent<FauxGravityBody> ().getRotateMe () == true)) { //if it is taller than it is wide
											chance = Random.Range (0, 101);
											if (chance <= 30){// || earthquakeNow) {	//chance of falling over
												gameObjects [i].GetComponent<FauxGravityBody> ().setRotateMe ();
												gameObjects [i].GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
												gameObjects [i].transform.Rotate (new Vector3 (gameObjects [i].transform.rotation.x + Random.Range (-90, 91), gameObjects [i].transform.rotation.y + Random.Range (-90, 91), gameObjects [i].transform.rotation.z + Random.Range (-90, 91)));	//fall over 
											}
										}
									}
									earthquakeDone = true;
									earthquakeNow = false;
									animator.SetBool("Attacking", false);
									animator.SetBool("MovingStraight", false);
									animator.SetBool("Running", false);
									animator.SetBool("MovingRight", false);
									animator.SetBool("MovingLeft", false);
									animator.SetFloat("Turning", 0f);
								}
								//checking for collision when falling in collision file
							} else if (spinNow || chance > prob/2) {	//Spin
								spinHappening = true;

								if(Application.loadedLevelName != "Tutorial"){
									if(stopAlarm == 0){
										stopAlarm = Time.time + 3;
										soundScript.playAlarmSound (Sounds.DISASTER_ALARM);
									}
									
									if(Time.time >= stopAlarm)
									{
										soundScript.stopAlarmSound(Sounds.DISASTER_ALARM);
										stopAlarm = 0f;
									}
								}
								
								if(!soundScript.alarmAudio.isPlaying){
									spinHappening = false;
									GameObject.Find ("Player").GetComponent<Sounds> ().playWorldSound (Sounds.SPINNING_WIND);
									spin = 2f;

									if(dizzyWearOfNext != 0){	
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

										if(temp.tag == "monster"){
											temp.GetComponentInParent<PositionMe> ().touching = false;
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
									animator.SetBool("Attacking", false);
									animator.SetBool("MovingStraight", false);
									animator.SetBool("Running", false);
									animator.SetBool("MovingRight", false);
									animator.SetBool("MovingLeft", false);
									animator.SetFloat("Turning", 0f);
									//print ("Spinning around and around");
								}
							}
						}
				}
			} 
		} else {
			nextDisaster = Time.time + delay;	//just so that natural disasters can't occure while game is paused
			dizzyWearOfNext = Time.time + dizzyDelay;
		}
	}
}
