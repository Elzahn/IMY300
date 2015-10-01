using UnityEngine;
using System.Collections;

public class FallThroughPlanet : MonoBehaviour {

	public bool canFallThroughPlanet { get; set; }
	public bool fallThroughPlanetUnlocked{ get; set; }
	private float nextUsage;
	private float delay = 10;
	private PlayerController playerScript;

	// Use this for initialization
	void Start () {
		canFallThroughPlanet = false; 
		nextUsage = Time.time + delay;
		fallThroughPlanetUnlocked = false; //will be changed to true after first mini boss when it is unlocked
		playerScript = this.GetComponent<PlayerController>();
	}

	//used to cause a direct fall
	public void fallNow(){
		var pos = transform.position;
		transform.position = new Vector3 (-pos.x, -pos.y, -pos.z);
		this.GetComponent<Sounds>().playWorldSound(Sounds.WARPING);
	}

	void Update(){
		if (playerScript.paused == false && fallThroughPlanetUnlocked == true) {
			if (canFallThroughPlanet == true) {
				if (Input.GetKeyDown (KeyCode.F) && Application.loadedLevelName != "SaveSpot") {
					canFallThroughPlanet = false;
					Camera.main.GetComponent<HUD>().turnOffLights("fall");
					var pos = transform.position;
					transform.position = new Vector3 (-pos.x, -pos.y, -pos.z);
					this.GetComponent<Sounds>().playWorldSound(Sounds.WARPING);
				//	print ("Cooldown of 10 seconds"); 	//show that waiting
					//PlayerLog.addStat("Cooldown of 10 seconds");
				} 
			}

			if (canFallThroughPlanet == false && Time.time >= nextUsage){
				nextUsage = Time.time + delay;
				canFallThroughPlanet = true;
				Camera.main.GetComponent<HUD>().setLight("fall");
				//print ("Recharged!");	//show when done waiting
			//	PlayerLog.addStat("Recharged!");
			}
		}/* else if(Input.GetKeyDown (KeyCode.F) && fallThroughPlanetUnlocked == false){
			print ("You have not unlocked the power to fall through the planet yet (Mapped to F)");
			//PlayerLog.addStat("You have not unlocked the power to fall through the planet yet (Mapped to F)");
		}*/
	}
}
