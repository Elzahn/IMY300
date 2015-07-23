using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestPositioning : MonoBehaviour {
		
		public GameObject tree1;
		
		const int TREE_COUNT = 100;
		
		FauxGravityAttractor planet;
		
		LinkedList<GameObject> trees = new LinkedList <GameObject> ();
		
		void Start () {
			planet = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
			GameObject tree;
			
			//Spawn Trees
			for (int i=0; i < TREE_COUNT; ++i) {
				tree = chooseTree(0);
				addTree(tree);
			}	  
		}

		GameObject chooseTree(int i) {
			switch(i) {
			default : 
				return tree1;
			}
		}

	void position(GameObject go){
		int tries = 6;
		Mesh mesh = GameObject.Find ("Planet").GetComponent<MeshFilter> ().mesh;
		Vector3 position;

		while (tries > 0) {
			tries--;
			position = Random.onUnitSphere * ((GameObject.Find("Planet").transform.lossyScale.x/2));
			print(position + " " + mesh.bounds.size.y);
			Collider[] collidedItems = Physics.OverlapSphere(position, 0.05f);
			List<Collider> tempList = new List<Collider>();

			foreach(Collider col in collidedItems){
				if(col.name != "Planet" && col.transform != go.transform){
					tempList.Add(col);
				}
			}

			if(tempList.Count() == 0){
				go.transform.parent.gameObject.transform.GetComponent<Rigidbody> ().position = position;
				GameObject.Find(go.transform.parent.gameObject.name).GetComponent<myTree>().check = Time.time;

				//GameObject.Find(go.transform.parent.gameObject.name).GetComponent<myTree>().pos = position;
				return;
			}
		}

		Destroy(go.transform.parent.transform.gameObject);
	}

	void addTree(GameObject tree) {	
			
		GameObject go = Instantiate(tree);
		/*go.AddComponent<FauxGravityBody>();
		go.AddComponent<Rigidbody> ();*/
		go.transform.localScale = new Vector3 (0.025f, 0.025f, 0.025f);	//size influences attraction to planet too small floates
		//go.AddComponent<myTree> ();

		GameObject child = go.transform.FindChild ("Cylinder001").gameObject;

		/*child.tag = "WorldObject";
		child.AddComponent<MeshCollider> ();
		child.GetComponent<MeshCollider> ().convex = true;
			*/
		go.GetComponent<FauxGravityBody>().attractor = planet;
		//go.tag = "WorldObject";
		/*Mesh mesh = GameObject.Find ("Planet").GetComponent<MeshFilter> ().mesh;
		go.transform.GetComponent<Rigidbody> ().position = Random.onUnitSphere * Random.value * ((mesh.bounds.size.y));
		*/
		position (child);
		trees.AddLast(go);
	}
}
