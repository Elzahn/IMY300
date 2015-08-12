using UnityEngine;

public class BossAlien : Enemy {
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

		hp = Mathf.RoundToInt(250 * Mathf.Pow (HP_MULT, level-1));
		maxHp = hp;
		hitChance = 0.28f * Mathf.Pow (HIT_MULT, level-1);
		critChance = 0.02f * Mathf.Pow (CRIT_MULT, level-1);
		damage = Mathf.RoundToInt(20 * Mathf.Pow (DAMAGE_MULT,level-1));
	}

	void Start () {
		/* Any other initlization */
		monsterAudio = gameObject.AddComponent<AudioSource>();

		typeID = "BossAlien";
		lootChance = 0.100f;
		maxLoot = 5;
	}
	
	/*void Update () {
		/* Called once per frame. AI comes Here */

	//}	
}
