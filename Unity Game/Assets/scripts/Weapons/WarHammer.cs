using UnityEngine;

public class WarHammer : Weapon {
	const float MULT = 1.4;
	const float BASE_DAMAGE = 14;
	public WarHammer(int level) : base(level, BASE_DAMAGE * Mathf.Pow(MULT, level-1) , 1.5, "Warhammer") {}
}
