using UnityEngine;

public class ApeAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */
	
	public AudioSource monsterAudio;
	private Animator animator;

	private float nextApeAttack, apeDelay = 3;
	private float nextARegeneration;
	private float delayARegeneration = 6;

	public override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.12f;
		const float HIT_MULT = 1.12f;
		const float DAMAGE_MULT = 1.2f;

		typeID = "ApeAlien";

		hp = Mathf.RoundToInt(70 * Mathf.Pow (HP_MULT, level-1));
		maxHp = hp;
		hitChance = 0.16f * Mathf.Pow (HIT_MULT, level-1);
		critChance = 0.012f * Mathf.Pow (CRIT_MULT, level-1);
		damage = Mathf.RoundToInt(10 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	void Start () {
		/* Any other initlization */
		animator = GetComponent<Animator>();
		animator.SetBool("Attacking", false);
		monsterAudio = gameObject.AddComponent<AudioSource>();
		typeID = "ApeAlien";
		lootChance = 0.4f;
		maxLoot = 2;
		nextApeAttack = Time.time + apeDelay;
		nextARegeneration = Time.time + delayARegeneration;
	}
	
	void Update () {
		PlayerController playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (playerScript.paused  == false && !Camera.main.GetComponent<CameraControl>().birdsEye) {
			/* Called once per frame. AI comes Here */
			
			GameObject player = GameObject.Find ("Player");
			Vector3 PlayerPos = player.GetComponent<Rigidbody> ().position;
			Vector3 myPos = GetComponent<Rigidbody> ().position;
			
			var viewdist = 22;
			var dark = LightRotation.getDark(this.gameObject);
			if (dark == "dark") {
				viewdist -= 8;
			} else if (dark == "dusk") {
				viewdist -= 4;
			}
			if (Vector3.Distance (PlayerPos, myPos) < viewdist) {
				
				if(GameObject.Find("Player").GetComponent<PlayerController>().moving){
					if(suspicion < 10){
						suspicion++;
					} else {
						if(!seekingRevenge){
							followPlayer();
						} else {
							seekOutPlayer();
						}
						animator.SetBool("Attacking", false);
					}
					
				} else {
					if(suspicion > 0)
					{
						suspicion--;
					}
				}

				if (Vector3.Distance (PlayerPos, myPos) < viewdist/2) {
					if (Time.time >= nextApeAttack) {
						nextApeAttack = Time.time + apeDelay;
						attack (player.GetComponent<PlayerAttributes> ());	//Attack Player
						animator.SetBool("Attacking", true);
					} else if(!seekingRevenge){
						followPlayer();
						animator.SetBool("Attacking", false);
					} else {
						seekOutPlayer();
						animator.SetBool("Attacking", false);
					}

				}
			}

			if (Time.time >= nextARegeneration) {
				nextARegeneration = Time.time + delayARegeneration;
				if (Time.time >= (lastDamage+3) && getHealth () < getMaxHp ()) {
					hp += (int)(getMaxHp () * 0.01);
				}
			}
		}else {
			nextARegeneration = Time.time + delayARegeneration;
			//lastDamage += 1;
		}
	}	
}
