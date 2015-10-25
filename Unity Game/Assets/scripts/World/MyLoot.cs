using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MyLoot : MonoBehaviour {
	public LinkedList<InventoryItem> myLoot;

	// Use this for initialization
	void Start () {
		myLoot = new LinkedList<InventoryItem>();
	}


	// Update is called once per frame
	void Update () {
	
	}
}
