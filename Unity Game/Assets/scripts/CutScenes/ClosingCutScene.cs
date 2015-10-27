﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class ClosingCutScene : MonoBehaviour {
	
	private MovieTexture movie;
	private GameObject player;
	private PlayerAttributes attributesScript;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		attributesScript = player.GetComponent<PlayerAttributes> ();
		
		GameObject.Find("MovieScreen").GetComponent<Renderer>().material.mainTexture = movie;
		movie.Play();

		GameObject.Find("Character_Final").GetComponent<Animator>().SetBool("Dead", false);
		GameObject.Find ("HUD").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Storage").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Inventory").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Loot").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Popup").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("SettingsCanvas").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("LoadCanvas").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("MainMenu").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Death").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("SaveCanvas").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Warp").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("BirdsEye").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("Dizzy").SetActive(false);
		GameObject.Find("Stamina").GetComponent<Image>().enabled = false;
		GameObject.Find("Health").GetComponent<Image>().enabled = false;
		GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ().text = "";
		GameObject.Find("MenuMask").GetComponent<Image>().fillAmount = 0;
	}
	
	void Update() {
		if(Input.GetButtonDown("Skip")){
			movie.Stop();
		}
		
		if (!movie.isPlaying) {
			Application.LoadLevel ("Main_Menu");
		}
	}
}
