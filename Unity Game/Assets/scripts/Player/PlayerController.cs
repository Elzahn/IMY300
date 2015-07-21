using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerController : MonoBehaviour {
	
	private PlayerAttributes playerAttributes;

	public const float RUN_MULT = 2f;

	public float moveSpeed;
	private Vector3 moveDir;
	private bool jumping = false, paused, showDeath, showPaused, soundPlays = false;
	public bool run = false, moving, showQuit = false;
	private Sounds sound;
	private float check;
	private int screenshotCount = 0;

	public void setJumping(){
		jumping = false;
		this.GetComponent<Animator>().SetBool("Jumping", jumping);
	}

	void Start(){
		playerAttributes = this.GetComponent<PlayerAttributes> ();
		paused = true;
		showDeath = false;
		showPaused = false;
		moving = false;
		sound = this.GetComponent<Sounds> ();
		check = Time.time;
	}

	public void playAnimation(){
		Animator animator = this.GetComponent<Animator> ();
		animator.speed = 1;
		animator.SetBool ("MovingStraight", false);
		animator.SetBool ("MovingRight", false);
		animator.SetBool ("MovingLeft", false);
		animator.SetFloat ("Turning", 0);
		animator.SetBool ("Jumping", false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.KeypadPlus)) {
			if(!Directory.Exists(Application.dataPath + "/Screenshots"))
			{    
				//if it doesn't, create it
				Directory.CreateDirectory(Application.dataPath + "/Screenshots");
			}
			
			string screenshotFilename;
			do
			{
				screenshotCount++;
				screenshotFilename = Application.dataPath  + "/Screenshots/screenshot" + screenshotCount + ".png";
				
			} while (System.IO.File.Exists(screenshotFilename));
			Application.CaptureScreenshot(screenshotFilename);
		}
		if (paused == true) {
			this.GetComponent<Animator> ().speed = 0;
		}

		if (paused == false) {
			playAnimation();

			if(Input.GetKeyDown(KeyCode.Tab)){
				showQuit = true;
			}

			if (playerAttributes.isDead () == true) {
				showDeath = true;
				GameObject.Find("Player").GetComponent<Sounds>().stopAlarmSound(1);
				paused = true;
			}

			if(Sounds.characterAudio.isPlaying == false)
			{
				soundPlays = false;
			}

			if (Input.GetKeyDown (KeyCode.LeftShift)) {   
				moveSpeed = 10;
				run = true;
				soundPlays = false;
			} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
				moveSpeed = 5;
				run = false;
				soundPlays = false;
			}

			if (playerAttributes.getStamina () <= 0) {
				if (playerAttributes.getStamina () < 0)
					playerAttributes.setStaminaToZero ();
				run = false; 
				moveSpeed = 5; 
			}

			if (playerAttributes.getDizzy () == true) {
				moveDir = new Vector3 (Input.GetAxisRaw ("Vertical"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Horizontal")).normalized;
			} else {
				moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Vertical")).normalized;
			}

			if (Input.GetAxisRaw ("Jump") == 1) {
				jumping = true;
				this.GetComponent<Animator>().SetBool("Jumping", jumping);
			}

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

			if ((Input.GetAxis ("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) && soundPlays == false) {
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
				if ((Input.GetAxis ("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)) {
					if (Input.GetAxis ("Vertical") != 0 && Input.GetAxisRaw("Horizontal") < 0){
						this.GetComponent<Animator>().SetBool("MovingLeft", moving);
						this.GetComponent<Animator>().SetBool("MovingRight", false);
						this.GetComponent<Animator>().SetBool("MovingStraight", false);
					} else if (Input.GetAxis ("Vertical") != 0 && Input.GetAxisRaw("Horizontal") > 0){
						this.GetComponent<Animator>().SetBool("MovingRight", moving);
						this.GetComponent<Animator>().SetBool("MovingStraight", false);
						this.GetComponent<Animator>().SetBool("MovingLeft", false);
					} else {
						this.GetComponent<Animator>().SetBool("MovingStraight", moving);
						this.GetComponent<Animator>().SetBool("MovingRight", false);
						this.GetComponent<Animator>().SetBool("MovingLeft", false);
					}
				}

				if (Time.time >= check) {	
					if (Input.GetAxis ("Vertical") == 0 && Input.GetAxis ("Horizontal") == 0) {
						moving = false;
						this.GetComponent<Animator>().SetBool("MovingStraight", moving);
						this.GetComponent<Animator>().SetBool("MovingRight", moving);
						this.GetComponent<Animator>().SetBool("MovingLeft", moving);
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
			int boxHeigh = 150;
			int boxWidth = 200;
			int top = Screen.height / 2 - boxHeigh / 2;
			int left = Screen.width / 2 - boxWidth / 2;
			int buttonWidth = 100;
			int itemHeight = 30;

			GUI.Box (new Rect (left, top, boxWidth, boxHeigh), "You died! Restart the level?");
			if (GUI.Button (new Rect (left + 30, top + 30, buttonWidth, itemHeight), "Restart level")) {
				paused = false;
				showDeath = false;
				playerAttributes.restoreHealthToFull ();
				playerAttributes.restoreStaminaToFull ();
				playerAttributes.resetXP ();
				GameObject.Find ("Player").transform.position = new Vector3 (0.63f, 21.9f, 1.68f);
				GameObject.Find ("Player").transform.rotation = new Quaternion (4.336792f, -0.0001220703f, 0.3787689f, 1);
				this.GetComponent<Sounds> ().stopSound ("alarm");
				this.GetComponent<Sounds> ().playWorldSound (2);
				playerAttributes.resetInventoryAndStorage ();
				PlayerLog.queue.Clear ();
				PlayerLog.stats = "";
				Application.LoadLevel (Application.loadedLevel);
			}
			if (GUI.Button (new Rect (left + 30, top + 90, buttonWidth, itemHeight), "Quit")) {
				this.GetComponent<Sounds> ().playWorldSound (2);
				Application.Quit ();
			}
		} else if (showPaused) {
			int boxHeigh = 50;
			int boxWidth = 200;
			int top = Screen.height / 2 - boxHeigh / 2;
			int left = Screen.width / 2 - boxWidth / 2;
			GUI.Box (new Rect (left, top, boxWidth, boxHeigh), "Paused \n(Press P to unpause)");
		} else if (showQuit) {
			paused = true;
			int boxHeigh = 150;
			int boxWidth = 250;
			int top = Screen.height / 2 - boxHeigh / 2;
			int left = Screen.width / 2 - boxWidth / 2;
			int buttonWidth = 100;
			int itemHeight = 30;

			GUI.Box (new Rect (left, top, boxWidth, boxHeigh), "Are you sure you want to quit?");
			if (GUI.Button (new Rect (left + 10, top + 60, buttonWidth, itemHeight), "Yes")) {
				this.GetComponent<Sounds> ().playWorldSound (2);
				Application.Quit();
			}
			if (GUI.Button (new Rect (left + buttonWidth + 30, top + 60, buttonWidth, itemHeight), "No")) {
				this.GetComponent<Sounds> ().playWorldSound (2);
				showQuit = false;
				paused = false;
			}
		}
	}
}
