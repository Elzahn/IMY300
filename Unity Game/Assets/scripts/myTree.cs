using UnityEngine;
using System.Collections;

public class myTree : MonoBehaviour {

	/*private bool posSet = false;
	private Vector3 pos;*/
	public float check;

	// Use this for initialization
	void Start () {

	}


	//check collision and set freezAll / isKinematic

	void OnCollisionEnter (Collision col){
		if (col.collider.name == "Planet") {
		//	this.GetComponent<Rigidbody>().isKinematic = true;//constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	//check in update after time if not iskinematic;
	//change unfreez to false of kinematic

	/*public void setTree( ){
		pos = this.GetComponent<Rigidbody> ().position;
		//this.GetComponent<Rigidbody> ().position = position;
		posSet = true;
		check = Time.time;
	}*/

	// Update is called once per frame
	void Update () {

		if(Time.time >= check + 1f && this.GetComponent<Rigidbody> ().constraints != RigidbodyConstraints.FreezeAll) {
			print ("destroy");
			Destroy(this.gameObject);
		}
	}
}
