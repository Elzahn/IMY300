using System;
using System.Runtime.Serialization;

[Serializable()]
public class CommonAccessory : Accessory {

	public CommonAccessory(int itemNum) : base("Common Accessory") {
		switch (itemNum) {
		case 1: {
			stamina = 10;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 2:{
			stamina = 0;
			hp = 10;
			damage = 0;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 3:{
			stamina = 0;
			hp = 0;
			damage = 2;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 4: {
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 1;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 5: {
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 2;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 6:{
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 0;
			hitChance = 2;
			critChance = 0;
			break;
		}
		case 7:{
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 0.5f;
			break;
		}
		}
	}

	CommonAccessory (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}
