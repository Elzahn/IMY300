using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 15;
	public Vector3 moveDir;
	
	// Update is called once per frame
	void Update () {
		moveDir = new Vector3 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Jump") , Input.GetAxisRaw("Vertical")).normalized;
	}

	void FixedUpdate() {
		var rigidbody = GetComponent<Rigidbody> ();
		rigidbody.MovePosition (rigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
	}
}
