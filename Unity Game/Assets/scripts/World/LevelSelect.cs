using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Renderer))]
public class LevelSelect : MonoBehaviour {
	public List<Material> materials;
	public static LevelSelect inst;
	private Renderer _rend;

	void Start () {
		_rend = this.GetComponent<Renderer>();
	}

	void Update () {
		if(Application.loadedLevelName == "Tutorial"){
			_rend.material = GetMat("Canopy");
		}else if(Application.loadedLevelName == "Scene"){
			_rend.material = GetMat("Rock_light-05");
		}
	}

	// Use this for initialization
	public void Awake () { if(inst == null){ inst = this; } }
	// Update is called once per frame        public void Update () { }
	
	public Material GetMat(string _name){
		if(materials != null && materials.Count > 0 && _name != null && _name != ""){
			foreach(Material mat in materials){ if(mat.name == _name){ return mat; } }
		}//call getMat here
		return null;
	}
	/*public Material GetMat(int _index){
		if(mats != null && mats.Count > 0 && _index >= 0 && _index < mats.Count){
			return mats[_index];
		}
		return null;
	}
	
	public Texture GetTex(string _name){
		if(mats != null && mats.Count > 0 && _name != null && _name != ""){
			foreach(Material mat in mats){ if(mat.name == _name){ return mat.mainTexture; } }
		}
		return null;
	}
	public Texture GetTex(int _index){
		if(mats != null && mats.Count > 0 && _index >= 0 && _index < mats.Count){
			return mats[_index].mainTexture;
		}
		return null;
	}*/
}

//Use as such
/*using UnityEngine;
using System.Collections;


public class MatChangeExample : MonoBehaviour {
	public int desiredMat;
	public bool useMat1;

	private Material mat0;
	private Material mat1;
	
	// Use this for initialization

		mat0 = _rend.material;
		mat1 = MaterialRepo.inst.GetMat( desiredMat );
	}
	// Update is called once per frame

}*/