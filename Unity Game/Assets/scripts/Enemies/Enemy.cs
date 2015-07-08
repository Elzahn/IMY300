using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	/**
	 * Automatically called after level is set. Should initilze other attributes based on level;
	 */
	public abstract void init();
	protected bool followThePlayer, inRange;

	void Start () {
		/* Any other initlization */	
	}
	
	void Update () {
		/* Called once per frame. AI comes Here */
		if(followThePlayer)
			followPlayer ();

		GameObject player = GameObject.Find("Player");
		GameObject persist = GameObject.Find ("Persist");
		Vector3 PlayerPos = player.GetComponent<Rigidbody>().position;
		Vector3 myPos = GetComponent<Rigidbody>().position;
		
		if (Vector3.Distance (PlayerPos, myPos) < 6) {
			inRange = true;
			//print ("there");
		}
	}

	void OnMouseDown() {
		//this.hp = 0;

		GameObject player = GameObject.Find("Player");
		GameObject persist = GameObject.Find ("Persist");
		Vector3 PlayerPos = player.GetComponent<Rigidbody>().position;
		Vector3 myPos = GetComponent<Rigidbody>().position;

		if (Vector3.Distance(PlayerPos, myPos) < 6) {
			player.GetComponent<PlayerAttributes>().attack(this);
			attack (persist.GetComponent<PlayerAttributes>());	//Attack Player
			this.followThePlayer = true;
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

	public void followPlayer(){
		GameObject player = GameObject.Find("Player");
		Vector3 PlayerPos = player.GetComponent<Rigidbody>().position;
		Vector3 myPos = GetComponent<Rigidbody>().position;

		float distance = Vector3.Distance (PlayerPos, myPos);
		Vector3 direction = PlayerPos - myPos;

		if (distance < 6) {
			this.transform.Translate(direction * 0.025f);
		}
	}

	public string attack(PlayerAttributes e) {
		float ran = Random.value;
		string message = "Monster Miss!";
		
		if (ran <= hitChance){			
			message = "Monster Hit!";
			int tmpDamage = damage;
			if (ran <= critChance) {
				tmpDamage *= 2;
				message = "Monster Critical Hit! ";
			}
			e.loseHP(tmpDamage);		
		} 

		print(message);
		return message;
	}

}
