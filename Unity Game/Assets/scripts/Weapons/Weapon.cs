using UnityEngine;
using System.Collections;

public class Weapon : InventoryItem{
	public readonly int damage;
	public readonly int level;
	// Use this for initialization
	public Weapon (int damage, int level, string typeID) : base(1, typeID) {
		this.damage = damage;
		this.level = level;
	}

}
