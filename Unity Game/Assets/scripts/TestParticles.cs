using UnityEngine;
using System.Collections;

public class TestParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("Mist").GetComponent<ParticleSystem> ().enableEmission = false;
		GameObject.Find ("Mist").GetComponent<ParticleSystem> ().Clear ();
		GameObject.Find ("Test").GetComponent<ParticleSystem> ().enableEmission = false;
		GameObject.Find ("Test").GetComponent<ParticleSystem> ().Clear ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			TestOn();
		} 
		if (Input.GetKeyDown (KeyCode.R)) {
			TestOff();
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			MistOn();
		}
		if (Input.GetKeyDown (KeyCode.Y)) {
			MistOff();
		}
	}

	void TestOn(){
		GameObject.Find ("Test").GetComponent<ParticleSystem> ().enableEmission = true;
	}

	void TestOff(){
		GameObject.Find ("Test").GetComponent<ParticleSystem> ().enableEmission = false;
		GameObject.Find ("Test").GetComponent<ParticleSystem> ().Clear ();
	}

	void MistOn(){
		GameObject.Find ("Mist").GetComponent<ParticleSystem> ().enableEmission = true;
	}

	void MistOff(){
		GameObject.Find ("Mist").GetComponent<ParticleSystem> ().enableEmission = false;
		GameObject.Find ("Mist").GetComponent<ParticleSystem> ().Clear ();
	}
}
