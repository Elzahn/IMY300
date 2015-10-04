using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public const float RUN_MULT = 2f;
    /**
	 * Access 'paused' like normal variable, do not use '_paused' ever!
	 * */
    private bool _paused;
    public Sprite Attack;
  //  private float check;
    private float moveSpeed;
    private PlayerAttributes playerAttributes;
    public bool run, moving, showQuit;
    private int screenshotCount;
    

	private bool showDeath, showPaused;//, soundPlays;
    private Sounds sound;
    public Vector3 moveDir { get; set; }
    public bool showAttack { get; private set; }

    public bool paused
    {
        get
        {
            if (_paused) {
                GetComponent<Sounds>().pauseSound("computerTalks");
            }
            return _paused;
        }
        set
        {
            _paused = value;
            GetComponent<Sounds>().resumeSound("all");
        }
    }

    /*public void setJumping(){
		jumping = false;

	}*/

    private void Start() {
        //transform.rotation = Quaternion.identity;
        playerAttributes = GetComponent<PlayerAttributes>();
        paused = false;
        showDeath = false;
        showPaused = false;
        moving = false;
        sound = GetComponent<Sounds>();
       // check = Time.time;
    }

    public void playAnimation() {
        var animator = GetComponent<Animator>();
        animator.speed = 1;
        animator.SetBool("MovingStraight", false);
        animator.SetBool("MovingRight", false);
        animator.SetBool("MovingLeft", false);
        animator.SetFloat("Turning", 0);
    }

    private void checkScreenshot() {
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            if (!Directory.Exists(Application.dataPath + "/Screenshots")) {
                //if it doesn't, create it
                Directory.CreateDirectory(Application.dataPath + "/Screenshots");
            }

            string screenshotFilename;
            do
            {
                screenshotCount++;
                screenshotFilename = Application.dataPath + "/Screenshots/screenshot" + screenshotCount + ".png";
            } while (File.Exists(screenshotFilename));
            Application.CaptureScreenshot(screenshotFilename);
        }
    }

    private void keysCheck() {
		if (Input.GetKeyDown(KeyCode.Keypad0)) {
			GetComponent<LevelSelect>().currentLevel = 0;
		}

        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            GetComponent<LevelSelect>().currentLevel = 1;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            GetComponent<LevelSelect>().currentLevel = 2;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            GetComponent<LevelSelect>().currentLevel = 3;
        }

        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            GetComponent<LevelSelect>().currentLevel = 4;
        }

        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            GetComponent<LevelSelect>().currentLevel = 5;
        }

        //Goto Tutorial Planet
        if (Input.GetKeyDown(KeyCode.Tab) && GetComponent<Tutorial>().startTutorial &&
            Application.loadedLevelName == "SaveSpot") {
            GetComponent<SaveSpotTeleport>().canEnterSaveSpot = false;
            GetComponent<SaveSpotTeleport>().setExitConf(false);
            GetComponent<Rigidbody>().mass = 0.1f;
            sound.playWorldSound(Sounds.TELEPORTING);
            playerAttributes.saveInventoryAndStorage();
            sound.stopSound("computer");

            transform.rotation = Quaternion.Euler(0f, 91.60388f, 0f);
            transform.position = new Vector3(0.26f, 16.06f, 0.316f);
            Application.LoadLevel("Tutorial");
        } else if (Input.GetKeyDown(KeyCode.Tab) && Application.loadedLevelName == "Tutorial") {
			GameObject.Find ("Health").GetComponent<Image> ().enabled = true;
			GameObject.Find ("Stamina").GetComponent<Image> ().enabled = true;
			GameObject.Find ("HUD Body Temp").GetComponent<Image> ().enabled = false;
            GetComponent<Tutorial>().tutorialDone = true;
            GetComponent<Tutorial>().teachInventory = true;
            playerAttributes.inventory.AddLast(TutorialSpawner.bossPowerCore);
            playerAttributes.storage.AddLast(new Cupcake());
            GetComponent<SaveSpotTeleport>().canEnterSaveSpot = false;
            GetComponent<SaveSpotTeleport>().loadTutorial = false;
            sound.playWorldSound(Sounds.TELEPORTING);
            sound.stopSound("computer");
            playerAttributes.returnToSaveSpot();
            GetComponent<Rigidbody>().mass = 100f;
            GetComponent<PlayerAttributes>().restoreHealthToFull();
            GetComponent<PlayerAttributes>().restoreStaminaToFull();
            GetComponent<SaveSpotTeleport>().setExitConf(true);
            Loot.gotPowerCore = true;
        }

        //skip loadingscreen
        if (Input.GetKeyDown(KeyCode.L) && Application.loadedLevelName == "Scene") {
            GameObject.Find("Planet").GetComponent<LoadingScreen>().loading = false;
            GameObject.Find("Loading Screen").GetComponent<Canvas>().enabled = false;
            if (GameObject.Find("Player").GetComponent<LevelSelect>().currentLevel == 1) {
                //GameObject.Find("Player").GetComponent<SaveSpotTeleport>().showedHealthHint = true;
                GameObject.Find("Player")
                    .GetComponent<Tutorial>()
                    .makeHint("Need a health pack? Look out for these flowers.",
                        GameObject.Find("Player").GetComponent<Tutorial>().Health);
            }
            GameObject.Find("Player").transform.rotation = Quaternion.Euler(0f, -95.3399f, 0f);
            GameObject.Find("Player").transform.position = new Vector3(-1.651f, 80.82f, 0.84f);
            GetComponent<Rigidbody>().isKinematic = false;
        }

        //build cheat to skip cutscenes

        //Skip Tutorial
        if (Input.GetKeyDown(KeyCode.Escape) && GetComponent<Tutorial>().startTutorial && !InventoryGUI.showInventory &&
            !InventoryGUI.showStorage) {
            if (GameObject.Find("Particles") != null) {
                GameObject.Find("Particles").GetComponentInParent<SphereCollider>().enabled = false;
            }
            GetComponent<Tutorial>().stopTutorial();
            GetComponent<Tutorial>().startTutorial = false;
            GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
            GetComponent<SaveSpotTeleport>().loadTutorial = false;
            GameObject.Find("Player").GetComponent<PlayerAttributes>().inventory.AddLast(new Longsword(1));
            GameObject.Find("Player").GetComponent<PlayerAttributes>().inventory.AddLast(new ButterKnife(5));
            GameObject.Find("Player").GetComponent<PlayerAttributes>().inventory.AddLast(new Warhammer(2));
            //GameObject.Find("Player").transform.position = new Vector3(9.41f, 79.19f, 7.75f);
            GameObject.Find("Player").transform.position = new Vector3(13.72f, 81.58f, 14.77f); //(9.4f, 81.38f, 6.62f);
			playerAttributes.returnToSaveSpot();

			if (Application.loadedLevelName == "Scene") {
                GameObject.Find("Tech Light").GetComponent<Light>().enabled = false;
                GameObject.Find("Console Light").GetComponent<Light>().enabled = false;
                GameObject.Find("Bedroom Light").GetComponent<Light>().enabled = false;
            }

			GameObject.Find ("Health").GetComponent<Image> ().enabled = true;
			GameObject.Find ("HUD Body Temp").GetComponent<Image> ().enabled = false;
			GameObject.Find ("Stamina").GetComponent<Image> ().enabled = true;
			playerAttributes.doorOpen = true;
            GetComponent<Rigidbody>().mass = 1000;
            GetComponent<Tutorial>().stopTutorial();
            //print ("Tutorial skipped you can now use the teleporter again.");
        } else if (Input.GetKeyDown(KeyCode.Escape) && !GetComponent<Tutorial>().startTutorial &&
                 !InventoryGUI.showInventory && !InventoryGUI.showStorage) {
            showQuit = true;
        }

        //Warp cheat
        if (Input.GetKeyDown(KeyCode.F1) && Application.loadedLevelName == "Scene") {
            GetComponent<Warping>().chooseDestinationUnlocked = true;
            GetComponent<Warping>().chooseDestination = true;
            print("Warp point destination choice unlocked.");
        }

        //Fall through planet cheat
        if (Input.GetKeyDown(KeyCode.F2) && Application.loadedLevelName == "Scene") {
            GetComponent<FallThroughPlanet>().fallThroughPlanetUnlocked = true;
            GetComponent<FallThroughPlanet>().canFallThroughPlanet = true;
            print("Fall through plannet unlocked.");
        }

        //Open teleported back
        if (Input.GetKeyDown(KeyCode.F3)) {
            GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
        }

        //LevelUp
        if (Input.GetKeyDown(KeyCode.F4)) {
            playerAttributes.levelMeUp();
        }

        //Spin cheat
        if (Input.GetKeyDown(KeyCode.F5) && Application.loadedLevelName != "SaveSpot") {
            GameObject.Find("Planet").GetComponent<NaturalDisasters>().spinPlanetNow();
        }

        //Earthquake cheat
        if (Input.GetKeyDown(KeyCode.F6) && Application.loadedLevelName != "SaveSpot") {
            GameObject.Find("Planet").GetComponent<NaturalDisasters>().makeEarthQuakeHappen();
        }

        //Skip AI voice
        if (Input.GetKeyDown(KeyCode.F7)) {
            sound.stopSound("computer");
        }

		//gives a weapon
		if (Input.GetKeyDown (KeyCode.F8)) {
			playerAttributes.inventory.AddLast(new Longsword(1));
			playerAttributes.inventory.AddLast(new PowerCore());
		}

		//gives accessories
		if (Input.GetKeyDown (KeyCode.F9)) {
			playerAttributes.inventory.AddLast(new CommonAccessory(1));
			playerAttributes.inventory.AddLast(new UncommonAccessory(3));
			playerAttributes.inventory.AddLast(new RareAccessory(4));
			playerAttributes.inventory.AddLast(new RareAccessory(2));
		}

		//gives usable items
		if (Input.GetKeyDown (KeyCode.F10)) {
			playerAttributes.inventory.AddLast(new MediumHealthPack());
			playerAttributes.inventory.AddLast(new LargeHealthPack());
			playerAttributes.inventory.AddLast(new Cupcake());
		}
    }

    public void checkIfAttackPossible() {
        var collidedItems = Physics.OverlapSphere(transform.position, 5f);
        var collidedWithNumMosters = 0;

        foreach (var col in collidedItems) {
            if (col.tag == "Monster") {
                collidedWithNumMosters++;
            }
        }
        //print (!this.GetComponent<Tutorial> ().healthHintShown);
        if (collidedWithNumMosters > 0 && !GetComponent<Tutorial>().healthHintShown) {
            showAttack = true;
            Camera.main.GetComponent<HUD>()
                .makeInteractionHint("Attack by pressing while your mouse is on the monster", Attack);
        } else {
            showAttack = false;
        }
    }

    // Update is called once per frame
    private void Update() {
        checkIfAttackPossible();

        checkScreenshot();

        moveSpeed = playerAttributes.speed;
        /**
		 * P Pauses or unpausese
		 * Only if showpause the same as pause.
		 * */
        if (Input.GetKeyDown(KeyCode.P) && paused == showPaused) {
            if (Application.loadedLevelName != "Scene" ||
                (Application.loadedLevelName == "Scene" &&
                 !GameObject.Find("Planet").GetComponent<LoadingScreen>().loading)) {
                paused = !paused;
                showPaused = paused;
            }
        }

        var animatorComponent = GetComponent<Animator>();
        if (paused) {
            animatorComponent.speed = 0;
            moveSpeed = 0;
        } else {
            playAnimation();

            keysCheck();

            if (playerAttributes.isDead()) {
                showDeath = true;
                GetComponent<Sounds>().stopAlarmSound(Sounds.LOW_HEALTH_ALARM);
                paused = true;
            }

           /* if (!GameObject.Find("Player").GetComponent<Sounds>().characterAudio.isPlaying) {
                soundPlays = false;
            }*/

            if (Input.GetKey(KeyCode.LeftShift) && playerAttributes.stamina > 0 &&
                Application.loadedLevelName != "SaveSpot" &&
                (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)) {
                if (GameObject.Find("Stamina").GetComponent<Image>().isActiveAndEnabled == false) {
                    GameObject.Find("Stamina").GetComponent<Image>().enabled = true;
                }
                run = true;
                moveSpeed *= RUN_MULT;
                playerAttributes.drainStamina();
                if (sound.characterAudio.isPlaying && sound.characterClip == Sounds.PLANET_WALKING) {
                    sound.stopSound("character");
                }

                if (!sound.characterAudio.isPlaying) {
                    sound.playCharacterSound(Sounds.PLANET_RUNNING);
                }

                animatorComponent.SetBool("Running", true);
                //Took it out to fix sound while running
                //soundPlays = false;
			} else if((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)) {
                run = false;
                animatorComponent.SetBool("Running", false);
                if (sound.characterAudio.isPlaying && sound.characterClip == Sounds.PLANET_RUNNING) {
                    sound.stopSound("character");
                }
                if (!sound.characterAudio.isPlaying) {
                    if (Application.loadedLevelName == "SaveSpot") {
                        sound.playCharacterSound(Sounds.SHIP_WALKING);
                    } else {
                        sound.playCharacterSound(Sounds.PLANET_WALKING);
                    }
                }
            }

            if ((Input.GetAxis("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)) {
                moving = true;
                if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") < 0) {
                    animatorComponent.SetBool("MovingLeft", moving);
                    animatorComponent.SetBool("MovingRight", false);
                    animatorComponent.SetBool("MovingStraight", false);
                } else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") > 0) {
                    animatorComponent.SetBool("MovingRight", moving);
                    animatorComponent.SetBool("MovingStraight", false);
                    animatorComponent.SetBool("MovingLeft", false);
                } else {
                    animatorComponent.SetBool("MovingStraight", moving);
                    animatorComponent.SetBool("MovingRight", false);
                    animatorComponent.SetBool("MovingLeft", false);
                }
            }

            if (!Camera.main.GetComponent<CameraControl>().birdsEye) {
                if (playerAttributes.dizzy) {
                    //moveDir = new Vector3 (Input.GetAxisRaw ("Vertical"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Horizontal")).normalized;
                    moveDir = new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal")).normalized;
                } else {
                    //moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Jump"), Input.GetAxisRaw ("Vertical")).normalized;
                    moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
                }
            }

            /*if (Input.GetAxisRaw ("Jump") == 1) {
				jumping = true;			
			}*/


            //if (Time.time >= check) {	
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) {
                moving = false;
                animatorComponent.SetBool("MovingStraight", moving);
                animatorComponent.SetBool("MovingRight", moving);
                animatorComponent.SetBool("MovingLeft", moving);
                animatorComponent.SetBool("Running", moving);
               // soundPlays = false;
				if(sound.characterClip == Sounds.PLANET_RUNNING || sound.characterClip == Sounds.PLANET_WALKING || sound.characterClip == Sounds.SHIP_WALKING){
                	GetComponent<Sounds>().stopSound("character");
				}
            }
            //check += 0.25f;
            //	}
            //}
        }
    }

    private void FixedUpdate() {
        if (!paused) {
            var rBody = GetComponent<Rigidbody>();
            rBody.MovePosition(rBody.position + transform.TransformDirection(moveDir)*moveSpeed*Time.deltaTime);
        }
    }

    private void OnGUI() {
        var SoundComponent = GetComponent<Sounds>();
        if (showDeath) {
            var boxHeigh = 150;
            var boxWidth = 200;
            var top = Screen.height/2 - boxHeigh/2;
            var left = Screen.width/2 - boxWidth/2;
            var buttonWidth = 100;
            var itemHeight = 30;

            GUI.Box(new Rect(left, top, boxWidth, boxHeigh), "You died! Restart the level?");
            if (GUI.Button(new Rect(left + 30, top + 30, buttonWidth, itemHeight), "Restart level")) {
                paused = false;
                showDeath = false;
                playerAttributes.restoreHealthToFull();
                playerAttributes.restoreStaminaToFull();
                playerAttributes.resetXP();
                //var playerObbject = GameObject.Find("Player");
				GameObject.Find("Player").transform.rotation = Quaternion.Euler(0f, -95.3399f, 0f);
				GameObject.Find("Player").transform.position = new Vector3(-1.651f, 80.82f, 0.84f);
                SoundComponent.stopSound("alarm");
                SoundComponent.playWorldSound(Sounds.BUTTON);
                playerAttributes.resetInventoryAndStorage();
                //PlayerLog.queue.Clear ();
                //PlayerLog.stats = "";
                /*if(GameObject.Find("Planet").GetComponent<LevelSelect>() != null){
					//playerAttributes.restoreHealthToFull();
				}*/

                Application.LoadLevel("Scene");
                GameObject.Find("Stamina").GetComponent<Image>().fillAmount = playerAttributes.stamina/
                                                                              playerAttributes.maxStamina();
                GameObject.Find("Health").GetComponent<Image>().fillAmount = playerAttributes.hp/
                                                                             playerAttributes.maxHP();
                GameObject.Find("XP").GetComponent<Image>().fillAmount = playerAttributes.xp/
                                                                         playerAttributes.getExpectedXP();
            }
            if (GUI.Button(new Rect(left + 30, top + 90, buttonWidth, itemHeight), "Quit")) {
                SoundComponent.playWorldSound(Sounds.BUTTON);
                Application.Quit();
            }
        } else if (showPaused) {
            sound.pauseSound("computer");
            var boxHeigh = 50;
            var boxWidth = 200;
            var top = Screen.height/2 - boxHeigh/2;
            var left = Screen.width/2 - boxWidth/2;
            GUI.Box(new Rect(left, top, boxWidth, boxHeigh), "Paused \n(Press P to unpause)");
        } else if (showQuit) {
            paused = true;
            var boxHeigh = 150;
            var boxWidth = 250;
            var top = Screen.height/2 - boxHeigh/2;
            var left = Screen.width/2 - boxWidth/2;
            var buttonWidth = 100;
            var itemHeight = 30;

            GUI.Box(new Rect(left, top, boxWidth, boxHeigh), "Are you sure you want to quit?");
            if (GUI.Button(new Rect(left + 10, top + 60, buttonWidth, itemHeight), "Yes")) {
                SoundComponent.playWorldSound(Sounds.BUTTON);
                Application.Quit();
            }
            if (GUI.Button(new Rect(left + buttonWidth + 30, top + 60, buttonWidth, itemHeight), "No")) {
                SoundComponent.playWorldSound(Sounds.BUTTON);
                showQuit = false;
                paused = false;
            }
        }
    }
}