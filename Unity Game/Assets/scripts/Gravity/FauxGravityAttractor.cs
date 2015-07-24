using UnityEngine;
using System.Collections;

public class FauxGravityAttractor : MonoBehaviour {

	public float gravity = -10;

	public void attract(Transform body) {
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 bodyUp = body.up;

		body.GetComponent<Rigidbody> ().AddForce (gravityUp * gravity);

		Quaternion targetRotation = Quaternion.FromToRotation (bodyUp, gravityUp) * body.rotation;

		if (body.GetComponent<FauxGravityBody>().getRotateMe() == true){// tag != "WorldObject") {	//So that it allows for the WorldObjects to fall during an earthquake. Can use Tag for Monsters
			body.rotation = Quaternion.Slerp (body.rotation, targetRotation, 50 * Time.deltaTime);
		}
	}

	//Ensure each object that doesn't touches the sphere anymore can be moved again
	void OnCollisionExit (Collision col){
		if (col.collider.tag == "WorldObject") {
			GameObject temp;
			if(col.collider.name == "Sphere001" || col.collider.name == "Cylinder001" || col.collider.name == "Box012"){
				temp = col.collider.transform.parent.gameObject;
			} else {
				temp = col.collider.gameObject;
			}

			temp.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
		}
	}

	//Ensure each object that touches the sphere and has a mesh attached can't be moved
	void OnCollisionEnter (Collision col){
		if (col.collider.tag == "WorldObject") {
			GameObject temp;
			if(col.collider.name == "Sphere001" || col.collider.name == "Cylinder001" || col.collider.name == "Box012"){
				temp = col.collider.transform.parent.gameObject;
			} else {
				temp = col.collider.gameObject;
			}

			temp.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		}
	}
}