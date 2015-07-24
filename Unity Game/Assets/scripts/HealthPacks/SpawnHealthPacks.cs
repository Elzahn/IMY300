using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnHealthPacks : MonoBehaviour {

	public GameObject medHealth;
	public GameObject largeHealth;

	const int TOTAL_HEALTH = 10;

	LinkedList<GameObject> healthPacks = new LinkedList <GameObject> ();

	// Use this for initialization
	void Start () {
		spawnHealthPacks ();
	}

	void Update(){
		bool done = true;
		foreach (GameObject healthPack in healthPacks) {
			if(done == true && healthPack.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll){
				done = true;
			} else {
				done = false;
			}
		}
		if (done ) {
			print ("Healthpacks placed");
		}
	}


	public void position(GameObject go){
		GameObject.Find(go.transform.parent.gameObject.name).GetComponent<PositionMe>().checkMyPosition = false;
		Vector3 position;
		bool placed = false;
		
		while (!placed) {
			
			position = Random.onUnitSphere * (GameObject.Find("Planet").GetComponent<SphereCollider>().radius * GameObject.Find("Planet").transform.lossyScale.x);
			
			Collider[] collidedItems = Physics.OverlapSphere(position, 1.5f);
			List<Collider> tempList = new List<Collider>();
			
			foreach(Collider col in collidedItems){
				if(col.name != "Planet" && col.transform != go.transform){
					tempList.Add(col);
				}
			}
			
			if(tempList.Count() == 0){
				go.transform.parent.gameObject.transform.GetComponent<Rigidbody> ().position = position;
				GameObject.Find(go.transform.parent.gameObject.name).GetComponent<PositionMe>().timeToCheckMyPosition = Time.time;
				GameObject.Find(go.transform.parent.gameObject.name).GetComponent<PositionMe>().checkMyPosition = true;
				return;
			}
		}
	}

	public void spawnHealthPacks ()
	{
		GameObject[] gameObjectsToDelete =  GameObject.FindGameObjectsWithTag ("MediumHealthPack");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}

		gameObjectsToDelete =  GameObject.FindGameObjectsWithTag ("LargeHealthPack");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}

		GameObject planet = GameObject.Find("Planet");
		float PlanetRadius = planet.GetComponent<SphereCollider>().radius;
		Mesh mesh = GameObject.Find("Planet").GetComponent<MeshFilter>().mesh;

		for (int i = 0; i < TOTAL_HEALTH; i++) {

			GameObject tempHealthPack;

			if(Random.Range(0,2) == 0){
				tempHealthPack = Instantiate(medHealth);
			} else {
				tempHealthPack = Instantiate(largeHealth);
			}

			position(tempHealthPack.transform.FindChild ("Box012").gameObject);//send child through

			tempHealthPack.GetComponent<FauxGravityBody>().attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();

			healthPacks.AddLast(tempHealthPack);
		}
	}
}
