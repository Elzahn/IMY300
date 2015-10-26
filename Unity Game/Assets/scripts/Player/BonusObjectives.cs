using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusObjectives : MonoBehaviour {

	private PlayerAttributes attributes;

	public bool killAllMonstersOnLevel{ 
		get { return attributes.myAttributes.bonusObjs[1];} 
		set { attributes.myAttributes.bonusObjs[1] = value;} 
	}
	public bool killAllMonstersInGame{ 
		get { return attributes.myAttributes.bonusObjs[1];} 
		set { attributes.myAttributes.bonusObjs[1] = value;} 
	}
	public bool reachTheStars{ 
		get { return attributes.myAttributes.bonusObjs[2];} 
		set { attributes.myAttributes.bonusObjs[2] = value;} 
	}
	public bool completeAllQuests { get {
			return killAllMonstersOnLevel && killAllMonstersInGame && reachTheStars;
		}}

	public int deadEnemiesOnLevel { 
		get { return attributes.myAttributes.deadEnemiesOnLevel;} 
		set { attributes.myAttributes.deadEnemiesOnLevel = value;}
	}
	public int deadEnemies{ 
		get { return attributes.myAttributes.deadEnemies;} 
		set { attributes.myAttributes.deadEnemies = value;}
	}

	private LevelSelect levelSelect;
	private bool showedBonus1 {
		get { return attributes.myAttributes.bonusObjsShown[0];} 
		set { attributes.myAttributes.bonusObjsShown[0] = value;} 
	}
	private bool showedBonus2{
		get { return attributes.myAttributes.bonusObjsShown[1];} 
		set { attributes.myAttributes.bonusObjsShown[1] = value;} 
	}
	private bool showedBonus3 {
		get { return attributes.myAttributes.bonusObjsShown[2];} 
		set { attributes.myAttributes.bonusObjsShown[2] = value;} 
	}
	private bool showedBonus4 {
		get { return attributes.myAttributes.bonusObjsShown[3];} 
		set { attributes.myAttributes.bonusObjsShown[3] = value;} 
	}

	// Use this for initialization
	void Start () {		
		attributes = GameObject.Find("Player").GetComponent<PlayerAttributes> ();
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

		if (completeAllQuests && !showedBonus1) {

			this.GetComponent<Tutorial>().makeHint("You completed all bonus objectives! Bonus Objective", this.GetComponent<Tutorial>().Middle);
			showedBonus1 = true;
		}

		if (deadEnemies == 155 && !showedBonus2){
			killAllMonstersInGame = true;
			this.GetComponent<Tutorial>().makeHint("You killed all the enemy lifeforms! Bonus Objective", this.GetComponent<Tutorial>().Middle);
			showedBonus2 = true;
		}

		if(!showedBonus3 && ((deadEnemiesOnLevel == 20 && levelSelect.currentLevel == 1) || (deadEnemiesOnLevel == 35  && levelSelect.currentLevel == 2) || (deadEnemiesOnLevel == 10 && levelSelect.currentLevel == 3) || (deadEnemiesOnLevel == 40 && levelSelect.currentLevel == 4) || (deadEnemiesOnLevel == 50 && levelSelect.currentLevel == 5))) {
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
