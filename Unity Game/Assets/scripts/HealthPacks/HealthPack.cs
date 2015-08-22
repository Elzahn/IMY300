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
		//Get the values from info and assign them to the appropriate properties
		healthValue = (float)info.GetValue("health", typeof(float));
	}
	
	//Serialization function.
	public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		//You can use any custom name for your name-value pair. But make sure you
		// read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
		// then you should read the same with "EmployeeId"
		base.GetObjectData(info, ctxt);
		info.AddValue("health", healthValue);

	}
}