using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {
	private bool expandTheHud;

	public void expandHud(){
		expandTheHud = true;
	}

	void Update(){
		//float expandHUDpanel = Time.time;
		if (expandTheHud) {
			Image expandingHUD = GameObject.Find ("Expand").GetComponent<Image> ();
		
			if (expandingHUD.fillAmount < 1) {
				//if(Time.time >= expandHUDpanel + 1)
				{
					//	print (expandingHUD.fillAmount);
					expandingHUD.fillAmount += 0.05f;
					//expandHUDpanel = Time.time;
				}
			}
		}
	}
}