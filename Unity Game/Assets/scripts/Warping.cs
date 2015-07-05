using UnityEngine;
using System.Collections;

public class Warping : MonoBehaviour {

	private GameObject target;
	private bool justWarped, waitingForMovement, chooseDestination, showDestinationChoice, paused;
	public bool chooseDestinationUnlocked;
	private Collider col;
	private float nextUsage;
	private float delay = 10;

	// Use this for initialization
	void Start () {
		justWarped = false;
		waitingForMovement = false;
		chooseDestinationUnlocked = false;	//unlocks at level 6
		chooseDestination = true;
		showDestinationChoice = false;
		paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!paused) {
			if (waitingForMovement && this.GetComponent<Rigidbody> ().velocity.magnitude > 0) {
				justWarped = false;
				waitingForMovement = false;
			}

			if (chooseDestination == false && Time.time >= nextUsage){
				nextUsage = Time.time + delay;
				chooseDestination = true;
				print ("You can now choose your destination when warping again.");
			}
		}
	}

	public bool getPaused()
	{
		return paused;
	}

	void generateRandomWarpPoint(int randomWarpPoint){
		paused = false;	//resume game
		showDestinationChoice = false;	//closes menu

		if("WarpPoint"+randomWarpPoint == col.gameObject.name){
			int newRandomWarpPoint = Random.Range(1,6);
			generateRandomWarpPoint(newRandomWarpPoint);
		}
		else{
			justWarped = true;
			GameObject newLocationWarpPoint = GameObject.Find("WarpPoint"+randomWarpPoint);
			Vector3 newLocation = newLocationWarpPoint.transform.position;
			this.transform.position = newLocation;	//new Vector3 (newLocation.x, newLocation.y, newLocation.z);
			PlayerAttributes playerAttributesScript = this.GetComponent<PlayerAttributes>();
			int healthToLose = (int)(playerAttributesScript.currentHealth() * 0.05);
			playerAttributesScript.loseHP(healthToLose);//loses 5% health when warping
		}
	}

	void OnTriggerEnter(Collider target){
		if (justWarped == false && target.gameObject.tag == "WarpPoint") {
			col = target;
			paused = true;	//Pause game

			if(chooseDestinationUnlocked && chooseDestination){
				showDestinationChoice = true;
			}
			else{
				int randomWarpPoint = Random.Range (1, 6);
				generateRandomWarpPoint (randomWarpPoint);
			}
		} else {
			waitingForMovement = true;
		}
	}
	void printMyNum(int n)
	{
		print (n);
	}

	void OnGUI()
	{
		if (showDestinationChoice) {

			int top = 30;

			//x, y top, length, height
			GUI.Box (new Rect (200, top, 400, 250), "To what warp point would you like to warp?");

			if("WarpPoint1" == col.gameObject.name){
				GUI.enabled = false;
			}
				
			if(GUI.Button(new Rect(320, top+30,150,20), "Warp point 1")) {
				chooseDestination = false;
				nextUsage = Time.time + delay;
				generateRandomWarpPoint(1);
			}

			GUI.enabled = true;

			if("WarpPoint2" == col.gameObject.name){
				GUI.enabled = false;
			}

			if(GUI.Button(new Rect(320, top+60,150,20), "Warp point 2")) {
				chooseDestination = false;
				nextUsage = Time.time + delay;
				generateRandomWarpPoint(2);
			}

			
			GUI.enabled = true;
			
			if("WarpPoint3" == col.gameObject.name){
				GUI.enabled = false;
			}

			if(GUI.Button(new Rect(320, top+90,150,20), "Warp point 3")) {
				chooseDestination = false;
				generateRandomWarpPoint(3);
				nextUsage = Time.time + delay;
			}

			
			GUI.enabled = true;
			
			if("WarpPoint4" == col.gameObject.name){
				GUI.enabled = false;
			}

			if(GUI.Button(new Rect(320, top+120,150,20), "Warp point 4")) {
				chooseDestination = false;
				nextUsage = Time.time + delay;
				generateRandomWarpPoint(4);
			}

			
			GUI.enabled = true;
			
			if("WarpPoint5" == col.gameObject.name){
				GUI.enabled = false;
			}

			if(GUI.Button(new Rect(320, top+150,150,20), "Warp point 5")) {
				chooseDestination = false;
				nextUsage = Time.time + delay;
				generateRandomWarpPoint(5);
			}

			
			GUI.enabled = true;

			if(GUI.Button(new Rect(320, top+180,150,20), "Random warp point")) {
				generateRandomWarpPoint(Random.Range (1, 6));
			}

			if(GUI.Button(new Rect(320, top+210,150,20), "Cancel")) {
				showDestinationChoice = false;
				paused = false;
			}
		}
	}
}
