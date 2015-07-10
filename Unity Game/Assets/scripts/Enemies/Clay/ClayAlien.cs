using UnityEngine;

public class ClayAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */

	public override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.14f;
		const float HIT_MULT = 1.12f;
		const float DAMAGE_MULT = 1.2f;

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
		typeID = "ClayAlien";
		lootChance = 0.80f;
		maxLoot = 3;
		changeDir = Time.time + delayedChange;
		dir = Random.Range (1, 5);
	}
	
	void Update () {
		/* Called once per frame. AI comes Here */
		PlayerController playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (playerScript.getPaused () == false) {
			if (Time.time >= changeDir) {
				changeDir += delayedChange;
				dir = Random.Range (1, 5);
			}
		
			walkAround (1.1f, dir);
		}
	}	
}
