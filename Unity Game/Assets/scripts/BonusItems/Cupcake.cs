using UnityEngine;
using System;
using System.Runtime.Serialization;
using System.Collections;

[Serializable()]
public class Cupcake : Usable {

	public Cupcake() : base("Cupcake", 2) {}

	public static void eatCupcake(){
		GameObject.Find ("Player").GetComponent<PlayerAttributes> ().levelMeUp ();
	}
}
