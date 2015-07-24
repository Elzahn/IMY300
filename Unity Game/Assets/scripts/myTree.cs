using UnityEngine;
using System.Collections;

public class myTree : MonoBehaviour {

	public float check;	//gives two seconds for trees to be attracted to the planet
	public bool checkMe = true;	//variable set to tell when tree's position has been set

	//Keeps Trees out of the start position
	void OnTriggerEnter(Collider col){
		if (col.name == "EntrancePlane" && checkMe == true) {
			TestPositioning planet = GameObject.Find("Planet").GetComponent<TestPositioning>();
			GameObject child = null;
			foreach(Transform t in transform)
			{
				if(t.gameObject.tag == "WorldObject"){
					child = t.gameObject;
					break;
				}
			}
			check = Time.time;
			planet.position(child);
		}
	}

	// Update is called once per frame
	void Update () {

		//Repositions trees not touching the planet
		if(Time.time >= check + 2f && checkMe == true && this.GetComponent<Rigidbody> ().constraints != RigidbodyConstraints.FreezeAll) {
			TestPositioning planet = GameObject.Find("Planet").GetComponent<TestPositioning>();
			GameObject child = null;
			foreach(Transform t in transform)
			{
				if(t.gameObject.tag == "WorldObject"){
					child = t.gameObject;
					break;
				}
			}
			check = Time.time;
			planet.position(child);
		}
	}
}
