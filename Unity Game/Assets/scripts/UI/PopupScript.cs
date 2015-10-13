using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupScript : MonoBehaviour {

	Warping warp;

	void Start(){
		warp = GameObject.Find ("Player").GetComponent<Warping> ();
	}

	void Update(){
		if (this.gameObject.GetComponent<Canvas>().enabled == true && warp.col != null) {
			print (warp.col.name);
			if("WarpPoint1" == warp.col.name){
				this.gameObject.transform.FindChild("Background").FindChild("Warp1").GetComponent<Button>().interactable = false;
			} else {
				this.gameObject.transform.FindChild("Background").FindChild("Warp1").GetComponent<Button>().interactable = true;
			}

			if("WarpPoint2" == warp.col.name){
				this.gameObject.transform.FindChild("Background").FindChild("Warp2").GetComponent<Button>().interactable = false;
			} else {
				this.gameObject.transform.FindChild("Background").FindChild("Warp2").GetComponent<Button>().interactable = true;
			}

			if("WarpPoint3" == warp.col.name){
				this.gameObject.transform.FindChild("Background").FindChild("Warp3").GetComponent<Button>().interactable = false;
			} else {
				this.gameObject.transform.FindChild("Background").FindChild("Warp3").GetComponent<Button>().interactable = true;
			}

			if("WarpPoint4" == warp.col.name){
				this.gameObject.transform.FindChild("Background").FindChild("Warp4").GetComponent<Button>().interactable = false;
			} else {
				this.gameObject.transform.FindChild("Background").FindChild("Warp4").GetComponent<Button>().interactable = true;
			}

			if("WarpPoint5" == warp.col.name){
				this.gameObject.transform.FindChild("Background").FindChild("Warp5").GetComponent<Button>().interactable = false;
			} else {
				this.gameObject.transform.FindChild("Background").FindChild("Warp5").GetComponent<Button>().interactable = true;
			}
		}
	}

	public void warpTo(int warpPoint){


		GameObject.Find("Player").GetComponent<Sounds>().playWorldSound(Sounds.BUTTON);
		warp.chooseDestination = false;
		Camera.main.GetComponent<HUD>().turnOffLights("warp");
		warp.nextUsage = Time.time + warp.delay;
		if (warpPoint == 6) {
			warp.generateRandomWarpPoint (Random.Range (1, 6));
		} else {
			warp.generateRandomWarpPoint(warpPoint);
		}
	}

	public void closeWarpDialog(){
		GameObject.Find ("Warp").GetComponent<Canvas> ().enabled = false;
	}
}
