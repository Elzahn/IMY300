using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	/**
	 * Automatically called after level is set. Should initilze other attributes based on level;
	 */
	public abstract void init();

	void Start () {
		/* Any other initlization */	
	}
	
	void Update () {
		/* Called once per frame. AI comes Here */
	}

	void OnMouseDown() {
		//this.hp = 0;

		GameObject go = GameObject.Find("Player");
		Vector3 PlayerPos = go.GetComponent<Rigidbody>().position;
		Vector3 myPos = GetComponent<Rigidbody>().position;

		if (Vector3.Distance(PlayerPos, myPos) < 6) {
			go.GetComponent<PlayerAttributes>().attack(this);
		}
	}



	public int hp { get; protected set; }
	public int level { get; set; }
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

		print(message);
		return message;
	}

}
