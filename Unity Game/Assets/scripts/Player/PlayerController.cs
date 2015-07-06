using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed ;
	public Vector3 moveDir;

	private Warping warpingScript;
	
	public Vector3 jump;

	public const float RUN_MULT = 2f;

	void Start(){
		warpingScript = this.GetComponent<Warping> ();
	}

	// Update is called once per frame
	void Update () {
		if (warpingScript.getPaused () == false) {
			moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Vertical")).normalized;
	
			var player = GetComponent<PlayerAttributes>();
			/** SHould actually be when level is crated **/
			player.setAttributes();
			moveSpeed = player.speed;

			if (Input.GetAxis("Run") > 0 && player.stamina > 0) {
				moveSpeed *= RUN_MULT;
			}
			moveDir = new Vector3 (Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized;
		}
	}

	void FixedUpdate() {
		if (warpingScript.getPaused () == false) {
			var rigidbody = GetComponent<Rigidbody> ();
			rigidbody.MovePosition (rigidbody.position + transform.TransformDirection (moveDir) * moveSpeed * Time.deltaTime);
		}
	}
}
