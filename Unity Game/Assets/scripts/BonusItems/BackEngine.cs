using System;
using System.Runtime.Serialization;

[Serializable()]
public class BackEngine : Usable {
	
	public BackEngine() : base("Back Engine", 3) {}

	public BackEngine (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}