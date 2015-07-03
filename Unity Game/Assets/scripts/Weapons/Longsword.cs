using UnityEngine;

public class Longsword: Weapon {
	const float MULT = 1.4;
	const float BASE_DAMAGE = 14;
	const float STAMINA_LOSS = 1;
	const string ID = "Longsword";
	public Longsword(int level) : base(level, BASE_DAMAGE * Mathf.Pow(MULT, level-1) , STAMINA_LOSS, ID) {}
}
