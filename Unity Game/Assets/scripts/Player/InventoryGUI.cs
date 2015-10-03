using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class InventoryGUI : MonoBehaviour {

	public static bool showInventory{ get; private set; }
	public static bool showStorage{ get; private set; }
	private PlayerController playerScript;
	//private PlayerAttributes attributesScript;
	public Texture2D icon;
	
	public static bool hasCollided {get; set;}
	public static bool HUDshows {get; set;}

	private HUD Hud;
	private Canvas Inventory;
	
	public static bool getStorage(){
		return showStorage;
	}

	public static bool getInventory(){
		return showInventory;
	}

	// Use this for initialization
	void Start () {
		hasCollided = false;
		HUDshows = false;
		Hud = Camera.main.GetComponent<HUD> ();
		showInventory = false;
		showStorage = false;
		Inventory = GameObject.Find ("Inventory").GetComponent<Canvas> ();
		Inventory.enabled = false;
		playerScript = this.GetComponent<PlayerController> ();
	//	attributesScript = this.GetComponent<PlayerAttributes> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!playerScript.paused) {

			//Done for the extra mapping of the inventory and storage close to the escape key. Else it thinks escape was pressed twice for example it also opens quite menu.
			if(!Inventory.enabled && (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.I))){
				showInventory = false;
				showStorage = false;
			}

			if(hasCollided && this.GetComponent<Tutorial>().teachStorage){
				Hud.makeInteractionHint("Press E to open storage", GameObject.Find("Player").GetComponent<SaveSpotTeleport>().pressE);
			}

			if(hasCollided && Input.GetKeyDown(KeyCode.E)){
				openStorage();
			}

			if (Input.GetKeyDown (KeyCode.I) && !showStorage) {
				openInventory ();
			}
		} else {
			if (Input.GetKeyDown (KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)) {
				if (showStorage) {
					closeStorage ();
				}
			}

			if (Input.GetKeyDown (KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)) {
				if (showInventory) {
					closeInventory ();
				}
			}
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.name == "Storage") {
			hasCollided = true;
		}
	}
	
	void OnTriggerExit(Collider col){
		hasCollided = false;
	}

	public void closeStorage(){
		hasCollided = true;
		HUDshows = true;
		this.GetComponent<Sounds>().playWorldSound (Sounds.STORAGE);
		playerScript.paused = false;	//Resume game
	}

	public void openStorage(){
		if(this.GetComponent<Tutorial>().teachStorage){
			showStorage = true;
			HUDshows = true;
			hasCollided = false;
			this.GetComponent<Sounds> ().playWorldSound (Sounds.STORAGE);
			playerScript.paused = true;	//Pause game
			GameObject.Find("Player").GetComponent<Sounds>().resumeSound("computer");
		}
	}

	public void openInventory(){
		GameObject planet = GameObject.Find ("Planet");
		if (this.GetComponent<Tutorial> ().teachInventory) {
			if((Application.loadedLevelName == "Scene" && !planet.GetComponent<LoadingScreen>().loading) || Application.loadedLevelName != "Scene"){
				hasCollided = false;
				Inventory.enabled = true;
				HUDshows = true;
				GameObject.Find("WeaponScroll").GetComponent<ScrollableList>().setUpInventory();
				showInventory = true;
				this.GetComponent<Sounds> ().playWorldSound (Sounds.INVENTORY);
				playerScript.paused = true;	//Pause game
				GameObject.Find("Player").GetComponent<Sounds>().resumeSound("computer");
			}
		}
	}

	public void closeInventory(){
		HUDshows = false;
		Inventory.enabled = false;
		this.GetComponent<Sounds>().playWorldSound (Sounds.INVENTORY);
		playerScript.paused = false;	//Resume game
	}

	/*void OnGUI(){
		if (showInventory) {
			int boxWidth = Screen.width;//800;
			int boxHeight = Screen.height;//800;
			int width = boxWidth/2;
			int left = Screen.width/2 - boxWidth/2;//40;
			int top = Screen.height/2 - boxHeight/2;//10;
			int secondLeft = left + width;//Screen.width/2 - left;// - boxWidth/3;
			int secondTop = top;
			int buttonWidth = 60;
			int itemHeight = 30;

			//Inventory

			 else {
				foreach (InventoryItem item in attributesScript.inventory.ToList()) {

					if(item.typeID != "Power Core"){

					}

					if(item.typeID != "Medium Health Pack" && item.typeID != "Large Health Pack" && item.typeID != "Cupcake"){
						if(item.typeID != "Power Core"){

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
					}
					top += itemHeight;
				}
			}

			//show character attributes

			secondTop += 100;

			if (attributesScript.accessories.Count != 0) {
				foreach (Accessory item in attributesScript.accessories.ToList()) {
					GUI.Label (new Rect (secondLeft + 30, secondTop + 80, width-buttonWidth, itemHeight), item.typeID);
					if (GUI.Button (new Rect (secondLeft + width - buttonWidth, secondTop + 80, buttonWidth, itemHeight), "Unequip")) {
						attributesScript.unequipAccessory (item);
						noItems.text = "";
						this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_ACCESSORY);
					}
					secondTop += itemHeight;
				}
			} else {

				noAccessories.text = "No accessories equiped";
				secondTop += itemHeight;
			}

			if (attributesScript.weapon == null) {

				noWeapon.text = "No weapon equiped";
				secondTop += itemHeight;
			} else {
				Text weapon = GameObject.Find ("NoWeapon").GetComponent<Text> ();
				weapon.text =  attributesScript.weapon.typeID;
				if (GUI.Button (new Rect (width+601, top+171, buttonWidth, itemHeight), "Unequip")) {
					if(attributesScript.weapon.typeID != "Warhammer"){
						this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_SWORD);
					} else {	
						this.GetComponent<Sounds>().playWorldSound(Sounds.EQUIP_HAMMER);
					}
					attributesScript.unequipWeapon ();
				}
			}
		} else if (showStorage) {
			int boxWidth = Screen.width;//800;
			int boxHeight = Screen.height;//800;
			int width = boxWidth/2;
			int left = Screen.width/2 - boxWidth/2;//40;
			int top = Screen.height/2 - boxHeight/2;//10;
			int secondLeft = left + width;//Screen.width/2 - left;// - boxWidth/3;
			int secondTop = top;
			int buttonWidth = 120;
			int itemHeight = 30;
			
			GUI.Box (new Rect (left, top, boxWidth, boxHeight), "Storage/Inventory Space (Press E to close)");
			GUI.Box (new Rect (left, top + 40, width, boxHeight), "Storage \t" + attributesScript.storage.Count + "/" + attributesScript.MAX_STORAGE);
			
			if (attributesScript.storage.Count == 0) {
				GUI.Label (new Rect (left + width/4, top + 100, width, itemHeight), "No items in storage");
			} else {
				foreach (InventoryItem item in attributesScript.storage.ToList()) {
					GUI.Label (new Rect (left + 30, top + 80, width-buttonWidth*2, itemHeight), item.typeID);
					if (GUI.Button (new Rect (left + width-buttonWidth*2, top + 80, buttonWidth, itemHeight), "Drop it")) {
						attributesScript.storage.Remove (item);
						this.GetComponent<Sounds>().playWorldSound(Sounds.DROP_ITEM);
					}
					if (GUI.Button (new Rect (left + width-buttonWidth, top + 80, buttonWidth, itemHeight), "Move to Inventory")) {
						attributesScript.addToInventory(item);
						attributesScript.storage.Remove (item);
						this.GetComponent<Sounds>().playWorldSound(Sounds.MOVE_ITEM);
					}
					top += itemHeight;
				}
			}

			GUI.Box (new Rect (secondLeft, secondTop + 40, width, boxHeight), "Inventory \t" + attributesScript.inventory.Count + "/" + attributesScript.inventorySize);
			
			if (attributesScript.inventory.Count == 0) {
				GUI.Label (new Rect (secondLeft + width/4, secondTop + 100, width-buttonWidth, itemHeight), "No items in inventory");
			} else {
				foreach (InventoryItem item in attributesScript.inventory.ToList()) {
					GUI.Label (new Rect (secondLeft + 30, secondTop + 80, width-buttonWidth, itemHeight), item.typeID);
					if(item.typeID != "Power Core"){
						if (GUI.Button (new Rect (secondLeft + width-buttonWidth*2, secondTop + 80, buttonWidth, itemHeight), "Drop it")) {
							attributesScript.inventory.Remove (item);
							this.GetComponent<Sounds>().playWorldSound(Sounds.DROP_ITEM);
						}
						if (GUI.Button (new Rect (secondLeft + width-buttonWidth, secondTop + 80, buttonWidth, itemHeight), "Move To Storage")) {
							attributesScript.addToStorage (item);
							attributesScript.inventory.Remove (item);
							this.GetComponent<Sounds>().playWorldSound(Sounds.MOVE_ITEM);
						}
					}
					secondTop += itemHeight;
				}
			}
		}
	}*/
}
