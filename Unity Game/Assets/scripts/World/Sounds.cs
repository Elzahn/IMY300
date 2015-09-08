using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	public AudioClip[] worldClips;
	public AudioClip[] ambienceClips;
	public AudioClip[] characterClips;
	public AudioClip[] monsterClips;
	public AudioClip[] deathClips;
	public AudioClip[] alarmClips;
	public AudioClip[] computerClips;
	public AudioSource worldAudio{ get; set; }
	public AudioSource characterAudio{get; set;}
	public AudioSource ambienceAudio{get; set;}
	public AudioSource monsterAudio{ get; set; }
	public AudioSource deathAudio{ get; set; }
	public AudioSource alarmAudio{get; set;}
	public AudioSource computerAudio{get; set;}
	private int worldClip, characterClip, alarmClip;
	public int computerClip {get; set;}
	private bool done = false;

	//World sounds
	public const int STORAGE = 0;
	public const int INVENTORY = 1;
	public const int BUTTON = 2;
	public const int TELEPORTING = 3;
	public const int HEALTH_COLLECTION = 4;
	public const int USE_HEALTH = 5;
	public const int WARPING = 6;
	public const int MOVE_ITEM = 7;
	public const int DROP_ITEM = 8;
	public const int EQUIP_SWORD = 9;
	public const int EQUIP_HAMMER = 10;
	public const int EQUIP_ACCESSORY = 11;
	public const int SPINNING_WIND = 12;
	public const int EARTHQUAKE = 13;
	public const int POWER_ON = 14;
	public const int LEVEL_UP = 15;

	//Character sounds
	public const int PLANET_WALKING = 0;
	public const int PLANET_RUNNING = 1;
	public const int SHIP_WALKING = 2;
	public const int MALE_HURT = 3;
	public const int FEMALE_HURT = 4;
	public const int SWORD_HIT = 5;
	public const int SWORD_CRIT = 6;
	public const int MISS = 7;
	public const int HAMMER_HIT = 8;
	public const int HAMMER_CRIT = 9;
	public const int FISTS_HIT = 10;
	public const int FISTS_CRIT = 11;

	//Monster sounds
	public const int MONSTER_WALKING = 0;
	public const int MONSTER_FOLLOWING = 1;
	public const int MONSTER_CRIT = 2;
	public const int MONSTER_HIT = 3;
	public const int MONSTER_MISS = 4;

	//Death sounds
	public const int DEAD_MONSTER = 0;
	public const int DEAD_PLAYER = 1;

	//Alarm sounds
	public const int DISASTER_ALARM = 0;
	public const int LOW_HEALTH_ALARM = 1;

	//Ambience sounds
	public const int SHIP_AMBIENCE = 0;
	public const int TUTORIAL_AMBIENCE = 1;
	public const int PLANET_1_AMBIENCE = 2;
	public const int PLANET_2_AMBIENCE = 3;
	public const int PLANET_3_AMBIENCE = 4;
	public const int PLANET_4_AMBIENCE = 5;
	public const int PLANET_5_AMBIENCE = 6;

	//Computer sounds
	public const int COMPUTER_WARP = 0;
	public const int COMPUTER_SATELLITE = 1;
	public const int COMPUTER_RUN = 2;
	public const int COMPUTER_INVENTORY = 3;
	public const int COMPUTER_DISASTERD = 4;
	public const int COMPUTER_GOBACK = 5;
	public const int COMPUTER_STORAGE = 6;
	public const int COMPUTER_FALL = 7;
	public const int COMPUTER_WARPDESTINATION = 8;

	void Start(){
		worldAudio = GameObject.Find ("World Audio").GetComponent<AudioSource> ();
		ambienceAudio = GameObject.Find ("Ambience Audio").GetComponent<AudioSource> ();
		characterAudio = GameObject.Find ("Character Audio").GetComponent<AudioSource> ();
		deathAudio = GameObject.Find ("Death Audio").GetComponent<AudioSource> ();
		alarmAudio = GameObject.Find ("Alarm Audio").GetComponent<AudioSource> ();
		computerAudio = GameObject.Find ("Computer Audio").GetComponent<AudioSource> ();
		computerClip = -1;
	}

	public void resumeSound(string source){
		switch (source) {
			case "character":
			{
				characterAudio.UnPause ();
				break;
			}
			case "world":
			{
				worldAudio.UnPause();
				break;
			}
			case "ambience":
			{
				ambienceAudio = GameObject.Find("Ambience Audio").GetComponent<AudioSource>();
				ambienceAudio.UnPause ();
				break;
			}
		default:
			{
				characterAudio = GameObject.Find ("Character Audio").GetComponent<AudioSource> ();
				characterAudio.UnPause ();
				if(worldClip == TELEPORTING || worldClip == HEALTH_COLLECTION || worldClip == WARPING){
					worldAudio.UnPause ();
				}
				if(alarmClip != DISASTER_ALARM){
					alarmAudio.UnPause ();
				}
				
				if(GameObject.Find("Planet") != null && GameObject.Find("Planet").GetComponent<EnemySpawner>() != null){
					GameObject.Find("Planet").GetComponent<EnemySpawner>().resumeEnemySound();
				}
				
				ambienceAudio = GameObject.Find("Ambience Audio").GetComponent<AudioSource>();
				ambienceAudio.UnPause ();

				//JUST FOR TESTING
				computerAudio = GameObject.Find ("Computer Audio").GetComponent<AudioSource> ();
				computerAudio.UnPause();
				break;
			}
		}
	}

	public void pauseSound(string source){
		switch (source) {
			case "character":
			{
				characterAudio.Pause ();
				break;
			}
			case "world":
			{
				worldAudio.Pause();
				break;
			}
			case "ambience":
			{
				ambienceAudio = GameObject.Find("Ambience Audio").GetComponent<AudioSource>();
				ambienceAudio.Pause ();
				break;
			}
			default:
			{
				characterAudio = GameObject.Find ("Character Audio").GetComponent<AudioSource> ();
				characterAudio.Pause ();
				if(worldClip == TELEPORTING || worldClip == HEALTH_COLLECTION || worldClip == WARPING){
					worldAudio.Pause ();
				}
				if(alarmClip != DISASTER_ALARM){
					alarmAudio.Pause ();
				}
				
				if(GameObject.Find("Planet") != null && GameObject.Find("Planet").GetComponent<EnemySpawner>() != null){
					GameObject.Find("Planet").GetComponent<EnemySpawner>().pauseEnemySound();
				}
				
				ambienceAudio = GameObject.Find("Ambience Audio").GetComponent<AudioSource>();
				ambienceAudio.Pause ();
	
				computerAudio.Pause();
				break;
			}
		}
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
		case "computer":
		{
			computerAudio.Stop();
			break;
		}
		default:
		{
			ambienceAudio = GameObject.Find("Ambience Audio").GetComponent<AudioSource>();
			ambienceAudio.Stop ();

			characterAudio = GameObject.Find ("Character Audio").GetComponent<AudioSource> ();
			characterAudio.Stop ();

			if(worldClip == TELEPORTING || worldClip == HEALTH_COLLECTION || worldClip == WARPING){
				worldAudio.Stop ();
			}
			if(alarmClip != DISASTER_ALARM){
				alarmAudio.Stop ();
			}

			if(GameObject.Find("Planet") != null && GameObject.Find("Planet").GetComponent<EnemySpawner>() != null){
				GameObject.Find("Planet").GetComponent<EnemySpawner>().stopEnemiesSound();
			}

			computerAudio.Stop();
			break;
		}
		}
	}

	public void pauseMonsterSound(Enemy monster){
		monster.GetComponent<AudioSource> ().Pause();
	}

	public void resumeMonsterSound(Enemy monster){
		if (monster.GetComponent<AudioSource> () != null) {
			monster.GetComponent<AudioSource> ().UnPause ();
		}
	}


	public void stopMonsterSound(Enemy monster){
		monster.GetComponent<AudioSource> ().Stop();
	}

	public void playAmbienceSound(int clipAt){
		ambienceAudio = GameObject.Find ("Ambience Audio").GetComponent<AudioSource> ();
		ambienceAudio.volume = 0.1f;
		ambienceAudio.loop = true;
		ambienceAudio.clip = ambienceClips [clipAt];
		ambienceAudio.Play ();
	}

	public void playMonsterSound(int clipAt, Enemy monster){
		monsterAudio = monster.GetComponent<AudioSource> ();
		monsterAudio.spatialBlend = 1f;
		monsterAudio.rolloffMode = AudioRolloffMode.Linear;
		monsterAudio.minDistance = 12;
		monsterAudio.maxDistance = 40;

		if (clipAt == MONSTER_WALKING) {
			if (monsterAudio.isPlaying == false && done) {
				monsterAudio.loop = true;
				monsterAudio.clip = monsterClips [clipAt];
				monsterAudio.Play ();
			}
		} else{
			monsterAudio.loop = false;
			monsterAudio.clip = monsterClips [clipAt];
			monsterAudio.Play ();
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
		if (clipAt == LOW_HEALTH_ALARM) {
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
		if (worldAudio.isPlaying == false && worldClip == TELEPORTING) {
			done = true;
		}
	}

	public void playWorldSound(int clipAt){
		if (worldAudio.isPlaying == true) {
			worldAudio.Stop ();
		}
		worldClip = clipAt;

		if (worldClip == TELEPORTING) {
			done = false;
		}

		if(worldClip == HEALTH_COLLECTION){
			worldAudio.volume = 0.25f;
		} else {
			worldAudio.volume = 1;
		}

		if (clipAt == SPINNING_WIND || clipAt == EARTHQUAKE) {
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

		if (characterClip >= PLANET_WALKING && characterClip <= SHIP_WALKING) {
			characterAudio.loop = true;
		} else {
			characterAudio.loop = false;
		}

		characterAudio.clip = characterClips [clipAt];
		characterAudio.Play ();
	}

	public void playComputerSound(int clipAt){
		computerClip = clipAt;
		computerAudio.loop = false;
		computerAudio.clip = computerClips [clipAt];
		computerAudio.Play ();
	}
}
