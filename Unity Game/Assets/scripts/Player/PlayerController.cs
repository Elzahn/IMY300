using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 15;
	public Vector3 moveDir;
	private Warping warpingScript;
	private PlayerAttributes playerAttributes;
	private bool jumping = false;

	void Start(){
		warpingScript = this.GetComponent<Warping> ();
		playerAttributes = this.GetComponent<PlayerAttributes> ();
	}

	// Update is called once per frame
	void Update () {
		if (warpingScript.getPaused () == false) {
			if(playerAttributes.getDizzy() == true){
				moveDir = new Vector3 (Input.GetAxisRaw ("Vertical"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Horizontal")).normalized;
			} else {
				moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Vertical")).normalized;
			}

			if(Input.GetAxisRaw("Jump") == 1){
				jumping = true;
			} else if(Input.GetAxisRaw("Jump") == 0){
					jumping = false;
				}
		}
	}

	void FixedUpdate() {
		if (warpingScript.getPaused () == false) {
			var rigidbody = GetComponent<Rigidbody> ();
			rigidbody.MovePosition (rigidbody.position + transform.TransformDirection (moveDir) * moveSpeed * Time.deltaTime);
		}
	}

	public bool getJumping(){
		return jumping;
	}
}
