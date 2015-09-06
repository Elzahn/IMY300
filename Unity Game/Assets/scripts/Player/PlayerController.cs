using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerController : MonoBehaviour {

	private PlayerAttributes playerAttributes;

	public const float RUN_MULT = 2f;

	private float moveSpeed;
	public Vector3 moveDir{ get; set; }

	public Sprite Attack;
	public bool showAttack{ get; private set;}
	/**
	 * Access 'jumping' like normal variable, do not use '_jumping' ever!
	 * */
	/*private bool _jumping = false;
	public bool jumping {
		get {
			return _jumping;
		}
		set {
			_jumping = value;
			this.GetComponent<Animator>().SetBool("Jumping", _jumping);
		}
	}*/

	private bool showDeath, showPaused, soundPlays = false;

	/**
	 * Access 'paused' like normal variable, do not use '_paused' ever!
	 * */
	private bool _paused;
	public bool paused {
		get {
			if (_paused) {
				this.GetComponent<Sounds>().pauseSound("all");
			}
			return _paused;
		} 
		set {
			_paused = value;
			this.GetComponent<Sounds>().resumeSound("all");
		} 
	}

	public bool run = false, moving, showQuit = false;
	private Sounds sound;
	private float check;
	private int screenshotCount = 0;

	/*public void setJumping(){
		jumping = false;

	}*/

	void Start(){
		//transform.rotation = Quaternion.identity;
		playerAttributes = this.GetComponent<PlayerAttributes> ();
		paused = false;
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

		//Goto Tutorial Planet
		if (Input.GetKeyDown (KeyCode.Tab) && this.GetComponent<Tutorial>().startTutorial && Application.loadedLevelName == "SaveSpot") {
			this.GetComponent<SaveSpotTeleport> ().canEnterSaveSpot = false;
			this.GetComponent<SaveSpotTeleport> ().setExitConf (false);
			this.GetComponent<Rigidbody> ().mass = 0.1f;
			sound.playWorldSound (Sounds.SHIP_DOOR);
			playerAttributes.saveInventoryAndStorage ();
			//this.transform.position = new Vector3 (0f, 15.03f, 0);
			//this.transform.position = new Vector3(-0.01f, 16.149f, -0.27f);
			sound.stopSound ("computer");
			this.transform.position = new Vector3(0, 16f, -0.327f);
			//this.GetComponent<Tutorial>().stopTutorial();
			Application.LoadLevel ("Tutorial");

		} else if (Input.GetKeyDown (KeyCode.Tab) && Application.loadedLevelName == "Tutorial"){
			this.GetComponent<Tutorial> ().tutorialDone = true;
			playerAttributes.inventory.AddLast(TutorialSpawner.bossPowerCore);
			playerAttributes.storage.AddLast(new Cupcake());
			this.GetComponent<SaveSpotTeleport>().canEnterSaveSpot = false;
			this.GetComponent<SaveSpotTeleport>().loadTutorial = false;
			sound.playWorldSound (Sounds.SHIP_DOOR);
			sound.stopSound ("computer");
			this.GetComponent<LevelSelect>().currentLevel++;
			//this.GetComponent<Tutorial>().stopTutorial();
			Application.LoadLevel ("SaveSpot");
			Resources.UnloadUnusedAssets();
			//this.transform.position = new Vector3 (-27.01f, 79.65f, 1.93f);
			this.transform.position = new Vector3 (13.72f, 81.58f, 14.77f);//(9.4f, 81.38f, 6.62f);
			this.transform.rotation = new Quaternion(0.0f, 1f, 0, 0f);
			this.GetComponent<Rigidbody> ().mass = 100f;
			this.GetComponent<PlayerAttributes> ().restoreHealthToFull();
			this.GetComponent<PlayerAttributes> ().restoreStaminaToFull();
			this.GetComponent<SaveSpotTeleport> ().setExitConf(true);
			Loot.gotPowerCore = true;
		}

		//skip loadingscreen
		if (Input.GetKeyDown (KeyCode.L) && Application.loadedLevelName == "Scene") {
			GameObject.Find("Planet").GetComponent<LoadingScreen>().loading = false;
			if(GameObject.Find("Player").GetComponent<LevelSelect>().currentLevel == 1){
				GameObject.Find("Player").GetComponent<SaveSpotTeleport>().showedHealthHint = true;
				GameObject.Find("Player").GetComponent<Tutorial>().makeHint("Need a health pack? Look out for the purple flowers.", GameObject.Find("Player").GetComponent<Tutorial>().Health);
			}
			//this.transform.position = new Vector3(9.41f, 79.19f, 7.75f);
			this.transform.position = new Vector3 (0.304f, 80.394f, 0.207f);//(0.32f, 80.37f, 032f);
			this.GetComponent<Rigidbody>().isKinematic = false;
		}

		//build cheat to skip cutscenes

		//Skip Tutorial
		if (Input.GetKeyDown (KeyCode.Escape) && this.GetComponent<Tutorial>().startTutorial) {
			GameObject.Find("Tech Light").GetComponent<Light>().enabled = false;
			GameObject.Find("Console Light").GetComponent<Light>().enabled = false;
			GameObject.Find("Bedroom Light").GetComponent<Light>().enabled = false;
			this.GetComponent<Tutorial>().stopTutorial();
			this.GetComponent<Tutorial>().startTutorial = false;
			this.GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
			this.GetComponent<SaveSpotTeleport>().loadTutorial = false;
			//GameObject.Find("Player").transform.position = new Vector3(9.41f, 79.19f, 7.75f);
			GameObject.Find("Player").transform.position = new Vector3 (13.72f, 81.58f, 14.77f);//(9.4f, 81.38f, 6.62f);
			this.GetComponent<LevelSelect>().currentLevel++;
			Application.LoadLevel("SaveSpot");
			this.GetComponent<Rigidbody>().mass = 1000;
			this.GetComponent<Tutorial>().stopTutorial();
			//print ("Tutorial skipped you can now use the teleporter again.");
		} else if(Input.GetKeyDown(KeyCode.Escape) && !this.GetComponent<Tutorial>().startTutorial){
			showQuit = true;
		}

		//Warp cheat
		if (Input.GetKeyDown (KeyCode.F1) && Application.loadedLevelName == "Scene") {
			this.GetComponent<Warping> ().chooseDestinationUnlocked = true;
			this.GetComponent<Warping> ().chooseDestination = true;
			print ("Warp point destination choice unlocked.");
		}

		//Fall through planet cheat
		if (Input.GetKeyDown (KeyCode.F2) && Application.loadedLevelName == "Scene") {
			this.GetComponent<FallThroughPlanet>().fallThroughPlanetUnlocked = true;
			this.GetComponent<FallThroughPlanet>().canFallThroughPlanet = true;
			print ("Fall through plannet unlocked.");
		}

		//Open teleported back
		if (Input.GetKeyDown (KeyCode.F3)) {
			this.GetComponent<SaveSpotTeleport> ().canEnterSaveSpot = true;
		}

		//LevelUp
		if (Input.GetKeyDown (KeyCode.F4)) {
			playerAttributes.levelMeUp ();
		}

		//Spin cheat
		if(Input.GetKeyDown(KeyCode.F5) && Application.loadedLevelName != "SaveSpot"){
			GameObject.Find("Planet").GetComponent<NaturalDisasters>().spinPlanetNow();
		}
		
		//Earthquake cheat
		if(Input.GetKeyDown(KeyCode.F6) && Application.loadedLevelName != "SaveSpot"){
			GameObject.Find("Planet").GetComponent<NaturalDisasters>().makeEarthQuakeHappen();
		}

		//Skip AI voice
		if(Input.GetKeyDown(KeyCode.F7)){
			sound.stopSound("computer");
		}
	}

	public void checkIfAttackPossible(){
		Collider[] collidedItems = Physics.OverlapSphere(this.transform.position, 5f);
		int collidedWithNumMosters = 0;
		
		foreach(Collider col in collidedItems){
			if(col.tag == "Monster"){
				collidedWithNumMosters++;
			}
		}

		if (collidedWithNumMosters > 0) {

			showAttack = true;
			Camera.main.GetComponent<HUD> ().makeInteractionHint ("Attack by pressing while your mouse is on the monster", Attack);
		} else {
			showAttack = false;
		}

	}

	// Update is called once per frame
	void Update () {
		checkIfAttackPossible ();

		checkScreenshot();

		moveSpeed = playerAttributes.speed;
		/**
		 * P Pauses or unpausese
		 * Only if showpause the same as pause.
		 * */
		if (Input.GetKeyDown (KeyCode.P) && paused == showPaused){
			if(Application.loadedLevelName != "Scene" || (Application.loadedLevelName == "Scene" && !GameObject.Find("Planet").GetComponent<LoadingScreen>().loading)) {
				paused = !paused;
				showPaused = paused;			
			}
		}

		if (paused) {
			this.GetComponent<Animator> ().speed = 0;
			moveSpeed = 0;

		} else {
			playAnimation();

			keysCheck();

			if (playerAttributes.isDead ()) {
				showDeath = true;
				this.GetComponent<Sounds>().stopAlarmSound(Sounds.LOW_HEALTH_ALARM);
				paused = true;
			}

			if (!GameObject.Find("Player").GetComponent<Sounds>().characterAudio.isPlaying) {
				soundPlays = false;
			}
			
			run = false;
			if (Input.GetAxis("Run") > 0 && playerAttributes.stamina > 0 && Application.loadedLevelName != "SaveSpot") {
				run = true;
				moveSpeed *= RUN_MULT;
				if(Application.loadedLevelName != "Tutorial"){
					playerAttributes.drainStamina ();
				}
				//Took it out to fix sound while running
				//soundPlays = false;
			}

			if(!Camera.main.GetComponent<CameraControl>().birdsEye){
				if (playerAttributes.dizzy) {
					//moveDir = new Vector3 (Input.GetAxisRaw ("Vertical"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Horizontal")).normalized;
					moveDir = new Vector3 (Input.GetAxisRaw ("Vertical"), 0, Input.GetAxisRaw ("Horizontal")).normalized;
				} else {
					//moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Vertical")).normalized;
					moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
				}
			}

			/*if (Input.GetAxisRaw ("Jump") == 1) {
				jumping = true;			
			}*/


			if ((Input.GetAxis ("Vertical") != 0 || Input.GetAxisRaw ("Horizontal") != 0) 
				&& !soundPlays && !Camera.main.GetComponent<CameraControl>().birdsEye) {
				soundPlays = true;
				moving = true;

				if(run){
					if(Application.loadedLevelName == "SaveSpot"){
						sound.playCharacterSound (Sounds.SHIP_RUNNING);
					} else {
						sound.playCharacterSound (Sounds.PLANET_RUNNING);
					}
				} else {
					if(Application.loadedLevelName == "SaveSpot"){
						sound.playCharacterSound (Sounds.SHIP_WALKING);
					} else {
						sound.playCharacterSound (Sounds.PLANET_WALKING);
					}
				}
			} else {
				if ((Input.GetAxis ("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)) {
					if (Input.GetAxis ("Vertical") == 0 && Input.GetAxis("Horizontal") < 0){
						this.GetComponent<Animator>().SetBool("MovingLeft", moving);
						this.GetComponent<Animator>().SetBool("MovingRight", false);
						this.GetComponent<Animator>().SetBool("MovingStraight", false);
					} else if (Input.GetAxis ("Vertical") == 0 && Input.GetAxis("Horizontal") > 0){
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
						this.GetComponent<Sounds>().stopSound ("character");
					}
					check += 0.25f;
				}
			}

		}
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
				this.GetComponent<Sounds> ().playWorldSound (Sounds.BUTTON);
				playerAttributes.resetInventoryAndStorage ();
				//PlayerLog.queue.Clear ();
				//PlayerLog.stats = "";
				Application.LoadLevel (Application.loadedLevel);
			}
			if (GUI.Button (new Rect (left + 30, top + 90, buttonWidth, itemHeight), "Quit")) {
				this.GetComponent<Sounds> ().playWorldSound (Sounds.BUTTON);
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
				this.GetComponent<Sounds> ().playWorldSound (Sounds.BUTTON);
				Application.Quit();
			}
			if (GUI.Button (new Rect (left + buttonWidth + 30, top + 60, buttonWidth, itemHeight), "No")) {
				this.GetComponent<Sounds> ().playWorldSound (Sounds.BUTTON);
				showQuit = false;
				paused = false;
			}
		}
	}
}
