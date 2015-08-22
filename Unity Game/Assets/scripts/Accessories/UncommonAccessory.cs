using System;
using System.Runtime.Serialization;

[Serializable()]
public class UncommonAccessory : Accessory {
	
	public UncommonAccessory(int itemNum) : base("Uncommon Accessory") {
		switch (itemNum) {
		case 1: {
			stamina = 10;
			hp = 10;
			damage = 0;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 2:{
			stamina = 10;
			hp = 0;
			damage = 10;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 3:{
			stamina = 10;
			hp = 0;
			damage = 0;
			speed = 2;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 4: {
			stamina = 0;
			hp = 10;
			damage = 10;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 5: {
			stamina = 0;
			hp = 10;
			damage = 0;
			speed = 2;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 6:{
			stamina = 0;
			hp = 0;
			damage = 10;
			speed = 2;
			inventory = 0;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 7:{
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 4;
			hitChance = 0;
			critChance = 0;
			break;
		}
		case 8:{
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 0;
			hitChance = 4;
			critChance = 0;
			break;
		}
		case 9:{
			stamina = 0;
			hp = 0;
			damage = 0;
			speed = 0;
			inventory = 0;
			hitChance = 0;
			critChance = 0.8f;
			break;
		}
		}
	}

	UncommonAccessory (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}

}
