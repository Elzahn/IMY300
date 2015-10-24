using System;
using System.Runtime.Serialization;

[Serializable()]
public class LandingGear : Usable {
	
	public LandingGear() : base("Landing Gear", 3) {}
	
	public LandingGear (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}