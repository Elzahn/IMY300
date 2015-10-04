using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Equip : MonoBehaviour
{
	public GameObject equipedPrefab;
	public GameObject noItem;
	
	public Sprite butterKnife;
	public Sprite longSword;
	public Sprite warHammer;
	public Sprite commonAccessory;
	public Sprite uncommonAccessory;
	public Sprite rareAccessory;

	private RectTransform containerRectTransform;
	private PlayerAttributes attributesScript;
	private float width;

	void Start()
	{
		containerRectTransform = gameObject.GetComponent<RectTransform>();
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();

		width = 500;
	}
	
	public void makeEquipmentList()
	{		

		GameObject[] gameObjectsToDelete = GameObject.FindGameObjectsWithTag ("Equiped");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}

		//Add equiped items
		if (attributesScript.weapon != null) {
			GameObject newItem = Instantiate (equipedPrefab) as GameObject;
			newItem.name = attributesScript.weapon.typeID;
			newItem.transform.SetParent (gameObject.transform, false);
			
			newItem.GetComponent<PlaceInList> ().myItem = attributesScript.weapon;
			
			Text weaponText = newItem.GetComponentInChildren<Text>();
			weaponText.text = attributesScript.weapon.typeID;
			
			//To get the weapon image
			Image[] images = newItem.GetComponentsInChildren<Image>();
			
			foreach(Image image in images){
				if(image.name == "ItemImage"){
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
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
			
			float x = (-containerRectTransform.rect.width / 2) - 240;
			float y = containerRectTransform.rect.height / 2;
			rectTransform.offsetMin = new Vector2 (x, y);
			
			//Determines the heigh of the item
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + 320;
			rectTransform.offsetMax = new Vector2 (x, y);
		} else {
			GameObject newItem = Instantiate (noItem) as GameObject;
			newItem.name = "No weapon equiped";
			newItem.GetComponentInChildren<Text>().text = "No weapon equiped";
			newItem.transform.SetParent (gameObject.transform, false);
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
			
			float x = (-containerRectTransform.rect.width / 2) - 340;
			float y = containerRectTransform.rect.height / 2;
			rectTransform.offsetMin = new Vector2 (x, y);
			
			//Determines the heigh of the item
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + 160;
			rectTransform.offsetMax = new Vector2 (x, y);
		}

		for(int i = 0; i < attributesScript.maxAccessories; i++){
			GameObject newItem;

			if(i < attributesScript.accessories.Count){
				Accessory tempAccessory = attributesScript.accessories.ElementAt(i);

				newItem = Instantiate (equipedPrefab) as GameObject;
				newItem.name = tempAccessory.typeID;
				newItem.transform.SetParent (gameObject.transform, false);
				
				newItem.GetComponent<PlaceInList> ().myItem = tempAccessory;
				
				Text accessoryText = newItem.GetComponentInChildren<Text>();
				accessoryText.text = tempAccessory.typeID;
				
				//To get the accessory image
				Image[] images = newItem.GetComponentsInChildren<Image>();
				
				foreach(Image image in images){
					if(image.name == "ItemImage"){
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
					}
				}

				RectTransform rectTransform = newItem.GetComponent<RectTransform> ();

				int height = i * 100;
				float x = (-containerRectTransform.rect.width /2) - 240;
				float y = containerRectTransform.rect.height / 2 - height * i ;
				rectTransform.offsetMin = new Vector2(x, y);
				
				//Determines the heigh of the item
				x = rectTransform.offsetMin.x + width;
				y = rectTransform.offsetMin.y - height;
				rectTransform.offsetMax = new Vector2(x, y);
			} else {
				newItem = Instantiate (noItem) as GameObject;
				newItem.name = i + " No accessory equiped";
				newItem.GetComponentInChildren<Text>().text = "No accessory equiped";
				newItem.transform.SetParent (gameObject.transform, false);

				RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
				
				int height  = 120;
				float x = (-containerRectTransform.rect.width /2) - 320;
				float y = containerRectTransform.rect.height / 2 - height * i ;
				rectTransform.offsetMin = new Vector2(x, y);
				
				//Determines the heigh of the item
				x = rectTransform.offsetMin.x + width;
				y = rectTransform.offsetMin.y - height;
				rectTransform.offsetMax = new Vector2(x, y);
			}
		}
	}
}
