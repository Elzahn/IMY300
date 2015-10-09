using UnityEngine;
using System.Collections;

public class MovieTexturePlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var movietexture = GetComponent<Renderer>().material.mainTexture as MovieTexture;
		// this line of code will make the Movie Texture begin playing
		if (movietexture != null) {
			movietexture.loop = true;
			(movietexture).Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
