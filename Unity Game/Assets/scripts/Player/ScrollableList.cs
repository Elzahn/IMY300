using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ScrollableList : MonoBehaviour
{
	public GameObject itemPrefab;
	private int itemCount;
	
	private RectTransform rowRectTransform;
	private RectTransform containerRectTransform;
	private PlayerAttributes attributesScript;
	
	float height, width, scrollHeight;
	
	void Start()
	{
		rowRectTransform = itemPrefab.GetComponent<RectTransform>();
		containerRectTransform = gameObject.GetComponent<RectTransform>();
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		
		height = rowRectTransform.rect.height;
		width = 500;

		itemCount = 3;	//set so that it has a starting value

		//adjust the height of the container so that it will just barely fit all its children
		scrollHeight = height * itemCount;
		
		//maak container groot genoeg om alle items te bevat
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
	}
	
	public void checkInventory()
	{
		itemCount = attributesScript.inventory.Count;	//shows all elements now

		//adjust the height of the container so that it will just barely fit all its children
		scrollHeight = height * itemCount;
		
		//maak container groot genoeg om alle items te bevat
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
		GameObject[] gameObjectsToDelete = GameObject.FindGameObjectsWithTag ("WeaponList");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}
		
		Text noItems = GameObject.Find ("NoItems").GetComponent<Text> ();
		int j = 0;
		
		if (attributesScript.inventory.Count == 0) {
			noItems.text =  "No items in inventory";
		} else {
			foreach (InventoryItem item in attributesScript.inventory.ToList()) {
				//create a new item, name it, and set the parent
				j++;
				GameObject newItem = Instantiate(itemPrefab) as GameObject;
				newItem.name = j +" " + item.typeID;
				newItem.transform.SetParent(gameObject.transform, false);
				
				newItem.GetComponent<PlaceInList>().myItem = item;
				
				Text Weapon = newItem.GetComponentInChildren(typeof(Text)) as Text;
				Weapon.text = item.typeID;
				RectTransform rectTransform = newItem.GetComponent<RectTransform>();
				
				float x = -containerRectTransform.rect.width /2;
				float y = containerRectTransform.rect.height / 2 - height * j + 20;
				rectTransform.offsetMin = new Vector2(x, y);
				
				//Determines the heigh of the item
				x = rectTransform.offsetMin.x + width;
				y = rectTransform.offsetMin.y + height - 100;
				rectTransform.offsetMax = new Vector2(x, y);
			}
			noItems.text = "";
		}
	}
}
