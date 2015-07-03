using UnityEngine;
using System.Collections;

public abstract class Weapon : InventoryItem{
	public int damage {get;};
	public int level {get; };
	public float staminaLoss {get; };
	// Use this for initialization
	public Weapon (int level, int damage, float stamina, string typeID) : base(1, typeID) {
		this.damage = damage;
		this.level = level;
		this.staminaLoss = stamina;
	}

}
