using UnityEngine;
using System.Collections.Generic;

public class PlayerAttributes : MonoBehaviour {

	const int HP_BASE = 100;
	const float HP_MULT = 1.8f;
	const int XP_BASE = 100;
	const float XP_MULT = 2;
	const int ATTACK_BASE = 6;
	const float ATTACK_MULT = 1.2f;
	const int CRIT_MULT = 2;

	/** 
	 * We need to persist this between levels....
	 * */
	private int xp = 0;
	public LinkedList <InventoryItem> inventory = new LinkedList<InventoryItem>();
	private int maxInventory = 15;

	/**
	 * This can be reset/recalculated at start of level
	 * */
	private int hp;	
	private int level;
	private float stamina;
	/**
	 * This must be re-equiped at the start of the level
	 * */
	private LinkedList <Accessory> accessories = new LinkedList<Accessory>();
	private Weapon weapon;

	void Update() {
		/* Called Once per frame */
		/** Health regenration etc. */
	}
	public float hitChance() {
		float tmp = 0.6f;
		foreach (Accessory a in accessories) {
			tmp += a.HitChance;
		}
		return tmp;
	}

	public float critChance() {
		float tmp = 0.05f;
		foreach (Accessory a in accessories) {
			tmp += a.getCritChance();
		}
		return tmp;
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
		if (xp > nextTreshold) {
			level++;
			this.hp = maxHP();
			this.stamina = maxStamina();
		
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
	private bool equipWeapon(Weapon weapon) {
		if (weapon == null)
			throw new System.ArgumentNullException ("weapon");
		//If level >= wepon min then Equip
		if (weapon.level <= level) {
			this.weapon = weapon;
			return true;
		}
		throw new RulesException("Weapon Level too high");
	}

	private bool equipAccessory(Accessory a) {
		if (accessories.Count < maxAccessories()) {

			accessories.AddLast(a);
			return true;
		} 
		throw new RulesException("Unequip another one first");
	}

	private bool unequipAccessory(Accessory a) {
		if (accessories.Contains (a)) {
			accessories.Remove (a);
			return true;
		} else {
				throw new RulesException("Accessory not equippoed");
		}
	}

	public bool addToInventory(InventoryItem a) {
		if (inventory.Count < maxInventory) {
			
			inventory.AddLast(a);
			return true;
		} 
		throw new RulesException("Inventory Full");
	}

	public string attack(Enemy e) {
		float ran = Random.value;
		float hc = hitChance();
		string message = "Miss!";

		if (ran <= hc){			
			message = "Hit! ";
			float cc = critChance ();
			int damage = Damage ();

			if (ran <= cc) {
				damage *= CRIT_MULT;
				message = "Critical Hit! ";
			}
			bool dead = e.loseHP(damage);
			stamina -= weapon.staminaLoss;
			if (dead) {
				xp += e.xpGain;
				message += levelUp();
			}
		} 

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
			tmp += a.Inventory;
		}
		return tmp;
	}

	public static int levelXP(int level) {
		return Mathf.RoundToInt(XP_BASE * Mathf.Pow (XP_MULT, level - 1) / (XP_MULT - 1));
	}
	
	public int determineLevel() {
		return  Mathf.RoundToInt (Mathf.Log (xp * (XP_MULT - 1) / XP_BASE) / Mathf.Log (XP_BASE));
	}
	
	public int maxHP() {
		var tmp = Mathf.RoundToInt(HP_BASE * Mathf.Pow(HP_MULT, level -1));		
		foreach (Accessory a in accessories) {
			tmp += a.HP;
		}
		return tmp;
	}
	
	public int maxStamina() {
		var tmp =  Mathf.RoundToInt(HP_BASE * Mathf.Pow(HP_MULT, level -1));	
		foreach (Accessory a in accessories) {
			tmp += a.Stamina;
		}
		return tmp;
	}

	public int Damage() {
		int tmp = baseAttack () + weapon.damage;
		foreach (Accessory a in accessories) {
			tmp += a.Damage;
		}
		return tmp;
	}
	
	private int baseAttack() {
			return  Mathf.RoundToInt(ATTACK_BASE * Mathf.Pow (ATTACK_MULT, level - 1));
	}
	
	public int maxAccessories() {
		return (level >= 5) ? 2 : 1;
	}

	public bool isDead() {
		return hp <= 0;
	}

	

}
