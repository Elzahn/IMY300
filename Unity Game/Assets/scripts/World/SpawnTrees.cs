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

	public void replaceHealth(Vector3 pos){
		print (pos);
		GameObject tempShrub = Instantiate(shrub);
		tempShrub.name = "TempShrub";
		tempShrub.transform.position = pos;
		tempShrub.GetComponent<FauxGravityBody>().attractor = planet;
		trees.AddLast(tempShrub); 
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

		/*if (done ) {
			print ("Trees planted");
		}*/

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
			
			Collider[] collidedItems = Physics.OverlapSphere(position, 0.5f);
			List<Collider> tempList = new List<Collider>();
			
			foreach(Collider col in collidedItems){
				if(col.name != "Planet" && col.transform != go.transform){
					tempList.Add(col);
				}
			}
			
			if(tempList.Count() == 0){
				go.transform.parent.gameObject.transform.GetComponent<Rigidbody> ().position = position;
				go.transform.parent.gameObject.gameObject.GetComponent<PositionMe>().timeToCheckMyPosition = Time.time;
				go.transform.parent.gameObject.gameObject.GetComponent<PositionMe>().checkMyPosition = true;
				return;
			}
		}
	}

	public void addTree(GameObject tree) {	

		GameObject go = Instantiate(tree);
		float size = 1f;
		if (tree != shrub) {
			size = Random.value;
			if(size < 0.25f){
				size = 0.25f;
			}
			int multiplyFactor = Random.Range(1, 3);
			size *= multiplyFactor;
		}

		//resize plants
		go.transform.localScale = new Vector3 ((size * tree.transform.localScale.x), (size * tree.transform.localScale.y), (size * tree.transform.localScale.z));

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