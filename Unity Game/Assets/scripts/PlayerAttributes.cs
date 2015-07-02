using UnityEngine;
using System.Collections.Generic;

public class PlayerAttributes : MonoBehaviour {

	public const int HP_BASE = 100;
	public const float HP_MULT = 1.8f;
	public const int XP_BASE = 100;
	public const float XP_MULT = 2;
	public const int ATTACK_BASE = 6;
	public const float ATTACK_MULT = 1.2f;

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
	private int stamina;
	/**
	 * This must be re-equiped at the start of the level
	 * */
	private LinkedList <Accessory> accessories = new LinkedList<Accessory>();
	private Weapon weapon;

	void Update() {
		/* Called Once per frame */
	}
	public float hitChance() {
		float tmp = 0.6;
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

	public bool attack(Enemy e) {
		return false;
	}

	/**
	 * Returns true if dead
	 * */
	public bool loseHP(int hp) {
		this.hp -= hp;
		return this.hp <= 0;
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

}
