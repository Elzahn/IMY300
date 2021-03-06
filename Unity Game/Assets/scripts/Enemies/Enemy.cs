using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour {
	/**
	 * Automatically called after level is set. Should initilze other attributes based on level;
	 */
	public abstract void init();
	public bool attackPlayer{get; set;}
	public float nextAttack{get; set;}
	public float delay{get; set;}

	protected float nextMAttack, mDelay = 1f;//0.5f;
	protected bool notCollided;
	public float lastDamage;
	protected float nextRegeneration;
	protected float delayRegeneration = 6;
	protected bool onPlayer;
	protected int suspicion;
	protected int SUSPICION_ALERT = 5;
	protected bool seekingRevenge;

	//private Sounds sound;

	void Start () {
		/* Any other initlization */
//		sound = GameObject.Find("Player").GetComponent<Sounds> ();
		attackPlayer = false;
		seekingRevenge = false;
		nextAttack = Time.time + delay;
		nextMAttack = Time.time + mDelay;
		notCollided = false;
		onPlayer = false;
		suspicion = 0;
		PlayerAttributes player = GameObject.Find ("Player").GetComponent<PlayerAttributes>();
		delay = 5 * (1.5f - player.stamina / player.maxStamina ());
		nextRegeneration = Time.time + delayRegeneration;
	}

	public void seekOutPlayer(){
		seekingRevenge = true;
		GameObject player = GameObject.Find("Player");
		Vector3 PlayerPos = player.GetComponent<Rigidbody>().position;		
		//transform.LookAt(PlayerPos);

		Vector3 lookPos = PlayerPos - transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);

		Vector3 myPos = GetComponent<Rigidbody>().position;
		
		float distance = Vector3.Distance (PlayerPos, myPos);
		Vector3 direction = PlayerPos - myPos;

		if (onPlayer == false) {//distance < 10 && 
			this.GetComponent<Rigidbody> ().MovePosition (this.GetComponent<Rigidbody> ().position + direction * 0.25f * Time.deltaTime);
		}

		if (distance >= 1.5f) {
			onPlayer = false;
		}
	}

	void Update () {
		PlayerController playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		if (playerScript.paused == false && !Camera.main.GetComponent<CameraControl>().birdsEye) {
			this.GetComponent<Animator>().speed = 1;
			/* Called once per frame. AI comes Here */
			GameObject player = GameObject.Find ("Player");
			Vector3 PlayerPos = player.GetComponent<Rigidbody> ().position;
			Vector3 myPos = GetComponent<Rigidbody> ().position;

			if (Vector3.Distance (PlayerPos, myPos) < 12) {

				if(GameObject.Find("Player").GetComponent<PlayerController>().moving){
					if(suspicion < SUSPICION_ALERT){
						suspicion++;
					} else {
						if(!seekingRevenge){
							followPlayer();
						} else {
							seekOutPlayer();
						}
					}

				} else {
					if(suspicion > 0)
					{
						suspicion--;
					}
				}

				if (Vector3.Distance (PlayerPos, myPos) < 8) {
					attackPlayer = true;
					if(!seekingRevenge){
						followPlayer();
					} else {
						seekOutPlayer();
					}
				} else {
					GameObject.Find("Player").GetComponent<Sounds>().stopMonsterSound(this);
				}
			} else{
				attackPlayer = false;
			}

			if (attackPlayer) {
				if (Time.time >= nextMAttack) {
					nextMAttack = Time.time + mDelay;
					attack (player.GetComponent<PlayerAttributes> ());
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
			this.GetComponent<Animator>().speed = 0;
			//lastDamage += 1;
		}
	}

	void OnCollisionEnter (Collision col){
		if (col.collider.name == "Player") {
			onPlayer = true;
		}
	}

	void OnMouseDown() {
		//this.hp = 0;
		GameObject player = GameObject.Find ("Player");

		if (player.GetComponent<PlayerController> ().paused == false) {
			Vector3 PlayerPos = player.GetComponent<Rigidbody> ().position;
			Vector3 myPos = GetComponent<Rigidbody> ().position;
			if (Time.time >= nextAttack) {
				nextAttack = Time.time + delay;
				if (Vector3.Distance (PlayerPos, myPos) < 6) {
					player.GetComponent<PlayerAttributes> ().attack (this);
					//attack (player.GetComponent<PlayerAttributes> ());	//Attack Player
					attackPlayer = true;
				}
			}
		}
	}

	public float maxHp{ get; protected set; }
	public float hp { get; protected set; }
	public int level { get; set; }
	public virtual int damage { get; protected set;}
	public float hitChance { get; protected set;}
	public float critChance { get; protected set;}
	public float lootChance { get; protected set;}
	public int maxLoot {get; protected set;}
	//public int xpGain { get; protected set;}
	public string typeID {get; protected set;}

	private static bool minibossDead = false;

	public bool isDead() {
		if (hp <= 0 && typeID == "BossAlien" && !minibossDead) {
			minibossDead = true;
			GameObject.Find("Player").GetComponent<FallThroughPlanet>().fallThroughPlanetUnlocked = true;
			GameObject.Find("Player").GetComponent<FallThroughPlanet>().canFallThroughPlanet = true;
			GameObject.Find("Player").GetComponent<PlayerAttributes>().fallFirst = true;
		}

		if(hp <= 0){
			GameObject.Find("Character_Final").GetComponent<Animator>().SetBool("Attacking", false);
		}

		return hp <= 0;
	}

	public bool loseHP(int loss) {
		hp -= loss;

		if (hp < 0)
			hp = 0;

		return isDead();
	}

	public void walkAround(float moveSpeed, int goTo){
		/*	Vector3 myPos = GetComponent<Rigidbody>().position;
		Vector3 tempPos = new Vector3 (Random.value*2, Random.value, Random.value*3);
		Vector3 direction = tempPos - myPos;
		this.transform.Translate(tempPos * 0.025f);*/

		Vector3 PlayerPos = GameObject.Find ("Player").GetComponent<Rigidbody> ().position;
		Vector3 myPos = GetComponent<Rigidbody> ().position;
		
		float distance = Vector3.Distance (PlayerPos, myPos);
		//Vector3 direction = PlayerPos - myPos;
		if (distance > 6 && suspicion < SUSPICION_ALERT) {
			GameObject.Find("Player").GetComponent<Sounds>().playMonsterSound (Sounds.MONSTER_WALKING, this);
			Vector3 moveDir;
			Rigidbody rigidbody = GetComponent<Rigidbody> ();
			//int goTo = Random.Range (1, 5);
			if (goTo == 1) {
				moveDir = (transform.forward).normalized;
			} else if (goTo == 1) {
				moveDir = (-transform.forward).normalized;
			} else if (goTo == 2) {
				moveDir = (transform.right).normalized;
			} else {
				moveDir = (-transform.right).normalized;
			}

			rigidbody.MovePosition (rigidbody.position + transform.TransformDirection (moveDir) * moveSpeed * Time.deltaTime);
		}
	}

	public void followPlayer(){
		GameObject player = GameObject.Find("Player");
		Vector3 PlayerPos = player.GetComponent<Rigidbody>().position;		
		//transform.LookAt(PlayerPos);

		Vector3 lookPos = PlayerPos - transform.position;


		//if(typeID != "BossAlien" && typeID != "OctoAlien"){
			lookPos.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);

		/*} else {
			//lookPos.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 400000f);
		}*/

		Vector3 myPos = GetComponent<Rigidbody>().position;

		float distance = Vector3.Distance (PlayerPos, myPos);
		Vector3 direction = PlayerPos - myPos;
		if (distance < 6 || suspicion >= 10) {
			//player.GetComponent<Sounds>().playMonsterSound (1, this);
			if (onPlayer == false) {//distance < 10 && 
				this.GetComponent<Rigidbody> ().MovePosition (this.GetComponent<Rigidbody> ().position + direction * 0.25f * Time.deltaTime);
				//this.transform.Translate (direction * 0.025f);
			} else if (distance >= 1.5f) {
				onPlayer = false;
			}
		}
	}

	public void followDark() {
		var d = LightRotation.getDark(this.gameObject);
		int speed = LightRotation.lightSpeed;
		if (d != "dark") {
			speed /= 2;
		} 
		var oldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		transform.RotateAround(Vector3.zero, Vector3.up, speed * Time.deltaTime);

		Vector3 lookPos = oldPos - transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
	}

	public string attack(PlayerAttributes player) {
		GetComponent<Animator>().SetBool("Attacking", true);
		player.lastDamage = Time.time;
		PlayerAttributes.giveAlarm = true;

		float ran = Random.value;
		string message = "Monster Miss! ";
		
		if (ran <= hitChance){			
			message = "Monster Hit! ";
			GameObject.Find("Player").GetComponent<Sounds>().playMonsterSound(Sounds.MONSTER_HIT, this);
			int tmpDamage = damage;
			if (ran <= critChance) {
				tmpDamage *= 2;
				message = "Monster Critical Hit! ";
				GameObject.Find("Player").GetComponent<Sounds>().playMonsterSound(Sounds.MONSTER_CRIT, this);
			}
			player.loseHP(tmpDamage);

			if(message == "Monster Miss! "){
				GameObject.Find("Player").GetComponent<Sounds>().playMonsterSound(Sounds.MONSTER_MISS, this);
			}
		} 
		if (GameObject.Find ("Player").GetComponent<PlayerAttributes> ().isDead ()) {
			GameObject.Find("Player").GetComponent<Sounds>().playDeathSound(Sounds.DEAD_MONSTER);
			message += "You died.";
		} else {
			message += player.hp + "/" + player.maxHP();
		}
		//print(message);
		//PlayerLog.addStat (message);
		return message;
	}

	public float getMaxHp(){
		return this.maxHp;
	}

	public float getHealth(){
		return this.hp;
	}


}
