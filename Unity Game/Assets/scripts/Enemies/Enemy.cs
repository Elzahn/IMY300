using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	/**
	 * Automatically called after level is set. Should initilze other attributes based on level;
	 */
	public abstract void init();
	protected bool suspision, attackPlayer;
	private float nextAttack, delay, nextMAttack, mDelay = 0.5f;
	protected bool notCollided;
	public float lastDamage;
	protected float nextRegeneration;
	protected float delayRegeneration = 6;

	void Start () {
		/* Any other initlization */
		suspision = false;
		attackPlayer = false;
		nextAttack = Time.time + delay;
		nextMAttack = Time.time + mDelay;
		notCollided = false;

		PlayerAttributes persist = GameObject.Find ("Persist").GetComponent<PlayerAttributes>();
		delay = (1.5f - persist.getStamina () / persist.maxStamina ());
		nextRegeneration = Time.time + delayRegeneration;
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
				this.suspision = true;
				if (Vector3.Distance (PlayerPos, myPos) < 6) {
					followPlayer ();
				}
			} else{
				attackPlayer = false;
			}

			if (attackPlayer) {
				if (Time.time >= nextMAttack) {
					nextMAttack = Time.time + mDelay;
					attack (persist.GetComponent<PlayerAttributes> ());
				}
			}

			if (Time.time >= nextRegeneration) {
				nextRegeneration = Time.time + delayRegeneration;
				if (Time.time >= (lastDamage+3) && getHealth () < getMaxHp ()) {
					hp += (int)(getMaxHp () * 0.01);
				}
			}
		} else {
			nextRegeneration = Time.time + delayRegeneration;
			//lastDamage += 1;
		}
	}

	void OnMouseDown() {
		//this.hp = 0;

		GameObject player = GameObject.Find("Player");
		GameObject persist = GameObject.Find ("Persist");
		Vector3 PlayerPos = player.GetComponent<Rigidbody>().position;
		Vector3 myPos = GetComponent<Rigidbody>().position;
		if (Time.time >= nextAttack) {
			nextAttack = Time.time + delay;
			if (Vector3.Distance (PlayerPos, myPos) < 6) {
				persist.GetComponent<PlayerAttributes> ().attack (this);
				//attack (persist.GetComponent<PlayerAttributes> ());	//Attack Player
				attackPlayer = true;
			}
		}
	}

	public int maxHp{ get; protected set; }
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

	public void walkAround(){
		Vector3 myPos = GetComponent<Rigidbody>().position;
		Vector3 tempPos = new Vector3 (Random.value*2, Random.value, Random.value*3);
		Vector3 direction = tempPos - myPos;
		this.transform.Translate(tempPos * 0.025f);
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
		e.lastDamage = Time.time;

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

	public int getMaxHp(){
		return this.maxHp;
	}

	public int getHealth(){
		return this.hp;
	}
}
