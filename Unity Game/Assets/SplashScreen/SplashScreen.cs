using UnityEngine;
using System.Collections;

/**
 * This class displays the splash screen for 'timer' amount of time and 
 * then loads the level in 'levelToLoad'. 
 * */
public class SplashScreen : MonoBehaviour {
	public float timer = 2f;
	public string levelToLoad = "Main_Menu";
	// Use this for initialization
	void Start () {
		StartCoroutine ("DisplayScene");
	}

	IEnumerator DisplayScene(){
		yield return new WaitForSeconds (timer);
		Application.LoadLevel (levelToLoad);
	}

}
