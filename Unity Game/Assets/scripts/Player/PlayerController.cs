using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 15;
	public Vector3 moveDir;
	private PlayerAttributes playerAttributes;
	private bool jumping = false, paused, showDeath;

	void Start(){
		playerAttributes = GameObject.Find ("Persist").GetComponent<PlayerAttributes> ();//this.GetComponent<PlayerAttributes> ();
		paused = false;
		showDeath = false;
	}

	// Update is called once per frame
	void Update () {
		if (paused == false) {
			if(playerAttributes.isDead() == true)
			{
				showDeath = true;
				paused = true;
			}

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
		if (paused == false) {
			var rigidbody = GetComponent<Rigidbody> ();
			rigidbody.MovePosition (rigidbody.position + transform.TransformDirection (moveDir) * moveSpeed * Time.deltaTime);
		}
	}

	public bool getJumping(){
		return jumping;
	}

	public bool getPaused()
	{
		return paused;
	}
	
	public void setPaused(bool value)
	{
		paused = value;
	}

	void OnGUI(){
		if (showDeath) {
			GUI.Box (new Rect (300, 40, 200, 150), "You died! Restart the level?");
			if (GUI.Button (new Rect (350, 90, 100, 30), "Restart level")) {
				Application.LoadLevel (Application.loadedLevel);
				paused = false;
				showDeath = false;
			}
			if (GUI.Button (new Rect (350, 140, 100, 30), "Quit")) {
				Application.Quit();
				showDeath = false;
				paused = false;
			}
		}
	}
}
