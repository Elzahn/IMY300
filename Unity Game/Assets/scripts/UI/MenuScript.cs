using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class MenuScript : MonoBehaviour {

	public Font alienFont;
	public Font defaultFont;

	private Canvas loadCanvas, settingsCanvas, saveCanvas;
	private PlayerAttributes attributesScript;
	private GameObject player;
	private static GameObject errorPopup;
	private float errorTime = 0f, checkTime = 3f;

	void Start(){
		player = GameObject.Find ("Player");
		attributesScript = player.GetComponent<PlayerAttributes> ();
		loadCanvas = GameObject.Find ("LoadCanvas").GetComponent<Canvas> ();
		saveCanvas = GameObject.Find ("SaveCanvas").GetComponent<Canvas> ();
		settingsCanvas = GameObject.Find ("SettingsCanvas").GetComponent<Canvas> ();
		if(errorPopup == null)
			errorPopup = GameObject.Find ("ErrorPopup");
		errorPopup.SetActive (false);
	}

	public void changeFont(){
		if (this.GetComponent<Button> ().interactable == true) {
			Text textMesh = this.GetComponentInChildren<Text> ();

			if (textMesh.font == defaultFont) {
				textMesh.font = alienFont;
				Color textColor;
				Color.TryParseHexString ("#95E0FFFF", out textColor);
				textMesh.color = textColor;

			} else {
				textMesh.font = defaultFont;
				textMesh.color = Color.white;
			}
		}
	}

	public void play(){
		Application.LoadLevel ("SaveSpot");
		
		player.GetComponent<Tutorial>().startTutorial = true;
		player.GetComponent<SaveSpotTeleport> ().canEnterSaveSpot = true;
		GameObject.Find ("HUD").GetComponent<Canvas> ().enabled = true;
		player.transform.rotation = Quaternion.Euler (351.66f, 179.447f, 358.8f);
		player.transform.up = Vector3.up;
		player.transform.position = new Vector3 (10.88f, 79.831f, -11.14f);
	}
	
	public void load(){
		chooseCamera ();

		if (Application.loadedLevelName == "Main_Menu") {
			Text textMesh = this.GetComponentInChildren<Text> ();
			textMesh.font = alienFont;
			Color textColor;
			Color.TryParseHexString ("#95E0FFFF", out textColor);
			textMesh.color = textColor;
		}

		disableCanvas ();
		loadCanvas.enabled = true;

		GameObject.Find ("LoadCanvas").transform.FindChild("Panel").FindChild("Autosave").FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (0);
		GameObject.Find ("LoadCanvas").transform.FindChild("Panel").FindChild("Slot1").FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (1);
		GameObject.Find ("LoadCanvas").transform.FindChild("Panel").FindChild("Slot2").FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (2);
		GameObject.Find ("LoadCanvas").transform.FindChild("Panel").FindChild("Slot3").FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (3);
	}

	private void chooseCamera(){
		if (Application.loadedLevelName != "Main_Menu") {
			GameObject.Find ("LoadCanvas").GetComponent<Canvas> ().worldCamera = Camera.main;
			GameObject.Find ("LoadCanvas").GetComponent<Canvas> ().planeDistance = 0.2f;
			GameObject.Find ("SettingsCanvas").GetComponent<Canvas> ().worldCamera = Camera.main;
			GameObject.Find ("SettingsCanvas").GetComponent<Canvas> ().planeDistance = 0.2f;
			GameObject.Find ("SaveCanvas").GetComponent<Canvas> ().worldCamera = Camera.main;
			GameObject.Find ("SaveCanvas").GetComponent<Canvas> ().planeDistance = 0.2f;
		} else if (Application.loadedLevelName == "Main_Menu") {
			foreach(Camera c in Camera.allCameras){
				if(c.name == "Main Menu Camera"){
					GameObject.Find ("LoadCanvas").GetComponent<Canvas> ().worldCamera = c;
					GameObject.Find ("LoadCanvas").GetComponent<Canvas> ().planeDistance = 1f;
					GameObject.Find ("SettingsCanvas").GetComponent<Canvas> ().worldCamera = c;
					GameObject.Find ("SettingsCanvas").GetComponent<Canvas> ().planeDistance = 1f;
				}
			}
		}
	}

	public void settings(){
		chooseCamera ();

		if (Application.loadedLevelName == "Main_Menu") {
			Text textMesh = this.GetComponentInChildren<Text> ();
			textMesh.font = alienFont;
			Color textColor;
			Color.TryParseHexString ("#95E0FFFF", out textColor);
			textMesh.color = textColor;
		}

		disableCanvas ();

		GameObject.Find ("Slider Narrative").GetComponent<Slider> ().value = attributesScript.narrativeShown;
		GameObject.Find ("Slider Sound").GetComponent<Slider> ().value = attributesScript.soundVolume;
		GameObject.Find ("Slider Difficult").GetComponent<Slider> ().value = attributesScript.difficulty;

		settingsCanvas.enabled = true;
	}

	public void quit(){
		Application.Quit ();
	}

	public void closeDialog(){
		loadCanvas.enabled = false;
		settingsCanvas.enabled = false;
		saveCanvas.enabled = false;
		enableCanvas ();
	}

	public void loadGame(int slot){
		bool error = false;

		try{
			attributesScript.load (slot);
		} catch (Exception){
			error = true;
			errorPopup.SetActive (true);
			errorTime = Time.time;
		}

		if(!error){
			player.transform.rotation = Quaternion.Euler (351.66f, 179.447f, 358.8f);
			player.transform.up = Vector3.up;
			player.transform.position = new Vector3 (10.88f, 79.831f, -11.14f);
			GameObject.Find ("HUD").GetComponent<Canvas> ().enabled = true;
			Application.LoadLevel("SaveSpot");
			ResumeGame ();
			loadCanvas.enabled = false;
			player.GetComponent<SaveSpotTeleport> ().canEnterSaveSpot = true;
			player.GetComponent<Tutorial>().stopTutorial();
			GameObject.Find("Stamina").GetComponent<Image>().enabled = true;
			GameObject.Find("Health").GetComponent<Image>().enabled = true;
			player.GetComponent<Tutorial>().teachInventory = true;
			GameObject.Find("HUD_Expand_Text").GetComponent<Text>().text = attributesScript.narrativeSoFar;
			GameObject.Find("Interaction").GetComponent<Image>().fillAmount = 0;
			GameObject.Find("MenuMask").GetComponent<Image>().fillAmount = 0;
			enableCanvas ();
		}
	}

	void Update(){
		if (Time.time >= errorTime + checkTime && errorTime != 0f) {
			errorPopup.SetActive (false);
			errorTime = 0f;
		}

		if (Application.loadedLevelName == "Tutorial" || GameObject.Find ("Player").GetComponent<Tutorial> ().startTutorial) {
			GameObject.Find("MainMenu").transform.FindChild("MenuMask").FindChild("Save").GetComponent<Button>().interactable = false;
			GameObject.Find("MainMenu").transform.FindChild("MenuMask").FindChild("Save").FindChild("Hover").GetComponent<Button>().interactable = false;
		} else {
			GameObject.Find("MainMenu").transform.FindChild("MenuMask").FindChild("Save").GetComponent<Button>().interactable = true;
			GameObject.Find("MainMenu").transform.FindChild("MenuMask").FindChild("Save").FindChild("Hover").GetComponent<Button>().interactable = true;
		}
	}

	public void hovering(){
		if (this.GetComponent<Button> ().interactable == true) {
			Color textColor;

			Color.TryParseHexString ("#1FFFECFF", out textColor);
			Text[] temp = this.GetComponentsInChildren<Text> ();
			foreach (Text t in temp) {
				t.color = textColor;
			}
		}
	}

	public void notHovering(){
		Text[] temp = this.GetComponentsInChildren<Text> ();
		foreach (Text t in temp) {
			t.color = Color.white;
		}
	}

	public void disableCanvas (){
		if (Application.loadedLevelName == "Main_Menu") {
			GameObject.Find ("Canvas").transform.FindChild ("Play").GetComponent<Button> ().interactable = false;
			GameObject.Find ("Canvas").transform.FindChild ("Load").GetComponent<Button> ().interactable = false;
			GameObject.Find ("Canvas").transform.FindChild ("Settings").GetComponent<Button> ().interactable = false;
			GameObject.Find ("Canvas").transform.FindChild ("Quit").GetComponent<Button> ().interactable = false;
		} else {
			Button[] buttons = GameObject.Find("MenuMask").GetComponentsInChildren<Button>();
			foreach(Button btn in buttons){
				btn.interactable = false;
			}
		}
	}

	public void enableCanvas (){
		if (Application.loadedLevelName == "Main_Menu") {
			GameObject.Find ("Canvas").transform.FindChild ("Play").GetComponent<Button> ().interactable = true;
			GameObject.Find ("Canvas").transform.FindChild ("Load").GetComponent<Button> ().interactable = true;
			GameObject.Find ("Canvas").transform.FindChild ("Settings").GetComponent<Button> ().interactable = true;
			GameObject.Find ("Canvas").transform.FindChild ("Quit").GetComponent<Button> ().interactable = true;
		} else {
			Button[] buttons = GameObject.Find("MenuMask").GetComponentsInChildren<Button>();
			foreach(Button btn in buttons){
				btn.interactable = true;
			}
		}
	}

	public void quitToMenu(){
		Application.LoadLevel("Main_Menu");
		ResumeGame ();
		player.transform.position = new Vector3 (-374f, 101.4f, 395.3f);
		player.transform.rotation = Quaternion.Euler (0f, 195.0949f, 0f);
		player.GetComponent<FauxGravityBody> ().attractor = null;
		player.GetComponent<Rigidbody> ().useGravity = true;
	}

	public void ResumeGame(){
		player.GetComponent<PlayerController> ().showQuit = false;
		player.GetComponent<PlayerController> ().paused = false;
	}

	public void save(){
		disableCanvas ();

		saveCanvas.enabled = true;
		GameObject.Find ("SaveCanvas").transform.FindChild("Panel").FindChild("Slot1").FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (1);
		GameObject.Find ("SaveCanvas").transform.FindChild("Panel").FindChild("Slot2").FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (2);
		GameObject.Find ("SaveCanvas").transform.FindChild("Panel").FindChild("Slot3").FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (3);
	}

	public void saveGame(int slot){
		attributesScript.save (slot);
		save ();
	}
}