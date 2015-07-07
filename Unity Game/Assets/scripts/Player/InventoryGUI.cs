using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {

	private bool showInventory;
	private PlayerController playerScript;
	private PlayerAttributes attributesScript;

	// Use this for initialization
	void Start () {
		showInventory = false;
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			if(showInventory) {
				closeInventory();
			} else {
				openInventory();
			}
		}
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

			GUI.Box (new Rect (left, top, 800, 800), "Character Review (Press I to close)\t"+attributesScript.inventory.Count+"/"+attributesScript.getMaxInventory());
			GUI.Box (new Rect (left, top+40, 400, height), "Inventory \t"+attributesScript.inventory.Count+"/"+attributesScript.getMaxInventory());

			if(attributesScript.inventory.Count == 0){
				GUI.Label(new Rect(left+120, top+100, 150, 20), "No items in inventory");
			}
			else {
				foreach (InventoryItem item in attributesScript.inventory) {
					GUI.Label(new Rect(left+30, top+80, 300, 30), item.typeID);
					if (GUI.Button(new Rect(left+170, top+80, 100, 30), "Drop it")){
						attributesScript.inventory.Remove(item);
					}
					if (GUI.Button(new Rect(left+270, top+80, 100, 30), "Equip")){
						attributesScript.inventory.Remove(item);
					}
					top += 30;
				}
			}

			//show character attributes
			GUI.Box (new Rect (secondLeft+400, secondTop+40, 400, height), "Character Attributes");
			GUI.Label(new Rect(secondLeft+430, secondTop+80, 150, 20), "Xp: " + attributesScript.getXp());
			GUI.Label(new Rect(secondLeft+430, secondTop+100, 150, 20), "Hp: " + attributesScript.getHealth());
			GUI.Label(new Rect(secondLeft+430, secondTop+120, 150, 20), "Stamina: " + attributesScript.getStamina());
			GUI.Label(new Rect(secondLeft+430, secondTop+140, 150, 20), "Level: " + attributesScript.getLevel());
		
			//set second top

			//display accessories

			//display weapons

			//buttons drop / unequip

		}
	}
}
