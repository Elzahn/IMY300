using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {
	private bool expandTheHud;
	private bool shrinkTheHud;

	private Text interactionText;
	private Image interaction;
	private Image interactionImage;
	private Image expandingHUD;
	private Image hint;
	private Image expand;
	private Image shrink;

	private GameObject player;

	void Start(){
		shrinkTheHud = false;
		expandTheHud = true;
		player = GameObject.Find ("Player");
		expandingHUD = GameObject.Find ("Expand").GetComponent<Image> ();
		expand = GameObject.Find ("Expanding").GetComponent<Image> ();
		shrink = GameObject.Find ("Shrink").GetComponent<Image> ();
		hint = GameObject.Find ("Hint").GetComponent<Image> ();
		interaction = GameObject.Find ("Interaction").GetComponent<Image> ();
		interactionText = GameObject.Find ("Interaction_Text").GetComponent<Text> ();
		interactionImage = GameObject.Find ("Interaction_Image").GetComponent<Image> ();
		expandingHUD.fillAmount = 1;
		expand.enabled = false;
	}

	public void makeInteractionHint(string _hintText, Sprite _hintImage){
		interaction.fillAmount = 1;
		hint.fillAmount = 0;
		GameObject.Find ("Player").GetComponent<Tutorial> ().moveHintOffScreen = false;
		GameObject.Find ("Player").GetComponent<Tutorial> ().moveHintOnScreen = false;
		interactionText.text = _hintText;
		interactionImage.sprite = _hintImage;
		interactionImage.GetComponent<Mask>().showMaskGraphic = true;
	}

	public void showOrHideNarative(){
		if (shrinkTheHud) {
			shrink.enabled = true;
			expand.enabled = false;
			shrinkTheHud = false;
			expandTheHud = true;
		} else {
			expand.enabled = true;
			shrink.enabled = false;
			expandTheHud = false;
			shrinkTheHud = true;
		}
	}

	void Update(){
		if (interaction.fillAmount == 0) {
			interactionImage.GetComponent<Mask>().showMaskGraphic = false;
		}

		if(!player.GetComponent<PlayerController>().showAttack && player.GetComponent<SaveSpotTeleport>().notInUse && !player.GetComponent<Collisions>().showLootConfirmation && !Loot.showInventoryHint && !player.GetComponent<Collisions>().showRestore && !InventoryGUI.hasCollided && !Collisions.showHealthConfirmation){
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