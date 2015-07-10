using UnityEngine;

public class TranslucentAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */


	private float nextTransAttack, transDelay = 3;
	private float nextTRegeneration;
	private float delayTRegeneration = 6;

	public override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.14f;
		const float HIT_MULT = 1.12f;
		const float DAMAGE_MULT = 1.2f;
		
		hp = Mathf.RoundToInt(150 * Mathf.Pow (HP_MULT, level-1));
		maxHp = hp;
		hitChance = 0.20f * Mathf.Pow (HIT_MULT, level-1);
		critChance = 0.014f * Mathf.Pow (CRIT_MULT, level-1);
		damage = Mathf.RoundToInt(15 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	void Start () {
		/* Any other initlization */
		typeID = "TranslucentAlien";
		lootChance = 0.85f;
		maxLoot = 4;
		nextTransAttack = Time.time + transDelay;
		nextTRegeneration = Time.time + delayTRegeneration;
	}
	
	void Update () {
		PlayerController playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (playerScript.getPaused () == false) {
			/* Called once per frame. AI comes Here */
		
			GameObject player = GameObject.Find ("Player");
			GameObject persist = GameObject.Find ("Persist");
			Vector3 PlayerPos = player.GetComponent<Rigidbody> ().position;
			Vector3 myPos = GetComponent<Rigidbody> ().position;

			if (Vector3.Distance (PlayerPos, myPos) < 8) {
				suspision = true;
				if (Vector3.Distance (PlayerPos, myPos) < 6) {
					if (Time.time >= nextTransAttack) {
						nextTransAttack = Time.time + transDelay;
						attack (persist.GetComponent<PlayerAttributes> ());	//Attack Player
					}
					followPlayer ();
				}
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
