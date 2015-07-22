using UnityEngine;
using System.Collections.Generic;

public class PlayerAttributes : MonoBehaviour {

	const int HP_BASE = 100;
	const float HP_MULT = 1.8f;
	const int STAM_BASE = 20;
	const float STAM_MULT = 1.2f;
	const int XP_BASE = 100;
	const float XP_MULT = 2;
	const float ATTACK_MULT = 1.2f;	
	const int ATTACK_BASE = 6;
	const int CRIT_MULT = 2;
	
	const int SPEED_BASE = 5;
	const int FEMALE_EXTRA_STAMINA = 20;
	const int MALE_EXTRA_DAMAGE = 12;

	const float HIT_CHANCE_BASE = 0.6f;
	const float DIZZY_HC_REDUCE = 0.3f;

	const float RUN_STAMINA_DRAIN = 1;
	const float REGEN_HP = 0.1f;
	const float REGEN_STAMINA = 0.1f;
	const float REGEN_ATTACK_DELAY = 3;	
	const float REGEN_INTERVAL = 6;

	const int WARP_UNLOCK_LEVEL = 6;
	const int XP_GAIN_PER_MONSTER_LEVEL = 20;

	//TODO: Ons kan hierdie const maak?
	private int maxInventory = 15;
	private int maxStorage = 50;

	private Sounds sound;

	/**
	 * Singelton
	 */
	public static PlayerAttributes instance;
	
	public LinkedList <InventoryItem> inventory = new LinkedList<InventoryItem>();
	public LinkedList <InventoryItem> storage = new LinkedList<InventoryItem> ();
	private LinkedList <InventoryItem> inventory_LevelStart = new LinkedList<InventoryItem>();
	private LinkedList <InventoryItem> storage_LevelStart = new LinkedList<InventoryItem>();
	public LinkedList <Accessory> accessories = new LinkedList<Accessory>();
	public Weapon weapon = null;

	/**
	 * Die volgende kan gewoon geaacess word soos public varaibles
	 */ 

	public int hp {get; private set;}	
	public int level {get; private set;}
	public float stamina {get; set;}
	public int xp {get; private set;}
	public bool dizzy {get; set;}
	
	public char gender {get {
			return gender;
		}

		set {
			if (value == 'f') {
				stamina += FEMALE_EXTRA_STAMINA;
			}
			this.gender = value;
		}}

	public int speed {
		get {
			int tmp = SPEED_BASE;
			foreach (Accessory a in accessories) {
				tmp += a.speed;
			}
			return tmp;
		}
	}

	public int damage { get {
			int tmp = baseDamage ();
			if (gender == 'm') {
				tmp += MALE_EXTRA_DAMAGE;
			}
			if (weapon != null)
				tmp += weapon.damage;
			foreach (Accessory a in accessories) {
				tmp += a.damage;
			}
			return tmp;
		}
	}

	public int inventorySize {get {
			int tmp = maxInventory;
			foreach (Accessory a in accessories) {
				tmp += a.inventory;
			}
			return tmp;
		}
	}
	/**
	 * last time damage taken
	 */ 
	public float lastDamage {get; set;}
	private float nextRegeneration;
	private PlayerController playerScript;
	public static bool giveAlarm;

	/**
	 *  Functions
	 * */
	void Start () {
		if (instance) {
			DestroyImmediate(gameObject);
			return;
		} else {
			DontDestroyOnLoad (gameObject);
			instance = this;
		}
				
		gender = '?';
		giveAlarm = true;
		dizzy = false;

		sound = GameObject.Find("Player").GetComponent<Sounds>();
		setInitialXp(0);
		nextRegeneration = Time.time + REGEN_INTERVAL;
		lastDamage = 0;
	}

	void Update() {
		/* Called Once per frame */
		/** Health regenration etc. */
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();
		
		if (!playerScript.getPaused ()) {
			
			if (this.hp <= 50 && giveAlarm) {//(this.maxHP()/4)){
				GameObject.Find ("Player").GetComponent<Sounds> ().playAlarmSound (1);
			} else {
				GameObject.Find ("Player").GetComponent<Sounds> ().stopAlarmSound (1);
			}
			
			string tempMessage = levelUp ();
			if (tempMessage != "") {
				PlayerLog.addStat (tempMessage);
			//	print (tempMessage);
			}

			Warping warp = this.gameObject.GetComponent<Warping> ();
			if (level == WARP_UNLOCK_LEVEL && warp != null) {
				warp.chooseDestinationUnlocked = true;
			}
			
			if (Time.time >= nextRegeneration) {
				nextRegeneration = Time.time + REGEN_INTERVAL;
				if (Time.time >= (lastDamage + REGEN_ATTACK_DELAY)) {
					if (hp < maxHP ()) {
						hp += (int)(maxHP () * REGEN_HP);
						giveAlarm = false;
					}

					if (stamina < maxStamina ()) {
						stamina += (int)(maxStamina () * REGEN_STAMINA);
					}
				}
				
			}
		} else {
			nextRegeneration = Time.time + REGEN_INTERVAL;
			//lastDamage += 1;
		}
	}

	public void setInitialXp(int xp) {
		this.xp = xp;		
		level = determineLevel ();
		hp = maxHP();
		stamina = maxStamina ();
	}

	public void setAttributes (int xp, LinkedList <InventoryItem> inventory, int inventoryMax, LinkedList <InventoryItem> storage, int storageMax){

		setInitialXp(xp);
		maxStorage = storageMax;
		maxInventory = inventoryMax;
		/**
		 * Storage and inventory at start of level
		 */ 
		this.inventory = inventory;
		foreach (InventoryItem item in inventory) {
			inventory_LevelStart.AddLast (item);
		}
		
		foreach (InventoryItem item in storage) {
			storage_LevelStart.AddLast(item);
		}
	}
	
	public void setAttributes (int xp, LinkedList <InventoryItem> inventory, int inventoryMax){
		this.xp = xp;
		this.inventory = inventory;
		this.maxInventory = inventoryMax;
		this.level = determineLevel ();
		this.hp = maxHP();
		this.stamina = maxStamina ();
	}



	public void addHealth(int val){
		this.hp += val;
		if (this.hp > this.maxHP ()) {
			this.restoreHealthToFull();
		}
	}

	public void drainStamina(){
		this.stamina -= RUN_STAMINA_DRAIN;//0.5f;
	}

	public void useHealthPack(InventoryItem item){
		HealthPack tempItem = (HealthPack)item;
		int healthRegen = (int)(maxHP() * tempItem.healthValue);
		addHealth(healthRegen);
	//	print ("adding " + healthRegen);
	}	

	public float hitChance { get {
		float tmp = HIT_CHANCE_BASE;

		foreach (Accessory a in accessories) {
			tmp += a.hitChance;
		}
		
		//if dizzy lose some hitChance
		if (dizzy) {
			tmp -= DIZZY_HC_REDUCE;
		}

		return tmp;
		}
	}

	public float critChance() {
		float tmp = 0.05f;
		foreach (Accessory a in accessories) {
			tmp += a.critChance;
		}
		return tmp;
	}

	public int getMaxInventory()
	{
		int tmp = maxInventory;
		foreach (Accessory a in accessories) {
			tmp += a.inventory;
		}
		return tmp;
	}

	public int getMaxStorage(){
		return maxStorage;
	}
		

	public string levelUp() {
		int nextTreshold = getExpctedXP();
		if (xp > nextTreshold) {
			level++;
			this.hp = maxHP();
			this.stamina = maxStamina();
			
			return "You  are now level " + level;
		}
		return "";
	}

	/**
	 * Cheat
	 */
	public void levelMeUp(){
		this.xp = levelXP (level);
		levelUp();
	}

	public bool equipItem(InventoryItem item) {
		switch (item.type) {
		case 0:
			return equipAccessory((Accessory) item);
		case 1:
			return equipWeapon((Weapon) item);
		default:
			throw new RulesException("Unknown Item");
		}
	}

	public bool unequipWeapon(){
		if (weapon != null) 
			inventory.AddLast(weapon);
		weapon = null;
	}
	/**
	 * Self Explanatory
	 * Return Error or Success Message
	 * */
	private bool equipWeapon(Weapon weap) {
		if (weap == null)
			throw new System.ArgumentNullException ("weapon");
		unequipWeapon();

		if (weap.level <= level) {
			this.weapon = weap;
			return true;
		}
		throw new RulesException("Weapon Level too high");
	}


	private bool equipAccessory(Accessory a) {
		if (accessories.Count < maxAccessories()) {
			accessories.AddLast(a);
			if(a.speed != 0){
				GameObject.Find("Player").GetComponent<PlayerController>().moveSpeed += a.speed;
			}
			return true;
		} 
		throw new RulesException("Unequip another one first");
	}

	public bool unequipAccessory(Accessory a) {
		if (accessories.Contains (a)) {
			accessories.Remove (a);
			inventory.AddLast (a);
			return true;
		}
		throw new RulesException ("Accessory not equipped");
	}



	public bool addToStorage(InventoryItem item){
		if (storage.Count < maxStorage) {
			storage.AddLast (item);
			return true;
		} else
			throw new RulesException ("Storage full");
	}

	public bool addToInventory(InventoryItem a) {
		if (inventory.Count < maxInventory) {
			
			inventory.AddLast(a);
			return true;
		}  else {
			throw new RulesException("Inventory Full");
		}
	}

	public void restoreHealthToFull()
	{
		hp = maxHP ();
	}

	public void restoreStaminaToFull(){
		stamina = maxStamina ();
	}

	public void resetXP(){
		if (level >= 2) {
			xp = levelXP (level - 1);
		} else 
			xp = 0;
	}

	public void saveInventoryAndStorage(){
		inventory_LevelStart.Clear ();
		foreach (InventoryItem item in inventory) {
			inventory_LevelStart.AddLast (item);
		}
		
		storage_LevelStart.Clear ();
		foreach (InventoryItem item in storage) {
			storage_LevelStart.AddLast(item);
		}
	}

	public void resetInventoryAndStorage(){
		inventory.Clear ();
		foreach (InventoryItem item in inventory_LevelStart) {
			inventory.AddLast (item);
		}

		storage.Clear ();
		foreach (InventoryItem item in storage_LevelStart) {
			storage.AddLast(item);
		}
	}

	public string attack(Enemy e) {
		//string name = e.typeID;
		e.lastDamage = Time.time;
		float ran = Random.value;
		float hc = hitChance;
		string message = "Miss! ";

		if (ran <= hc) {			
			message = "Hit! ";
		
			float cc = critChance ();
			int tmpdamage = damage;

			/*Critical Hit */
			if (ran <= cc) {

				tmpdamage *= CRIT_MULT;
				message = "Critical Hit! ";

				if (weapon != null){
					if (weapon.typeID == "Warhammer") {
						sound.playCharacterSound (11);
					} else {
						sound.playCharacterSound (8);
					}
				} else {
					sound.playCharacterSound (13);
				}

			} else {
				/* Non-crit SOunds */
				if (weapon != null){
					if (weapon.typeID == "Warhammer") {
						sound.playCharacterSound (10);
					} else {
						sound.playCharacterSound (7);
					}
				} else {
					sound.playCharacterSound (12);
				}
			}

			bool dead = e.loseHP(tmpdamage);

			if (weapon != null) {
				//print (this.stamina);
				stamina -= weapon.staminaLoss;
				//print (this.stamina);
			}
			if (dead) {					
				message += " Monster died!\n";
				message += addXP(e.level * XP_GAIN_PER_MONSTER_LEVEL);
			} else {
				//add Stats to message
				message += e.getHealth()+"/"+e.getMaxHp();
			}
		} 
		if (message == "Miss! ") {
			sound.playCharacterSound(9);
			message += e.getHealth () + "/" + e.getMaxHp ();
		}
		print(message);
		PlayerLog.addStat (message);
		return message;
	}

	/**
	 * Returns true if dead
	 * */
	public bool loseHP(int loss) {
		this.hp -= loss;
		return isDead ();
	}

	/**
	 * Returns the XP needed to get to level n
	 */ 
	public int levelXP(int n) {
		return (int) (XP_BASE * Mathf.Pow (XP_MULT, n - 1) / (XP_MULT - 1)) - XP_BASE;
	}

	public int getExpctedXP() {
		return levelXP(level+1);
	}

	/**
	 * Det
	public int determineLevel() {
		if (xp == 0) return 1;

		return Mathf.RoundToInt(Mathf.Log (xp * (XP_MULT - 1) / XP_BASE) / Mathf.Log (XP_BASE))+1.5;
	}
	
	public int maxHP() {
		var tmp = Mathf.RoundToInt(HP_BASE * Mathf.Pow(HP_MULT, level -1));		
		foreach (Accessory a in accessories) {
			tmp += a.hp;
		}
		return tmp;
	}
	
	public int maxStamina() {
		var tmp =  Mathf.RoundToInt(HP_BASE * Mathf.Pow(HP_MULT, level -1));	
		foreach (Accessory a in accessories) {
			tmp += a.stamina;
		}
		if (gender == 'f')
			tmp += FEMALE_EXTRA_STAMINA;
		return tmp;
	}

	private int baseDamage() {
		return  Mathf.RoundToInt((ATTACK_BASE + level -1) * Mathf.Pow (ATTACK_MULT, level - 1));
	}
	
	public int maxAccessories() {
		return (level >= 5) ? 2 : 1;
	}

	public bool isDead() {
		return hp <= 0;
	}

	public string addXP(int value){
		xp += value;
		return levelUp();
	}
}
