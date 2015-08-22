using System;
using System.Runtime.Serialization;

[Serializable()]
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

	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("damage", damage);
		info.AddValue("level", level);
		info.AddValue("stamina", staminaLoss);

	}

	protected Weapon(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
	{
		//Get the values from info and assign them to the appropriate properties
		damage = (int)info.GetValue("damage", typeof(int));
		level = (int)info.GetValue("level", typeof(int));
		staminaLoss = (float)info.GetValue("stamina", typeof(float));
	}

}
