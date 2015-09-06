using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class Equip : MonoBehaviour {
	private PlayerController playerScript;
	private PlayerAttributes attributesScript;
	// Use this for initialization
	void Start () {
		playerScript = this.GetComponent<PlayerController> ();
		attributesScript = this.GetComponent<PlayerAttributes> ();
		foreach (InventoryItem item in attributesScript.inventory.ToList()) {
			if(item.typeID != "Power Core"){
			/*if (GUI.Button (new Rect (left + width-(buttonWidth*2), top + 80, buttonWidth, itemHeight), "Drop it")) {
					attributesScript.inventory.Remove (item);
					this.GetComponent<Sounds>().playWorldSound(Sounds.DROP_ITEM);
				}
			}*/

			if(item.typeID != "Medium Health Pack" && item.typeID != "Large Health Pack" && item.typeID != "Cupcake"){
				if(item.typeID != "Power Core"){
					//if (GUI.Button (new Rect (left + width-(buttonWidth), top + 80, buttonWidth, itemHeight), "Equip")) {
						attributesScript.equipItem (item);
						if(item.typeID == "Rare Accessory" || item.typeID == "Common Accessory" || item.typeID == "Uncommon Accessory"){
							this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_ACCESSORY);
						} else if(item.typeID == "Warhammer"){
							this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_HAMMER);
						} else if(item.typeID != "Warhammer"){
							this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_SWORD);
						}
					//}
				}
			}/* else {
				if (GUI.Button (new Rect (left + width-(buttonWidth), top + 80, buttonWidth, itemHeight), "Use")) {
					if(item.typeID != "Cupcake"){
						attributesScript.useHealthPack (item);
						attributesScript.inventory.Remove (item);
						this.GetComponent<Sounds>().playWorldSound(Sounds.USE_HEALTH);
					} else {
						attributesScript.inventory.Remove (item);
						Cupcake.eatCupcake();
					}
				}
			}*/
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
