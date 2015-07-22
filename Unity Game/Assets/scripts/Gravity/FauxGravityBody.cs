using UnityEngine;
using System.Collections;

public class FauxGravityBody : MonoBehaviour {

	public FauxGravityAttractor attractor;
	private Transform myTransform;
	private bool rotateMe = true;

	// Use this for initialization
	void Start () {
		var rigidbody = GetComponent<Rigidbody> ();
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		rigidbody.useGravity = false;
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName != "SaveSpot") {
			attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
			this.GetComponent<Rigidbody>().useGravity = false;
			attractor.attract (myTransform);
		} else {
			this.GetComponent<Rigidbody>().useGravity = true;
		}
	}

	public void setRotateMe(){
		rotateMe = false;
	}
	
	public bool getRotateMe(){
		return rotateMe;
	}
}
