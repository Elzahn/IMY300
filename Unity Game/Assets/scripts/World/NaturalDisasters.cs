using UnityEngine;
using System.Collections;

public class NaturalDisasters : MonoBehaviour {
	
	private Warping warpingScript;
	private PlayerController playerScript;
	private float nextDisaster, delay = 60, shakeAmount, decreaseFactor, dizzyWearOfNext, dizzyDelay = 10;	
	public float shake, spin;	//how long the shake/spin lasts
	private Transform cameraTransform;
	private Vector3 originalCamPos;
	private Quaternion originalCamRotation;
	private bool spinningDone, earthquakeDone;	//set to ensure that game isn't resumed entire time

	// Use this for initialization
	void Start () {
		warpingScript = GameObject.Find ("Player").GetComponent<Warping>();
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController>();
		nextDisaster = Time.time + delay;
		dizzyWearOfNext = Time.time + dizzyDelay;
		cameraTransform = GameObject.Find ("Main Camera").transform;
		originalCamPos = cameraTransform.localPosition;
		shake = 0f;
		spin = 0f;
		decreaseFactor = 1.0f;
		shakeAmount = 0.7f;
		originalCamRotation = cameraTransform.localRotation;
		spinningDone = false;
		earthquakeDone = false;
	}
	
	// Update is called once per frame
	void Update () {

		GameObject.Find ("Directional Light").transform.Rotate (0, -0.001f, 0);	//Rotates light

		if (shake > 0) {
			cameraTransform.localPosition = originalCamPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
			nextDisaster = Time.time + delay;
		} else if (spin > 0) {
			cameraTransform.Rotate (5, 10, 5);
			spin -= Time.deltaTime * decreaseFactor;
			nextDisaster = Time.time + delay;
		} else if((shake <= 0 && earthquakeDone == true) || (spin <= 0 && spinningDone == true)){
			spin = 0f;
			shake = 0f;	
			cameraTransform.localPosition = originalCamPos;
			playerScript.setPaused (false);
			cameraTransform.localRotation = originalCamRotation;
		}

		if (playerScript.getPaused () == false) {
			if(GameObject.Find ("Player").GetComponent<PlayerAttributes>().getDizzy() == true && Time.time >= dizzyWearOfNext){
				GameObject.Find ("Player").GetComponent<PlayerAttributes>().setDizzy(false);
			}

			if (Time.time >= nextDisaster) {
				nextDisaster = Time.time + delay;
				int chance = Random.Range(0,101);

				if(chance <= 20){
					if(chance <= 10){
						shake = 2f;
						playerScript.setPaused (true);
						warpingScript.spawnTeleports ();

						GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("WorldObject");

						for (int i = 0; i < gameObjects.Length; i++) {
							if ((gameObjects [i].transform.localScale.y > gameObjects [i].transform.localScale.x) && (gameObjects [i].GetComponent<FauxGravityBody> ().getRotateMe () == true)) { //if it is taller than it is wide
								chance = Random.Range(0, 101);
								if(chance <= 30){	//chance of falling over
									gameObjects [i].GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
									gameObjects [i].transform.Rotate (new Vector3 (gameObjects [i].transform.rotation.x + Random.Range(-90, 91), gameObjects [i].transform.rotation.y + Random.Range(-90, 91), gameObjects [i].transform.rotation.z + Random.Range(-90, 91)));	//fall over 	//fall over 
									gameObjects [i].GetComponent<FauxGravityBody> ().setRotateMe ();
								}
							}
						}
						earthquakeDone = true;
						//checking for collision when falling in collision file
									
						print ("Earth quake!");
					} else {
						spin = 2f;
						if(GameObject.Find ("Player").GetComponent<PlayerController>().getJumping() == false){
							GameObject.Find ("Player").GetComponent<PlayerAttributes>().setDizzy(true);
							dizzyWearOfNext = Time.time + dizzyDelay;
						}

						GameObject[] objectsToBeMoved = GameObject.FindGameObjectsWithTag("WorldObject");	
						GameObject[] enemiesToBeMoved = GameObject.FindGameObjectsWithTag("Enemy");	
						
						int startPoint = Random.Range(0, objectsToBeMoved.Length);
						int index = startPoint;
						int moveDirection;	
				
						for(int i = 0; i < objectsToBeMoved.Length/2; i++){
							moveDirection = Random.Range(1, 21);
							objectsToBeMoved[index].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
							objectsToBeMoved[index].transform.position = new Vector3(moveDirection, objectsToBeMoved[index].transform.position.y, moveDirection);
							index++;
						}

						startPoint = Random.Range(0, enemiesToBeMoved.Length);
						index = startPoint;

						for(int i = 0; i < enemiesToBeMoved.Length/2; i++){
							moveDirection = Random.Range(1, 21);	
							//Not sure if the freeze rotation is needed
							//enemiesToBeMoved[index].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
							enemiesToBeMoved[index].transform.position = new Vector3(moveDirection, enemiesToBeMoved[index].transform.position.y, moveDirection);
							index++;
						}
						
						playerScript.setPaused (true);

						spinningDone = true;
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
