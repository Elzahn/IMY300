﻿using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public bool startTutorial{ get; set; }
	public bool teachStorage{ get; set; }
	public bool teachInventory{ get; set; }
	public  bool tutorialDone{ get; set; }

	private bool justStarted = true;
	private bool justArrivedOnPlanet = false;

	private Sounds sound;
	private bool showWASD = false;
	private bool showRun = false;
	public bool showAttack { get; set; }
	private bool showVisuals = true;

	private float showVisualQue;

	public Texture2D Attack;

	private int showVisualDuration = 5;

	// Use this for initialization
	void Start () {
		showAttack = false;
		print ("Press Escape to skip Tutorial");
		startTutorial = true;
		tutorialDone = false;
		teachStorage = false;
		teachInventory = false;
		sound = GameObject.Find("Player").GetComponent<Sounds>();
	}

	// Used to determine what happens next in the tutorial
	void Update () {
		if (startTutorial) {
			if(justStarted){
				playIntro();
			}

			if(Application.loadedLevelName == "Tutorial" && !justArrivedOnPlanet){
				leadTheWay();
			}

			if(Application.loadedLevelName == "SaveSpot" && tutorialDone && !sound.worldAudio.isPlaying){
				lastWordsOfWisdom();
			}
		}
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

		if(!sound.computerAudio.isPlaying && sound.computerClip != Sounds.COMPUTER_WARP){
			sound.playComputerSound(Sounds.COMPUTER_WARP);
			setupVisuals();
			showWASD = true;
		}

		if(!sound.computerAudio.isPlaying){
			this.GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
			justStarted = false;
		}
	}

	//Must be before the variable showing the instruction is set to true
	//Example setupVisuals(); showWASD = true;
	//This clears the tutorial visuals so that the new one can be shown
	public void setupVisuals(){
		showWASD = false;
		showRun = false;

		showVisuals = true;
		showVisualQue = Time.time + showVisualDuration;
	}

	public void leadTheWay(){
		//called to clear previous instruction if still on screen
		this.GetComponent<SaveSpotTeleport>().canEnterSaveSpot = false;
		setupVisuals();
		if (!sound.worldAudio.isPlaying) {
			justArrivedOnPlanet = true;
			setupVisuals();
			showRun = true;
			sound.playComputerSound (Sounds.COMPUTER_RUN);
		}
	}

	public void lastWordsOfWisdom(){
		//cut scene
		teachStorage = true;
		startTutorial = false;
		this.GetComponent<SaveSpotTeleport> ().loadTutorial = false;
	}
	
	public void OnGUI(){
		if (startTutorial) {
			if(showVisuals){
				if (showWASD) {
					GUI.depth = 0;
					GUI.color = new Color32 (255, 255, 255, 50);
					GUI.Box (new Rect (140, Screen.height - 50, Screen.width - 300, 120), (""));
					GUI.color = new Color32 (255, 255, 255, 255);
					GUI.Label (new Rect (Screen.width/2-100, Screen.height - 35, Screen.width - 300, 120), ("Move around using W/A/S/D"));
					//GUI.DrawTexture (new Rect (Screen.width / 2 - Screen.width / 8, Screen.height / 2 - Screen.height / 3, Screen.width / 4, Screen.height / 4), WASD);
				} else if(showRun){
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
					GUI.DrawTexture (new Rect (Screen.width / 2 - 25, Screen.height - 45, 30, 40), Attack);
				}
			}
			if (Time.time >= showVisualQue) {
				showVisuals = false;
			}
		}
	}
}