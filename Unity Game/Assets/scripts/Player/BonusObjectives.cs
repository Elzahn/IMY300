using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusObjectives : MonoBehaviour {

	public bool killAllMonstersOnLevel{ get; set; }
	public bool killAllMonstersInGame{ get; set; }
	public bool reachTheStars{ get; set; }
	public bool completeAllQuests { get; set;}

	public int deadEnemiesOnLevel { get; set;}
	public int deadEnemies{ get; set; }

	private LevelSelect levelSelect;
	private bool showedBonus1 = false;
	private bool showedBonus2 = false;
	private bool showedBonus3 = false;
	private bool showedBonus4 = false;
	// Use this for initialization
	void Start () {
		killAllMonstersInGame = false;
		killAllMonstersOnLevel = false;
		reachTheStars = false;
		deadEnemies = 0;
		deadEnemiesOnLevel = 0;
		levelSelect = this.GetComponent<LevelSelect> ();
	}

	void OnTriggerExit(Collider col){
		if (col.name == "Planet") {
			if (!showedBonus4) {
				this.GetComponent<Tutorial> ().makeHint ("You reached the starts! Bonus Objective", this.GetComponent<Tutorial> ().Middle);
				showedBonus4 = true;
				reachTheStars = true;
				this.GetComponent<PlayerAttributes> ().inventory.AddLast (new BonusAccessory ());
			}
		}
	}

	// Update is called once per frame
	void Update () {

		if (reachTheStars && killAllMonstersOnLevel && killAllMonstersInGame && !showedBonus1) {
			completeAllQuests = true;
			this.GetComponent<Tutorial>().makeHint("You completed all bonus objectives! Bonus Objective", this.GetComponent<Tutorial>().Middle);
			showedBonus1 = true;
		}

		if (deadEnemies == 2 && !showedBonus2){
			killAllMonstersInGame = true;
			this.GetComponent<Tutorial>().makeHint("You killed all the enemy lifeforms! Bonus Objective", this.GetComponent<Tutorial>().Middle);
			showedBonus2 = true;
		}

		if(!showedBonus3 && ((deadEnemiesOnLevel == 1 && levelSelect.currentLevel == 1) || (deadEnemiesOnLevel == 35  && levelSelect.currentLevel == 2) || (deadEnemiesOnLevel == 10 && levelSelect.currentLevel == 3) || (deadEnemiesOnLevel == 40 && levelSelect.currentLevel == 4) || (deadEnemiesOnLevel == 50 && levelSelect.currentLevel == 5))) {
			this.GetComponent<Tutorial>().makeHint("You cleared a planet of all enemy lifeforms! Bonus Objective", this.GetComponent<Tutorial>().Middle);
			this.GetComponent<PlayerAttributes>().inventory.AddLast(new BonusWeapon(1));
			showedBonus3 = true;
			killAllMonstersOnLevel = true;
		}

		if (reachTheStars)
			GameObject.Find ("Bonus1StrikeThrough").GetComponent<Text> ().enabled = true;

		if (killAllMonstersInGame) 
			GameObject.Find ("Bonus2StrikeThrough").GetComponent<Text> ().enabled = true;
		
		if(killAllMonstersOnLevel)
			GameObject.Find ("Bonus3StrikeThrough").GetComponent<Text> ().enabled = true;

		if(completeAllQuests)
			GameObject.Find ("Bonus4StrikeThrough").GetComponent<Text> ().enabled = true;
	}
}
