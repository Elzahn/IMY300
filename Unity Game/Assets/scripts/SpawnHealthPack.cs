using UnityEngine;
using System.Collections;

public class SpawnHealthPack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		spawnHealthPacks ();
	}

	public void spawnHealthPacks ()
	{
		GameObject[] gameObjectsToDelete =  GameObject.FindGameObjectsWithTag ("HealthPack");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}
		
		GameObject planet = GameObject.Find("Planet");
		float PlanetRadius = planet.GetComponent<SphereCollider>().radius;

		for (int i = 1; i <= 10; i++) {
			GameObject tempHealthPack = GameObject.CreatePrimitive (PrimitiveType.Cube);
			tempHealthPack.transform.position = Random.onUnitSphere * PlanetRadius;
			tempHealthPack.name = "HealthPack"+i;
			if(Random.Range(0,1) == 0){
				tempHealthPack.tag = "HealthPack2";
			} else {
				tempHealthPack.tag = "HealthPack4";
			}
			tempHealthPack.transform.GetComponent<BoxCollider> ().isTrigger = true;
		}
	}
}
