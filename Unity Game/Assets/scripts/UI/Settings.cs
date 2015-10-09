using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Settings : MonoBehaviour {

	private PlayerAttributes attributesScript;
	private Slider narrativeSlider, soundSlider, difficultySilder;
	public static int counter{ get; set; }	//used so all three slides can first be set before it changes the starting values

	void Start () {
		counter = 0;
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		narrativeSlider = GameObject.Find ("Slider Narrative").GetComponent<Slider> ();
		soundSlider = GameObject.Find ("Slider Sound").GetComponent<Slider> ();
		difficultySilder = GameObject.Find ("Slider Difficult").GetComponent<Slider> ();
	}

	public void saveValues(){
		if (counter >= 3) {
			attributesScript.narrativeShown = narrativeSlider.value;
			attributesScript.soundVolume = soundSlider.value;
			attributesScript.difficulty = difficultySilder.value;
		} else {
			counter++;
		}
	}
}
