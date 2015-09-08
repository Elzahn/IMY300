using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class InventoryScript : MonoBehaviour {
	private PlayerAttributes attributesScript;
	private ScrollableList scrollableList;

	void Start(){
		attributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes> ();
		scrollableList = GameObject.Find ("WeaponScroll").GetComponent<ScrollableList> ();
	}

	public void dropWeapon(){
		InventoryItem temp = this.transform.parent.GetComponent<PlaceInList> ().myItem;
		attributesScript.inventory.Remove (temp);
		GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(Sounds.DROP_ITEM);
		scrollableList.checkInventory();
	}
	
	public void equipWeapon(){
		print ("F");
	}
}
