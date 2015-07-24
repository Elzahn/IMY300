using UnityEngine;
using System.Collections;

public class PositionMe : MonoBehaviour {

	public float timeToCheckMyPosition;	//gives two seconds for trees to be attracted to the planet
	public bool checkMyPosition = true;	//variable set to tell when tree's position has been set
	public bool touching = false; //is the monster touching the sphere

	//Keeps Trees out of the start position
	void OnTriggerEnter(Collider col){
		if (this.tag == "WorldObject") {
			if (col.name == "EntrancePlane" && checkMyPosition == true) {
				//Finds chid with the worldObject tag
				GameObject child = null;
				foreach (Transform t in transform) {
					if (t.gameObject.tag == "WorldObject") {
						child = t.gameObject;
						break;
					}
				}
				timeToCheckMyPosition = Time.time;
				GameObject.Find ("Planet").GetComponent<SpawnTrees> ().position (child);
			}
		} else if (this.tag == "Monster") {
			timeToCheckMyPosition = Time.time;
			GameObject.Find ("Planet").GetComponent<EnemySpawner> ().position (this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		//Repositions trees that aren't touching the planet after 2 seconds
		if (this.tag == "WorldObject") {
			if (Time.time >= timeToCheckMyPosition + 2f && checkMyPosition == true && this.GetComponent<Rigidbody> ().constraints != RigidbodyConstraints.FreezeAll) {

				timeToCheckMyPosition = Time.time;

				GameObject child = null;
				foreach (Transform t in transform) {
					if (t.gameObject.tag == "WorldObject") {
						child = t.gameObject;
						break;
					}
				}

				GameObject.Find ("Planet").GetComponent<SpawnTrees> ().position (child);
			}
		} else if (this.tag == "Monster") {
			if ((Time.time >= timeToCheckMyPosition + 2f && checkMyPosition == true && touching == false) || (checkMyPosition == false && touching == false)) {
				
				timeToCheckMyPosition = Time.time;
				
				GameObject.Find ("Planet").GetComponent<EnemySpawner> ().position (this.gameObject);
			} 
		}
	}
}
