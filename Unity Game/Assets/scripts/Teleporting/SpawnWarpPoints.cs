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

		Mesh mesh = GameObject.Find("Planet").GetComponent<MeshFilter>().mesh;
		//float PlanetRadius = GameObject.Find("Planet").GetComponent<SphereCollider> ().radius;

		for (int i = 1; i < 6; i++) {
			GameObject warpPoint1 = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			//warpPoint1.transform.position = Random.onUnitSphere * PlanetRadius;//(mesh.bounds.size.y/2);//
			warpPoint1.name = "WarpPoint" + i;
			warpPoint1.AddComponent<Rigidbody>();
			warpPoint1.GetComponent<Rigidbody>().position = Random.onUnitSphere * (mesh.bounds.size.y/2);//Random.onUnitSphere * PlanetRadius;
			warpPoint1.AddComponent<FauxGravityBody>();
			warpPoint1.GetComponent<FauxGravityBody>().attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
			//warpPoint1.transform.GetComponent<CapsuleCollider> ().isTrigger = true;
			warpPoint1.tag = "WarpPoint";
		//	warpPoint1.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		}
	}

}
