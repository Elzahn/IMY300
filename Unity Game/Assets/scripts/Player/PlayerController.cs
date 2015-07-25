using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerController : MonoBehaviour {

	private PlayerAttributes playerAttributes;

	public const float RUN_MULT = 2f;

	private float moveSpeed;
	private Vector3 moveDir;

	/**
	 * Access 'jumping' like normal variable, do not use '_jumping' ever!
	 * */
	private bool _jumping = false;
	public bool jumping {
		get {
			return _jumping;
		}
		set {
			_jumping = value;
			this.GetComponent<Animator>().SetBool("Jumping", _jumping);
		}
	}

	private bool showDeath, showPaused, soundPlays = false;

	/**
	 * Access 'paused' like normal variable, do not use '_paused' ever!
	 * */
	private bool _paused;
	public bool paused {
		get {
			if (_paused) {
				this.GetComponent<Sounds>().stopSound("all");
			}
			return _paused;
		} 
		set {
			_paused = value;
		} 
	}

	public bool run = false, moving, showQuit = false;
	private Sounds sound;
	private float check;
	private int screenshotCount = 0;

	public void setJumping(){
		jumping = false;

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

	void checkScreenshot() {
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
	}

	void keysCheck() {
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
			playerAttributes.levelMeUp ();
		}
		
		if (Input.GetKeyDown (KeyCode.R)) {
			GameObject.Find ("Main Camera").GetComponent<SmoothMouseLook> ().resetRotation ();
		}

		//Spin cheat
		if(Input.GetKeyDown(KeyCode.F5)){
			GameObject.Find("Planet").GetComponent<NaturalDisasters>().spinPlanetNow();
		}


		//Earthquake cheat
		if(Input.GetKeyDown(KeyCode.F6)){
			GameObject.Find("Planet").GetComponent<NaturalDisasters>().makeEarthQuakeHappen();
		}
		
		if(Input.GetKeyDown(KeyCode.Escape)){
			showQuit = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		checkScreenshot();

		moveSpeed = playerAttributes.speed;
		/**
		 * P Pauses or unpausese
		 * Only if showpuase the same as pause.
		 * */
		if (Input.GetKeyDown (KeyCode.P) && paused == showPaused) {
			paused = !paused;
			showPaused = paused;			
		}

		if (paused) {
			this.GetComponent<Animator> ().speed = 0;
			moveSpeed = 0;

		} else {
			playAnimation();

			keysCheck();

			if (playerAttributes.isDead ()) {
				showDeath = true;
				this.GetComponent<Sounds>().stopAlarmSound(1);
				paused = true;
			}

			if (!Sounds.characterAudio.isPlaying) {
				soundPlays = false;
			}
			
			run = false;
			if (Input.GetAxis("Run") > 0 && playerAttributes.stamina > 0) {
				run = true;
				moveSpeed *= RUN_MULT;
				playerAttributes.drainStamina ();
				soundPlays = false;
			}

			if (playerAttributes.dizzy) {
				moveDir = new Vector3 (Input.GetAxisRaw ("Vertical"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Horizontal")).normalized;
			} else {
				moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Vertical")).normalized;
			}

			if (Input.GetAxisRaw ("Jump") == 1) {
				jumping = true;			
			}


			if ((Input.GetAxis ("Vertical") != 0 || Input.GetAxisRaw ("Horizontal") != 0) 
				&& !soundPlays) {
				soundPlays = true;
				moving = true;

				if(run){
					if(Application.loadedLevelName == "SaveSpot"){
						sound.playCharacterSound (3);
					} else {
						sound.playCharacterSound (1);
					}
				} else {
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

		} /*else {

		}*/
	}

	void FixedUpdate() {
		if (!paused) {
			var rigidbody = GetComponent<Rigidbody> ();
			rigidbody.MovePosition (rigidbody.position + transform.TransformDirection (moveDir) * moveSpeed * Time.deltaTime);
		}
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
