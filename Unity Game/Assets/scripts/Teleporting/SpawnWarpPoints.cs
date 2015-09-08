using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnWarpPoints : MonoBehaviour {

	public GameObject warpPoint;

	public const int TOTAL_WARPS = 5;

	LinkedList<GameObject> warpPoints = new LinkedList <GameObject> ();

	void Start(){
		spawnTeleports ();
	}

	public static void spawnNewTeleports(){
		GameObject.Find("Planet").GetComponent<SpawnWarpPoints>().spawnTeleports();
	}

	public int amountWarpsPlaced(){
		int done = 0;
		for (int i = 0; i < 5; i++) {
			done = 0;
			foreach (GameObject warp in warpPoints) {
				if (warp != null && warp.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll) {
					done++;
				}
			}
		}
		
		return done;
	}

	public bool wasPlaced(){
	/*	bool done = true;
		foreach (GameObject warpPoint in warpPoints) {
			if(done == true && warpPoint != null && warpPoint.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll){
				done = true;
			} else {
				done = false;
			}
		}
		if (done ) {
			print ("WarpPoints created");
		}*/

		bool done = true;
		
		for (int i = 0; i < 3; i++) {
			if (amountWarpsPlaced () == TOTAL_WARPS && done == true) {
				done = true;
			} else {
				done = false;
			}
		}

		return done;
	}

	public void position(GameObject go){
		//go.GetComponentInChildren<PositionMe>().checkMyPosition = false;
		Vector3 position;
		bool created = false;
		
		while (!created) {
			
			position = Random.onUnitSphere * (GameObject.Find("Planet").GetComponent<SphereCollider>().radius * GameObject.Find("Planet").transform.lossyScale.x);
			
			Collider[] collidedItems = Physics.OverlapSphere(position, 1.5f);
			List<Collider> tempList = new List<Collider>();
			
			foreach(Collider col in collidedItems){
				if(col.name != "Planet" && col.transform != go.transform){
					tempList.Add(col);
				}
			}
			
			if(tempList.Count() == 0){
				go.transform.GetComponentInParent<Rigidbody> ().position = position;
				go.GetComponentInChildren<PositionMe>().timeToCheckMyPosition = Time.time;
			//	go.GetComponentInChildren<PositionMe>().checkMyPosition = true;
				return;
			}
		}
	}

	//Removes any existing teleports and replaces them placing the new teleports at random positions on the sphere.
	public void spawnTeleports ()
	{
		GameObject[] gameObjectsToDelete = GameObject.FindGameObjectsWithTag ("WarpPoint");
		
		for (int i = 0; i < gameObjectsToDelete.Length; i++) {
			Destroy (gameObjectsToDelete [i]);
		}

		for (int i = 1; i <= TOTAL_WARPS; i++) {

			GameObject tempWarpPoint = Instantiate(warpPoint);
			tempWarpPoint.name = "WarpPoint" + i;

			position(tempWarpPoint);

			tempWarpPoint.GetComponent<FauxGravityBody>().attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();

			warpPoints.AddLast(tempWarpPoint);
		}
	}

}
