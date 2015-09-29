using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelection : MonoBehaviour {

	private PlayerAttributes attributesScript;
	ParticleSystem levelUp;
//	private PlayerController playerScript;

	// Use this for initialization 
	void Start () {
		GameObject.Find ("Loading Screen").GetComponent<Canvas> ().enabled = false;
		attributesScript = this.GetComponent<PlayerAttributes> ();
		PlayerLog.showHUD = true;
		levelUp = GameObject.Find ("LevelUp").GetComponent<ParticleSystem> ();
		levelUp.enableEmission = false;
		levelUp.Clear ();

		//playerScript = this.GetComponent<PlayerController> ();		
	}

	void OnGUI()
	{
		if (attributesScript.gender == '?') {
		/*	playerScript.paused = true;

			int boxWidth = 250;
			int boxHeight = 150;
			int top = (int)Screen.height/2 - boxHeight/2;//250;
			int left = (int)Screen.width/2 - boxWidth/2;//550;

			GUI.Box (new Rect (left, top, boxWidth, boxHeight), "Choose your character");
			if (GUI.Button (new Rect (left+80, top+50, 100, 30), "Male")) {
				print ("You chose a male");
				attributesScript.setGender('m');
				this.GetComponent<Sounds>().playWorldSound (Sounds.BUTTON);
				playerScript.paused = false;
			}
			if (GUI.Button (new Rect (left+80, top+90, 100, 30), "Female")) {
				print ("You chose a female");
				attributesScript.setGender('f');
				this.GetComponent<Sounds>().playWorldSound (Sounds.BUTTON);
				playerScript.paused = false;
			}*/
		}
	}
}
