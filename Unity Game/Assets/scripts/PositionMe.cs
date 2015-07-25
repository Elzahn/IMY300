using UnityEngine;
using System.Collections;

public class PositionMe : MonoBehaviour {

	//couldn't get it to work if vars aren't declared like this
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
			if (col.name == "EntrancePlane" && checkMyPosition == true) {
				timeToCheckMyPosition = Time.time;
				GameObject.Find ("Planet").GetComponent<EnemySpawner> ().position (this.gameObject);
			}
		} else if (this.tag == "MediumHealthPack" || this.tag == "LargeHealthPack") {
			if (col.name == "EntrancePlane" && checkMyPosition == true) {
				timeToCheckMyPosition = Time.time;

				//Finds chid with the MediumHealthPack or LargeHealthPack tag
				GameObject child = null;
				foreach (Transform t in transform) {
					if (t.gameObject.tag == "MediumHealthPack" || t.gameObject.tag == "LargeHealthPack") {
						child = t.gameObject;
						break;
					}
				}
				GameObject.Find ("Planet").GetComponent<SpawnHealthPacks> ().position (child);
			}
		} else if (this.tag == "WarpPoint") {
			if (col.name == "EntrancePlane" && checkMyPosition == true) {
				timeToCheckMyPosition = Time.time;
				
				//Finds chid with the WarpPoint tag
				GameObject child = null;
				foreach (Transform t in transform) {
					if (t.gameObject.tag == "WarpPoint") {
						child = t.gameObject;
						break;
					}
				}
				GameObject.Find ("Planet").GetComponent<SpawnWarpPoints> ().position (child);
			}
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
		}//Repositions monsters that aren't touching the planet after 2 seconds
		else if (this.tag == "Monster") {
			if ((Time.time >= timeToCheckMyPosition + 2f && checkMyPosition == true && touching == false) || (checkMyPosition == false && touching == false)) {
				
				timeToCheckMyPosition = Time.time;
				
				GameObject.Find ("Planet").GetComponent<EnemySpawner> ().position (this.gameObject);
			} 
		} //Repositions healthpacks that aren't touching the planet after 2 seconds
		else if (this.tag == "MediumHealthPack" || this.tag == "LargeHealthPack") {
			if (Time.time >= timeToCheckMyPosition + 2f && checkMyPosition == true && this.GetComponent<Rigidbody> ().constraints != RigidbodyConstraints.FreezeAll) {
				
				timeToCheckMyPosition = Time.time;
				
				GameObject child = null;
				foreach (Transform t in transform) {
					if (t.gameObject.tag == "MediumHealthPack" || t.gameObject.tag == "LargeHealthPack") {
						child = t.gameObject;
						break;
					}
				}
				
				GameObject.Find ("Planet").GetComponent<SpawnHealthPacks> ().position (child);
			}
		} //Repositions warpPoints that aren't touching the planet after 2 seconds
		else if (this.tag == "WarpPoint") {	
			if (Time.time >= timeToCheckMyPosition + 2f && checkMyPosition == true && this.GetComponent<Rigidbody> ().constraints != RigidbodyConstraints.FreezeAll) {
				
				timeToCheckMyPosition = Time.time;
				
				GameObject child = null;
				foreach (Transform t in transform) {
					if (t.gameObject.tag == "WarpPoint") {
						child = t.gameObject;
						break;
					}
				}
				
				GameObject.Find ("Planet").GetComponent<SpawnWarpPoints> ().position (child);
			}
		}
	}
}
