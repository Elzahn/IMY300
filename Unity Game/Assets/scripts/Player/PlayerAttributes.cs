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
	
	public static PlayerAttributes Instance;


	private char gender = '?';
	private Sounds sound;
	
	public LinkedList <InventoryItem> inventory = new LinkedList<InventoryItem>();
	public LinkedList <InventoryItem> storage = new LinkedList<InventoryItem> ();
	private LinkedList <InventoryItem> inventory_LevelStart = new LinkedList<InventoryItem>();
	private LinkedList <InventoryItem> storage_LevelStart = new LinkedList<InventoryItem>();
	public LinkedList <Accessory> accessories = new LinkedList<Accessory>();
	public Weapon weapon = null;


	public int hp {get; private set;}	
	public int level {get; private set;}
	public float stamina {get; set;}
	public int xp {get; private set;};
	private int maxInventory = 15;
	private int maxStorage = 50;
	private bool dizzy = false;
	public int speed {
		get {
			int tmp = SPEED_BASE;
			foreach (Accessory a in accessories) {
				tmp += a.speed;
			}
			return tmp;
		}
	}


	public float lastDamage;
	private float nextRegeneration;
	private float delayRegeneration = 6;
	private PlayerController playerScript;
	public static bool giveAlarm;

	public void addHealth(int val){
		this.hp += val;
		if (this.hp > this.maxHP ()) {
			this.restoreHealthToFull();
		}
	}

	public void setStaminaToZero(){
		this.stamina = 0;
	}

	public void drainStamina(){
		this.stamina -= 1;//0.5f;
	}

	public void useHealthPack(InventoryItem item){
		HealthPack tempItem = (HealthPack)item;
		int healthRegen = (int)(maxHP() * tempItem.healthValue);
		addHealth(healthRegen);
		print ("adding " + healthRegen);
	}

	void Update() {
		/* Called Once per frame */
		/** Health regenration etc. */
		playerScript = GameObject.Find ("Player").GetComponent<PlayerController> ();

		if (playerScript.getPaused () == false) {

			if(this.getHealth() <= 50 && giveAlarm){//(this.maxHP()/4)){
				GameObject.Find("Player").GetComponent<Sounds>().playAlarmSound(1);
			} else {
				GameObject.Find ("Player").GetComponent<Sounds>().stopAlarmSound(1);
			}

			string tempMessage = levelUp ();
			if(tempMessage != ""){
				PlayerLog.addStat(tempMessage);
				print(tempMessage);
			}

			if(level == 6 && GameObject.Find("Player").GetComponent<Warping>() != null){
				GameObject.Find ("Player").GetComponent<Warping>().chooseDestinationUnlocked = true;
			}

			if (Time.time >= nextRegeneration) {
				nextRegeneration = Time.time + delayRegeneration;
				if (Time.time >= (lastDamage+3) && getHealth () < maxHP ()) {
					hp += (int)(maxHP () * 0.01);
					giveAlarm = false;
				}
				if (Time.time >= (lastDamage+3) && stamina < maxStamina ()) {
					stamina += (int)(maxStamina () * 0.01);
				}

			}
		} else {
			nextRegeneration = Time.time + delayRegeneration;
			//lastDamage += 1;
		}
	}

	public void setGender(char gender){
		if (gender == 'f') {
			stamina += 20;
		}
		this.gender = gender;
	}

	public char getGender(){
		return this.gender;
	}

	public void setDizzy(bool value){
		dizzy = value;
	}

	public bool getDizzy(){
		return dizzy;
	}

	public int currentHealth()
	{
		return hp;
	}

	public float hitChance() {
		float tmp = 0.6f;

		foreach (Accessory a in accessories) {
			tmp += a.hitChance;
		}

		//if dizzy lose some hitChance
		if (dizzy) {
			tmp -= 0.3f;
		}

		return tmp;
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
		int tmp = maxStorage;
		/*foreach (Accessory a in accessories) {
			tmp += a.storage;
		}*/
		return tmp;
	}

	void Start(){
		if (Instance) {
			DestroyImmediate(gameObject);
		} else {
			DontDestroyOnLoad (gameObject);
			Instance = this;
		}

		giveAlarm = true;
		sound = GameObject.Find("Player").GetComponent<Sounds>();
		setAttributes (0, inventory, maxInventory, maxStorage, 6);
		nextRegeneration = Time.time + delayRegeneration;
	}

	public void setAttributes (int xp, LinkedList <InventoryItem> inventory, int inventoryMax, int storageMax){
		this.xp = xp;
		this.inventory = inventory;
		level = determineLevel ();
		hp = maxHP();
		stamina = maxStamina ();
		maxStorage = storageMax;
		maxInventory = inventoryMax;
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

	public string levelUp() {
		int nextTreshold = levelXP (level + 1);
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
	public void LevelMeUp(){
		this.xp = levelXP (level);
	}

	public string levelUp() {
		int nextTreshold = levelXP (level);
		if (xp >= nextTreshold) {
			level++;
			hp = maxHP();
			stamina = maxStamina();
			return "You  are now level " + level;
		}
		return "";
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
			if(a.speed != 0){
				GameObject.Find("Player").GetComponent<PlayerController>().moveSpeed -= a.speed;
			}
			return true;
		} else {
				throw new RulesException("Accessory not equipped");
		}
	}



	public bool addToStorage(InventoryItem item){
		if (storage.Count < maxStorage) {
			storage.AddLast (item);
			return true;
		} else {
			throw new RulesException("Storage full");
		}
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
		float hc = hitChance();
		string message = "Miss! ";

		if (ran <= hc) {			
			message = "Hit! ";
			if (weapon != null){
				if (weapon.typeID == "Warhammer") {
					sound.playCharacterSound (10);
				} else {
					sound.playCharacterSound (7);
				}
			} else {
				sound.playCharacterSound (12);
			}
			float cc = critChance ();
			int tmpdamage = damage ();

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
			}

			bool dead = e.loseHP(tmpdamage);

			if (weapon != null) {
				print (this.stamina);
				stamina -= weapon.staminaLoss;
				print (this.stamina);
			}
			if (dead) {
				//xp += e.xpGain;
				//Added XP here
				addXP(getLevel() * 20);
				//message += levelUp();
				message += " Monster died!";
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
	public bool loseHP(int hp) {
		this.hp -= hp;
		return isDead ();
	}

	public int inventorySize() {
		int tmp = maxInventory;
		foreach (Accessory a in accessories) {
			tmp += a.inventory;
		}
		return tmp;
	}

	public static int levelXP(int level) {
		return Mathf.RoundToInt(XP_BASE * Mathf.Pow (XP_MULT, level - 1) / (XP_MULT - 1));
	}
	
	public int determineLevel() {
		int tempLevel = Mathf.RoundToInt (Mathf.Log (xp * (XP_MULT - 1) / XP_BASE) / Mathf.Log (XP_BASE));

		if (tempLevel <= 0) {
			return 1;
		}
		return tempLevel;//Mathf.RoundToInt (Mathf.Log (xp * (XP_MULT - 1) / XP_BASE) / Mathf.Log (XP_BASE));
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
			tmp += 20;
		return tmp;
	}

	public int damage() {
		int tmp = baseAttack ();
		if (gender == 'm') {
			tmp += 12;
		}
		if (weapon != null)
			tmp += weapon.damage;
		foreach (Accessory a in accessories) {
			tmp += a.damage;
		}
		return tmp;
	}
	
	private int baseAttack() {
			return  Mathf.RoundToInt((ATTACK_BASE + level -1) * Mathf.Pow (ATTACK_MULT, level - 1));
	}
	
	public int maxAccessories() {
		return (level >= 5) ? 2 : 1;
	}

	public bool isDead() {
		return hp <= 0;
	}

	public int getHealth(){
		return hp;
	}

	public int getXp(){
		return xp;
	}

	public int getLevel(){
		return level;
	}

	public float getStamina(){
		return stamina;
	}

	public void addXP(int value){
		xp += value;
	}

	public void setAttributes (int xp, LinkedList <InventoryItem> inventory, int inventoryMax){
		this.xp = xp;
		this.inventory = inventory;
		this.maxInventory = inventoryMax;
		this.level = determineLevel ();
		this.hp = maxHP();
		this.stamina = maxStamina ();
	}
}
