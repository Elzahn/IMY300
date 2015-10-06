using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class StorageScript : MonoBehaviour {
	private PlayerAttributes attributesScript;
	private StorageList storageList;
	private Sounds sound;
	
	void Start(){
		attributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes> ();
		storageList = GameObject.Find ("StorageInventoryWeaponScroll").GetComponent<StorageList> ();
		sound = GameObject.Find ("Player").GetComponent<Sounds>();
	}
	
	public void dropInventoryItem(){
		InventoryItem item;
		
		if (this.transform.parent.GetComponent<PlaceInList>() != null) {
			item = this.transform.parent.GetComponent<PlaceInList> ().myItem;
		} else {
			item = this.transform.parent.parent.GetComponent<PlaceInList> ().myItem;
		}
		
		attributesScript.inventory.Remove (item);
		sound.playWorldSound(Sounds.DROP_ITEM);
		storageList.setUpStorage();
	}

	public void dropStorageItem(){
		InventoryItem item;
		
		if (this.transform.parent.GetComponent<PlaceInList>() != null) {
			item = this.transform.parent.GetComponent<PlaceInList> ().myItem;
		} else {
			item = this.transform.parent.parent.GetComponent<PlaceInList> ().myItem;
		}
		
		attributesScript.storage.Remove (item);
		sound.playWorldSound(Sounds.DROP_ITEM);
		storageList.setUpStorage();
	}

	public void storeItem(){
		InventoryItem item;
		
		if (this.transform.parent.GetComponent<PlaceInList>() != null) {
			item = this.transform.parent.GetComponent<PlaceInList> ().myItem;
		} else {
			item = this.transform.parent.parent.GetComponent<PlaceInList> ().myItem;
		}
		
		attributesScript.addToStorage (item);
		attributesScript.inventory.Remove (item);
		sound.GetComponent<Sounds>().playWorldSound(Sounds.MOVE_ITEM);
		storageList.setUpStorage ();
	}

	public void takeItem(){
		InventoryItem item;
		
		if (this.transform.parent.GetComponent<PlaceInList>() != null) {
			item = this.transform.parent.GetComponent<PlaceInList> ().myItem;
		} else {
			item = this.transform.parent.parent.GetComponent<PlaceInList> ().myItem;
		}
		
		attributesScript.addToInventory (item);
		attributesScript.storage.Remove (item);
		sound.GetComponent<Sounds>().playWorldSound(Sounds.MOVE_ITEM);
		storageList.setUpStorage ();
	}
}
