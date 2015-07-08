using UnityEngine;

public class ApeAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */
	
	private float nextAttack, delay = 3;

	public override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.12f;
		const float HIT_MULT = 1.12f;
		const float DAMAGE_MULT = 1.2f;

		hp = Mathf.RoundToInt(70 * Mathf.Pow (HP_MULT, level-1));
		hitChance = 0.16f * Mathf.Pow (HIT_MULT, level-1);
		critChance = 0.012f * Mathf.Pow (CRIT_MULT, level-1);
		damage = Mathf.RoundToInt(10 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	void Start () {
		/* Any other initlization */
		typeID = "ApeAlien";
		lootChance = 0.75f;
		maxLoot = 2;
		nextAttack = Time.time + delay;
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
					if (Time.time >= nextAttack) {
						nextAttack = Time.time + delay;
						attack (persist.GetComponent<PlayerAttributes> ());	//Attack Player
					}
					followPlayer ();
				}
			}
		}
	}	
}
