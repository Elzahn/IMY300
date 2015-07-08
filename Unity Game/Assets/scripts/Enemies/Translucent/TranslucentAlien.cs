using UnityEngine;

public class TranslucentAlien : Enemy {
	/**
	 * Automatically called after level is set. 
	 * Should initilze other attributes dpendent on level;
	 */

	private GameObject persist;

	public override void init() {
		const float HP_MULT = 1.6f;
		const float CRIT_MULT = 1.14f;
		const float HIT_MULT = 1.12f;
		const float DAMAGE_MULT = 1.2f;

		hp = Mathf.RoundToInt(150 * Mathf.Pow (HP_MULT, level-1));
		hitChance = 0.20f * Mathf.Pow (HIT_MULT, level-1);
		critChance = 0.014f * Mathf.Pow (CRIT_MULT, level-1);
		damage = Mathf.RoundToInt(15 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	void Start () {
		/* Any other initlization */
		typeID = "TranslucentAlien";
		lootChance = 0.85f;
		maxLoot = 4;
		persist = GameObject.Find("Persist");
	}
	
	void Update () {
		/* Called once per frame. AI comes Here */
		
		if(followThePlayer)
			followPlayer ();

		if (inRange) {
			//print ("hi");
			attack (persist.GetComponent<PlayerAttributes> ());
		}
	}	
}
