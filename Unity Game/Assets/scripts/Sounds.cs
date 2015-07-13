using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	public AudioClip[] worldClips;
	public AudioClip[] ambienceClips;
	public AudioClip[] characterClips;
	public static AudioSource worldAudio, characterAudio, ambienceAudio;
	private int worldClip, characterClip, ambienceClip;

	void Start(){
		worldAudio = GameObject.Find ("World Audio").GetComponent<AudioSource> ();
		ambienceAudio = GameObject.Find ("Ambience Audio").GetComponent<AudioSource> ();
		characterAudio = GameObject.Find ("Character Audio").GetComponent<AudioSource> ();
	}

	public void stopSound(string source){
		switch (source) {
		case "character":
		{
			characterAudio.Stop ();
			break;
		}
		default:
		{
			characterAudio.Stop ();
			if(worldClip != 0 && worldClip != 1 && worldClip != 7){
				worldAudio.Stop ();
			}
			break;
		}
		}
	}

	public void playWorldSound(int clipAt){
		if (worldAudio.isPlaying == true) {
			worldAudio.Stop ();
		}

		worldAudio.loop = false;
		worldAudio.clip = worldClips [clipAt];
		worldAudio.Play ();
		worldClip = clipAt;
	}

	public void playCharacterSound(int clipAt){
		characterAudio.loop = true;
		characterAudio.clip = characterClips [clipAt];
		characterAudio.Play ();
	}

	public void playSound(string array, int clipAtIndex){
		if (array == "character") {
		//	characterClip = clipAtIndex;
		//	myAudio.clip = audioClips [clipAtIndex];
			//	if (!myAudio.isPlaying) {
			//if (characterClip == 3 || characterClip == 4 || characterClip == 6 || characterClip == 7) {//walking or running
			GameObject.Find ("Audio Source").GetComponent<AudioSource> ().loop = true;
			/*	} else {
				GameObject.Find ("Audio Source").GetComponent<AudioSource> ().loop = false;
			}*/
		//	myAudio.Play ();
			/*} else if(clip != 6 || clip != 7 || clip != 3 || clip != 4){
			playSound(clipAtIndex);
		}*/
		} else {
			GameObject.Find ("Audio Source").GetComponent<AudioSource> ().loop = false;

		//	myAudio.Play ();
		}
	}
}
