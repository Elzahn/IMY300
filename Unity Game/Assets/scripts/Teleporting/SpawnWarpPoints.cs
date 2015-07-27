using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnWarpPoints : MonoBehaviour {

	public GameObject warpPoint;

	const int TOTAL_WARPS = 5;

	LinkedList<GameObject> warpPoints = new LinkedList <GameObject> ();

	void Start(){
		spawnTeleports ();
	}

	public static void spawnNewTeleports(){
		GameObject.Find("Planet").GetComponent<SpawnWarpPoints>().spawnTeleports();
	}

	void Update(){
		bool done = true;
		foreach (GameObject warpPoint in warpPoints) {
			if(done == true && warpPoint != null &&warpPoint.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll){
				done = true;
			} else {
				done = false;
			}
		}
		if (done ) {
			print ("WarpPoints created");
		}
	}

	public void position(GameObject go){
		GameObject.Find(go.transform.parent.gameObject.name).GetComponent<PositionMe>().checkMyPosition = false;
		Vector3 position;
		bool created = false;
		
		while (!created) {
			
			position = Random.onUnitSphere * (GameObject.Find("Planet").GetComponent<SphereCollider>().radius * GameObject.Find("Planet").transform.lossyScale.x);
			
			Collider[] collidedItems = Physics.OverlapSphere(position, 0.5f);
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

			GameObject child = tempWarpPoint.transform.FindChild ("Box012").gameObject;

			position(child);

			tempWarpPoint.GetComponent<FauxGravityBody>().attractor = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();

			warpPoints.AddLast(tempWarpPoint);
		}
	}

}
