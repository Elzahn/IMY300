using UnityEngine;
using System.Collections;

public class StopMovement : MonoBehaviour {

	void OnCollisionEnter (Collision col){
		if (col.collider.tag == "WarpPoint") {
			col.collider.GetComponent<Rigidbody>().isKinematic = true;
		}
	}
}
