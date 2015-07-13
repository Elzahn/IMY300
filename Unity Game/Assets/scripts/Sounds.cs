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
			if(worldClip != 0 && worldClip != 1 && worldClip != 5 && worldClip != 7 && worldClip != 8 && worldClip != 9 && worldClip != 10 && worldClip != 11){
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
		characterClip = clipAt;

		if (characterClip >= 0 && characterClip <= 3) {
			characterAudio.loop = true;
		} else {
			characterAudio.loop = false;
		}

		characterAudio.clip = characterClips [clipAt];
		characterAudio.Play ();
	}
}
