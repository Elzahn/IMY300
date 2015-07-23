using UnityEngine;
using System.Collections;

public class myTree : MonoBehaviour {

	public float check;

	// Update is called once per frame
	void Update () {

		//Destroys trees not touching the planet
		if(Time.time >= check + 2f && this.GetComponent<Rigidbody> ().constraints != RigidbodyConstraints.FreezeAll) {
			Destroy(this.gameObject);
		}
	}
}
