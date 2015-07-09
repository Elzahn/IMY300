using UnityEngine;
using System.Collections;
using System.Linq;

public class InventoryGUI : MonoBehaviour {

	private bool showInventory, showStorage;
	private PlayerController playerScript;
	private PlayerAttributes attributesScript;

	// Use this for initialization
	void Start () {
		showInventory = false;
		showStorage = false;
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (GameObject.Find ("Persist") != null) {
			attributesScript = GameObject.Find ("Persist").GetComponent<PlayerAttributes> ();
		} else {
			attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I) && showStorage == false) {
			if(showInventory) {
				closeInventory();
			} else if(showStorage == false){
				openInventory();
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if(showStorage){
				closeStorage();
			}
		}
	}

	void OnCollisionEnter (Collision col){
		if (col.collider.name == "Storage") {
			openStorage();
		}
	}

	public void closeStorage(){
		showStorage = false;
		playerScript.setPaused (false);	//Resume game
	}

	public void openStorage(){
		showStorage = true;
		playerScript.setPaused (true);	//Pause game
	}

	public void openInventory(){
		showInventory = true;
		playerScript.setPaused (true);	//Pause game
	}

	public void closeInventory(){
		showInventory = false;
		playerScript.setPaused (false);	//Resume game
	}

	void OnGUI()
	{
		if (showInventory) {
			
			int left = 40;
			int top = 10;
			int secondLeft = left;
			int secondTop = top;
			int height = 400;

			GUI.Box (new Rect (left, top, 800, 800), "Character Review (Press I to close)");
			GUI.Box (new Rect (left, top + 40, 400, height), "Inventory \t" + attributesScript.inventory.Count + "/" + attributesScript.getMaxInventory ());

			if (attributesScript.inventory.Count == 0) {
				GUI.Label (new Rect (left + 120, top + 100, 150, 20), "No items in inventory");
			} else {
				foreach (InventoryItem item in attributesScript.inventory.ToList()) {
					GUI.Label (new Rect (left + 30, top + 80, 300, 30), item.typeID);
					if (GUI.Button (new Rect (left + 170, top + 80, 100, 30), "Drop it")) {
						attributesScript.inventory.Remove (item);
					}

					if(item.typeID != "Medium Health Pack" && item.typeID != "Large Health Pack"){
						if (GUI.Button (new Rect (left + 270, top + 80, 100, 30), "Equip")) {
							attributesScript.equipItem (item);
							attributesScript.inventory.Remove (item);
						}
					} else {
						if (GUI.Button (new Rect (left + 270, top + 80, 100, 30), "Use")) {
							attributesScript.useHealthPack (item);
							attributesScript.inventory.Remove (item);
						}
					}
					top += 30;
				}
			}

			//show character attributes
			GUI.Box (new Rect (secondLeft + 400, secondTop + 40, 400, height), "Character Attributes");
			GUI.Label (new Rect (secondLeft + 430, secondTop + 80, 150, 20), "Xp: " + attributesScript.getXp () + "/" + attributesScript.getExpectedXP ());
			GUI.Label (new Rect (secondLeft + 430, secondTop + 100, 150, 20), "Hp: " + attributesScript.getHealth () + "/" + attributesScript.maxHP());
			GUI.Label (new Rect (secondLeft + 430, secondTop + 120, 150, 20), "Stamina: " + attributesScript.getStamina () + "/" + attributesScript.maxStamina());
			GUI.Label (new Rect (secondLeft + 430, secondTop + 140, 150, 20), "Level: " + attributesScript.getLevel ());
		
			secondTop += 100;
			secondLeft += 400;

			if (attributesScript.accessories.Count != 0) {
				foreach (Accessory item in attributesScript.accessories.ToList()) {
					GUI.Label (new Rect (secondLeft + 30, secondTop + 80, 300, 30), item.typeID);
					if (GUI.Button (new Rect (secondLeft + 270, secondTop + 80, 100, 30), "Unequip")) {
						attributesScript.unequipAccessory (item);
						attributesScript.addToInventory (item);
					}
					secondTop += 30;
				}
			} else {
				GUI.Label (new Rect (secondLeft + 30, secondTop + 90, 300, 30), "No accessories equiped");
				secondTop += 30;
			}

			if (attributesScript.weapon == null) {
				GUI.Label (new Rect (secondLeft + 30, secondTop + 80, 300, 30), "No weapon equiped");
				secondTop += 30;
			} else {
				GUI.Label (new Rect (secondLeft + 30, secondTop + 80, 300, 30), attributesScript.weapon.typeID);
				if (GUI.Button (new Rect (secondLeft + 270, secondTop + 80, 100, 30), "Unequip")) {
					attributesScript.addToInventory (attributesScript.weapon);
					attributesScript.unequipWeapon (attributesScript.weapon);
				}
			}
		} else if (showStorage) {
			int left = 40;
			int top = 10;
			int secondLeft = left;
			int secondTop = top;
			int height = 400;
			
			GUI.Box (new Rect (left, top, 800, 800), "Storage/Inventory Space (Press Esc to close)");
			GUI.Box (new Rect (left, top + 40, 400, height), "Storage \t" + attributesScript.storage.Count + "/" + attributesScript.getMaxStorage ());
			
			if (attributesScript.storage.Count == 0) {
				GUI.Label (new Rect (left + 120, top + 100, 150, 20), "No items in storage");
			} else {
				foreach (InventoryItem item in attributesScript.storage.ToList()) {
					GUI.Label (new Rect (left + 30, top + 80, 300, 30), item.typeID);
					if (GUI.Button (new Rect (left + 170, top + 80, 100, 30), "Drop it")) {
						attributesScript.storage.Remove (item);
					}
					if (GUI.Button (new Rect (left + 270, top + 80, 100, 30), "Move to Inventory")) {
						attributesScript.addToInventory(item);
						attributesScript.storage.Remove (item);
					}
					top += 30;
				}
			}
						
			secondLeft += 400;

			GUI.Box (new Rect (secondLeft, secondTop + 40, 400, height), "Inventory \t" + attributesScript.inventory.Count + "/" + attributesScript.getMaxInventory ());
			
			if (attributesScript.inventory.Count == 0) {
				GUI.Label (new Rect (secondLeft + 120, secondTop + 100, 150, 20), "No items in inventory");
			} else {
				foreach (InventoryItem item in attributesScript.inventory.ToList()) {
					GUI.Label (new Rect (secondLeft + 30, secondTop + 80, 300, 30), item.typeID);
					if (GUI.Button (new Rect (secondLeft + 170, secondTop + 80, 100, 30), "Drop it")) {
						attributesScript.inventory.Remove (item);
					}
					if (GUI.Button (new Rect (secondLeft + 270, secondTop + 80, 100, 30), "Move To Storage")) {
						attributesScript.addToStorage (item);
						attributesScript.inventory.Remove (item);
					}
					secondTop += 30;
				}
			}
		}
	}
}
