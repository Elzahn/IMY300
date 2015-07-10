using UnityEngine;

public class MossAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */

	public override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.1f;
		const float HIT_MULT = 1.1f;
		const float DAMAGE_MULT = 1.2f;

		hp = Mathf.RoundToInt(50 * Mathf.Pow (HP_MULT, level-1));
		maxHp = hp;
		critChance = 0.01f * Mathf.Pow (CRIT_MULT, level-1);
		hitChance = 0.15f * Mathf.Pow (HIT_MULT, level-1);
		damage = Mathf.RoundToInt(8 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	void Start () {
		/* Any other initlization */
		typeID = "MossAlien";
		lootChance = 0.7f;
		maxLoot = 1;
		changeDir = Time.time + delayedChange;
		dir = Random.Range (1, 5);
	}

	private float changeDir;
	private float delayedChange = 5;
	private int dir;

	void Update () {
		/* Called once per frame. AI comes Here */
		PlayerController playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (playerScript.getPaused () == false) {
			if (Time.time >= changeDir) {
				changeDir += delayedChange;
				dir = Random.Range (1, 5);
			}

			walkAround (0.5f, dir);
		}
	}	
}
