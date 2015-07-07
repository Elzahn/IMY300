using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	private bool showInventory;
	// Use this for initialization
	void Start () {
		showInventory = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			showInventory = true;
		}
	}
}
