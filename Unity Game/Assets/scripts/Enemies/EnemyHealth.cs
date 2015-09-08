using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	private Enemy enemyScript;
	
	public Canvas canvas;

	private Image healthSlider;
	private Renderer myRenderer;

	void Start () {
		enemyScript = GetComponent<Enemy> ();

		myRenderer = GetComponentInChildren<Renderer>();

		//Shows Enemy Name in text box
		this.GetComponentInChildren<Text> ().text = enemyScript.typeID;

		healthSlider = this.GetComponentInChildren<Image> ();
	}

	void Update(){

		healthSlider.fillAmount = enemyScript.hp / enemyScript.maxHp;

		Vector3 worldPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

		float distance = (worldPos - Camera.main.transform.position).magnitude;

		float alpha = 3 - (distance) / 2.0f;

		//Shows or hide healthbar depending on distance from player
		if (myRenderer.isVisible && alpha > -4)
		{
			canvas.enabled = true;
		}
		else
		{
			canvas.enabled = false;
		}
	}
}