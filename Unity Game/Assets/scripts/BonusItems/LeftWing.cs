using System;
using System.Runtime.Serialization;

[Serializable()]
public class LeftWing : Usable {
	
	public LeftWing() : base("Left Wing", 3) {}
	
	public LeftWing (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}