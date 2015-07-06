using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	/**
	 * Automatically called after level is set. Should initilze other attributes based on level;
	 */
	protected abstract void init();

	void Start () {
		/* Any other initlization */	
	}
	
	void Update () {
		/* Called once per frame. AI comes Here */
	}



	public int hp { get; protected set; }
	public int level { get {return level;}  set {level = value; init(); } }
	public int damage { get; protected set;}
	public float hitChance { get; protected set;}
	public float critChance { get; protected set;}
	public float lootChance { get; protected set;}
	public int maxLoot {get; protected set;}
	public int xpGain { get; protected set;}
	public string typeID {get; protected set;}

	public bool isDead() {
		return hp <= 0;
	}

	public bool loseHP(int loss) {
		hp -= loss;
		return isDead();
	}



	public string attack(PlayerAttributes e) {
		float ran = Random.value;
		string message = "Miss!";
		
		if (ran <= hitChance){			
			message = "Hit! ";
			int tmpDamage = damage;
			if (ran <= critChance) {
				tmpDamage *= 2;
				message = "Critical Hit! ";
			}
			e.loseHP(tmpDamage);		
		} 
		
		return message;
	}

}
