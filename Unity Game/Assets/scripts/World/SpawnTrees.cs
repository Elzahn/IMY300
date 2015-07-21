using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnTrees : MonoBehaviour {
	
	public GameObject tree1;
	public GameObject tree2;
	public GameObject tree3;

//	private PlayerAttributes attributesScript;
//	private PlayerController playerScript;
	// Use this for initialization
	
	const int TREE_COUNT = 70;
	const int NORMAL_TREE_TYPES = 3;
	
	FauxGravityAttractor planet;
	
	LinkedList<GameObject> trees = new LinkedList <GameObject> ();
	
	void Start () {
	//	attributesScript = GameObject.Find ("Player").GetComponent<PlayerAttributes> ();
		
	//	playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		planet = GameObject.Find("Planet").GetComponent<FauxGravityAttractor>();
		GameObject tree;
		
		//Spawn Trees
		for (int i=0; i < TREE_COUNT; ++i) {
			int index = Random.Range(0, 3);//Mathf.RoundToInt(Random.value * NORMAL_TREE_TYPES);
			tree = chooseTree(index);
			addTree(tree);
		}	  
	}
	
	GameObject chooseTree(int i) {
		switch(i) {
		case 0 : 
			return tree1;
		case 1 : 
			return tree2;
		default :  
			return tree3;
		}
	}
	
	void addTree(GameObject tree) {	

		GameObject go = Instantiate(tree);
		go.AddComponent<FauxGravityBody>();
		go.AddComponent<Rigidbody> ();
		GameObject child;
		if (tree == tree2) {
			child = go.transform.FindChild ("Sphere001").gameObject;
		} else {
			child = go.transform.FindChild ("Cylinder001").gameObject;
		}

		child.tag = "WorldObject";
		child.AddComponent<MeshCollider> ();
		child.GetComponent<MeshCollider> ().convex = true;
		go.GetComponent<FauxGravityBody>().attractor = planet;
		go.transform.localScale = new Vector3(0.025f, 0.027f, 0.025f);
		go.tag = "WorldObject";

		trees.AddLast(go);
		
		//TODO Position correctly
		Mesh mesh = GameObject.Find ("Planet").GetComponent<MeshFilter>().mesh;
		
		go.GetComponent<Rigidbody> ().position = Random.onUnitSphere * (mesh.bounds.size.y/2); 
	}
}