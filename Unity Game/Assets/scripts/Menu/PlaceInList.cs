using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlaceInList : MonoBehaviour {

	public InventoryItem myItem{get; set;}
	public Image desc { get; set; }
	public Text itemName { get; set; }
	public Sprite itemImage{ get; set; }

	public void showItemDescription(){
		switch (myItem.type) {
		case 0:
			//Accessory
			break;
		case 1: {
			//Weapon
			foreach (Transform child in desc.transform)
				child.gameObject.SetActive (true);

			itemName.enabled = false;
			desc.enabled = true;

			Text[] tempText = desc.GetComponentsInChildren<Text>();

			foreach(Text textItem in tempText){
				switch(textItem.name){
				case "DescName":
					textItem.text = itemName.text;
					break;
				case "DescBaseDamage":
					textItem.text = "Damage: " + ((Weapon)myItem).damage;
					break;
				case "DescStaminaLoss":
					textItem.text = "Stamina: -" + ((Weapon)myItem).staminaLoss;
					break;
				case "DescLevel":
					textItem.text = "Level: " + ((Weapon)myItem).level;
					break;
				}
			}
			
			Image[] tempImage = desc.GetComponentsInChildren<Image>();
			
			foreach(Image imageItem in tempImage){
				if(imageItem.name == "DescWeaponImage"){
					imageItem.sprite = itemImage;
					break;
				}
			}

			break;
		}
		case 2:
			//Health pack
			break;
		case 3:
			//Ship piece
			break;
		}
	}

	public void hideItemDescription(){
		itemName.enabled = true;
		foreach (Transform child in desc.transform) {
			if(child.gameObject.name != "MouseHover")
				child.gameObject.SetActive (false);
		}
		desc.enabled = false;
	}
}