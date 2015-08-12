using UnityEngine;

public class Longsword: Weapon {
	const float MULT = 1.4f;
	const float BASE_DAMAGE = 10;
	const float STAMINA_LOSS = 1;
	const string ID = "Longsword";
	public Longsword(int level) : base(level, Mathf.RoundToInt(BASE_DAMAGE * Mathf.Pow(MULT, level-1)) , STAMINA_LOSS, ID) {}
}
