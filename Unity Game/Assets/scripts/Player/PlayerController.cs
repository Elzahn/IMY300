using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 10f;
	private Vector3 moveDir;
	private PlayerAttributes playerAttributes;
	private bool jumping = false, paused, showDeath, showPaused, soundPlays = false;
	public bool run = false, moving;
	private Sounds sound;
	private float check;

	public void setJumping(){
		jumping = false;
	}

	void Start(){
		playerAttributes = this.GetComponent<PlayerAttributes> ();
		paused = false;
		showDeath = false;
		showPaused = false;
		moving = false;
		sound = this.GetComponent<Sounds> ();
		check = Time.time;
	}
 
	// Update is called once per frame
	void Update () {
		if (paused == false) {

			if (playerAttributes.isDead () == true) {
				showDeath = true;
				paused = true;
			}

			if(Sounds.characterAudio.isPlaying == false)
			{
				soundPlays = false;
			}

			if (Input.GetKeyDown (KeyCode.LeftShift)) {   
				moveSpeed = 20;
				run = true;
				soundPlays = false;
			} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
				moveSpeed = 10;
				run = false;
				soundPlays = false;
			}

			if (playerAttributes.getStamina () <= 0) {
				if (playerAttributes.getStamina () < 0)
					playerAttributes.setStaminaToZero ();
				run = false; 
				moveSpeed = 10; 
			}

			if (playerAttributes.getDizzy () == true) {
				moveDir = new Vector3 (Input.GetAxisRaw ("Vertical"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Horizontal")).normalized;
			} else {
				moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Vertical")).normalized;
			}

			if (Input.GetAxisRaw ("Jump") == 1) {
				jumping = true;
			}/* else if (Input.GetAxisRaw ("Jump") == 0) {
				jumping = false;
			}*/

			if (Input.GetKeyDown (KeyCode.P)) {
				showPaused = true;
				setPaused (true);
			}

			if (Input.GetKeyDown (KeyCode.F1)) {
				this.GetComponent<Warping> ().chooseDestinationUnlocked = true;
				print ("Warp point destination choice unlocked.");
			}

			if (Input.GetKeyDown (KeyCode.F2)) {
				FallThroughPlanet.fallThroughPlanetUnlocked = true;
				print ("Fall through plannet unlocked.");
			}

			if (Input.GetKeyDown (KeyCode.F3)) {
				this.GetComponent<SaveSpotTeleport> ().setEnterSaveSpot ();
				print ("You killed the boss!");
			}

			if (Input.GetKeyDown (KeyCode.F4)) {
				GameObject.Find ("Player").GetComponent<PlayerAttributes> ().LevelMeUp ();
			}

			if (Input.GetKeyDown (KeyCode.R)) {
				GameObject.Find ("Main Camera").GetComponent<SmoothMouseLook> ().resetRotation ();
			}

			if ((Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) && soundPlays == false) {
				soundPlays = true;
				moving = true;
				if(run){
					playerAttributes.drainStamina ();
					if(Application.loadedLevelName == "SaveSpot"){
						sound.playCharacterSound (3);
					} else {
						sound.playCharacterSound (1);
					}
				} else if(run == false){
					if(Application.loadedLevelName == "SaveSpot"){
						sound.playCharacterSound (1);
					} else {
						sound.playCharacterSound (0);
					}
				}
			} else {
				if (Time.time >= check) {	
					if (Input.GetAxis ("Vertical") == 0 && Input.GetAxis ("Horizontal") == 0) {
						moving = false;
						soundPlays = false;
						sound.stopSound ("character");
					}
					check += 0.25f;
				}
			}
		} else {
			if (Input.GetKeyDown (KeyCode.P)) {
				paused = false;
				showPaused = false;
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
		if (paused) {
			this.GetComponent<Sounds>().stopSound("all");
		}
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
				GameObject.Find("Player").transform.position = new Vector3(0.63f, 21.9f, 1.68f);
				GameObject.Find("Player").transform.rotation = new Quaternion(4.336792f, -0.0001220703f, 0.3787689f, 1);
				this.GetComponent<Sounds>().playWorldSound(2);
				Application.LoadLevel (Application.loadedLevel);
			}
			if (GUI.Button (new Rect (350, 140, 100, 30), "Quit")) {
				this.GetComponent<Sounds>().playWorldSound(2);
				Application.Quit ();
			}
		} else if (showPaused) {
			GUI.Box (new Rect (300, 40, 200, 50), "Paused \n(Press P to unpause)");
		}
	}
}
