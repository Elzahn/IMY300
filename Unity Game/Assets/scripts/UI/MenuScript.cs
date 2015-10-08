using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MenuScript : MonoBehaviour {

	public Font alienFont;
	public Font defaultFont;

	private PlayerAttributes attributesScript;
	private GameObject player;
	void Start(){
		player = GameObject.Find ("Player");
		attributesScript = player.GetComponent<PlayerAttributes> ();
		player.GetComponent<Animator>().SetBool("Samba", true);
		//GameObject.Find ("Player").GetComponent<Tutorial> ().enabled = false;
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
		player.GetComponent<Animator>().SetBool("Samba", false);
		player.transform.rotation = Quaternion.Euler (351.66f, 179.447f, 358.8f);
		player.transform.up = Vector3.up;
		player.transform.position = new Vector3 (13.18f, 81.55f, 14.8f);
	}

	public void load(){
		player.GetComponent<Animator>().SetBool("Samba", false);
		/*int slot = 1;

		try{
			attributesScript.load (slot);
		} catch (IOException exception){
			//show no save on that slot
		}

		player.transform.rotation = Quaternion.Euler (351.66f, 179.447f, 358.8f);
		player.transform.up = Vector3.up;
		player.transform.position = new Vector3 (13.18f, 81.55f, 14.8f);*/
	}

	public void settings(){

	}

	public void quit(){
		Application.Quit ();
	}
}