using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {
	private bool expandTheHud;
	private bool shrinkTheHud;

	private Image expandingHUD;
	private Image shrinkButton;
	private Image expandButton;
	//private Image hudText

	void Start(){
		expandingHUD = GameObject.Find ("Expand").GetComponent<Image> ();
		shrinkButton = GameObject.Find ("Shrink_Button").GetComponent<Image> ();
		expandButton = GameObject.Find ("Expand_Button").GetComponent<Image> ();
		shrinkButton.enabled = true;
		expandButton.enabled = false;
		expandingHUD.fillAmount = 1;
	}

	public void expandHud(){
		expandTheHud = true;
		shrinkTheHud = false;
		expandButton.enabled = false;
		shrinkButton.enabled = true;
	}

	public void shrinkHUD(){
		shrinkTheHud = true;
		expandTheHud = false;
		expandButton.enabled = true;
		shrinkButton.enabled = false;
	}

	void Update(){
		if (expandTheHud) {
			if (expandingHUD.fillAmount < 1) {
				expandingHUD.fillAmount += 0.05f;
			}
		}

		if (shrinkTheHud) {
			if(expandingHUD.fillAmount > 0){
				expandingHUD.fillAmount -= 0.05f;
			}
		}
	}
}