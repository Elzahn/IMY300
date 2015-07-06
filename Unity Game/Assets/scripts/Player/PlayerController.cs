using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 15;
	public Vector3 moveDir;
	private Warping warpingScript;

	void Start(){
		warpingScript = this.GetComponent<Warping> ();
	}

	// Update is called once per frame
	void Update () {
		if (warpingScript.getPaused () == false) {
			moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Vertical")).normalized;
		}
	}

	void FixedUpdate() {
		if (warpingScript.getPaused () == false) {
			var rigidbody = GetComponent<Rigidbody> ();
			rigidbody.MovePosition (rigidbody.position + transform.TransformDirection (moveDir) * moveSpeed * Time.deltaTime);
		}
	}
}
