using UnityEngine;
using System;
using System.Runtime.Serialization;
using System.Collections;

[Serializable()]
public class Cupcake : Accessory {

	public Cupcake() : base("Cupcake") {
		stamina = 0;
		hp = 0;
		damage = 0;
		speed = 0;
		inventory = 0;
		hitChance = 0;
		critChance = 0;
	}

	public static void eatCupcake(){
		GameObject.Find ("Player").GetComponent<PlayerAttributes> ().levelMeUp ();
	}
}
