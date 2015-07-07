using UnityEngine;
using System.Collections.Generic;

public class PlayerAttributes : MonoBehaviour {

	const int HP_BASE = 100;
	const float HP_MULT = 1.8f;
	const int XP_BASE = 100;
	const float XP_MULT = 2;
	const float ATTACK_MULT = 1.2f;
	const int CRIT_MULT = 2;

	/** 
	 * We need to persist this between levels....
	 * */
	private int xp = 0;
	public LinkedList <InventoryItem> inventory = new LinkedList<InventoryItem>();
	public LinkedList <InventoryItem> storage = new LinkedList<InventoryItem> ();
	private int maxInventory = 15;
	private bool dizzy = false;
	private int attackBase;
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

	void Update() {
		/* Called Once per frame */
		levelUp ();
		/** Health regenration etc. */
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
		return maxInventory;
	}

	void Start(){
		setAttributes (0, inventory, maxInventory);
		attackBase = 6;
	}

	public void setAttributes (int xp, LinkedList <InventoryItem> inventory, int inventoryMax){
		this.xp = xp;
		this.inventory = inventory;
		this.maxInventory = inventoryMax;
		this.level = determineLevel ();
		this.hp = maxHP();
		this.stamina = maxStamina ();
	}
		
	public string levelUp() {
		int nextTreshold = levelXP (level + 1);
		if (xp >= nextTreshold) {
			level++;
			this.hp = maxHP();
			this.stamina = maxStamina();
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
			if (weap.level <= level) {
				this.weapon = weap;
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
			return true;
		} 
		throw new RulesException("Unequip another one first");
	}

	public bool unequipAccessory(Accessory a) {
		if (accessories.Contains (a)) {
			accessories.Remove (a);
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

	public bool addToInventory(InventoryItem a) {
		if (inventory.Count < maxInventory) {
			
			inventory.AddLast(a);
			return true;
		}  else {
			throw new RulesException("Inventory Full");
		}
	}

	public string attack(Enemy e) {
		float ran = Random.value;
		float hc = hitChance();
		string message = "Miss!";

		if (ran <= hc){			
			message = "Hit! ";
			float cc = critChance ();
			int tmpdamage = damage ();

			if (ran <= cc) {
				tmpdamage *= CRIT_MULT;
				message = "Critical Hit! ";
			}
			bool dead = e.loseHP(tmpdamage);
			if (weapon != null) {
			stamina -= weapon.staminaLoss;
			}
			if (dead) {
				xp += e.xpGain;
				message += levelUp();
			}
		} 
		
		print(message);
		return message;
	}

	/**
	 * Returns true if dead
	 * */
	public bool loseHP(int hp) {
		this.hp -= hp;
		return isDead();
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

		if (tempLevel == 0) {
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
		return tmp;
	}

	public int damage() {
		int tmp = baseAttack ();
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
		return this.hp;
	}

	public int getXp(){
		return this.xp;
	}

	public int getLevel(){
		return this.level;
	}

	public float getStamina(){
		return this.stamina;
	}

	public void addXP(int value){
		this.xp += value;
	}
}
