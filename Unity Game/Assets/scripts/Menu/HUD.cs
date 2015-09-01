using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {
	private bool expandTheHud;
	private bool shrinkTheHud;

	private Image expandingHUD;
	private Image shrinkButton;
	private Image expandButton;
	private Text expandText;
	private Scrollbar scrollbar;

	//private Image hudText

	void Start(){
		expandingHUD = GameObject.Find ("Expand").GetComponent<Image> ();
		shrinkButton = GameObject.Find ("Shrink_Button").GetComponent<Image> ();
		expandButton = GameObject.Find ("Expand_Button").GetComponent<Image> ();
		expandText = GameObject.Find ("HUD_Extend_Text").GetComponent<Text> ();
		scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
		shrinkButton.enabled = true;
		expandButton.enabled = false;
		expandingHUD.fillAmount = 1;
	}

	public void expandHud(){
		expandText.text += "\nHi there";
		Canvas.ForceUpdateCanvases();
		scrollbar.value = 0f;
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