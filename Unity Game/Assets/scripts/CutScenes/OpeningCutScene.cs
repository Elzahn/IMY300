using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class OpeningCutScene : MonoBehaviour {

	private MovieTexture movie;
	private GameObject player;
	private PlayerAttributes attributesScript;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		attributesScript = player.GetComponent<PlayerAttributes> ();
		player.GetComponent<Sounds> ().stopSound ("all");
		player.GetComponent<Sounds> ().playAmbienceSound (Sounds.SHIP_AMBIENCE);

		movie = (MovieTexture)GameObject.Find ("MovieScreen").GetComponent<Renderer> ().material.mainTexture;// = movie;
		movie.Play();
	}

	void Update() {
		if(Input.GetButtonDown("Skip")){
			movie.Stop();
		}

		if (!movie.isPlaying) {
			GameObject.Find("Character_Final").GetComponent<Animator>().SetBool("Dead", false);
			Application.LoadLevel ("SaveSpot");
			
			player.GetComponent<Tutorial> ().restartTutorial ();
			player.GetComponent<Tutorial>().startTutorial = true;
			player.GetComponent<SaveSpotTeleport>().loadTutorial = true;
			player.GetComponent<SaveSpotTeleport> ().canEnterSaveSpot = true;
			GameObject.Find ("HUD").GetComponent<Canvas> ().enabled = true;
			player.transform.rotation = Quaternion.Euler (351.66f, 179.447f, 358.8f);
			player.transform.up = Vector3.up;
			player.transform.position = new Vector3 (10.88f, 79.831f, -11.14f);
			GameObject.Find("Stamina").GetComponent<Image>().enabled = false;
			GameObject.Find("Health").GetComponent<Image>().enabled = false;
			player.GetComponent<Tutorial>().teachInventory = false;
			GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ().text = "";
			GameObject.Find("MenuMask").GetComponent<Image>().fillAmount = 0;
			player.GetComponent<LevelSelect>().currentLevel = 0;
			attributesScript.resetPlayer ();
			player.GetComponent<Tutorial> ().tutorialDone = false;
			player.GetComponent<LevelSelect> ().spawnedLevel = false;
		}
	}

}
