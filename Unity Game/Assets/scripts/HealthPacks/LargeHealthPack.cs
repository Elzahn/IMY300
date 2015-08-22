using System;
using System.Runtime.Serialization;

[Serializable()]
public class LargeHealthPack : HealthPack {
	
	public LargeHealthPack() : base("Large Health Pack", 0.4f) {}
	LargeHealthPack (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}
