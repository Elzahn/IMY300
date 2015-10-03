using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlaceInList : MonoBehaviour {

	public InventoryItem myItem{get; set;}
	public Image desc { get; set; }
	public Text itemName { get; set; }
	public Sprite itemImage{ get; set; }

	private PlayerAttributes playerAttributes;

	void Start(){
		playerAttributes = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		//set up in scrollableList line 114 around
	}
	
	public void showItemDescription(){
		switch (myItem.type) {
		case 0:
			//Accessory
			break;
		case 1: {
			//Weapon
			foreach (Transform child in desc.transform) {
				if(child.gameObject.name == "Equip" && ((Weapon)myItem).level > playerAttributes.level){
					child.gameObject.GetComponent<Button>().interactable = false;
				} else if(child.gameObject.name == "DescLevel" && ((Weapon)myItem).level > playerAttributes.level){
					child.gameObject.GetComponent<Text>().color = Color.red;
					child.gameObject.SetActive (true);
				} else {
					child.gameObject.SetActive (true);
				}
			}
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

	public void showEquiped(){
		foreach (Transform child in desc.transform) {
			if (child.gameObject.name != "MouseHover")
				child.gameObject.SetActive (true);
			if(child.name == "DescName"){
				child.gameObject.GetComponent<Text>().text = itemName.text;
			} else if(child.name == "DescWeaponImage"){
				child.gameObject.GetComponent<Image>().sprite = itemImage;
			}
		}
		desc.enabled = true;
	}
}