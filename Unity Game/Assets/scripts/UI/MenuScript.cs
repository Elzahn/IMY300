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
		Text textMesh = this.GetComponentInChildren<Text>();

		if (textMesh.font == defaultFont) {
			textMesh.font = alienFont;
			Color textColor;
			Color.TryParseHexString("#95E0FFFF", out textColor);
			textMesh.color = textColor;

		} else {
			textMesh.font = defaultFont;
			textMesh.color = Color.white;
		}
	}

	public void play(){
		Application.LoadLevel ("SaveSpot");
		
		player.GetComponent<Tutorial>().startTutorial = true;
		GameObject.Find ("HUD").GetComponent<Canvas> ().enabled = true;
		player.transform.rotation = Quaternion.Euler (351.66f, 179.447f, 358.8f);
		player.transform.up = Vector3.up;
		player.transform.position = new Vector3 (10.88f, 79.831f, -11.14f);
	}
	
	public void load(){
		loadCanvas.enabled = true;
		GameObject.Find ("Autosave").transform.FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (0);
		GameObject.Find ("Slot1").transform.FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (1);
		GameObject.Find ("Slot2").transform.FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (2);
		GameObject.Find ("Slot3").transform.FindChild ("Info").GetComponent<Text> ().text = attributesScript.readData (3);
	}

	public void settings(){
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
		Color.TryParseHexString("#95E0FFFF", out textColor);
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
}