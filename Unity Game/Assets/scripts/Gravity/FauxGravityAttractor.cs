﻿using UnityEngine;
using System.Collections;

public class FauxGravityAttractor : MonoBehaviour {

	public float gravity = -10000;

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
			if (col.collider.name == "Sphere001" || col.collider.name == "Cylinder001" || col.collider.name == "Box012" || col.collider.name == "Plant_Pot") {
				temp = col.collider.transform.parent.gameObject;
			} else {
				temp = col.collider.gameObject;
			}

			temp.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
		}/* else if (col.collider.tag == "Monster" && col.collider.name != "Ape_Body" && col.collider.name != "MossAlien" && col.collider.name != "body" && col.collider.name != "MonsterBody") {
			col.collider.GetComponent<PositionMe> ().touching = false;
		}*/ else if (col.collider.tag == "Monster") {

			col.collider.GetComponentInParent<PositionMe> ().touching = false;
			col.collider.GetComponentInParent<PositionMe> ().checkMyPosition = true;
		} else if (col.collider.tag == "MediumHealthPack" || col.collider.tag == "LargeHealthPack") {
			if(col.collider.name == "Box012"){
				col.collider.transform.parent.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
			} else {
				col.collider.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
			}
		} else if (col.collider.tag == "WarpPoint") {//seperate since warppoint hierarchy might change with remodel
			col.collider.transform.parent.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Loot") {
			col.transform.parent.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.tag == "Loot") {
			col.transform.parent.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
		}
	}

	//Ensure each object that touches the sphere and has a mesh attached can't be moved
	void OnCollisionEnter (Collision col){
		if (col.collider.tag == "WorldObject") {
			GameObject temp;
			if (col.collider.name == "Sphere001" || col.collider.name == "Cylinder001" || col.collider.name == "Box012" || col.collider.name == "Plant_Pot") {
				temp = col.collider.transform.parent.gameObject;
			} else {
				temp = col.collider.gameObject;
			}

			temp.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		}/* else if (col.collider.tag == "Monster" && col.collider.name != "Ape_Body" && col.collider.name != "MossAlien" && col.collider.name != "body" && col.collider.name != "MonsterBody") {
			col.collider.GetComponent<PositionMe> ().touching = true;
		} */else if (col.collider.tag == "Monster") {
			col.collider.GetComponentInParent<PositionMe> ().touching = true;
			col.collider.GetComponentInParent<PositionMe> ().checkMyPosition = true;
		} else if (col.collider.tag == "MediumHealthPack" || col.collider.tag == "LargeHealthPack") {
			if (col.collider.name == "Box012") {
				col.collider.transform.parent.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			} else {
				col.collider.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			}
		} else if (col.collider.tag == "WarpPoint") {	//seperate since warppoint hierarchy might change with remodel
			col.collider.transform.parent.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		} /*else if (col.collider.tag == "Loot") {
			col.collider.transform.parent.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		}*/
	}
}