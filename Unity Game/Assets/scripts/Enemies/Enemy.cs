using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	protected abstract void init(int lev);

	public int hp { get; protected set; }

	//private int _level;
	public int level { get {return level;}  set {level = value; init(level); } }
	public int damage { get; protected set;}
	public float hitChance { get; protected set;}
	public float critChance { get; protected set;}
	public float lootChance { get; protected set;}

	public int xpGain { get; protected set;}

	public bool isDead() {
		return hp <= 0;
	}

	public bool loseHP(int loss) {
		hp -= loss;
		return isDead();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/* AI comes Here */
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
