﻿using UnityEngine;
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
		attractor.attract(myTransform);
	}

	public void setRotateMe(){
		rotateMe = false;
	}
	
	public bool getRotateMe(){
		return rotateMe;
	}
}