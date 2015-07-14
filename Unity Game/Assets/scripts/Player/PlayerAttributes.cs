using UnityEngine;
using System.Collections.Generic;

public class PlayerAttributes : MonoBehaviour {

	const int HP_BASE = 100;
	const float HP_MULT = 1.8f;
	const int XP_BASE = 100;
	const float XP_MULT = 2;
	const float ATTACK_MULT = 1.2f;
	const int CRIT_MULT = 2;

	public static PlayerAttributes Instance;
	/** 
	 * We need to persist this between levels....
	 * */
	private int xp = 0;
	public LinkedList <InventoryItem> inventory = new LinkedList<InventoryItem>();
	public LinkedList <InventoryItem> storage = new LinkedList<InventoryItem> ();
	private int maxInventory = 15;
	private int maxStorage = 50;
	private bool dizzy = false;
	private int attackBase = 6;
	private char gender = '?';
	private Sounds sound;
	private LinkedList<InventoryItem> tempInventory = new LinkedList<InventoryItem>();
	private LinkedList<InventoryItem> tempStorage = new LinkedList<InventoryItem>();
	/**
	 * This can be reset/recalculated at start of level
	 * */
	private int hp;	
	public int level {get; private set;}
	private float stamina;
	/**
	 * This must be re-equiped at the start of the level
	 * */
	public LinkedList <Accessory> accessories = new LinkedList<Accessory>();
	public Weapon weapon = null;
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
				if (Time.time >= (lastDamage+3) && getStamina () < maxStamina ()) {
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

	public void setAttributes (int xp, LinkedList <InventoryItem> inventory, int inventoryMax, int storageMax, int attackBase){
		this.xp = xp;
		this.inventory = inventory;
		this.attackBase = attackBase;
		level = determineLevel ();
		hp = maxHP();
		stamina = maxStamina ();
		maxStorage = storageMax;
		maxInventory = inventoryMax;
		tempInventory = inventory;
		tempStorage = storage;
	}

	public int getExpectedXP()
	{
		return levelXP (level);
	}

	public void LevelMeUp(){	//just for testing
		this.xp = getExpectedXP ();
	}

	public string levelUp() {
		int nextTreshold = levelXP (level);
		if (xp >= nextTreshold) {
			level++;
			hp = maxHP();
			stamina = maxStamina();
			attackBase += 1;
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

	/**
	 * Return Error or Success Message
	 * */
	private bool equipWeapon(Weapon weap) {
		if (weap == null)
			throw new System.ArgumentNullException ("weapon");
		//If level >= wepon min then Equip
		if (weapon == null) {
			if (weap.level <= this.level) {
				weapon = weap;
				return true;
			}
			throw new RulesException ("Weapon Level too high");
		} else {
			throw new RulesException ("Unequip weapon first first");
		}
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

	public bool unequipWeapon(Weapon w){
		if (weapon == w) {
			weapon = null;
			return true;
		}
		throw new RulesException ("Weapon not equipped");
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

	public void resetInventoryAndStorage(){
		inventory = tempInventory;
		storage = tempStorage;
	}

	public string attack(Enemy e) {
		string name = e.typeID;
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
			return  Mathf.RoundToInt(attackBase * Mathf.Pow (ATTACK_MULT, level - 1));
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
}
