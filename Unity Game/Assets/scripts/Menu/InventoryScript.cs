using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class InventoryScript : MonoBehaviour {
	private PlayerAttributes attributesScript;
	private ScrollableList scrollableList;
	private Sounds sound;

	void Start(){
		attributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes> ();
		scrollableList = GameObject.Find ("WeaponScroll").GetComponent<ScrollableList> ();
		sound = GameObject.Find ("Player").GetComponent<Sounds>();
	}

	public void dropWeapon(){
		InventoryItem item;
		
		if (this.transform.parent.GetComponent<PlaceInList>() != null) {
			item = this.transform.parent.GetComponent<PlaceInList> ().myItem;
		} else {
			item = this.transform.parent.parent.GetComponent<PlaceInList> ().myItem;
		}

		attributesScript.inventory.Remove (item);
		sound.playWorldSound(Sounds.DROP_ITEM);
		scrollableList.checkInventory();
	}
	
	public void equipWeapon(){
		InventoryItem item;

		if (this.transform.parent.GetComponent<PlaceInList>() != null) {
			item = this.transform.parent.GetComponent<PlaceInList> ().myItem;
		} else {
			item = this.transform.parent.parent.GetComponent<PlaceInList> ().myItem;
		}

		attributesScript.equipItem (item);
		
		if(item.typeID == "Rare Accessory" || item.typeID == "Common Accessory" || item.typeID == "Uncommon Accessory"){
			sound.playWorldSound(Sounds.EQUIP_ACCESSORY);
		} else if(item.typeID == "Warhammer"){
			sound.playWorldSound(Sounds.EQUIP_HAMMER);
		} else if(item.typeID != "Warhammer"){
			sound.playWorldSound(Sounds.EQUIP_SWORD);
		}
		scrollableList.checkInventory();
	}
}
