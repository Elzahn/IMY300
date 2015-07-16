using UnityEngine;
using System.Collections;

public class SpawnHealthPack : MonoBehaviour {

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
			GameObject tempHealthPack = GameObject.CreatePrimitive (PrimitiveType.Cube);
			tempHealthPack.transform.position = Random.onUnitSphere * PlanetRadius;
			tempHealthPack.name = "HealthPack"+i;
			if(Random.Range(0,2) == 0){
				tempHealthPack.tag  = "MediumHealthPack";
			} else {
				tempHealthPack.tag = "LargeHealthPack";
			}

			tempHealthPack.AddComponent<Rigidbody>();
			tempHealthPack.GetComponent<Rigidbody>().position = Random.onUnitSphere * (mesh.bounds.size.y/2);//Random.onUnitSphere * PlanetRadius;
			tempHealthPack.AddComponent<FauxGravityBody>();
			tempHealthPack.GetComponent<FauxGravityBody>().attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();

			//tempHealthPack.transform.GetComponent<BoxCollider> ().isTrigger = true;
		}
	}
}
