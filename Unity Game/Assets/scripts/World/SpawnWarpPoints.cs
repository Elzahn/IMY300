using UnityEngine;
using System.Collections;

public class SpawnWarpPoints : MonoBehaviour {

	void Start(){
		spawnTeleports ();
	}

	//Removes any existing teleports and replaces them placing the new teleports at random positions on the sphere.
	public static void spawnTeleports ()
	{
		GameObject[] gameObjectsToDelete = GameObject.FindGameObjectsWithTag ("WarpPoint");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}

		float PlanetRadius = GameObject.Find("Planet").GetComponent<SphereCollider> ().radius;

		for (int i = 1; i < 6; i++) {
			GameObject warpPoint1 = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			warpPoint1.transform.position = Random.onUnitSphere * PlanetRadius;
			warpPoint1.name = "WarpPoint" + i;
			warpPoint1.transform.GetComponent<CapsuleCollider> ().isTrigger = true;
			warpPoint1.tag = "WarpPoint";
		}
	}

}
