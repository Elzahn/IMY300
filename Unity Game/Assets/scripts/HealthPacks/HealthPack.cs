using System.Runtime.Serialization;
using System;

[Serializable()]
public abstract class HealthPack : InventoryItem {

	public readonly float healthValue;
	public HealthPack(string typeID, float healthValue) : base(2, typeID) {
		this.healthValue = healthValue;
	}

	public HealthPack(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
	{

		healthValue = (float)info.GetValue("health", typeof(float));
	}
	
	//Serialization function.
	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		base.GetObjectData(info, ctxt);
		info.AddValue("health", healthValue);
	}
}