using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class LootScrollList : MonoBehaviour
{
	public GameObject weaponPrefab;
	public GameObject accessoryPrefab;
	public GameObject shipPiecePrefab;
	
	public Sprite butterKnife;
	public Sprite longSword;
	public Sprite warHammer;
	public Sprite commonAccessory;
	public Sprite uncommonAccessory;
	public Sprite rareAccessory;
	public Sprite shipPiece;
	
	private int itemCount;
	private RectTransform rowRectTransform;
	private RectTransform containerRectTransform;
	private float height, width, scrollHeight;
	private Image itemDesc;
	private PlayerAttributes playerAttributes;

	void Start()
	{
		playerAttributes = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();

		rowRectTransform = weaponPrefab.GetComponent<RectTransform>();
		containerRectTransform = gameObject.GetComponent<RectTransform>();
		
		height = rowRectTransform.rect.height;
		width = 670;
		
		itemCount = 3;	//set so that it has a starting value
		
		//adjust the height of the container so that it will just barely fit all its children
		scrollHeight = height * itemCount;
		
		//maak container groot genoeg om alle items te bevat
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
	}
	
	public void gatherLoot(string myName, LinkedList<InventoryItem> myLoot){
		showLoot (myLoot);
		GameObject.Find ("LootName").GetComponent<Text> ().text = myName;
	}
	
	public void showLoot(LinkedList<InventoryItem> myLoot)
	{
		itemCount = myLoot.Count;	//shows all elements now
		
		//adjust the height of the container so that it will just barely fit all its children
		scrollHeight = height * itemCount;
		
		//maak container groot genoeg om alle items te bevat
		containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
		containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
		
		GameObject[] gameObjectsToDelete = GameObject.FindGameObjectsWithTag ("LootList");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}
		
		int j = 0;

		foreach (InventoryItem item in myLoot.ToList()) {
			//create a new item, name it, and set the parent
			j++;
			GameObject newItem = null;
			RectTransform rectTransform = null;
			float x, y;
				
			if(item.type == 0){	//Accessories
				newItem = Instantiate(accessoryPrefab) as GameObject;
				newItem.name = j + " " + item.typeID;
				newItem.transform.SetParent(gameObject.transform, false);
				newItem.transform.localScale = new Vector3(1.3f, 0.35f, 0.4035253f);

				newItem.GetComponent<PlaceInList>().myItem = item;
					
				Text accessoryText = newItem.GetComponentInChildren<Text>();
				accessoryText.text = item.typeID;
					
				//To get the weapon image
				Image[] images = newItem.GetComponentsInChildren<Image>();

				foreach(Image image in images){
					if(image.name == "AccessoryImage"){
						if(accessoryText.text == "Common Accessory"){
							image.sprite = commonAccessory;
							newItem.GetComponent<PlaceInList>().itemImage = commonAccessory;	//sets image for description
						} else if(accessoryText.text == "Uncommon Accessory"){
							image.sprite = uncommonAccessory;
							newItem.GetComponent<PlaceInList>().itemImage = uncommonAccessory;	//sets image for description
						} else if(accessoryText.text == "Rare Accessory"){
							image.sprite = rareAccessory;
							newItem.GetComponent<PlaceInList>().itemImage = rareAccessory;	//sets image for description
						}
					} else if(image.name == "ItemDescBackground"){
						//sets all variables for the description of the item
						newItem.GetComponent<PlaceInList>().desc = image;
						newItem.GetComponent<PlaceInList>().itemName = accessoryText;
						foreach (Transform child in image.transform) {
							if(child.gameObject.name != "MouseHover")
								child.gameObject.SetActive (false);
						}
						image.enabled = false;
					} else if(image.gameObject.name == "Take" && playerAttributes.inventoryFull()){
						image.GetComponent<Button>().interactable = false;
						image.GetComponent<Image>().enabled = false;
						image.transform.FindChild("Text").GetComponent<Text>().enabled = false;
					} 
				}

				rectTransform = newItem.GetComponent<RectTransform>();

				x = (-containerRectTransform.rect.width /2) - 20;
				y = containerRectTransform.rect.height / 2 - height * j + 50;
				rectTransform.offsetMin = new Vector2(x, y);

				//Determines the heigh of the item
				x = rectTransform.offsetMin.x + width;
				y = rectTransform.offsetMin.y + height - 100;
				rectTransform.offsetMax = new Vector2(x, y);

			} else if(item.type == 1){	//Weapons
				newItem = Instantiate(weaponPrefab) as GameObject;
				newItem.name = j + " " + item.typeID;
				newItem.transform.SetParent(gameObject.transform, false);
				newItem.transform.localScale = new Vector3(1.3f, 0.35f, 0.4035253f);

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
					} else if(image.gameObject.name == "Take" && playerAttributes.inventoryFull()){
						image.GetComponent<Button>().interactable = false;
						image.GetComponent<Image>().enabled = false;
						image.transform.FindChild("Text").GetComponent<Text>().enabled = false;
					} 
				}
			} else if(item.type == 3){	//Ship piece
				newItem = Instantiate(shipPiecePrefab) as GameObject;
				newItem.name = j + " " + item.typeID;
				newItem.transform.SetParent(gameObject.transform, false);
				newItem.transform.localScale = new Vector3(1.3f, 0.35f, 0.4035253f);

				newItem.GetComponent<PlaceInList>().myItem = item;

				Text shipText = newItem.GetComponentInChildren<Text>();
				shipText.text = item.typeID;
					
				//To get the weapon image
				Image[] images = newItem.GetComponentsInChildren<Image>();

					foreach(Image image in images){
					if(image.name == "ItemImage"){
						if(shipText.text == "Power Core"){
							image.sprite = shipPiece;
							newItem.GetComponent<PlaceInList>().itemImage = shipPiece;	//sets image for description
						} 
					} else if(image.name == "ItemDescBackground"){
						//sets all variables for the description of the item
						newItem.GetComponent<PlaceInList>().desc = image;
						newItem.GetComponent<PlaceInList>().itemName = shipText;
						foreach (Transform child in image.transform) {
							if(child.gameObject.name != "MouseHover")
								child.gameObject.SetActive (false);
						}
						image.enabled = false;
					} else if(image.gameObject.name == "Take" && playerAttributes.inventoryFull()){
						image.GetComponent<Button>().interactable = false;
						image.GetComponent<Image>().enabled = false;
						image.transform.FindChild("Text").GetComponent<Text>().enabled = false;
					} 
				}
			}

			rectTransform = newItem.GetComponent<RectTransform>();

			x = (-containerRectTransform.rect.width /2) - 20;
			y = containerRectTransform.rect.height / 2 - height * j + 50;
			rectTransform.offsetMin = new Vector2(x, y);

			//Determines the heigh of the item
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height - 100;
			rectTransform.offsetMax = new Vector2(x, y);
		}

		Canvas.ForceUpdateCanvases();
		Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
		scrollbar.value = 1f;
	}
}
