using System;
using System.Runtime.Serialization;

[Serializable()]
public class MediumHealthPack : HealthPack {
	
	public MediumHealthPack() : base("Medium Health Pack", 0.2f) {}
	MediumHealthPack (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}
