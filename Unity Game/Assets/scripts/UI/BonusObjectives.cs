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
		if (col.name == "Planet")
			reachTheStars = true;
	}

	// Update is called once per frame
	void Update () {

		if (reachTheStars && killAllMonstersOnLevel && killAllMonstersInGame)
			completeAllQuests = true;

		if (deadEnemies == 155)
			killAllMonstersInGame = true;

		if((deadEnemiesOnLevel == 20 && levelSelect.currentLevel == 1) || (deadEnemiesOnLevel == 35  && levelSelect.currentLevel == 2) || (deadEnemiesOnLevel == 10 && levelSelect.currentLevel == 3) || (deadEnemiesOnLevel == 40 && levelSelect.currentLevel == 4) || (deadEnemiesOnLevel == 50 && levelSelect.currentLevel == 5))
			killAllMonstersOnLevel = true;

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
