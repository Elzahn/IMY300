using UnityEngine;
using System.Collections;

public class SetCharacterPosition : MonoBehaviour {

	private Quaternion tempRot = new Quaternion (0.0f, 0.8f, 0.0f, 0.7f);//(4.336792f, 94.88489f, 0.3787689f, 1);

	// Use this for initialization
	void Start () {
		if(GameObject.Find("Player").transform.rotation != tempRot)
		{
			GameObject.Find("Player").transform.rotation = tempRot;
		}
	
		GameObject.Find("Player").transform.position = new Vector3(-142.91f, 36.11f, -0.45f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
