using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	private PlayerAttributes attributesScript;
	private PlayerController playerScript;

	// Use this for initialization
	void Start () {
		attributesScript = this.GetComponent<PlayerAttributes> ();
		playerScript = this.GetComponent<PlayerController> ();

		//take this out when gender GUI is replaced
		playerScript.paused = false;
		
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
				this.GetComponent<Sounds>().playWorldSound (2);
				playerScript.paused = false;
			}
			if (GUI.Button (new Rect (left+80, top+90, 100, 30), "Female")) {
				print ("You chose a female");
				attributesScript.setGender('f');
				this.GetComponent<Sounds>().playWorldSound (2);
				playerScript.paused = false;
			}*/
		}
	}
}
