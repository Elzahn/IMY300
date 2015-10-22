using System;
using System.Runtime.Serialization;

[Serializable()]
public class BonusAccessory : Accessory {
	
	public BonusAccessory() : base("Bonus Accessory") {
		stamina = 0;
		hp = 7;
		damage = 0;
		speed = 0;
		inventory = 0;
		hitChance = 7;
		critChance = 6;
	}
}