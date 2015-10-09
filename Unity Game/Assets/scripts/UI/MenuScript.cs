using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MenuScript : MonoBehaviour {

	public Font alienFont;
	public Font defaultFont;

	private Canvas loadCanvas, settingsCanvas;
	private PlayerAttributes attributesScript;
	private GameObject player;
	private static GameObject errorPopup;
	private float errorTime = 0f, checkTime = 3f;

	void Start(){
		player = GameObject.Find ("Player");
		attributesScript = player.GetComponent<PlayerAttributes> ();
		loadCanvas = GameObject.Find ("LoadCanvas").GetComponent<Canvas> ();
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
		disableCanvas ();
		loadCanvas.enabled = true;
		GameObject.Find ("Autosave").transform.FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (0);
		GameObject.Find ("Slot1").transform.FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (1);
		GameObject.Find ("Slot2").transform.FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (2);
		GameObject.Find ("Slot3").transform.FindChild ("Hovering").FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (3);
	}

	public void settings(){
		disableCanvas ();
		Settings.counter = 0;

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
		enableCanvas ();
	}

	public void loadGame(int slot){
		bool error = false;

		try{
			attributesScript.load (slot);
		} catch (IOException exception){
			print (exception);
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
		}
	}

	void Update(){
		if (Time.time >= errorTime + checkTime && errorTime != 0f) {
			errorPopup.SetActive(false);
			errorTime = 0f;
		}
	}

	public void hovering(){
		Color textColor;
		Color.TryParseHexString("#1FFFECFF", out textColor);
		Text[] temp = this.GetComponentsInChildren<Text> ();
		foreach (Text t in temp) {
			t.color = textColor;
		}
	}

	public void notHovering(){
		Text[] temp = this.GetComponentsInChildren<Text> ();
		foreach (Text t in temp) {
			t.color = Color.white;
		}
	}

	public void disableCanvas (){
		GameObject.Find ("Canvas").transform.FindChild("Play").GetComponent<Button> ().interactable = false;
		GameObject.Find ("Canvas").transform.FindChild("Load").GetComponent<Button> ().interactable = false;
		GameObject.Find ("Canvas").transform.FindChild("Settings").GetComponent<Button> ().interactable = false;
		GameObject.Find ("Canvas").transform.FindChild("Quit").GetComponent<Button> ().interactable = false;
	}

	public void enableCanvas (){
		GameObject.Find ("Canvas").transform.FindChild("Play").GetComponent<Button> ().interactable = true;
		GameObject.Find ("Canvas").transform.FindChild("Load").GetComponent<Button> ().interactable = true;
		GameObject.Find ("Canvas").transform.FindChild("Settings").GetComponent<Button> ().interactable = true;
		GameObject.Find ("Canvas").transform.FindChild("Quit").GetComponent<Button> ().interactable = true;
	}

	public void quitToMenu(){
		Application.LoadLevel("Main_Menu");
		player.transform.rotation = Quaternion.Euler (-374.03f, 101.4f, 395.26f);
		player.transform.up = Vector3.up;
		player.transform.position = new Vector3 (0f, -164.9053f, 0f);
		player.GetComponent<FauxGravityBody> ().attractor = null;
		player.GetComponent<Rigidbody> ().useGravity = true;

	}
}