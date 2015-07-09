using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	private PlayerAttributes attributesScript;
	private PlayerController playerScript;

	// Use this for initialization
	void Start () {
		attributesScript = GameObject.Find("Persist").GetComponent<PlayerAttributes> ();
		playerScript = this.GetComponent<PlayerController> ();
	}

	void OnGUI()
	{
		if (attributesScript.getGender () == '?') {
			playerScript.setPaused (true);
			GUI.Box (new Rect (300, 30, 250, 250), "Choose your character");
			if (GUI.Button (new Rect (380, 80, 100, 30), "Male")) {
				print ("You chose a male");
				attributesScript.setGender ('m');
				playerScript.setPaused (false);
			}
			if (GUI.Button (new Rect (380, 120, 100, 30), "Female")) {
				print ("You chose a female");
				attributesScript.setGender ('f');
				playerScript.setPaused (false);
			}
		} else {
			Application.LoadLevel ("Scene");
		}
	}
}
