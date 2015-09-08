using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ScrollableList : MonoBehaviour
{
    public GameObject itemPrefab;
    public int itemCount = 10, columnCount = 1;

	private RectTransform rowRectTransform;
	private RectTransform containerRectTransform;
	private PlayerAttributes attributesScript;
    void Start()
    {
		rowRectTransform = itemPrefab.GetComponent<RectTransform>();
        containerRectTransform = gameObject.GetComponent<RectTransform>();
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		
		GameObject[] gameObjectsToDelete = GameObject.FindGameObjectsWithTag ("WeaponList");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}
		itemCount = 5;//attributesScript.inventory.Count;

        //calculate the width and height of each child item.
		float width = 500;//containerRectTransform.rect.width;// / columnCount;
        float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;
        int rowCount = itemCount / columnCount;
		if (itemCount > 0)
		{
        if (itemCount % rowCount > 0)
            rowCount++;
		}
        //adjust the height of the container so that it will just barely fit all its children
        float scrollHeight = height * rowCount;
        containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
        containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
		
        /*int j = 0;
        for (int i = 0; i < itemCount; i++)
        {
            //this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
            if (i % columnCount == 0)
                j++;

            //create a new item, name it, and set the parent
            GameObject newItem = Instantiate(itemPrefab) as GameObject;
            newItem.name = gameObject.name + " item at (" + i + "," + j + ")";
            newItem.transform.SetParent(gameObject.transform, false);

            //move and size the new item
            RectTransform rectTransform = newItem.GetComponent<RectTransform>();

            float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
            float y = containerRectTransform.rect.height / 2 - height * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x + width;
            y = rectTransform.offsetMin.y + height;
            rectTransform.offsetMax = new Vector2(x, y);
        }*/
			Text NOInv = GameObject.Find ("NoInv").GetComponent<Text> ();
			int j = 0;
			if (attributesScript.inventory.Count == 0) {
				NOInv.text =  "No items in inventory";
			} else {
				foreach (InventoryItem item in attributesScript.inventory.ToList()) {
					 //create a new item, name it, and set the parent
					GameObject newItem = Instantiate(itemPrefab) as GameObject;
					newItem.name = gameObject.name + " item at (" + j + item.typeID + ")";
					newItem.transform.SetParent(gameObject.transform, false);
					RectTransform rectTransform = newItem.GetComponent<RectTransform>();

					float x = -containerRectTransform.rect.width / 2 + width;
					float y = containerRectTransform.rect.height / 2 - height * j;
					rectTransform.offsetMin = new Vector2(x, y);

					x = rectTransform.offsetMin.x + width;
					y = rectTransform.offsetMin.y + height;
					rectTransform.offsetMax = new Vector2(x, y);
					/*NOInv.text = "";
					Text W = GameObject.Find ("weap").GetComponent<Text> ();
					W.text = item.typeID;
					if(item.typeID != "Power Core"){
						if (GUI.Button (new Rect (width+101, top+151, buttonWidth, itemHeight), "Drop it")) {
							attributesScript.inventory.Remove (item);
							W.text = "";
							this.GetComponent<Sounds>().playWorldSound(Sounds.DROP_ITEM);
						}
					}

					if(item.typeID != "Medium Health Pack" && item.typeID != "Large Health Pack" && item.typeID != "Cupcake"){
						if(item.typeID != "Power Core"){
							if (GUI.Button (new Rect (width+201, top+151, buttonWidth, itemHeight), "Equip")) {
								attributesScript.equipItem (item);
								W.text = "";
								if(item.typeID == "Rare Accessory" || item.typeID == "Common Accessory" || item.typeID == "Uncommon Accessory"){
									this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_ACCESSORY);
								} else if(item.typeID == "Warhammer"){
									this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_HAMMER);
								} else if(item.typeID != "Warhammer"){
									this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_SWORD);
								}
							}
						}
					} else {
						if (GUI.Button (new Rect (width+201, top+151, buttonWidth, itemHeight), "Use")) {
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

	public void checkInventory()
	{
		GameObject[] gameObjectsToDelete = GameObject.FindGameObjectsWithTag ("WeaponList");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}
		float width = 500;//containerRectTransform.rect.width;// / columnCount;
        float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;
		Text NOInv = GameObject.Find ("NoInv").GetComponent<Text> ();
			int j = 0;
			if (attributesScript.inventory.Count == 0) {
				NOInv.text =  "No items in inventory";
			} else {
				foreach (InventoryItem item in attributesScript.inventory.ToList()) {
					 //create a new item, name it, and set the parent
					j++;
					GameObject newItem = Instantiate(itemPrefab) as GameObject;
					newItem.name = j +" " + item.typeID;
					newItem.transform.SetParent(gameObject.transform, false);
					
					newItem.GetComponent<PlaceInList>().myItem = item;

					Text W = newItem.GetComponentInChildren(typeof(Text)) as Text;
					W.text = item.typeID;
					RectTransform rectTransform = newItem.GetComponent<RectTransform>();

					float x = -containerRectTransform.rect.width /2;
					float y = containerRectTransform.rect.height / 2 - height * j-100;
					rectTransform.offsetMin = new Vector2(x, y);

					x = rectTransform.offsetMin.x + width;
					y = rectTransform.offsetMin.y + height-100;
					rectTransform.offsetMax = new Vector2(x, y);
					NOInv.text = "";
					/*iitem.typeID != "Power Core"){
						if (GUI.Button (new Rect (width+101, top+151, buttonWidth, itemHeight), "Drop it")) {
							attributesScript.inventory.Remove (item);
							W.text = "";
							this.GetComponent<Sounds>().playWorldSound(Sounds.DROP_ITEM);
						}
					}

					if(item.typeID != "Medium Health Pack" && item.typeID != "Large Health Pack" && item.typeID != "Cupcake"){
						if(item.typeID != "Power Core"){
							if (GUI.Button (new Rect (width+201, top+151, buttonWidth, itemHeight), "Equip")) {
								attributesScript.equipItem (item);
								W.text = "";
								if(item.typeID == "Rare Accessory" || item.typeID == "Common Accessory" || item.typeID == "Uncommon Accessory"){
									this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_ACCESSORY);
								} else if(item.typeID == "Warhammer"){
									this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_HAMMER);
								} else if(item.typeID != "Warhammer"){
									this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_SWORD);
								}
							}
						}
					} else {
						if (GUI.Button (new Rect (width+201, top+151, buttonWidth, itemHeight), "Use")) {
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
}
