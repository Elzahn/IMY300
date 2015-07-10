using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 15;
	public Vector3 moveDir;
	private PlayerAttributes playerAttributes;
	private bool jumping = false, paused, showDeath, showPaused;
	public bool run = false;

	void Start(){
		playerAttributes = GameObject.Find ("Persist").GetComponent<PlayerAttributes> ();//this.GetComponent<PlayerAttributes> ();
		paused = false;
		showDeath = false;
		showPaused = false;
	}

	// Update is called once per frame
	void Update () {
		if (paused == false) {

			if(playerAttributes.isDead() == true)
			{
				showDeath = true;
				paused = true;
			}
			
			if(Input.GetKeyDown(KeyCode.LeftShift))	{   
				moveSpeed *= 2;
				run = true;
			} else if(Input.GetKeyUp(KeyCode.LeftShift)){
				moveSpeed /= 2;
				run = false;
			}

			if(run){
				playerAttributes.drainStamina();
			}

			if(playerAttributes.getStamina() <= 0) {
				if(playerAttributes.getStamina() < 0)
					playerAttributes.setStaminaToZero();
				run = false; 
				moveSpeed = 15; 
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

			if(Input.GetKeyDown(KeyCode.P)){
				showPaused = true;
				setPaused(true);
			}

			if(Input.GetKeyDown(KeyCode.F1)){
				this.GetComponent<Warping>().chooseDestinationUnlocked = true;
				print ("Warp point destination choice unlocked.");
			}

			if(Input.GetKeyDown(KeyCode.F2)){
				FallThroughPlanet.fallThroughPlanetUnlocked = true;
				print ("Fall through plannet unlocked.");
			}

			if(Input.GetKeyDown(KeyCode.F3)){
				this.GetComponent<SaveSpotTeleport>().setEnterSaveSpot();
				print ("You killed the boss!");
			}

			if(Input.GetKeyDown(KeyCode.F4)){
				GameObject.Find("Persist").GetComponent<PlayerAttributes>().LevelMeUp();
			}

			if(Input.GetKeyDown(KeyCode.R)){
				GameObject.Find("Main Camera").GetComponent<SmoothMouseLook>().resetRotation();
			}
		} else if (Input.GetKeyDown (KeyCode.P)) {
			paused = false;
			showPaused = false;
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
				paused = false;
				showDeath = false;
				playerAttributes.restoreHealthToFull ();
				playerAttributes.restoreStaminaToFull ();
				playerAttributes.resetXP ();

				Application.LoadLevel (Application.loadedLevel);
			}
			if (GUI.Button (new Rect (350, 140, 100, 30), "Quit")) {
				Application.Quit ();
				showDeath = false;
				paused = false;
			}
		} else if (showPaused) {
			GUI.Box (new Rect (300, 40, 200, 50), "Paused \n(Press P to unpause)");
		}
	}
}
