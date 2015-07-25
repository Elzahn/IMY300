using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Loot : MonoBehaviour {

	private LinkedList<InventoryItem> myLoot = new LinkedList<InventoryItem>();
	public string myName { get; private set; }
	public bool showLoot { get; private set; }
	private PlayerAttributes attributesScript;
	private PlayerController playerScript;

	void Start () {
		attributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes> ();
		playerScript = GameObject.Find("Player").GetComponent<PlayerController> ();
	}

	public void storeLoot(LinkedList<InventoryItem> tempLoot, string name){
		myName = name;
		foreach (InventoryItem item in tempLoot) {
			myLoot.AddLast(item);
		}
	}

	public void showMyLoot(){
		showLoot = true;
	}

	void OnGUI(){
		if (showLoot) {
			playerScript.paused = true;
			int boxHeight = 250;
			int boxWidth = 400;
			int top = Screen.height/2-boxHeight/2;//30;
			int left = Screen.width/2-boxWidth/2;//200;
			int itemHeight = 30;
			int buttonWidth = 100;
			int closeTop = top;
			
			GUI.Box (new Rect (left, top, boxWidth, boxHeight), name);
			
			foreach (InventoryItem item in myLoot.ToList()) {
				GUI.Label (new Rect (left + 30, top + 40, boxWidth-buttonWidth, itemHeight), item.typeID);
				if (GUI.Button (new Rect (left + 270, top + 40, buttonWidth, itemHeight), "Take it")) {
					attributesScript.addToInventory (item);
					myLoot.Remove (item);
					GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(2);
					if (myLoot.Count == 0) {
						showLoot = false;
						Destroy(this.gameObject);
						GameObject.Find("Player").GetComponent<Collisions>().setLootConf();
						playerScript.paused = false;
					}
				}
				top += 30;
			}
			if (GUI.Button (new Rect (left + 270, closeTop + (boxHeight - itemHeight), buttonWidth, itemHeight), "Close")) {
				showLoot = false;
				playerScript.paused = false;
				GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(2);
			}
		}
	}
}
