using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Loot : MonoBehaviour {

	public LinkedList<InventoryItem> myLoot = new LinkedList<InventoryItem>();
	public string myName;

	public void storeLoot(LinkedList<InventoryItem> tempLoot, string name){
		myName = name;
		foreach (InventoryItem item in tempLoot) {
			myLoot.AddLast(item);
		}
		print (myLoot.Count ());
	}
}
