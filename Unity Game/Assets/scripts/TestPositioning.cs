using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestPositioning : MonoBehaviour {
		
		public GameObject tree1;
		
		const int TREE_COUNT = 10;
		
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
		
		void addTree(GameObject tree) {	
			
			GameObject go = Instantiate(tree);
			go.AddComponent<FauxGravityBody>();
			go.AddComponent<Rigidbody> ();
		go.transform.localScale = new Vector3 (0.0025f, 0.0025f, 0.0025f);

			GameObject child = go.transform.FindChild ("Cylinder001").gameObject;

			child.tag = "WorldObject";
			child.AddComponent<MeshCollider> ();
			child.GetComponent<MeshCollider> ().convex = true;
			
			
			go.GetComponent<FauxGravityBody>().attractor = planet;
			go.tag = "WorldObject";
			
			//TODO Position correctly
			Mesh mesh = GameObject.Find ("Planet").GetComponent<MeshFilter> ().mesh;
			//go.transform.position = Random.insideUnitSphere * ((mesh.bounds.size.y/4));//bounds.size.y/8); 
			
			go.transform.GetComponent<Rigidbody> ().position = Random.onUnitSphere * ((mesh.bounds.size.y/10));//bounds.size.y/8); 
			
			trees.AddLast(go);
			
		}
}
