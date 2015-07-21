using UnityEngine;
using System.Collections.Generic;

public class oldAtributes : MonoBehaviour {
{
	public int hp {get; private set;}	
	public int level {get; private set;}
	public float stamina {get; set;}
	private LinkedList <Accessory> accessories = new LinkedList<Accessory>();
	private Weapon weapon;
	
	void Update() {
		/* Called Once per frame */
		/** Health regenration etc. */
	}
	public float hitChance() {
		float tmp = 0.6f;
		foreach (Accessory a in accessories) {
			tmp += a.hitChance;
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
	
	
	/**
	 * Self Explanatory
	* Return Error or Success Message
	* */
	private bool equipWeapon(Weapon weap) {
		if (weap == null)
			throw new System.ArgumentNullException ("weapon");
		//If level >= wepon min then Equip
		if (weap.level <= level) {
			this.weapon = weap;
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
			int tmpdamage = damage ();
			
			if (ran <= cc) {
				tmpdamage *= CRIT_MULT;
				message = "Critical Hit! ";
			}
			bool dead = e.loseHP(tmpdamage);
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
			tmp += a.inventory;
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
			tmp += a.hp;
		}
		return tmp;
	}
	
	public int maxStamina() {
		var tmp =  Mathf.RoundToInt(STAM_BASE * Mathf.Pow(STAM_MULT, level -1));	
		foreach (Accessory a in accessories) {
			tmp += a.stamina;
		}
		return tmp;
	}
	
	public int damage() {
		int tmp = baseAttack () + weapon.damage;
		foreach (Accessory a in accessories) {
			tmp += a.damage;
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

