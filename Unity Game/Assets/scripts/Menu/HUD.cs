using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {
	private bool expandTheHud;
	private bool shrinkTheHud;

	private Image expandingHUD;
	private Image shrinkButton;
	private Image expandButton;
	private Image interaction;
	private Text interactionText;
	private Image interactionImage;
	private Image hint;

	private GameObject player;
//	private Image hint;
	//private Text expandText;
	//private Scrollbar scrollbar;

	//private Image hudText

	void Start(){
		player = GameObject.Find ("Player");
		expandingHUD = GameObject.Find ("Expand").GetComponent<Image> ();
		shrinkButton = GameObject.Find ("Shrink_Button").GetComponent<Image> ();
		expandButton = GameObject.Find ("Expand_Button").GetComponent<Image> ();
		hint = GameObject.Find ("Hint").GetComponent<Image> ();
		interaction = GameObject.Find ("Interaction").GetComponent<Image> ();
		interactionText = GameObject.Find ("Interaction_Text").GetComponent<Text> ();
		interactionImage = GameObject.Find ("Interaction_Image").GetComponent<Image> ();
		//hint = GameObject.Find ("Hint").GetComponent<Image> ();
		//expandText = GameObject.Find ("HUD_Extend_Text").GetComponent<Text> ();
		//scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
		shrinkButton.enabled = true;
		expandButton.enabled = false;
		//hint.enabled = false;
		expandingHUD.fillAmount = 1;
	}

	public void makeInteractionHint(string _hintText, Sprite _hintImage){
		interaction.fillAmount = 1;
		hint.fillAmount = 0;
		GameObject.Find ("Player").GetComponent<Tutorial> ().moveHintOffScreen = false;
		GameObject.Find ("Player").GetComponent<Tutorial> ().moveHintOnScreen = false;
		interactionText.text = _hintText;
		interactionImage.sprite = _hintImage;
	}

	public void expandHud(){
		//expandText.text += "\nHi there";
		//Canvas.ForceUpdateCanvases();
		//scrollbar.value = 0f;
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
		if(!player.GetComponent<PlayerController>().showAttack && player.GetComponent<SaveSpotTeleport>().notInUse && !player.GetComponent<Collisions>().showLootConfirmation && !Loot.showInventoryHint && !player.GetComponent<Collisions>().showRestore && !InventoryGUI.hasCollided){
			interaction.fillAmount = 0;
		}

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