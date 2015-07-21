using UnityEngine;
using System.Collections;

public class SpawnHealthPack : MonoBehaviour {

	public GameObject medHealth;
	public GameObject largeHealth;

	// Use this for initialization
	void Start () {
		spawnHealthPacks ();
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

		for (int i = 1; i <= 10; i++) {

			GameObject tempHealthPack;

			if(Random.Range(0,2) == 0){
				tempHealthPack = Instantiate(medHealth);
				tempHealthPack.tag  = "MediumHealthPack";
			} else {
				tempHealthPack = Instantiate(largeHealth);
				tempHealthPack.tag = "LargeHealthPack";
			}

			GameObject child = tempHealthPack.transform.FindChild ("Box012").gameObject;

			if(tempHealthPack.tag == "MediumHealthPack"){
				child.tag = "MediumHealthPack";
			} else {
				child.tag = "LargeHealthPack";
			}

			child.AddComponent<MeshCollider> ();
			child.GetComponent<MeshCollider> ().convex = true;

			tempHealthPack.transform.position = Random.onUnitSphere * PlanetRadius;
			tempHealthPack.name = "HealthPack"+i;

			tempHealthPack.AddComponent<Rigidbody>();
			Vector3 position = Random.onUnitSphere * ((mesh.bounds.size.y/4) +10);

			if(Physics.CheckSphere (position, 20)){
				Rigidbody rigid = tempHealthPack.GetComponent<Rigidbody> () ;
				rigid.position = position;
			} 

			//tempHealthPack.GetComponent<Rigidbody>().position = position;//Random.onUnitSphere * PlanetRadius;
			tempHealthPack.AddComponent<FauxGravityBody>();
			tempHealthPack.GetComponent<FauxGravityBody>().attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();

			//tempHealthPack.transform.GetComponent<BoxCollider> ().isTrigger = true;
		}
	}
}
