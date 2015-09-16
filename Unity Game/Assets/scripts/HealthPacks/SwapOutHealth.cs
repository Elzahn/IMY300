using UnityEngine;
using System.Collections;

public class SwapOutHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Transform[] children = this.GetComponentsInChildren<Transform>();
		foreach (Transform child in children) {
			if (child.name == "Shrub") {
				child.gameObject.SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	public void swapShrub () {
		Vector3 tempPos = Vector3.zero;
		Transform[] children = this.GetComponentsInChildren<Transform>();
		foreach (Transform child in children) {
			if (child.name == "MedHealth" || child.name == "LargeHealth") {
				child.gameObject.SetActive (false);
				GameObject Shrub = this.transform.FindChild ("Shrub").gameObject;
				Shrub.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
				Shrub.GetComponent<PositionMe> ().checkMyPosition = false;
				Shrub.transform.position = child.transform.position;
				Shrub.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
				Shrub.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
				Shrub.SetActive (true);
				print (this);
			}
		}


	}
}
