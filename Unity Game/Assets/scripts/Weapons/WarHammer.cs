using UnityEngine;

public class Warhammer : Weapon {
	const float MULT = 1.4f;
	const float BASE_DAMAGE = 14;
	const float STAMINA_LOSS = 1.5f;
	const string ID = "Warhammer";
	public Warhammer(int level) : base(level, Mathf.RoundToInt(BASE_DAMAGE * Mathf.Pow(MULT, level-1)) , STAMINA_LOSS, ID) {}
}
