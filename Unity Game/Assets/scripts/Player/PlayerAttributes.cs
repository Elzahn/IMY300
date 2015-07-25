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
	public int maxStorage = 50;

	private Sounds soundComponent;
	private PlayerController controllerComponent;

	/**
	 * Singleton
	 */
	public static PlayerAttributes instance;

	/****************************************************** Inventory en goed  *****************************************************/
	public LinkedList <InventoryItem> inventory = new LinkedList<InventoryItem>();
	public LinkedList <InventoryItem> storage = new LinkedList<InventoryItem> ();
	private LinkedList <InventoryItem> inventory_LevelStart = new LinkedList<InventoryItem>();
	private LinkedList <InventoryItem> storage_LevelStart = new LinkedList<InventoryItem>();
	public LinkedList <Accessory> accessories = new LinkedList<Accessory>();


	/******************************************************* Public properties *****************************************************/ 
	public Weapon weapon;
	public int hp {get; private set;}	
	public int level {get; private set;}
	public float stamina {get; set;}
	public int xp {get; private set;}
	public bool dizzy {get; set;}
	
	public char gender {get ; private set ;}
		
	public void setGender(char value) {
		if (value == 'f') {
			stamina += FEMALE_EXTRA_STAMINA;
		}
		gender = value;
	}

	public int speed {
		get {
			int tmp = SPEED_BASE;
			foreach (Accessory a in accessories) {
				tmp += a.speed;
			}
			return tmp;
		}
	}

	public int damage { 
		get {
			int tmp = baseDamage ();
			if (gender == 'm') {
				tmp += MALE_EXTRA_DAMAGE;
			}
			if (weapon != null) {
				tmp += weapon.damage;
			}
			foreach (Accessory a in accessories) {
				tmp += a.damage;
			}
			return tmp;
		}
	}

	public int inventorySize {
		get {
			int tmp = maxInventory;
			foreach (Accessory a in accessories) {
				tmp += a.inventory;
			}
			return tmp;
		}
	}

	public float hitChance { 
		get {
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
	
	public float critChance { 
		get {
			float tmp = 0.05f;
			foreach (Accessory a in accessories) {
				tmp += a.critChance;
			}
			return tmp;
			}
	}

	public int maxAccessories { 
		get {
			return (level >= 5) ? 2 : 1;
		}
	}

	public float lastDamage {get; set;} //Last time damage taken
	private float nextRegeneration;
	public static bool giveAlarm;


	/**************************************************** Monobehaviour functions *********************************************
	 * Start - Called after creation
	 * Update - Called Every frame
	 * ***********************************************************************************************************************/
	void Start () {
		//Singleton
		if (instance) {
			DestroyImmediate(gameObject);
		} else {
			DontDestroyOnLoad (gameObject);
			instance = this;
		}
				
		giveAlarm = true;
		this.gender = '?';
		this.dizzy = false;

		this.setInitialXp(0);
		this.nextRegeneration = Time.time + REGEN_INTERVAL;
		this.lastDamage = 0;
		
		this.soundComponent = GameObject.Find("Player").gameObject.GetComponent<Sounds>(); //must be GameObject.Find("Player") else it tries to acces what has been destroyed
		this.controllerComponent = GameObject.Find("Player").gameObject.GetComponent<PlayerController> (); //must be GameObject.Find("Player") else it tries to acces what has been destroyed
	}

	void Update() {
		if (stamina < 0) 
			stamina = 0;
		// Health regenration etc.
		if (!controllerComponent.getPaused ()) {
			
			if (this.hp <= 50 && giveAlarm) {
				soundComponent.playAlarmSound (1);
			} else {
				soundComponent.stopAlarmSound (1);
			}
			
			string tempMessage = levelUp ();
			if (tempMessage != "") {
				PlayerLog.addStat (tempMessage);			
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
						stamina += (maxStamina () * REGEN_STAMINA);
					}
				}
				
			}
		} else {
			nextRegeneration = Time.time + REGEN_INTERVAL;
		}
	}

	/************************************************************************ Initialization *********************************************************************/
	
	public void setInitialXp(int xp) {
		this.xp = xp;		
		level = determineLevel ();
		hp = maxHP();
		stamina = maxStamina ();
	}

	public void setAttributes (int xp, LinkedList <InventoryItem> inventory, int inventoryMax, LinkedList <InventoryItem> storage, int storageMax) {
		
		setInitialXp(xp);
		maxStorage = storageMax;
		maxInventory = inventoryMax;
		
		this.inventory = inventory;
		this.storage = storage;
		/**
		 * Storage and inventory at start of level
		 */ 
		foreach (InventoryItem item in inventory) {
			inventory_LevelStart.AddLast (item);
		}
		
		foreach (InventoryItem item in storage) {
			storage_LevelStart.AddLast(item);
		}
	}

	/********************************************************** Level and XP *********************************************************************/
	public string levelUp() {
		int nextTreshold = getExpectedXP();
		if (xp > nextTreshold) {
			level++;
			hp = maxHP ();
			stamina = maxStamina ();
			
			return "You  are now level " + level;
		}
		return "";
	}
	
	/** Cheat */
	public void levelMeUp(){
		xp = levelXP (level);
		levelUp();
	}

	/** Returns the XP needed to get to level n */ 
	public int levelXP(int n) {
		return (int) (XP_BASE * (Mathf.Pow (XP_MULT, n - 1) / (XP_MULT - 1) - 1));
	}
	
	public int getExpectedXP() {
		return levelXP(level+1);
	}
	
	/** Determine level based on xp - Dont doubt the math, I tested it this time */ 
	public int determineLevel() {
		if (xp == 0) return 1;
		
		return Mathf.RoundToInt(Mathf.Log ( ((xp + XP_BASE) * (XP_MULT - 1) / XP_BASE) / Mathf.Log (XP_MULT) +0.5f));
	}


	
	public void resetXP(){
		xp = levelXP (level);
	}
	
	public string addXP(int value){
		xp += value;
		return levelUp();
	}

	/***************************** Inventory ************************************/

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
		if (weapon != null)  {
			inventory.AddLast(weapon);
		}
		weapon = null;
		return true;
	}
	/**
	 * Self Explanatory
	 * Return Error or Success Message
	 */
	bool equipWeapon(Weapon weap) {
		if (weap == null) {
			throw new System.ArgumentNullException ("weapon");
		}
		unequipWeapon();

		if (weap.level <= level) {
			this.weapon = weap;
			return true;
		}
		throw new RulesException("Weapon Level too high");
	}

	bool equipAccessory(Accessory a) {
		if (accessories.Count < maxAccessories){
			accessories.AddLast(a);		
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
		}
		throw new RulesException ("Storage full");
	}

	public bool addToInventory(InventoryItem a) {
		if (inventory.Count < maxInventory) {
			
			inventory.AddLast (a);
			return true;
		}
		throw new RulesException ("Inventory Full");
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

	/************************ Health and Stamina *********************/
	public void useHealthPack(InventoryItem item) {
		HealthPack tempItem = (HealthPack) item;
		int healthRegen = (int)(maxHP() * tempItem.healthValue);
		addHealth(healthRegen);
	}	

	public void addHealth(int val){
		this.hp += val;
		if (this.hp > this.maxHP ()) {
			restoreHealthToFull();
		}
	}

	public void restoreHealthToFull() {
			hp = maxHP ();
	}
	

	/** Returns true if dead */
	public bool loseHP(int loss) {
		hp -= loss;
		return isDead ();
	}
	
	public int maxHP() {
		var tmp = Mathf.RoundToInt(HP_BASE * Mathf.Pow(HP_MULT, level -1));		
		foreach (Accessory a in accessories) {
			tmp += a.hp;
		}
		return tmp;
	}
	
	public bool isDead() {
		return hp <= 0;
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
		
	public void drainStamina(){
		this.stamina -= RUN_STAMINA_DRAIN;//0.5f;
	}
	
	public void restoreStaminaToFull(){
		stamina = maxStamina ();
	}

	/*********************** A*****************************/

	public string attack(Enemy e) {
		//string name = e.typeID;
		e.lastDamage = Time.time;
		float ran = Random.value;
		float hc = hitChance;
		string message = "Miss! ";

		if (ran <= hc) {			
			message = "Hit! ";
		
			float cc = this.critChance;
			int tmpdamage = damage;

			/*Critical Hit */
			if (ran <= cc) {

				tmpdamage *= CRIT_MULT;
				message = "Critical Hit! ";

				if (weapon != null){
					if (weapon.typeID == "Warhammer") {
						soundComponent.playCharacterSound (11);
					} else {
						soundComponent.playCharacterSound (8);
					}
				} else {
					soundComponent.playCharacterSound (13);
				}

			} else {
				/* Non-crit Sounds */
				if (weapon != null){
					if (weapon.typeID == "Warhammer") {
						soundComponent.playCharacterSound (10);
					} else {
						soundComponent.playCharacterSound (7);
					}
				} else {
					soundComponent.playCharacterSound (12);
				}
			}

			bool dead = e.loseHP(tmpdamage);

			if (weapon != null) {
				stamina -= weapon.staminaLoss;
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
			soundComponent.playCharacterSound(9);
			message += e.getHealth () + "/" + e.getMaxHp ();
		}

		PlayerLog.addStat (message);
		return message;
	}


	int baseDamage() {
		return  Mathf.RoundToInt((ATTACK_BASE + level -1) * Mathf.Pow (ATTACK_MULT, level - 1));
	}


}
