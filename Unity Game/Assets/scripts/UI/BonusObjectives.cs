using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusObjectives : MonoBehaviour {

	public bool killAllMonstersOnLevel{ get; set; }
	public bool killAllMonstersInGame{ get; set; }
	public bool reachTheStars{ get; set; }
	public bool completeAllQuests { get; set;}

	// Use this for initialization
	void Start () {
		killAllMonstersInGame = false;
		killAllMonstersOnLevel = false;
		reachTheStars = false;
	}

	void OnTriggerExit(Collider col){
		if (col.name == "Planet")
			reachTheStars = true;
	}

	// Update is called once per frame
	void Update () {

		if (reachTheStars && killAllMonstersOnLevel && killAllMonstersInGame)
			completeAllQuests = true;

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
