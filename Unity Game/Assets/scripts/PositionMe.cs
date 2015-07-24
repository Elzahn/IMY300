using UnityEngine;
using System.Collections;

public class PositionMe : MonoBehaviour {

	public float timeToCheckTreePosition;	//gives two seconds for trees to be attracted to the planet
	public bool checkMyPosition = true;	//variable set to tell when tree's position has been set

	//Keeps Trees out of the start position
	void OnTriggerEnter(Collider col){
		if (col.name == "EntrancePlane" && checkMyPosition == true) {
			TestPositioning planet = GameObject.Find("Planet").GetComponent<TestPositioning>();

			//Finds chid with the worldObject tag
			GameObject child = null;
			foreach(Transform t in transform)
			{
				if(t.gameObject.tag == "WorldObject"){
					child = t.gameObject;
					break;
				}
			}
			timeToCheckTreePosition = Time.time;
			planet.position(child);
		}
	}

	// Update is called once per frame
	void Update () {
		//Repositions trees that aren't touching the planet after 2 seconds
		if (Time.time >= timeToCheckTreePosition + 2f && checkMyPosition == true && this.GetComponent<Rigidbody> ().constraints != RigidbodyConstraints.FreezeAll) {
			SpawnTrees planet = GameObject.Find ("Planet").GetComponent<SpawnTrees> ();
			GameObject child = null;
			foreach (Transform t in transform) {
				if (t.gameObject.tag == "WorldObject") {
					child = t.gameObject;
					break;
				}
			}
			timeToCheckTreePosition = Time.time;
			planet.position (child);
		}
	}
}
