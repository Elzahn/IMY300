using UnityEngine;

public class MossAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */

	protected override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.1f;
		const float HIT_MULT = 1.1f;
		const float DAMAGE_MULT = 1.2f;

		hp = Mathf.RoundToInt(50 * Mathf.Pow (HP_MULT, level-1));
		critChance = 0.01f * Mathf.Pow (CRIT_MULT, level-1);
		hitChance = 0.15f * Mathf.Pow (HIT_MULT, level-1);
		damage = Mathf.RoundToInt(8 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	void Start () {
		/* Any other initlization */
		typeID = "MossAlien";
		lootChance = 0.7f;
		maxLoot = 1;
	}
	
	void Update () {
		/* Called once per frame. AI comes Here */
	}	
}
