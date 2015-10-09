using System;
using System.Runtime.Serialization;

[Serializable()]
public class PowerCore : Usable {
	
	public PowerCore() : base("Power Core", 3) {}

	public PowerCore (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}