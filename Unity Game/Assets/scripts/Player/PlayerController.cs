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
    private float moveSpeed;
    private PlayerAttributes playerAttributes;
    public bool run, moving, showQuit;
    private int screenshotCount;
    
	private Canvas MainMenu;
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
		MainMenu = GameObject.Find ("MainMenu").GetComponent<Canvas> ();
		MainMenu.enabled = false;
        playerAttributes = GetComponent<PlayerAttributes>();
        paused = false;
		GameObject.Find("Death").GetComponent<Canvas>().enabled = false;
		GameObject.Find("Popup").GetComponent<Canvas>().enabled = false;
        moving = false;
        sound = GetComponent<Sounds>();
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
			sound.resumeSound("ambience");
            if (GameObject.Find("Player").GetComponent<LevelSelect>().currentLevel == 1) {
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

		//Suicide
		if(Input.GetKeyDown(KeyCode.Backslash)){
			playerAttributes.loseHP((int)playerAttributes.hp);
		}

		//Almost Suicide
		if(Input.GetKeyDown(KeyCode.RightBracket)){
			playerAttributes.loseHP((int)playerAttributes.hp - 10);
		}

        //Skip Tutorial
        if (Input.GetKeyDown(KeyCode.Equals) && GetComponent<Tutorial>().startTutorial) {
			this.GetComponent<Tutorial> ().tutorialDone = false;
            if (GameObject.Find("Particles") != null) {
                GameObject.Find("Particles").GetComponentInParent<SphereCollider>().enabled = false;
            }
            GetComponent<Tutorial>().stopTutorial();
            GetComponent<Tutorial>().startTutorial = false;
            GetComponent<SaveSpotTeleport>().canEnterSaveSpot = true;
            GetComponent<SaveSpotTeleport>().loadTutorial = false;
			/*Loot.gotPowerCore = true;
			playerAttributes.gotCore = true;*/
            GameObject.Find("Player").GetComponent<PlayerAttributes>().inventory.AddLast(new Longsword(1));
            GameObject.Find("Player").GetComponent<PlayerAttributes>().inventory.AddLast(new ButterKnife(5));
            GameObject.Find("Player").GetComponent<PlayerAttributes>().inventory.AddLast(new Warhammer(2));
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
			GetComponent<Tutorial>().tutorialDone = true;
        } 

        //Warp cheat
        if (Input.GetKeyDown(KeyCode.F1) && Application.loadedLevelName == "Scene") {
            GetComponent<Warping>().chooseDestinationUnlocked = true;
            GetComponent<Warping>().chooseDestination = true;
        }

        //Fall through planet cheat
        if (Input.GetKeyDown(KeyCode.F2) && Application.loadedLevelName == "Scene") {
            GetComponent<FallThroughPlanet>().fallThroughPlanetUnlocked = true;
            GetComponent<FallThroughPlanet>().canFallThroughPlanet = true;
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
		checkScreenshot ();

		if (Application.loadedLevelName != "Main_Menu") {
			checkIfAttackPossible ();

			moveSpeed = playerAttributes.speed;
			/**
		 * P Pauses or unpausese
		 * Only if showpause the same as pause.
		 * */
			if (Input.GetButtonDown ("Pause")) {
				if (Application.loadedLevelName != "Scene" ||
					(Application.loadedLevelName == "Scene" &&
					!GameObject.Find ("Planet").GetComponent<LoadingScreen> ().loading)) {
					paused = !paused;
					if (GameObject.Find ("Popup").GetComponent<Canvas> ().enabled) {
						GameObject.Find ("Popup").GetComponent<Canvas> ().enabled = false;
					} else {
						GameObject.Find ("Popup").GetComponent<Canvas> ().enabled = true;
						sound.pauseSound ("computer");
					}
				}
			}

			var animatorComponent = GetComponent<Animator> ();
			if (paused) {
				animatorComponent.speed = 0;
				moveSpeed = 0;
			} else {
				playAnimation ();

				keysCheck ();

				if (playerAttributes.isDead ()) {
					GameObject.Find ("Death").GetComponent<Canvas> ().enabled = true;
					GetComponent<Sounds> ().stopAlarmSound (Sounds.LOW_HEALTH_ALARM);
					paused = true;
				}

				if (Input.GetButton ("Run") && playerAttributes.stamina > 0 &&
					Application.loadedLevelName != "SaveSpot" &&
					(Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0)) {
					if (GameObject.Find ("Stamina").GetComponent<Image> ().isActiveAndEnabled == false) {
						GameObject.Find ("Stamina").GetComponent<Image> ().enabled = true;
					}
					run = true;
					moveSpeed *= RUN_MULT;
					playerAttributes.drainStamina ();
					if (sound.characterAudio.isPlaying && sound.characterClip == Sounds.PLANET_WALKING) {
						sound.stopSound ("character");
					}

					if (!sound.characterAudio.isPlaying) {
						sound.playCharacterSound (Sounds.PLANET_RUNNING);
					}

					animatorComponent.SetBool ("Running", true);

				} else if ((Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0)) {
					run = false;
					animatorComponent.SetBool ("Running", false);
					if (sound.characterAudio.isPlaying && sound.characterClip == Sounds.PLANET_RUNNING) {
						sound.stopSound ("character");
					}
					if (!sound.characterAudio.isPlaying) {
						if (Application.loadedLevelName == "SaveSpot") {
							sound.playCharacterSound (Sounds.SHIP_WALKING);
						} else {
							sound.playCharacterSound (Sounds.PLANET_WALKING);
						}
					}
				}

				if ((Input.GetAxisRaw ("Vertical") != 0 || Input.GetAxisRaw ("Horizontal") != 0)) {
					moving = true;
					if (Input.GetAxisRaw ("Vertical") == 0 && Input.GetAxisRaw ("Horizontal") < 0) {
						animatorComponent.SetBool ("MovingLeft", moving);
						animatorComponent.SetBool ("MovingRight", false);
						animatorComponent.SetBool ("MovingStraight", false);
					} else if (Input.GetAxisRaw ("Vertical") == 0 && Input.GetAxis ("Horizontal") > 0) {
						animatorComponent.SetBool ("MovingRight", moving);
						animatorComponent.SetBool ("MovingStraight", false);
						animatorComponent.SetBool ("MovingLeft", false);
					} else {
						animatorComponent.SetBool ("MovingStraight", moving);
						animatorComponent.SetBool ("MovingRight", false);
						animatorComponent.SetBool ("MovingLeft", false);
					}
				}

				if (!Camera.main.GetComponent<CameraControl> ().birdsEye) {
					if (playerAttributes.dizzy) {
						moveDir = new Vector3 (Input.GetAxisRaw ("Vertical"), 0, Input.GetAxisRaw ("Horizontal")).normalized;
					} else {
						moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
					}
				}

				/*if (Input.GetAxisRaw ("Jump") == 1) {
				jumping = true;			
			}*/


				if (Input.GetAxisRaw ("Vertical") == 0 && Input.GetAxisRaw ("Horizontal") == 0) {
					moving = false;
					animatorComponent.SetBool ("MovingStraight", moving);
					animatorComponent.SetBool ("MovingRight", moving);
					animatorComponent.SetBool ("MovingLeft", moving);
					animatorComponent.SetBool ("Running", moving);

					if (sound.characterClip == Sounds.PLANET_RUNNING || sound.characterClip == Sounds.PLANET_WALKING || sound.characterClip == Sounds.SHIP_WALKING) {
						GetComponent<Sounds> ().stopSound ("character");
					}
				}
			}
		}
    }

    private void FixedUpdate() {
		if (showQuit) {
			MainMenu.enabled = true;
			paused = true;
			if(GameObject.Find("MenuMask").GetComponent<Image>().fillAmount < 1){
				GameObject.Find("MenuMask").GetComponent<Image>().fillAmount += 0.01f;
			}
		} else {
			if(GameObject.Find("MenuMask").GetComponent<Image>().fillAmount > 0){
				GameObject.Find("MenuMask").GetComponent<Image>().fillAmount -= 0.01f;
			} else if(GameObject.Find("MenuMask").GetComponent<Image>().fillAmount == 0){
				MainMenu.enabled = false;
			}
		}

        if (!paused) {
            var rBody = GetComponent<Rigidbody>();
            rBody.MovePosition(rBody.position + transform.TransformDirection(moveDir)*moveSpeed*Time.deltaTime);
        }
    }

	public void restart(){
		Application.LoadLevel("Scene");
		paused = false;
		playerAttributes.restoreHealthToFull();
		playerAttributes.restoreStaminaToFull();
		playerAttributes.resetXP();
		
		GameObject.Find("Player").transform.rotation = Quaternion.Euler(0f, -95.3399f, 0f);
		GameObject.Find("Player").transform.position = new Vector3(-1.651f, 80.82f, 0.84f);
		sound.stopSound("alarm");
		sound.playWorldSound(Sounds.BUTTON);
		playerAttributes.resetInventoryAndStorage();

		GameObject.Find("Stamina").GetComponent<Image>().fillAmount = playerAttributes.stamina/
			playerAttributes.maxStamina();
		GameObject.Find("Health").GetComponent<Image>().fillAmount = playerAttributes.hp/
			playerAttributes.maxHP();
		GameObject.Find("XP").GetComponent<Image>().fillAmount = playerAttributes.xp/
			playerAttributes.getExpectedXP();
		GameObject.Find ("Death").GetComponent<Canvas> ().enabled = false;
		this.GetComponent<LevelSelect> ().spawnedLevel = false;
	}

	public void quit(){
		sound.playWorldSound(Sounds.BUTTON);
		Application.Quit();
	}
}