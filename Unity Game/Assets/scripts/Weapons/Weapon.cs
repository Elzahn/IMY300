using UnityEngine;
using System.Collections;

public abstract class Weapon : InventoryItem{
	public int damage {get; private set;}
	public int level {get; private set;}
	public float staminaLoss {get; private set;}
	// Use this for initialization
	public Weapon (int level, int damage, float stamina, string typeID) : base(1, typeID) {
		this.damage = damage;
		this.level = level;
		this.staminaLoss = stamina;
	}

}
