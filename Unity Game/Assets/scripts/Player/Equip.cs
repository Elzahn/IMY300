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

	//private RectTransform rowRectTransform;
	private RectTransform containerRectTransform;
	private PlayerAttributes attributesScript;
	private float width;//, scrollHeight;
//	private Image itemDesc;

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
		/*	rowRectTransform = equipedPrefab.GetComponent<RectTransform>();
			height = rowRectTransform.rect.height;
			//adjust the height of the container so that it will just barely fit all its children
			scrollHeight = height * itemCount;
			
			//maak container groot genoeg om alle items te bevat
			containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
			containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);*/

			GameObject newItem = Instantiate (equipedPrefab) as GameObject;
			newItem.name = attributesScript.weapon.typeID;
			newItem.transform.SetParent (gameObject.transform, false);
			
			newItem.GetComponent<PlaceInList> ().myItem = attributesScript.weapon;
			
			Text weaponText = newItem.GetComponentInChildren<Text>();
			weaponText.text = attributesScript.weapon.typeID;
			
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
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
			
			float x = (-containerRectTransform.rect.width / 2) - 230;
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
		
		/*int l = 0;
		foreach (Accessory accessory in attributesScript.accessories) {

			l++;
			GameObject newItem = Instantiate (noItem) as GameObject;
			newItem.name = l + " No accessory equiped";
			newItem.GetComponentInChildren<Text>().text = "No accessory equiped";
			newItem.transform.SetParent (GameObject.Find ("EquipedScroll").transform, false);
			
			//newItem.GetComponent<PlaceInList> ().myItem = attributesScript.weapon;
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
			
			float k;
			if( l == 0){
				k = 1;
			} else {
				k = 1.6f;
			}
			
			float x = (-containerRectTransform.rect.width /2) - 70;
			float y = containerRectTransform.rect.height / 2 - height * k - 170;
			rectTransform.offsetMin = new Vector2(x, y);
			
			//Determines the heigh of the item
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height - 100;
			rectTransform.offsetMax = new Vector2(x, y);
		}
		
		for(int i = attributesScript.accessories.Count; i < attributesScript.maxAccessories; i++){
			GameObject newItem = Instantiate (noItem) as GameObject;
			newItem.name = i + " No accessory equiped";
			newItem.GetComponentInChildren<Text>().text = "No accessory equiped";
			newItem.transform.SetParent (GameObject.Find ("EquipedScroll").transform, false);
			
			//newItem.GetComponent<PlaceInList> ().myItem = attributesScript.weapon;
			
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
			
			float k;
			if( i == 0){
				k = 1;
			} else {
				k = 1.6f;
			}
			
			/*float x = (-containerRectTransform.rect.width /2) - 70;
			float y = containerRectTransform.rect.height / 2 - height * k - 170;
			rectTransform.offsetMin = new Vector2(x, y);
			
			//Determines the heigh of the item
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height - 100;
			rectTransform.offsetMax = new Vector2(x, y);*/
		//}
	}
}
