using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ScrollableList : MonoBehaviour
{
	public GameObject itemPrefab;
	public GameObject equipedPrefab;
	public GameObject noItem;

	public Sprite butterKnife;
	public Sprite longSword;
	public Sprite warHammer;

	private int itemCount;
	private RectTransform rowRectTransform;
	private RectTransform containerRectTransform;
	private PlayerAttributes attributesScript;
	private float height, width, scrollHeight;
	private Text inventory, xp, hp, stamina, level, noItems, noAccessories, weapon;
	private Image itemDesc;

	void Start()
	{
		inventory = GameObject.Find ("InventoryText").GetComponent<Text> ();
		xp = GameObject.Find ("XPStat").GetComponent<Text> ();
		hp = GameObject.Find ("HPStat").GetComponent<Text> ();
		stamina = GameObject.Find ("StaminaStat").GetComponent<Text> ();
		level = GameObject.Find ("LevelStat").GetComponent<Text> ();
		noItems = GameObject.Find ("NoItems").GetComponent<Text> ();
		//noAccessories = GameObject.Find ("NoAccessories").GetComponent<Text> ();
		//weapon = GameObject.Find ("NoWeapon").GetComponent<Text> ();

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

	public void setUpInventory(){
		checkInventory ();
		showInventoryInfo ();
		GameObject.Find ("EquipedScroll").GetComponent<Equip> ().makeEquipmentList ();
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
				newItem.transform.localScale = new Vector3(1f, 0.4f, 0.4035253f);

				newItem.GetComponent<PlaceInList>().myItem = item;
				
				Text weaponText = newItem.GetComponentInChildren<Text>();
				weaponText.text = item.typeID;

				//To get the weapon image
				Image[] images = newItem.GetComponentsInChildren<Image>();

				foreach(Image image in images){
					if(image.name == "WeaponImage"){
						if(weaponText.text == "Longsword"){
							image.sprite = longSword;
							newItem.GetComponent<PlaceInList>().itemImage = longSword;	//sets image for description
						} else if(weaponText.text == "Warhammer"){
							image.sprite = warHammer;
							newItem.GetComponent<PlaceInList>().itemImage = warHammer;	//sets image for description
						} else if(weaponText.text == "ButterKnife"){
							image.sprite = butterKnife;
							newItem.GetComponent<PlaceInList>().itemImage = butterKnife;	//sets image for description
						}
					} else if(image.name == "ItemDescBackground"){
						//sets all variables for the description of the item
						newItem.GetComponent<PlaceInList>().desc = image;
						newItem.GetComponent<PlaceInList>().itemName = weaponText;
						foreach (Transform child in image.transform) {
							if(child.gameObject.name != "MouseHover")
								child.gameObject.SetActive (false);
						}
						image.enabled = false;
					}
				}

				RectTransform rectTransform = newItem.GetComponent<RectTransform>();
				
				float x = (-containerRectTransform.rect.width /2) - 20;
				float y = containerRectTransform.rect.height / 2 - height * j + 50;
				rectTransform.offsetMin = new Vector2(x, y);
				
				//Determines the heigh of the item
				x = rectTransform.offsetMin.x + width;
				y = rectTransform.offsetMin.y + height - 100;
				rectTransform.offsetMax = new Vector2(x, y);
			}
			noItems.text = "";

			Canvas.ForceUpdateCanvases();
			Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
			scrollbar.value = 1f;
		}
		
		//makeEquipmentList ();
	}
	
	public void showInventoryInfo(){

		inventory.text = "Inventory \t" + attributesScript.inventory.Count + "/" + attributesScript.inventorySize;
		xp.text = "Xp: " + attributesScript.xp + "/" + attributesScript.getExpectedXP ();
		hp.text = "Hp: " + attributesScript.hp + "/" + attributesScript.maxHP();
		stamina.text = "Stamina: " + attributesScript.stamina + "/" + attributesScript.maxStamina();
		level.text = "Level: " + attributesScript.level;

		if (attributesScript.inventory.Count == 0) {
			noItems.text = "No items in inventory";
		}/* else {
			noItems.text = "";
		}*/

		/*if (attributesScript.accessories.Count == 0) {
			noAccessories.text = "No accessories equiped";
		}/* else {
			noAccessories.text = "";
		}*

		if (attributesScript.weapon == null) {
			weapon.text = "No weapon equiped";
		} else {
			weapon.text = "";
		}*/
	}

	public void makeEquipmentList(){
		//Add equiped items
		
		scrollHeight = height * 3;
		containerRectTransform = GameObject.Find ("EquipedScroll").GetComponent<RectTransform> ();
		containerRectTransform.offsetMin = new Vector2 (containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2 (containerRectTransform.offsetMax.x, scrollHeight / 2);
		
		if (attributesScript.weapon != null) {
			GameObject newItem = Instantiate (equipedPrefab) as GameObject;
			newItem.name = attributesScript.weapon.typeID;
			newItem.transform.SetParent (GameObject.Find ("EquipedScroll").transform, false);
			
			newItem.GetComponent<PlaceInList> ().myItem = attributesScript.weapon;
			
			Text weaponText = newItem.GetComponentInChildren<Text> ();
			weaponText.text = attributesScript.weapon.typeID;
			
			//To get the weapon image
			Image[] images = newItem.GetComponentsInChildren<Image> ();
			
			foreach (Image image in images) {
				if (image.name == "WeaponImage") {
					if (weaponText.text == "Longsword") {
						image.sprite = longSword;
						newItem.GetComponent<PlaceInList> ().itemImage = longSword;	//sets image for description
					} else if (weaponText.text == "Warhammer") {
						image.sprite = warHammer;
						newItem.GetComponent<PlaceInList> ().itemImage = warHammer;	//sets image for description
					} else if (weaponText.text == "ButterKnife") {
						image.sprite = butterKnife;
						newItem.GetComponent<PlaceInList> ().itemImage = butterKnife;	//sets image for description
					}
				} else if (image.name == "ItemDescBackground") {
					//sets all variables for the description of the item
					newItem.GetComponent<PlaceInList> ().desc = image;
					newItem.GetComponent<PlaceInList> ().itemName = weaponText;
					foreach (Transform child in image.transform) {
						if (child.gameObject.name != "MouseHover")
							child.gameObject.SetActive (false);
					}
					image.enabled = false;
				}
			}
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
			
			float x = (-containerRectTransform.rect.width / 2);
			float y = containerRectTransform.rect.height / 2 + 20;
			rectTransform.offsetMin = new Vector2 (x, y);
			
			//Determines the heigh of the item
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y - 150;
			rectTransform.offsetMax = new Vector2 (x, y);
		} else {
			GameObject newItem = Instantiate (noItem) as GameObject;
			newItem.name = "No weapon equiped";
			newItem.GetComponentInChildren<Text> ().text = "No weapon equiped";
			newItem.transform.SetParent (GameObject.Find ("EquipedScroll").transform, false);
			
			//newItem.GetComponent<PlaceInList> ().myItem = attributesScript.weapon;
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
			
			float x = (-containerRectTransform.rect.width / 2) - 90;
			float y = containerRectTransform.rect.height / 2 - 110;
			rectTransform.offsetMin = new Vector2 (x, y);
			
			//Determines the heigh of the item
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y - 200;
			rectTransform.offsetMax = new Vector2 (x, y);
		}
		
		int l = 0;
		foreach (Accessory accessory in attributesScript.accessories) {
			l++;
			GameObject newItem = Instantiate (noItem) as GameObject;
			newItem.name = l + " No accessory equiped";
			newItem.GetComponentInChildren<Text> ().text = "No accessory equiped";
			newItem.transform.SetParent (GameObject.Find ("EquipedScroll").transform, false);
			
			//newItem.GetComponent<PlaceInList> ().myItem = attributesScript.weapon;
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
			
			float k;
			if (l == 0) {
				k = 1;
			} else {
				k = 1.6f;
			}
			
			float x = (-containerRectTransform.rect.width / 2) - 70;
			float y = containerRectTransform.rect.height / 2 - height * k - 170;
			rectTransform.offsetMin = new Vector2 (x, y);
			
			//Determines the heigh of the item
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height - 100;
			rectTransform.offsetMax = new Vector2 (x, y);
		}
		
		for (int i = attributesScript.accessories.Count; i < attributesScript.maxAccessories; i++) {
			GameObject newItem = Instantiate (noItem) as GameObject;
			newItem.name = i + " No accessory equiped";
			newItem.GetComponentInChildren<Text> ().text = "No accessory equiped";
			newItem.transform.SetParent (GameObject.Find ("EquipedScroll").transform, false);
			
			//newItem.GetComponent<PlaceInList> ().myItem = attributesScript.weapon;
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
			
			float k;
			if (i == 0) {
				k = 1;
			} else {
				k = 1.6f;
			}
			
			float x = (-containerRectTransform.rect.width / 2) - 70;
			float y = containerRectTransform.rect.height / 2 - height * k - 170;
			rectTransform.offsetMin = new Vector2 (x, y);
			
			//Determines the heigh of the item
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height - 100;
			rectTransform.offsetMax = new Vector2 (x, y);
		}
	}
}
