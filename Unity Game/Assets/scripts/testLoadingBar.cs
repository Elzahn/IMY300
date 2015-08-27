using UnityEngine;
using System.Collections;

public class testLoadingBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	//	barDisplay = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
		// for this example, the bar display is linked to the current time,
		// however you would set this value based on your desired display
		// eg, the loading progress, the player's health, or whatever.
		/*if (Input.GetKeyDown (KeyCode.Space)) {

			add ();
		}
		if (Input.GetKeyDown (KeyCode.C)) {
			addTwo ();
		}*/
		barDisplay = Time.time * 0.5f;//Time.time * 0.05f;//0.1 = 1 unit//0.005f good for constant update
	}

	void addTwo(){
		barDisplay += 2 * 0.1f;
	}

	void add(){
		barDisplay += 0.1f;
	}

	float barDisplay = 0;
	Vector2 pos = new Vector2(20,40);
	Vector2 size = new Vector2(120,40);
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;
	
	void OnGUI()
	{
		
		// draw the background:
		GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
		GUI.DrawTexture (new Rect (0,0, size.x, size.y), progressBarEmpty);
		
		// draw the filled-in part:
		GUI.BeginGroup (new Rect (0, 0, size.x * barDisplay, size.y));
		GUI.DrawTexture (new Rect (0,0, size.x, size.y), progressBarFull);
		GUI.EndGroup ();
		
		GUI.EndGroup ();
		
	} 
}
