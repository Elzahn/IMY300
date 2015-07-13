using UnityEngine;
using System.Collections;

public class FallThroughPlanet : MonoBehaviour {

	private bool canFallThroughPlanet;
	public static bool fallThroughPlanetUnlocked;
	private float nextUsage;
	private float delay = 10;
	private PlayerController playerScript;

	// Use this for initialization
	void Start () {
		canFallThroughPlanet = true; 
		nextUsage = Time.time + delay;
		fallThroughPlanetUnlocked = false; //will be changed to true after first mini boss when it is unlocked
		playerScript = this.GetComponent<PlayerController>();
	}
	
	void Update(){
		if (playerScript.getPaused() == false && fallThroughPlanetUnlocked == true) {
			if (canFallThroughPlanet == true) {
				if (Input.GetKeyDown (KeyCode.F)) {
					canFallThroughPlanet = false;
				
					var pos = transform.position;
					transform.position = new Vector3 (-pos.x, -pos.y, -pos.z);
					this.GetComponent<Sounds>().playWorldSound(6);
					print ("Cooldown of 10 seconds"); 	//show that waiting
				} 
			}

			if (canFallThroughPlanet == false && Time.time >= nextUsage){
				nextUsage = Time.time + delay;
				canFallThroughPlanet = true;
				print ("Recharged!");	//show when done waiting
			}
		} else if(Input.GetKeyDown (KeyCode.F) && fallThroughPlanetUnlocked == false){
			print ("You have not unlocked the power to fall through the planet yet (Mapped to F)");
		}
	}
}
