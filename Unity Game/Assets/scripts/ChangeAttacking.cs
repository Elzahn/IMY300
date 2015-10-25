using UnityEngine;
using System.Collections;

public class ChangeAttacking : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var animatorComponent = GameObject.Find("Character_Final").GetComponent<Animator>();
		animatorComponent.SetBool("Attacking", false);
	}
}
