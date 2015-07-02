using UnityEngine;
using System.Collections;

public abstract class InventoryItem {

	/**
	 *  0 - Accessory
	 *  1 - Weapon
	 */
	public readonly int type;
	public readonly string typeID;
	public InventoryItem(int t, string typeID) {
		type = t;
		this.typeID = typeID;
	}

}
