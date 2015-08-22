using System;
using System.Runtime.Serialization;

[Serializable()]
public abstract class Accessory : InventoryItem {

	public int damage {get; protected set; }

	public  int stamina {get; protected set;}

	public int hp  { get; protected set; }

	public int hitChance  { get; protected set; }

	public float critChance { get; protected set; }

	public int inventory {get; protected set; }

	public int speed {get; protected set;}

	public Accessory(string typeID) : base(0, typeID) {}

	public override void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData (info, ctxt);

		info.AddValue("damage", damage);
		info.AddValue("hp", hp);
		info.AddValue("stamina", stamina);
		
		info.AddValue("hit", hitChance);
		info.AddValue("crit", critChance);
		info.AddValue("inventory", inventory);
		info.AddValue("speed", speed);
	}

	public Accessory(SerializationInfo info, StreamingContext ctxt) : base (info, ctxt){
		damage = (int)info.GetValue("damage", typeof(int));
		hp = (int)info.GetValue("hp", typeof(int));
		stamina = (int) info.GetValue("stamina", typeof(int));		
		hitChance = (int) info.GetValue("hit", typeof(int));
		critChance = (float) info.GetValue("crit", typeof(float));
		inventory = (int) info.GetValue("inventory", typeof(int));
		speed = (int) info.GetValue("speed", typeof(int));
	}
}
