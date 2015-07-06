using UnityEngine;
using System.Collections;

public class NaturalDisasters : MonoBehaviour {
	
	private Warping warpingScript;
	private float nextDisaster, delay = 6, shakeAmount, decreaseFactor, dizzyWearOfNext, dizzyDelay = 10;	
	public float shake, spin;	//how long the shake/spin lasts
	private Transform cameraTransform;
	private Vector3 originalCamPos;
	private Quaternion originalCamRotation;

	// Use this for initialization
	void Start () {
		warpingScript = GameObject.Find ("Player").GetComponent<Warping>();
		nextDisaster = Time.time + delay;
		dizzyWearOfNext = Time.time + dizzyDelay;
		cameraTransform = GameObject.Find ("Main Camera").transform;
		originalCamPos = cameraTransform.localPosition;
		shake = 0f;
		spin = 0f;
		decreaseFactor = 1.0f;
		shakeAmount = 0.7f;
		originalCamRotation = cameraTransform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {

		if (shake > 0) {
			cameraTransform.localPosition = originalCamPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
			nextDisaster = Time.time + delay;
		} else if (spin > 0) {
			cameraTransform.Rotate (5, 10, 5);
			spin -= Time.deltaTime * decreaseFactor;
			nextDisaster = Time.time + delay;
		} else {
			spin = 0f;
			shake = 0f;	
			cameraTransform.localPosition = originalCamPos;
			warpingScript.setPaused (false);
			cameraTransform.localRotation = originalCamRotation;

			//if 10secs gone setDizzy(false);
		}

		if (warpingScript.getPaused () == false) {
			if(Time.time >= dizzyWearOfNext){
				GameObject.Find ("Player").GetComponent<PlayerAttributes>().setDizzy(false);
			}

			if (Time.time >= nextDisaster) {
				nextDisaster = Time.time + delay;
				int chance = Random.Range(0,101);

				//if(chance <= 20){
					if(chance <= 10){
						shake = 2f;
						warpingScript.setPaused (true);
						warpingScript.spawnTeleports ();

						GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("WorldObject");

						for (int i = 0; i < gameObjects.Length; i++) {
							if ((gameObjects [i].transform.localScale.y > gameObjects [i].transform.localScale.x) && (gameObjects [i].GetComponent<WorldObjectsGravity> ().getRotateMe () == true)) { //if it is taller than it is wide
								gameObjects [i].GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
								gameObjects [i].transform.Rotate (new Vector3 (gameObjects [i].transform.rotation.x + Random.Range(-90, 91), gameObjects [i].transform.rotation.y + Random.Range(-90, 91), gameObjects [i].transform.rotation.z + Random.Range(-90, 91)));	//fall over 	//fall over 
								gameObjects [i].GetComponent<WorldObjectsGravity> ().setRotateMe ();
							}
						}

						//check for collision when falling in collision file
									
						print ("Earth quake!");
					} else {
						spin = 2f;
						GameObject.Find ("Player").GetComponent<PlayerAttributes>().setDizzy(true);
						dizzyWearOfNext = Time.time + dizzyDelay;
						warpingScript.setPaused (true);
						print ("Spinning around and around");
					}
				//}
				//make delay 60
			}
		} else {
			nextDisaster = Time.time + delay;	//just so that natural disasters can't occure while game is paused
			dizzyWearOfNext = Time.time + dizzyDelay;
		}
	}
}
