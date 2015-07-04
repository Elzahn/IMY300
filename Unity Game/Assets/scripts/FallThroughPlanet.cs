using UnityEngine;
using System.Collections;

public class FallThroughPlanet : MonoBehaviour {
	
	public bool canFallThroughPlanet, fallThroughPlanetUnlocked;
	private float nextUsage;
	private float delay = 10;
	
	// Use this for initialization
	void Start () {
		canFallThroughPlanet = true; 
		nextUsage = Time.time + delay;
		fallThroughPlanetUnlocked = false; //will be changed to true after first mini boss when it is unlocked
	}
	
	void Update(){
		if (fallThroughPlanetUnlocked == true) {
			if (canFallThroughPlanet == true) {
				if (Input.GetKeyDown (KeyCode.F)) {
					canFallThroughPlanet = false;
				
					var pos = transform.position;
					transform.position = new Vector3 (-pos.x, -pos.y, -pos.z);
					print ("Cooldown of 10 seconds");
					//show that waiting
				}
			}

			if (Time.time >= nextUsage){
				nextUsage = Time.time + delay;
				canFallThroughPlanet = true;
				print ("Recharged!");
				//show when done waiting
			}
		} else {
			print ("You have not unlocked the power to fall through the planet yet (Mapped to F)");
		}
	}
}
