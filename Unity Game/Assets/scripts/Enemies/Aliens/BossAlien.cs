using UnityEngine;

public class BossAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */
	
	private Animator animator;

	public AudioSource monsterAudio;

	public override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.14f;
		const float HIT_MULT = 1.12f;
		const float DAMAGE_MULT = 1.2f;

		typeID = "BossAlien";

		hp = Mathf.RoundToInt(250 * Mathf.Pow (HP_MULT, level-1));
		maxHp = hp;
		hitChance = 0.28f * Mathf.Pow (HIT_MULT, level-1);
		critChance = 0.02f * Mathf.Pow (CRIT_MULT, level-1);
		damage = Mathf.RoundToInt(20 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	void Start () {
		/* Any other initlization */
		monsterAudio = gameObject.AddComponent<AudioSource>();
		animator = GetComponent<Animator>();
		animator.SetBool("Attacking", false);
		typeID = "BossAlien";
		lootChance = 0.5f;
		maxLoot = 3;
	}

	void Update () {
		PlayerController playerScript;
		if (GameObject.Find ("Player")) {
			playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();

			if (playerScript.paused == false && !Camera.main.GetComponent<CameraControl> ().birdsEye) {
				this.GetComponent<Animator> ().speed = 1;
				this.GetComponentInChildren<ParticleSystem> ().enableEmission = false;
				/* Called once per frame. AI comes Here */
				GameObject player = GameObject.Find ("Player");
				Vector3 PlayerPos = player.GetComponent<Rigidbody> ().position;

			
				/*Vector3 lookPos = PlayerPos - transform.position;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 4000f);*/

				Vector3 myPos = GetComponent<Rigidbody> ().position;
			
				if (Vector3.Distance (PlayerPos, myPos) < 16) {
				
					if (GameObject.Find ("Player").GetComponent<PlayerController> ().moving) {
						if (suspicion < SUSPICION_ALERT) {
							suspicion++;
						} else {
							if (!seekingRevenge) {
								followPlayer ();
							} else {
								seekOutPlayer ();
							}
							animator.SetBool ("Attacking", false);
						}
					
					} else {
						if (suspicion > 0) {
							suspicion--;
						}
					}
				
					if (Vector3.Distance (PlayerPos, myPos) < 6) {
						if (!seekingRevenge) {
							followPlayer ();
						} else {
							seekOutPlayer ();
						}
						animator.SetBool ("Attacking", false);
					} else {
						GameObject.Find ("Player").GetComponent<Sounds> ().stopMonsterSound (this);
					}
				} else {
					attackPlayer = false;
					animator.SetBool ("Attacking", false);
				}
			
				if (attackPlayer) {
					if (Time.time >= nextMAttack) {
						nextMAttack = Time.time + mDelay;
						attack (player.GetComponent<PlayerAttributes> ());
					}
				
					animator.SetBool ("Attacking", true);
				}
			
				if (Time.time >= nextRegeneration) {
					nextRegeneration = Time.time + delayRegeneration;
					if (Time.time >= (lastDamage + 3) && getHealth () < getMaxHp ()) {
						hp += (int)(getMaxHp () * 0.01);
					}
				}
			} else {
				nextRegeneration = Time.time + delayRegeneration;
				this.GetComponent<Animator> ().speed = 0;
				if (Camera.main.GetComponent<CameraControl> ().birdsEye) {
					this.GetComponentInChildren<ParticleSystem> ().enableEmission = true;
					this.GetComponentInChildren<ParticleSystem> ().emissionRate = 10;
				}
				//lastDamage += 1;
			}
		}
	}
	
	void OnMouseDown() {
		//this.hp = 0;
		GameObject player = GameObject.Find ("Player");
		
		if (player.GetComponent<PlayerController> ().paused == false) {
			Vector3 PlayerPos = player.GetComponent<Rigidbody> ().position;
			Vector3 myPos = GetComponent<Rigidbody> ().position;
			if (Time.time >= nextAttack) {
				nextAttack = Time.time + delay;
				if (Vector3.Distance (PlayerPos, myPos) < 6) {
					player.GetComponent<PlayerAttributes> ().attack (this);
					//attack (player.GetComponent<PlayerAttributes> ());	//Attack Player
					attackPlayer = true;
					animator.SetBool("Attacking", true);
				}
			}
		}
	}
}
