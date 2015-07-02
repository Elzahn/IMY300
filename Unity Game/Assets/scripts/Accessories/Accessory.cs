using UnityEngine;
using System.Collections;

public abstract class Accessory : InventoryItem {

	public readonly int damage;
	public readonly int stamina;
	public readonly int HP;
	public readonly int hitChance;
	public readonly int critChance;
	public readonly int inventory;
	public readonly int speed;

	public Accessory(string typeID) : base(0, typeID) {}
}
