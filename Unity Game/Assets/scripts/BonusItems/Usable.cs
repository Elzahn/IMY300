using System;
using System.Runtime.Serialization;

[Serializable()]
public class Usable : InventoryItem {
	
	public Usable(String typeID, int type): base(type, typeID){}

	protected Usable(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}
