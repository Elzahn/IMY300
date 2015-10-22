using UnityEngine;
using System.Collections;

public class Warping : MonoBehaviour {

	private GameObject target;
	private bool waitingForMovement;
	public bool chooseDestinationUnlocked{ get; set; }
	public bool chooseDestination{ get; set; }

	public Collider col{ get; private set;}
	public float nextUsage{ get; set;}
	public float delay{ get; private set; }

	private GameObject planet;
	private float PlanetRadius;
	private PlayerController playerScript;
	private PlayerAttributes attributesScript;
	private bool temp;

	// Use this for initialization
	void Start () {
		temp = false;
		col = null;
		delay = 10;
		waitingForMovement = false;
		chooseDestinationUnlocked = false;	//unlocks at level 6
		chooseDestination = true;
		GameObject.Find("Warp").GetComponent<Canvas>().enabled = false;
		playerScript = GameObject.Find("Player").GetComponent<PlayerController> ();
		attributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes> ();
	}

	// Update is called once per frame
	void Update () {
		if (playerScript.paused == false) {
			if (waitingForMovement && GameObject.Find("Player").GetComponent<Rigidbody> ().velocity.magnitude > 0) {
				waitingForMovement = false;
			} else {
				waitingForMovement = true;
			}

			if (chooseDestination == false && Time.time >= nextUsage){
				nextUsage = Time.time + delay;
				chooseDestination = true;
				Camera.main.GetComponent<HUD>().setLight("warp");
				//print ("You can now choose your destination when warping again.");
				//PlayerLog.addStat("You can now choose your destination when warping again.");
			}
		}
	}

	public void generateRandomWarpPoint(int randomWarpPoint){
		playerScript.paused = false;	//resume game
		GameObject.Find("Warp").GetComponent<Canvas>().enabled = false;

		if("WarpPoint"+randomWarpPoint == col.name && !attributesScript.justWarped){
			generateRandomWarpPoint(Random.Range(1,6));
		}
		else{
			GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(Sounds.WARPING);
			attributesScript.justWarped = true;
			temp = true;
			GameObject newLocationWarpPoint = GameObject.Find("WarpPoint"+randomWarpPoint);
			Vector3 newLocation = newLocationWarpPoint.transform.position;
			GameObject.Find("Player").transform.position = new Vector3(newLocation.x+1,newLocation.y,newLocation.z+1);	//new Vector3 (newLocation.x, newLocation.y, newLocation.z);
			PlayerAttributes playerAttributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes>();
			int healthToLose = (int)(playerAttributesScript.hp * 0.05);
			playerAttributesScript.loseHP(healthToLose);//loses 5% health when warping
			//print ("You lose " + healthToLose + " health");
			//PlayerLog.addStat("You lose " + healthToLose + " health");
		}
	}

	void OnCollisionEnter(Collision col){
		if (col.collider.name == "Planet" && !temp) {
			attributesScript.justWarped = false;
		}
	}

	void OnTriggerEnter (Collider target){
		if (attributesScript.justWarped == false && target.tag == "WarpPoint") {
			col = target.transform.parent.gameObject.GetComponent<Collider>();
			playerScript.paused =(true);	//Pause game

			if(chooseDestinationUnlocked && chooseDestination){
				GameObject.Find("Warp").GetComponent<Canvas>().enabled = true;
			}
			else{
				generateRandomWarpPoint(Random.Range(1, 6));
			}
		}
	}

	void OnTriggerExit(Collider col){
		//if (!waitingForMovement ) {
		if(col.tag == "WarpPoint"){
			temp = false;
			//attributesScript.justWarped = false;
		}
	}
}
