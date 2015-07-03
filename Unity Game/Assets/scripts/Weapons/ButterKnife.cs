using UnityEngine;
/**
 * Only from level 5
 * */
public class ButterKnife: Weapon {
	const float MULT = 1.4;
	const float BASE_DAMAGE = 73;
	const float STAMINA_LOSS = 3;
	const string ID = "ButterKnife";
	public ButterKnife(int level) : base(level, BASE_DAMAGE * Mathf.Pow(MULT, level-5) , STAMINA_LOSS, ID) {}
}
