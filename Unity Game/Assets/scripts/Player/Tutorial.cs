using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public bool startTutorial{ get; set; }
	public bool teachStorage{ get; set; }
	public bool teachInventory{ get; set; }
	public bool tutorialDone{ get; set; }
	public bool showVisuals { get; set; }
	public bool moveHintOnScreen { get; set; }
	public bool moveHintOffScreen { get; set; }
	public bool healthHintShown { get; set; }
	public float showVisualQue { get; set; }

	private Image health;
	private Image stamina;
	private Image interaction;
	private Image hint;
	private Image hintImage;
	private Text hintText;
	private Text hudText;
	private bool justStarted = true;
	private bool justArrivedOnPlanet = false;
	private Sounds sound;
	private int visualDuration = 7;
	private PlayerAttributes attribteScript;

	public Sprite Walk;
	public Sprite Run;
	public Sprite PressI;
	public Sprite Middle;
	public Sprite Warp;
	public Sprite PressE;
	public Sprite Health;
	public Sprite CameraMove;
	public Sprite Dark;
	public Sprite Health_Bar;

	void Start () {
		if (Application.loadedLevelName == "SaveSpot") {
			GameObject.Find ("Tech Light").GetComponent<Light> ().enabled = false;
			GameObject.Find ("Console Light").GetComponent<Light> ().enabled = false;
			GameObject.Find ("Bedroom Light").GetComponent<Light> ().enabled = false;
		}

		attribteScript = this.GetComponent<PlayerAttributes>();
		this.GetComponent<SaveSpotTeleport> ().canEnterSaveSpot = true;

		moveHintOnScreen = false;
		moveHintOffScreen = false;
		healthHintShown = false;

		health = GameObject.Find ("Health").GetComponent<Image> ();
		stamina = GameObject.Find ("Stamina").GetComponent<Image> ();
		hudText = GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ();
		interaction = GameObject.Find ("Interaction").GetComponent<Image> ();
		hint = GameObject.Find ("Hint").GetComponent<Image> ();
		hintText = GameObject.Find ("Hint_Text").GetComponent<Text> ();
		hintImage = GameObject.Find ("Hint_Image").GetComponent<Image> ();
		sound = GameObject.Find("Player").GetComponent<Sounds>();

		showVisuals = true;
		startTutorial = false;
		tutorialDone = false;
		teachStorage = false;
		teachInventory = false;

		health.enabled = false;
		stamina.enabled = false;
	}

	// Used to determine what happens next in the tutorial
	void Update () {
		if (this.GetComponent<PlayerController> ().paused) {
			//sound.pauseSound("all");
			GameObject[] monsters =  GameObject.FindGameObjectsWithTag("Monster");

			foreach(GameObject m in monsters){
				sound.pauseMonsterSound(m.GetComponent<Enemy>());
			}

			showVisualQue = Time.time + visualDuration;
		} else {

			if (Time.time >= showVisualQue && hint.fillAmount >= 1){
				moveHintOffScreen = true;
				moveHintOnScreen = false;
			}

			if(Time.time >= showVisualQue && healthHintShown){
				healthHintShown = false;
			}

			if(Application.loadedLevelName != "Scene"){
				if(moveHintOnScreen){
					if(hint.fillAmount < 1)
					{
						hint.fillAmount += 0.01f;
					}
				}
				
				if(moveHintOffScreen){
					if(hint.fillAmount > 0){
						hint.fillAmount -= 0.01f;
					}

					if (hint.fillAmount <= 0 && hintText.text == Loot.inventoryHintText) {
						Loot.showInventoryHint = false;
					}
				}
			} else if(Application.loadedLevelName == "Scene"){
				if(moveHintOnScreen){
					if(hint.fillAmount < 1)
					{
						hint.fillAmount += 0.1f;
					}
				}

				if(moveHintOffScreen){
					if(hint.fillAmount > 0){
						hint.fillAmount -= 0.1f;
					}
					if(hint.fillAmount <= 0 && hintText.text == "Some monsters prefer the dark as it makes them stronger."){
						this.GetComponent<SaveSpotTeleport>().showedHealthHint = true;
					}
				}
			}

			if(hint.fillAmount <= 0 && hintText.text == "Need a health pack? Look out for these flowers."){
				makeHint("Some monsters prefer the dark as it makes them stronger while others follow the light.", Dark);
			}

			if(hint.fillAmount >= 1 && hintText.text == "Run with left shift + W/A/S/D. Just remember that running and attacks deplete your stamina."){
				stamina.enabled = true;
				if (GameObject.Find ("HUD Body Temp").GetComponent<Image> () != null) {
					GameObject.Find ("HUD Body Temp").GetComponent<Image> ().enabled = false;
				}
			}

			if(hint.fillAmount <= 0 && hintText.text == "Run with left shift + W/A/S/D. Just remember that running and attacks deplete your stamina."){
				this.GetComponent<SaveSpotTeleport>().loadTutorial = false;
			}

			if(hint.fillAmount <= 0 && hintText.text == "Move around using W/A/S/D"){
				makeHint("Rotate the camera by moving the mouse while holding down", CameraMove);
			}

			if(hint.fillAmount <= 0 && hintText.text == "Access the satelites with "){
				makeHint ("Run with left shift + W/A/S/D. Just remember that running and attacks deplete your stamina.", Run);
			}

			if(Application.loadedLevelName == "Tutorial"){
				sound.resumeSound("all");
				GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
				foreach(GameObject m in monsters){
					if(m.GetComponent<Enemy>().GetComponent<AudioSource> () != null){
						sound.resumeMonsterSound(m.GetComponent<Enemy>());
					}
				}
			}
		}

		if (startTutorial && !Application.isLoadingLevel) {
			if (justStarted) {
				playIntro ();
			}

		if (Application.loadedLevelName == "Tutorial" && !justArrivedOnPlanet) {
				leadTheWay ();
		}
		
		if(Application.loadedLevelName == "Tutorial" && !sound.computerAudio.isPlaying && sound.computerClip == Sounds.COMPUTER_SATELLITE){
			teachToRun();
		}

		if (Application.loadedLevelName == "SaveSpot" && tutorialDone && !sound.worldAudio.isPlaying) {
			lastWordsOfWisdom ();
			}
		}
	}

	public void showHealthHint(){
		health.enabled = true;
		if (GameObject.Find ("HUD Body Temp").GetComponent<Image> () != null) {
			GameObject.Find ("HUD Body Temp").GetComponent<Image> ().enabled = false;
		}
		Camera.main.GetComponent<HUD>().makeInteractionHint ("Remember to watch your health during a battle.", Health_Bar);
		healthHintShown = true;
		showVisualQue = Time.time + visualDuration;
	}

	public void stopTutorial(){
		startTutorial = false;
		sound.stopSound ("computer");
		teachStorage = true;
		teachInventory = true;
		//stop cutscenes
	}

	public void playIntro(){
		//show cutcenes
		if (!this.GetComponent<PlayerController> ().paused) {
			sound.resumeSound("all");

			if(!sound.computerAudio.isPlaying && sound.computerClip != Sounds.COMPUTER_WARP){
				sound.playComputerSound(Sounds.COMPUTER_WARP);
				hudText.text += "\n\nThe ship's Power Core has disengaged during the crash. You'll need to go outside and retrieve it before the emergency power shuts down. I have detected three potentially hostile lifeforms on the planet. They appear to have taken possession of the core. You may need to take aggressive action to retrieve it. Look around to find the teleportation pad and go outside.\n";
				attribteScript.narrativeSoFar += "\n\nThe ship's Power Core has disengaged during the crash. You'll need to go outside and retrieve it before the emergency power shuts down. I have detected three potentially hostile lifeforms on the planet. They appear to have taken possession of the core. You may need to take aggressive action to retrieve it. Look around to find the teleportation pad and go outside.\n";
				makeHint("Move around using W/A/S/D", Walk);
				Canvas.ForceUpdateCanvases();
				Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
				scrollbar.value = 0f;
			}

			if(!sound.computerAudio.isPlaying){
				justStarted = false;
			}
		} else {
			sound.pauseSound("all");
		}
	}

	public void leadTheWay(){
		if (!this.GetComponent<PlayerController> ().paused) {
			sound.resumeSound("all");
			GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
			foreach(GameObject m in monsters){
				if(m.GetComponent<Enemy>().GetComponent<AudioSource> () != null){
					sound.resumeMonsterSound(m.GetComponent<Enemy>());
				}
			}
			this.GetComponent<SaveSpotTeleport> ().canEnterSaveSpot = false;

			makeHint("Access the satelites with ", Middle);
		
			if (!sound.worldAudio.isPlaying) {
				justArrivedOnPlanet = true;
				sound.playComputerSound (Sounds.COMPUTER_SATELLITE);
				hudText.text += "\n\nI have linked you to satellites orbiting the planet. This will assist you during your mission.\n\n";
				attribteScript.narrativeSoFar += "\n\nI have linked you to satellites orbiting the planet. This will assist you during your mission.\n\n";
				Canvas.ForceUpdateCanvases();
				Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
				scrollbar.value = 0f;
			}
		} else {
			sound.pauseSound("all");
			GameObject[] monsters =  GameObject.FindGameObjectsWithTag("Monster");
			foreach(GameObject m in monsters){
				sound.pauseMonsterSound(m.GetComponent<Enemy>());
			}
		}
	}

	public void teachToRun(){
		if (!this.GetComponent<PlayerController> ().paused) {
			sound.playComputerSound (Sounds.COMPUTER_RUN);
			//makeHint ("Run with left shift + W/A/S/D. Just remember that running and attacks deplete your stamina.", Run);
			hudText.text += "After you have retrieved the power core from the lifeforms head back to the teleporter to return to the ship.\n\n";
			attribteScript.narrativeSoFar += "After you have retrieved the power core from the lifeforms head back to the teleporter to return to the ship.\n\n";
			Canvas.ForceUpdateCanvases();
			Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
			scrollbar.value = 0f;
		}
	}

	public void lastWordsOfWisdom(){
		if (!this.GetComponent<PlayerController> ().paused) {
			sound.resumeSound("all");
			//cut scene
			teachStorage = true;
			this.GetComponent<SaveSpotTeleport> ().loadTutorial = false;
		} else {
			sound.pauseSound("all");
		}
	}

	public void makeHint(string _hintText, Sprite _hintImage){
		if (_hintText == Loot.inventoryHintText || (Application.loadedLevelName == "Scene" && !GameObject.Find("Planet").GetComponent<LoadingScreen> ().loading)) {
			interaction.fillAmount = 0;
		}
		hint.fillAmount = 0.001f;

		showVisualQue = Time.time + visualDuration;
		moveHintOnScreen = true;
		moveHintOffScreen = false;
		hintText.text = _hintText;
		hintImage.sprite = _hintImage;
	}
}