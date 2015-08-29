using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class LevelSelect : MonoBehaviour {

	public List<Material> materials;
	public static LevelSelect instance;

	private Renderer myRenderer;
	private GameObject planet;

	public int currentLevel { get; set; }

	void Start () {

	}

	void Update () {
		if (myRenderer != null) {
			myRenderer.material = GetMaterial (currentLevel);
		} else {
			planet = GameObject.Find ("Planet");
			if (planet != null) {
				myRenderer = planet.GetComponent<Renderer> ();
			} else {
				myRenderer = null;
			}
		}
	}

	// Use this for initialization
	public void Awake() {
		if(instance == null){
			instance = this; 
		}
	}
	
	public Material GetMaterial(int level){
		if(materials != null && materials.Count > 0 && level >= 0 && level < materials.Count){
			return materials[level];
		}
		return null;
	}
}