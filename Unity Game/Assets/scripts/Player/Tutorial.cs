using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//Move hint to -180.5, -90.7, 0
//Move hint of screen 203.9
public class Tutorial : MonoBehaviour {

	public bool startTutorial{ get; set; }
	public bool teachStorage{ get; set; }
	public bool teachInventory{ get; set; }
	public  bool tutorialDone{ get; set; }
	public bool showVisuals { get; set; }

	private Image hint;
	private Text hintText;
	private Image hintImage;
	private bool justStarted = true;
	private bool justArrivedOnPlanet = false;
	private bool moveHintOnScreen;
	private bool moveHintOffScreen;

	private Sounds sound;
	private bool showWASD = false;
	private bool showRun = false;
	public bool showAttack { get; set; }
	public float showVisualQue { get; set; }

	public Sprite Attack;
	public Sprite Walk;

	private int visualDuration = 7;

	// Use this for initialization
	void Start () {
		moveHintOnScreen = false;
		moveHintOffScreen = false;

		hint = GameObject.Find ("Hint").GetComponent<Image> ();
		hintText = GameObject.Find ("Hint_Text").GetComponent<Text> ();
		hintImage = GameObject.Find ("Hint_Image").GetComponent<Image> ();

		this.GetComponent<SaveSpotTeleport> ().canEnterSaveSpot = true;
		showVisuals = true;
		showAttack = false;
		//print ("Press Escape to skip Tutorial");
		startTutorial = true;
		tutorialDone = false;
		teachStorage = false;
		teachInventory = false;
		sound = GameObject.Find("Player").GetComponent<Sounds>();
	}

	// Used to determine what happens next in the tutorial
	void Update () {
		if (this.GetComponent<PlayerController> ().paused) {
			sound.pauseSound("all");
			GameObject[] monsters =  GameObject.FindGameObjectsWithTag("Monster");

			foreach(GameObject m in monsters){
				sound.pauseMonsterSound(m.GetComponent<Enemy>());
			}

			showVisualQue = Time.time + visualDuration;
		} else {

			if (Time.time >= showVisualQue){// && GameObject.Find ("Planet") != null && GameObject.Find ("Planet").GetComponent<LoadingScreen>().loading == false) {
				moveHintOffScreen = true;
				moveHintOnScreen = false;
			}

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
			}

			sound.resumeSound("all");
			GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
			foreach(GameObject m in monsters){
				if(m.GetComponent<Enemy>().GetComponent<AudioSource> () != null){
					sound.resumeMonsterSound(m.GetComponent<Enemy>());
				}
			}
		}

		if (startTutorial) {
			if (justStarted) {
				playIntro ();
			}

		if (Application.loadedLevelName == "Tutorial" && !justArrivedOnPlanet) {
				leadTheWay ();
		}
		
		if (Application.loadedLevelName == "SaveSpot" && tutorialDone && !sound.worldAudio.isPlaying) {
			lastWordsOfWisdom ();
			}
		}
		//} else {

	}

	public void stopTutorial(){
		startTutorial = false;
		sound.stopSound ("computer");
		teachStorage = true;
		teachInventory = true;
		setupVisuals ();
		//stop cutscenes
	}

	public void playIntro(){
		//show cutcenes
		if (!this.GetComponent<PlayerController> ().paused) {
			sound.resumeSound("all");

			if(!sound.computerAudio.isPlaying && sound.computerClip != Sounds.COMPUTER_WARP){
				sound.playComputerSound(Sounds.COMPUTER_WARP);
				setupVisuals();
				//showWASD = true;
				makeHint("Move around using W/A/S/D", Walk);
			}

			if(!sound.computerAudio.isPlaying){
		//		this.GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
				justStarted = false;
			}
		} else {
			sound.pauseSound("all");
		}
	}

	//Must be before the variable showing the instruction is set to true
	//Example setupVisuals(); showWASD = true;
	//This clears the tutorial visuals so that the new one can be shown
	public void setupVisuals(){
		showWASD = false;
		showRun = false;
		if (GameObject.Find ("Planet") != null) {
			GameObject.Find ("Planet").GetComponent<TutorialSpawner> ().showInventory = false;
		}
		showVisuals = true;
		showVisualQue = Time.time + visualDuration;
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
	//		if (GameObject.Find ("Planet") != null && GameObject.Find ("Planet").GetComponent<LoadingScreen> ().loading == false) {
				//called to clear previous instruction if still on screen
				setupVisuals ();
				if (!sound.worldAudio.isPlaying) {
					justArrivedOnPlanet = true;
					setupVisuals ();
					showRun = true;
					sound.playComputerSound (Sounds.COMPUTER_RUN);
				}
			//}
		} else {
			sound.pauseSound("all");
			GameObject[] monsters =  GameObject.FindGameObjectsWithTag("Monster");
			foreach(GameObject m in monsters){
				sound.pauseMonsterSound(m.GetComponent<Enemy>());
			}
		}
	}

	public void lastWordsOfWisdom(){
		if (!this.GetComponent<PlayerController> ().paused) {
			sound.resumeSound("all");
			//cut scene
			teachStorage = true;
			startTutorial = false;
			this.GetComponent<SaveSpotTeleport> ().loadTutorial = false;
		} else {
			sound.pauseSound("all");
		}
	}

	public void makeHint(string _hintText, Sprite _hintImage){
		showVisualQue = Time.time + visualDuration;
		moveHintOnScreen = true;
		moveHintOffScreen = false;
		hintText.text = _hintText;
		hintImage.sprite = _hintImage;
	}

	public void OnGUI(){
		if (startTutorial) {
			if(showVisuals){
				if(showRun){
					GUI.depth = 0;
					GUI.color = new Color32 (255, 255, 255, 100);
					GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), (""));
					GUI.color = new Color32 (255, 255, 255, 255);
					GUI.Label (new Rect (Screen.width/2-100, Screen.height - 35, Screen.width - 300, 120), ("Run with left shift + W/A/S/D"));
				} else if(showAttack){
					GUI.depth = 0;
					GUI.color = new Color32 (255, 255, 255, 100);
					GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), (""));
					GUI.color = new Color32 (255, 255, 255, 255);
					GUI.Label (new Rect (Screen.width/2-100, Screen.height - 35, Screen.width - 300, 120), ("Attack with "));
					//GUI.DrawTexture (new Rect (Screen.width / 2 - 25, Screen.height - 45, 30, 40), Attack);
				}
			}
			if (Time.time >= showVisualQue){// && GameObject.Find ("Planet") != null && GameObject.Find ("Planet").GetComponent<LoadingScreen>().loading == false) {
				showVisuals = false;
			}
		}
	}
}
