using UnityEngine;
using System;
using System.Runtime.Serialization;

[Serializable()]
public class BonusWeapon : Weapon {
	const float MULT = 3.4f;
	const float BASE_DAMAGE = 23;
	const float STAMINA_LOSS = 0.5f;
	const string ID = "BonusWeapon";
	public BonusWeapon(int level) : base(level, Mathf.RoundToInt(BASE_DAMAGE * Mathf.Pow(MULT, level-1)) , STAMINA_LOSS, ID) {}

	BonusWeapon (SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) {}
}