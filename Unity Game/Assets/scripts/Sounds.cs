using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	public AudioClip[] worldClips;
	public AudioClip[] ambienceClips;
	public AudioClip[] characterClips;
	public AudioClip[] monsterClips;
	public AudioClip[] deathClips;
	public AudioClip[] alarmClips;
	public static AudioSource worldAudio, characterAudio, ambienceAudio, monsterAudio, deathAudio, alarmAudio;
	private int worldClip, characterClip, ambienceClip, monsterClip, deathClip, alarmClip;
	private bool done = false;

	void Start(){
		worldAudio = GameObject.Find ("World Audio").GetComponent<AudioSource> ();
		ambienceAudio = GameObject.Find ("Ambience Audio").GetComponent<AudioSource> ();
		characterAudio = GameObject.Find ("Character Audio").GetComponent<AudioSource> ();
		deathAudio = GameObject.Find ("Death Audio").GetComponent<AudioSource> ();
		alarmAudio = GameObject.Find ("Alarm Audio").GetComponent<AudioSource> ();
	}

	public void stopSound(string source){
		switch (source) {
		case "character":
		{
			characterAudio.Stop ();
			break;
		}
		case "world":
		{
			worldAudio.Stop();
			break;
		}
		/*case "alarm":
		{
			alarmAudio.Stop();
			break;
		}*/
		default:
		{
			characterAudio = GameObject.Find ("Character Audio").GetComponent<AudioSource> ();
			characterAudio.Stop ();
			if(worldClip != 0 && worldClip != 1 && worldClip != 2 && worldClip != 5 && worldClip != 7 && worldClip != 8 && worldClip != 9 && worldClip != 10 && worldClip != 11 && worldClip != 12 && worldClip != 13){
				worldAudio.Stop ();
			}
		/*	GameObject[] temp = GameObject.FindGameObjectsWithTag("Monster");
			foreach(GameObject monster in temp){
				monsterAudio = monster.GetComponent<AudioSource> ();
				monsterAudio.Stop ();
			}*/
			if(alarmClip != 0)
			{
				alarmAudio.Stop ();
			}
			break;
		}
		}
	}

	public void stopMonsterSound(Enemy monster){
		monster.GetComponent<AudioSource> ().Stop();
	}

	public void playAmbienceSound(int clipAt){
		ambienceAudio = GameObject.Find ("Ambience Audio").GetComponent<AudioSource> ();
		ambienceAudio.volume = 0.1f;
		ambienceAudio.loop = true;
		ambienceClip = clipAt;
		ambienceAudio.clip = ambienceClips [clipAt];
		ambienceAudio.Play ();
	}

	public void playMonsterSound(int clipAt, Enemy monster){
		monsterAudio = monster.GetComponent<AudioSource> ();
		monsterAudio.spatialBlend = 1f;
		monsterAudio.rolloffMode = AudioRolloffMode.Linear;
		monsterAudio.minDistance = 12;
		monsterAudio.maxDistance = 40;

		if (clipAt == 0) {
			if (monsterAudio.isPlaying == false && done) {
				monsterAudio.loop = true;
				monsterAudio.clip = monsterClips [clipAt];
				monsterAudio.Play ();
				monsterClip = clipAt;
			}
		} else{// if(monsterAudio.isPlaying == false){
			//monsterAudio.Stop();
			monsterAudio.loop = false;
			monsterAudio.clip = monsterClips [clipAt];
			monsterAudio.Play ();
			monsterClip = clipAt;
		}
	}

	public void stopAlarmSound(int alarm)
	{
		if (alarmClip == alarm) {
			alarmAudio.Stop ();
		}
	}

	public void playDeathSound(int clipAt){
		deathAudio.loop = false;
		deathAudio.clip = deathClips [clipAt];
		deathAudio.Play ();
	}

	public void playAlarmSound(int clipAt){
		if (clipAt == 1) {
			alarmAudio.volume = 0.05f;
		} else {
			alarmAudio.volume = 1;
		}

		if (alarmAudio.isPlaying == false || alarmClip != clipAt) {
			alarmAudio.loop = true;
			alarmAudio.clip = alarmClips [clipAt];
			alarmAudio.Play ();
		}
		alarmClip = clipAt;
	}

	void Update(){
		if (worldAudio.isPlaying == false && worldClip == 3) {
			done = true;
		}
	}

	public void playWorldSound(int clipAt){
		if (worldAudio.isPlaying == true) {
			worldAudio.Stop ();
		}
		worldClip = clipAt;

		if (worldClip == 3) {
			done = false;
		}

		if(worldClip == 4){
			worldAudio.volume = 0.25f;
		} else {
			worldAudio.volume = 1;
		}

		if (clipAt == 12 || clipAt == 13) {
			worldAudio.loop = true;
		} else {
			worldAudio.loop = false;
		}
		worldAudio.clip = worldClips [clipAt];
		worldAudio.Play ();
	}

	public void playCharacterSound(int clipAt){
		characterClip = clipAt;
		characterAudio.volume = 0.5f;

		if (characterClip >= 0 && characterClip <= 3) {
			characterAudio.loop = true;
		} else {
			characterAudio.loop = false;
		}

	/*	if (characterAudio.isPlaying) {
			characterAudio.Stop ();
		}*/
		characterAudio.clip = characterClips [clipAt];
		characterAudio.Play ();
	}
}
