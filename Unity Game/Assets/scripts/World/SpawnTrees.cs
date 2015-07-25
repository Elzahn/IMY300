using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnTrees : MonoBehaviour {
	
	public GameObject tree1;
	public GameObject tree2;
	public GameObject tree3;

	public GameObject shrub;

	const int TREE_COUNT = 300;
	
	FauxGravityAttractor planet;
	
	LinkedList<GameObject> trees = new LinkedList <GameObject> ();

	void Start () {
		planet = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
		GameObject tree;
		
		//Spawn Trees
		for (int i=0; i < TREE_COUNT; ++i) {
			int index = Random.Range(0, 4);
			tree = chooseTree(index);
			addTree(tree);
		}	  
	}

	public bool isTreesPlanted(){
		bool done = true;
		foreach (GameObject tree in trees) {
			if(done == true && tree.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll){
				done = true;
			} else {
				done = false;
			}
		}

		if (done ) {
			print ("Trees planted");
		}

		return done;

	}

	void Update(){
		isTreesPlanted ();
	}

	GameObject chooseTree(int i) {
		switch(i) {
		case 0 : 
			return tree1;
		case 1 : 
			return tree2;
		case 2:
			return tree3;
		default :  
			return shrub;
		}
	}

	public void position(GameObject go){
		GameObject.Find(go.transform.parent.gameObject.name).GetComponent<PositionMe>().checkMyPosition = false;
		Vector3 position;
		bool planted = false;
		
		while (!planted) {
			
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

	void addTree(GameObject tree) {	
		
		GameObject go = Instantiate(tree);
		
		//Finds chid with the worldObject tag
		GameObject child = null;
		foreach(Transform t in go.transform)
		{
			if(t.gameObject.tag == "WorldObject"){
				child = t.gameObject;
				break;
			}
		}
		go.GetComponent<FauxGravityBody>().attractor = planet;
		
		position (child);
		trees.AddLast(go);
	}
}