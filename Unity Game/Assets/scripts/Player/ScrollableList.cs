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
	private float height, width, scrollHeight;
	private Text inventory, xp, hp, stamina, level, noItems, noAccessories, noWeapon;

	void Start()
	{

		inventory = GameObject.Find ("InventoryText").GetComponent<Text> ();
		xp = GameObject.Find ("XPStat").GetComponent<Text> ();
		hp = GameObject.Find ("HPStat").GetComponent<Text> ();
		stamina = GameObject.Find ("StaminaStat").GetComponent<Text> ();
		level = GameObject.Find ("LevelStat").GetComponent<Text> ();
		noItems = GameObject.Find ("NoItems").GetComponent<Text> ();
		noAccessories = GameObject.Find ("NoAccessories").GetComponent<Text> ();
		noWeapon = GameObject.Find ("NoWeapon").GetComponent<Text> ();

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
		showInventoryInfo ();
		
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
				newItem.transform.localScale = new Vector3(1f, 0.4f, 0.4035253f);

				newItem.GetComponent<PlaceInList>().myItem = item;
				
				Text Weapon = newItem.GetComponentInChildren(typeof(Text)) as Text;
				Weapon.text = item.typeID;
				RectTransform rectTransform = newItem.GetComponent<RectTransform>();
				
				float x = (-containerRectTransform.rect.width /2) - 20;
				float y = containerRectTransform.rect.height / 2 - height * j + 50;
				rectTransform.offsetMin = new Vector2(x, y);
				
				//Determines the heigh of the item
				x = rectTransform.offsetMin.x + width;
				y = rectTransform.offsetMin.y + height - 100;
				rectTransform.offsetMax = new Vector2(x, y);

				Canvas.ForceUpdateCanvases();
				Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
				scrollbar.value = 0f;
			}
			noItems.text = "";
		}
	}

	public void showInventoryInfo(){

		inventory.text = "Inventory \t" + attributesScript.inventory.Count + "/" + attributesScript.inventorySize;
		xp.text = "Xp: " + attributesScript.xp + "/" + attributesScript.getExpectedXP ();
		hp.text = "Hp: " + attributesScript.hp + "/" + attributesScript.maxHP();
		stamina.text = "Stamina: " + attributesScript.stamina + "/" + attributesScript.maxStamina();
		level.text = "Level: " + attributesScript.level;

		if (attributesScript.inventory.Count == 0) {
			noItems.text = "No items in inventory";
		}

		if (attributesScript.accessories.Count == 0) {
			noAccessories.text = "No accessories equiped";
		}
		if (attributesScript.weapon == null) {
			noWeapon.text = "No weapon equiped";
		}
	}
}
