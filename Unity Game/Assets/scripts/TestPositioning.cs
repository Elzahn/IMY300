using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestPositioning : MonoBehaviour {
		
		public GameObject tree1;
		public GameObject tree2;
		public GameObject tree3;
		public GameObject tree4;
		
		const int TREE_COUNT = 400;//300 max
		
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
		GameObject.Find(go.transform.parent.gameObject.name).GetComponent<myTree>().checkMe = true;
		GameObject.Find (go.transform.parent.gameObject.name).GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		//int tries = 12;
		Mesh mesh = GameObject.Find ("Planet").GetComponent<MeshFilter> ().mesh;
		Vector3 position;
		bool planted = false;

		while (!planted) {
			//tries--;

			position = Random.onUnitSphere * (GameObject.Find("Planet").GetComponent<SphereCollider>().radius/2);

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
				GameObject.Find(go.transform.parent.gameObject.name).GetComponent<myTree>().check = Time.time;
				GameObject.Find(go.transform.parent.gameObject.name).GetComponent<myTree>().checkMe = false;
				//GameObject.Find(go.transform.parent.gameObject.name).GetComponent<myTree>().pos = position;
				return;
			}
		}

	//	Destroy(go.transform.parent.transform.gameObject);
	}

	void addTree(GameObject tree) {	
			
		GameObject go = Instantiate(tree);
		/*go.AddComponent<FauxGravityBody>();
		go.AddComponent<Rigidbody> ();*/

		//go.AddComponent<myTree> ();

		GameObject child;
		if (tree == tree1 || tree == tree3) {
			child = go.transform.FindChild ("Cylinder001").gameObject;
			go.transform.localScale = new Vector3 (0.025f, 0.025f, 0.025f);	//size influences attraction to planet too small floates
		} else if (tree == tree2) {
			child = go.transform.FindChild ("Sphere001").gameObject;
			go.transform.localScale = new Vector3 (0.025f, 0.025f, 0.025f);	//size influences attraction to planet too small floates
		} else {
			child = go.transform.FindChild ("Box012").gameObject;
			go.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);	//size influences attraction to planet too small floates
		}
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
