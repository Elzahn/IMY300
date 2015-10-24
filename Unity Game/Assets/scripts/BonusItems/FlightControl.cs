using System;
using System.Runtime.Serialization;

[Serializable()]
public class FlightControl : Usable {
	
	public FlightControl() : base("Flight Control", 3) {}
	
	public FlightControl (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}