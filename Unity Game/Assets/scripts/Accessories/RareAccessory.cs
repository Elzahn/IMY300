using System;
using System.Runtime.Serialization;

[Serializable()]
public class RareAccessory : Accessory {
	
	public RareAccessory(int itemNum) : base("Rare Accessory") {
		switch (itemNum) {
		case 1: {
			stamina = 10;
			hp = 10;
			damage = 0;
			speed = 2;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 2:{
			stamina = 10;
			hp = 10;
			damage = 10;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 3:{
			stamina = 0;
			hp = 10;
			damage = 10;
			speed = 2;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 4: {
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 6;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 5: {
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 0;
			hitChance = 6;
			critChance = 0;
			break;
		}
		case 6:{
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 1.2f;
			break;
		}
		}
	}

	RareAccessory (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}