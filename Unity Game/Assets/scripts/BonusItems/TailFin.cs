using System;
using System.Runtime.Serialization;

[Serializable()]
public class TailFin : Usable {
	
	public TailFin() : base("TailFin", 3) {}
	
	public TailFin (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}