using UnityEngine;
/**
 * Only from level 5. Its not restricted here but level < 5 will have damage 0 OR 1.
 * */
public class ButterKnife: Weapon {
	const float MULT = 1.4f;
	const float BASE_DAMAGE = 73;
	const float STAMINA_LOSS = 3;
	const string ID = "ButterKnife";
	public ButterKnife(int level) : base(level, Mathf.RoundToInt(BASE_DAMAGE * Mathf.Pow(MULT, level-5)) , STAMINA_LOSS, ID) {}
}
