﻿using UnityEngine;
using System.Collections;

public class SpawnWarpPoints : MonoBehaviour {

	public GameObject warpPoint;

	void Start(){
		spawnTeleports ();
	}

	public static void spawnNewTeleports(){
		GameObject.Find("Planet").GetComponent<SpawnWarpPoints>().spawnTeleports();
	}

	//Removes any existing teleports and replaces them placing the new teleports at random positions on the sphere.
	public void spawnTeleports ()
	{
		GameObject[] gameObjectsToDelete = GameObject.FindGameObjectsWithTag ("WarpPoint");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}

		Mesh mesh = GameObject.Find("Planet").GetComponent<MeshFilter>().mesh;
		//float PlanetRadius = GameObject.Find("Planet").GetComponent<SphereCollider> ().radius;

		for (int i = 1; i < 6; i++) {
			GameObject warpPoint1 = Instantiate(warpPoint);
			//warpPoint1.transform.position = Random.onUnitSphere * PlanetRadius;//(mesh.bounds.size.y/2);//
			warpPoint1.name = "WarpPoint" + i;
			warpPoint1.AddComponent<Rigidbody>();

			Vector3 position = Random.onUnitSphere * ((mesh.bounds.size.y/4)+15);
			
			if(Physics.CheckSphere (position, 20)){
				Rigidbody rigid = warpPoint1.GetComponent<Rigidbody> () ;
				rigid.position = position;
			} 

			GameObject child = warpPoint1.transform.FindChild ("Box012").gameObject;
			child.tag = "WarpPoint";
			child.AddComponent<MeshCollider> ();
			child.GetComponent<MeshCollider> ().convex = true;

			//warpPoint1.GetComponent<Rigidbody>().position = position;//Random.onUnitSphere * (mesh.bounds.size.y/2);//Random.onUnitSphere * PlanetRadius;
			warpPoint1.AddComponent<FauxGravityBody>();
			warpPoint1.GetComponent<FauxGravityBody>().attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
			//warpPoint1.transform.GetComponent<CapsuleCollider> ().isTrigger = true;
			warpPoint1.tag = "WarpPoint";
		//	warpPoint1.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		}
	}

}
