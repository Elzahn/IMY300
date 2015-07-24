using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestPositioning : MonoBehaviour {
		
		public GameObject tree1;
		public GameObject tree2;
		public GameObject tree3;
		public GameObject tree4;

		const int TREE_COUNT = 300;
		
		FauxGravityAttractor planet;
		
		LinkedList<GameObject> trees = new LinkedList <GameObject> ();
		
	//just to see when all trees have been placed
	void Update(){
		bool done = true;
		foreach (GameObject tree in trees) {
			if(done == true && tree.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll){
				done = true;
			} else {
				done = false;
			}
		}
		if (done ) {
			print ("done");
		}
	}

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

		GameObject chooseTree(int i) {
			switch(i) {
			case 0 : 
				return tree1;
		case 1:
			return tree2;
		case 2: 
			return tree3;
		default:
			return tree4;
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
				planted = true;
				go.transform.parent.gameObject.transform.GetComponent<Rigidbody> ().position = position;
				GameObject.Find(go.transform.parent.gameObject.name).GetComponent<PositionMe>().timeToCheckTreePosition = Time.time;
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
