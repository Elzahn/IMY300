using UnityEngine;
using System.Collections;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System;
using System.IO;

[Serializable()]
public abstract class InventoryItem : ISerializable{

	/**
	 *  0 - Accessory
	 *  1 - Weapon
	 *  2 - HealthPack
	 */
	public readonly int type;
	public readonly string typeID;
	public InventoryItem(int t, string typeID) {
		type = t;
		this.typeID = typeID;
	}
	//Deserialization constructor.
	public InventoryItem(SerializationInfo info, StreamingContext ctxt)
	{
		//Get the values from info and assign them to the appropriate properties
		type = (int)info.GetValue("type", typeof(int));
		typeID = (String)info.GetValue("typeID", typeof(string));
	}
	
	//Serialization function.
	public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		//You can use any custom name for your name-value pair. But make sure you
		// read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
		// then you should read the same with "EmployeeId"
		info.AddValue("type", type);
		info.AddValue("typeID", typeID);
	}
}
