using UnityEngine;
using System.Collections;

public class SaveSpotSound : MonoBehaviour {

	private Sounds sound;

	// Use this for initialization
	void Start () {
		sound = GameObject.Find ("Player").GetComponent<Sounds> ();
		sound.playAmbienceSound (Sounds.SHIP_AMBIENCE);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
