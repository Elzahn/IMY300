using UnityEngine;
using System.Collections;

public class LightTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		print("Object is in " + LightRotation.getDark(this.gameObject));
	}
}
