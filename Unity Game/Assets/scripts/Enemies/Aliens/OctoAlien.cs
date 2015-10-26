using UnityEngine;

public class OctoAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */

	public AudioSource monsterAudio;
	private Animator animator;

	public override int damage { get {
			var tmp = base.damage;
			var dark = LightRotation.getDark(this.gameObject);
			if (dark == "dark") {
				tmp += 10;
			} else if (dark == "dusk") {
				tmp += 5;
			}
			return tmp;
		} }

	private float nextTransAttack, transDelay = 3;
	private float nextTRegeneration;
	private float delayTRegeneration = 6;

	public override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.14f;
		const float HIT_MULT = 1.12f;
		const float DAMAGE_MULT = 1.2f;

		typeID = "OctoAlien";

		hp = Mathf.RoundToInt(150 * Mathf.Pow (HP_MULT, level-1));
		maxHp = hp;
		hitChance = 0.20f * Mathf.Pow (HIT_MULT, level-1);
		critChance = 0.014f * Mathf.Pow (CRIT_MULT, level-1);
		damage = Mathf.RoundToInt(15 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	void Start () {
		/* Any other initlization */
		animator = GetComponent<Animator>();
		animator.SetBool("Attacking", false);
		monsterAudio = gameObject.AddComponent<AudioSource>();

		typeID = "OctoAlien";

		lootChance = 0.65f;
		maxLoot = 4;
		nextTransAttack = Time.time + transDelay;
		nextTRegeneration = Time.time + delayTRegeneration;
	}
	
	void Update () {
		PlayerController playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (playerScript.paused == false && !Camera.main.GetComponent<CameraControl>().birdsEye) {
			/* Called once per frame. AI comes Here */
		
			GameObject player = GameObject.Find ("Player");
			Vector3 PlayerPos = player.GetComponent<Rigidbody> ().position;
			Vector3 myPos = GetComponent<Rigidbody> ().position;

			if (Vector3.Distance (PlayerPos, myPos) < 12) {
				
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

				if (Vector3.Distance (PlayerPos, myPos) < 6) {
					if (Time.time >= nextTransAttack) {
						nextTransAttack = Time.time + transDelay;
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
			} else {
				if(!seekingRevenge){
					followDark();
				} else {
					seekOutPlayer();
				}
				animator.SetBool("Attacking", false);
			}

			if (Time.time >= nextTRegeneration) {
				nextTRegeneration = Time.time + delayTRegeneration;
				if (Time.time >= (lastDamage+3) && getHealth () < getMaxHp ()) {
					hp += (int)(getMaxHp () * 0.01);
				}
			}
		} else {
			nextTRegeneration = Time.time + delayTRegeneration;
			//lastDamage += 1;
		}
	}	
}
