using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class InventoryGUI : MonoBehaviour {

	public static bool showInventory{ get; private set; }
	public static bool showStorage{ get; private set; }
	private PlayerController playerScript;
	public Texture2D icon;
	
	public static bool hasCollided {get; set;}
	public static bool HUDshows {get; set;}

	private HUD Hud;
	private Canvas Inventory;
	private Canvas Storage;

	// Use this for initialization
	void Start () {
		hasCollided = false;
		HUDshows = false;
		Hud = Camera.main.GetComponent<HUD> ();
		showInventory = false;
		showStorage = false;
		Inventory = GameObject.Find ("Inventory").GetComponent<Canvas> ();
		Inventory.enabled = false;
		Storage = GameObject.Find ("Storage").GetComponent<Canvas> ();
		Storage.enabled = false;
		playerScript = this.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!playerScript.paused) {

			if(hasCollided && this.GetComponent<Tutorial>().teachStorage){
				Hud.makeInteractionHint("Press E to open storage", GameObject.Find("Player").GetComponent<SaveSpotTeleport>().pressE);
			}

			if(hasCollided && Input.GetKeyDown(KeyCode.E)){
				openStorage();
			}

			if (Input.GetKeyDown (KeyCode.I) && !showStorage) {
				openInventory ();
			}

			if(Application.loadedLevelName == "Tutorial" || Application.loadedLevelName == "SaveSpot"){
				if (Input.GetKeyDown (KeyCode.Escape) && !this.GetComponent<Tutorial>().startTutorial){
					playerScript.showQuit = true;
				}
			} else {
				if (Input.GetKeyDown (KeyCode.Escape) && !this.GetComponent<Tutorial>().startTutorial && !this.GetComponent<LoadingScreen>().loading){
					playerScript.showQuit = true;
				}
			}

		} else {
			if (Input.GetKeyDown (KeyCode.Escape) && !this.GetComponent<Tutorial>().startTutorial && !this.GetComponent<LoadingScreen>().loading){
				playerScript.showQuit = false;
				playerScript.paused = false;
			}

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
		HUDshows = false;
		Storage.enabled = false;
		showStorage = false;
		this.GetComponent<Sounds>().playWorldSound (Sounds.STORAGE);
		playerScript.paused = false;	//Resume game
	}

	public void openStorage(){
		if(this.GetComponent<Tutorial>().teachStorage){
			showStorage = true;
			Storage.enabled = true;
			GameObject.Find("StorageInventoryWeaponScroll").GetComponent<StorageList>().setUpStorage();
			HUDshows = true;
			this.GetComponent<Sounds> ().playWorldSound (Sounds.STORAGE);
			playerScript.paused = true;	//Pause game
			GameObject.Find("Player").GetComponent<Sounds>().resumeSound("computer");
		}
	}

	public void openInventory(){
		GameObject planet = GameObject.Find ("Planet");
		if (this.GetComponent<Tutorial> ().teachInventory) {
			GameObject.Find("Hint").GetComponent<Image>().enabled = false;
			GameObject.Find("Hint_Image").GetComponent<Image>().enabled = false;
			GameObject.Find("Hint_Text").GetComponent<Text>().enabled = false;
			GameObject.Find("Interaction").GetComponent<Image>().enabled = false;
			GameObject.Find("Interaction_Image").GetComponent<Image>().enabled = false;
			GameObject.Find("Interaction_Text").GetComponent<Text>().enabled = false;
			if((Application.loadedLevelName == "Scene" && !planet.GetComponent<LoadingScreen>().loading) || Application.loadedLevelName != "Scene"){
				Inventory.enabled = true;
				showInventory = true;
				HUDshows = true;
				GameObject.Find("WeaponScroll").GetComponent<ScrollableList>().setUpInventory();
				this.GetComponent<Sounds> ().playWorldSound (Sounds.INVENTORY);
				playerScript.paused = true;	//Pause game
				GameObject.Find("Player").GetComponent<Sounds>().resumeSound("computer");
			}
		}
	}

	public void closeInventory(){
		GameObject.Find("Hint").GetComponent<Image>().enabled = true;
		GameObject.Find("Hint_Image").GetComponent<Image>().enabled = true;
		GameObject.Find("Hint_Text").GetComponent<Text>().enabled = true;
		GameObject.Find("Hint_Image").GetComponent<Image>().enabled = true;
		GameObject.Find("Interaction").GetComponent<Image>().enabled = true;
		GameObject.Find("Interaction_Text").GetComponent<Text>().enabled = true;
		HUDshows = false;
		showInventory = false;
		Inventory.enabled = false;
		this.GetComponent<Sounds>().playWorldSound (Sounds.INVENTORY);
		playerScript.paused = false;	//Resume game
	}
}
