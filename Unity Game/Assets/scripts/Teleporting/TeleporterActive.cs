using UnityEngine;
using System.Collections;

public class TeleporterActive : MonoBehaviour {

	private GameObject particles;
	private SaveSpotTeleport saveSpot;

	// Use this for initialization
	void Start () {
		saveSpot = GameObject.Find ("Player").GetComponent<SaveSpotTeleport> ();
		particles = GameObject.Find ("Particles");
		if (particles != null) {
			particles.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName != "SaveSpot" && saveSpot.canEnterSaveSpot) {
			particles.SetActive (true);
		} else {
			particles.SetActive(false);
		}
	}
}
