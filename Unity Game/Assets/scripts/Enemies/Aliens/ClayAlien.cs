using UnityEngine;

public class ClayAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */
	
	public AudioSource monsterAudio;

	public override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.14f;
		const float HIT_MULT = 1.12f;
		const float DAMAGE_MULT = 1.2f;

		typeID = "ClayAlien";

		hp = Mathf.RoundToInt(100 * Mathf.Pow (HP_MULT, level-1));
		maxHp = hp;
		hitChance = 0.18f * Mathf.Pow (HIT_MULT, level-1);
		critChance = 0.012f * Mathf.Pow (CRIT_MULT, level-1);
		damage = Mathf.RoundToInt(13 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	private float changeDir;
	private float delayedChange = 5;
	private int dir;

	void Start () {
		/* Any other initlization */
		
		monsterAudio = gameObject.AddComponent<AudioSource>();
		
		lootChance = 0.60f;
		maxLoot = 3;
		changeDir = Time.time + delayedChange;
		dir = Random.Range (1, 5);
	}
	
	void Update () {
		/* Called once per frame. AI comes Here */
		GameObject player = GameObject.Find("Player");
		PlayerController playerScript = player.GetComponent<PlayerController> ();
		if (!playerScript.paused && !Camera.main.GetComponent<CameraControl>().birdsEye) {


			Vector3 PlayerPos = player.GetComponent<Rigidbody> ().position;		
			transform.LookAt (PlayerPos);
			Vector3 myPos = GetComponent<Rigidbody> ().position;

			float distance = Vector3.Distance (PlayerPos, myPos);
			if (distance < 6 || suspicion >= 10) {
				//player.GetComponent<Sounds>().playMonsterSound (1, this);
				if (!onPlayer) {//distance < 10 && 
					Vector3 direction = PlayerPos - myPos;
					this.GetComponent<Rigidbody> ().MovePosition (this.GetComponent<Rigidbody> ().position + direction * 0.25f * Time.deltaTime);
					//this.transform.Translate (direction * 0.025f);
				} else if (distance >= 1.5f) {
					onPlayer = false;
				}
			}
			var viewdist = 13;
			var dark = LightRotation.getDark (this.gameObject);
			if (dark == "dark") {
				viewdist += 5;
			} else if (dark == "dusk") {
				viewdist += 2;
			}
			if (Vector3.Distance (PlayerPos, myPos) < viewdist) {
				
				if (GameObject.Find ("Player").GetComponent<PlayerController> ().moving) {
					if (suspicion < 10) {
						suspicion++;
					} else {
						if(!seekingRevenge){
							followPlayer();
						} else {
							seekOutPlayer();
						}
					}
					
				} else {
					if (suspicion > 0) {
						suspicion--;
					}
				}
				
				if (Vector3.Distance (PlayerPos, myPos) < viewdist / 2) {
					if(!seekingRevenge){
						followPlayer();
					} else {
						seekOutPlayer();
					}
				}
			} else {
				attackPlayer = false;
			}
			
			if (attackPlayer) {
				if (Time.time >= nextMAttack) {
					nextMAttack = Time.time + mDelay;
					attack (player.GetComponent<PlayerAttributes> ());
				}
			}
			
			if (Time.time >= nextRegeneration) {
				nextRegeneration = Time.time + delayRegeneration;
				if (Time.time >= (lastDamage + 3) && getHealth () < getMaxHp ()) {
					hp += (int)(getMaxHp () * 0.01);
				}
			}
			
			if (Time.time >= changeDir) {
				changeDir += delayedChange;
				dir = Random.Range (1, 5);
			}

			walkAround (0.5f, dir);
		} else {
			nextRegeneration = Time.time + delayRegeneration;
			//lastDamage += 1;
		}
	}	
}
