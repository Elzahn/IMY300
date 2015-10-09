﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Loot : MonoBehaviour {

	public static LinkedList<InventoryItem> myLoot = new LinkedList<InventoryItem> ();

	public static bool showInventoryHint;
	public static string inventoryHintText;

	public string myName { get; private set; }
	public static bool gotPowerCore{ get; set; }

	private PlayerAttributes attributesScript;
	private PlayerController playerScript;
	private Canvas loot;
	private Text hudText;
	private static GameObject lootToDelete;

	void Start () {
		hudText = GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ();
		attributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes> ();
		playerScript = GameObject.Find("Player").GetComponent<PlayerController> ();
		loot = GameObject.Find ("Loot").GetComponent<Canvas> ();
		inventoryHintText = "Keep an eye on your accumulated XP. You can access the inventory by pressing ";
		//loot.enabled = false;
	}

	public void storeLoot(LinkedList<InventoryItem> tempLoot, string name){
		myName = name;
		foreach (InventoryItem item in tempLoot) {
			myLoot.AddLast(item);
		}
	}

	public void showMyLoot(){
		lootToDelete = this.transform.parent.gameObject;
		GameObject.Find ("Loot").GetComponent<Canvas> ().enabled = true;
		GameObject.Find("Hint").GetComponent<Image>().enabled = false;
		GameObject.Find("Hint_Image").GetComponent<Image>().enabled = false;
		GameObject.Find("Hint_Text").GetComponent<Text>().enabled = false;
		GameObject.Find("Interaction").GetComponent<Image>().enabled = false;
		GameObject.Find("Interaction_Image").GetComponent<Image>().enabled = false;
		GameObject.Find("Interaction_Text").GetComponent<Text>().enabled = false;
		InventoryGUI.HUDshows = true;
		GameObject.Find ("Player").GetComponent<PlayerController> ().paused = true;
		GameObject.Find("LootScroll").GetComponent<LootScrollList>().gatherLoot(myName, myLoot);
	}

	public void takeLootItem(){
		takeIt (this.transform.parent.parent.GetComponent<PlaceInList> ().myItem);
	}

	public void takeAll(){
		foreach (InventoryItem item in myLoot.ToList()) {
			if(!attributesScript.inventoryFull()){
				takeIt(item);
			}
		}
	}

	public void takeIt(InventoryItem item){
		attributesScript.addToInventory (item);
		myLoot.Remove (item);
		GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(Sounds.BUTTON);
		
		if(item.typeID == "Power Core"){
			gotPowerCore = true;
			GameObject.Find("Player").GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
		}

		GameObject.Find("LootScroll").GetComponent<LootScrollList>().gatherLoot(myName, myLoot);

		if (myLoot.Count == 0) {
			if(showInventoryHint){
				GameObject.Find("Player").GetComponent<Tutorial>().makeHint(inventoryHintText, GameObject.Find ("Player").GetComponent<Tutorial>().PressI);
				hudText.text += "Congratz you found a weapon. Now try to use it.\n\n";
				attributesScript.narrativeSoFar += "Congratz you found a weapon. Now try to use it.\n\n";
				GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_INVENTORY);
				Canvas.ForceUpdateCanvases();
				Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
				scrollbar.value = 0f;
			} 

			loot.enabled = false;
			GameObject.Find("Hint").GetComponent<Image>().enabled = true;
			GameObject.Find("Hint_Image").GetComponent<Image>().enabled = true;
			GameObject.Find("Hint_Text").GetComponent<Text>().enabled = true;
			GameObject.Find("Hint_Image").GetComponent<Image>().enabled = true;
			GameObject.Find("Interaction").GetComponent<Image>().enabled = true;
			GameObject.Find("Interaction_Image").GetComponent<Image>().enabled = true;
			GameObject.Find("Interaction_Text").GetComponent<Text>().enabled = true;
			GameObject.Find("Player").GetComponent<Collisions>().setLootConf();
			playerScript.paused = false;
			InventoryGUI.HUDshows = false;
			Destroy(lootToDelete);
		}
	}

	public void closeLootDialog(){
		loot.enabled = false;
		GameObject.Find("Hint").GetComponent<Image>().enabled = true;
		GameObject.Find("Hint_Image").GetComponent<Image>().enabled = true;
		GameObject.Find("Hint_Text").GetComponent<Text>().enabled = true;
		GameObject.Find("Hint_Image").GetComponent<Image>().enabled = true;
		GameObject.Find("Interaction").GetComponent<Image>().enabled = true;
		GameObject.Find("Interaction_Image").GetComponent<Image>().enabled = true;
		GameObject.Find("Interaction_Text").GetComponent<Text>().enabled = true;
		playerScript.paused = false;
		GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(Sounds.BUTTON);
		InventoryGUI.HUDshows = false;
	}
}
