using UnityEngine;
using System.Collections;

public class FauxGravityAttractor : MonoBehaviour {

	public float gravity = -10;

	public void attract(Transform body) {
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 bodyUp = body.up;

		body.GetComponent<Rigidbody> ().AddForce (gravityUp * gravity);

		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp) * body.rotation;
		if (body.name == "Player") {	//So that it allows for the WorldObjects to fall during an earthquake. Can use Tag for Monsters
			body.rotation = Quaternion.Slerp (body.rotation, targetRotation, 50 * Time.deltaTime);
		}
	}

	//Ensure each object touches sphere if it has the mesh attached.
	void OnCollisionEnter (Collision col){
		if (col.collider.tag == "WorldObject") {
			col.collider.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		}
	}
}