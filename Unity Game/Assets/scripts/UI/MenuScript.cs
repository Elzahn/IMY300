using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public Font alienFont;
	public Font defaultFont;

	private PlayerAttributes attributesScript;

	void Start(){
		attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		GameObject.Find ("Player").GetComponent<Tutorial> ().enabled = false;
	}

	public void changeFont(){
		Text textMesh = this.GetComponentInChildren<Text>();

		if (textMesh.font == defaultFont) {
			textMesh.font = alienFont;
			Color textColor;
			Color.TryParseHexString("#95E0FFFF", out textColor);
			textMesh.color = textColor;

		} else {
			textMesh.font = defaultFont;
			textMesh.color = Color.white;
		}

		//textMesh.renderer.sharedMaterial = ArialFont.material;*/
	}

	public void play(){
		Application.LoadLevel ("SaveSpot");
	}

	public void load(){

	}

	public void settings(){

	}

	public void quit(){
		Application.Quit ();
	}
}