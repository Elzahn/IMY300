using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class PlayerAttributes : MonoBehaviour {
    [Serializable()]
	public class AttributeContainer : ISerializable {
		public AttributeContainer(){
			xp = 0;
			level = 1;
			currentLevel = 0;
			accessories = new LinkedList<Accessory>();
			gender = '?';
			
			inventory = new LinkedList<InventoryItem>();
			storage = new LinkedList<InventoryItem> ();
			inventory_LevelStart = new LinkedList<InventoryItem>();
			storage_LevelStart = new LinkedList<InventoryItem>();
			weapon = null;
			fallUnlocked = false;
			fallActive = false;
			warpUnlock = false;
			warpActive = false;
			soundVolume = 1;
			narrativeShown = 1;
		}

		public float xp;
		public float hp;

		//Player Level
		public int level;
		public float stamina;	
		public bool dizzy;
		
		public char gender;
		//Planet Level
		public int currentLevel;

		public Weapon weapon;
		public LinkedList <Accessory> accessories = new LinkedList<Accessory>();

		public LinkedList <InventoryItem> inventory = new LinkedList<InventoryItem>();
		public LinkedList <InventoryItem> storage = new LinkedList<InventoryItem> ();
		public LinkedList <InventoryItem> inventory_LevelStart = new LinkedList<InventoryItem>();
		public LinkedList <InventoryItem> storage_LevelStart = new LinkedList<InventoryItem>();
		
		public float difficulty;
		public float soundVolume;
		public float narrativeShown;
		public string narrativeSoFar;
		public bool gotCore;


		public float levelUpShown;
		public bool fallFirst;
		public bool doorOpen;
		public bool justWarped;

		public bool fallUnlocked = false;
		public bool fallActive = false;
		public bool warpUnlock = false;
		public bool warpActive = false;

	
		public bool[] inventoryShipPieces = new bool[6];
		public bool[] bonusObjs = new bool[3];
		public bool[] bonusObjsShown = new bool[4];
		public bool[] bonusObjsEnemiesKilled = new bool[5];
		public int shipPieces;
		public int deadEnemiesOnLevel;
		public int deadEnemies;

		public void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			info.AddValue("xp", xp);
			info.AddValue("hp", hp);
			info.AddValue("level", level);
			info.AddValue("stamina", stamina);

			info.AddValue("dizzy", dizzy);
			info.AddValue("gender", gender);
			info.AddValue("levels", currentLevel);

			info.AddValue("inventory", inventory);
			info.AddValue("storage", storage);

			info.AddValue("weapon", weapon);
			info.AddValue("accessories", accessories);

			info.AddValue("difficulty", difficulty);
			info.AddValue("soundVolume", soundVolume);
			info.AddValue("narrativeShown", narrativeShown);
			info.AddValue ("narrativeSoFar", narrativeSoFar);
			info.AddValue ("gotCore", gotCore);

			info.AddValue("levelUpShown", levelUpShown);
			info.AddValue("fallFirst", fallFirst);
			info.AddValue("doorOpen", doorOpen);
			info.AddValue("justWraped", justWarped);

			info.AddValue("fallUnlocked", fallUnlocked);
            info.AddValue("fallActive", fallActive);
            info.AddValue("warpUnlocked",warpUnlock);
			info.AddValue("warpActive",warpActive);

			info.AddValue("inventoryShipPieces", inventoryShipPieces);
			info.AddValue("bonusObjs", bonusObjs);
			info.AddValue("bonusObjsShown", bonusObjsShown);
			info.AddValue("bonusObjsEnemiesKilled", bonusObjsEnemiesKilled);
			info.AddValue("deadEnemies", deadEnemies);
			info.AddValue("shipPieceCount", shipPieces);
		}
		
		AttributeContainer(SerializationInfo info, StreamingContext context) {
			xp 		= (int)  info.GetValue("xp",	 typeof(int));
			hp 		= (int)  info.GetValue("hp", 	 typeof(int));
			level 	= (int)  info.GetValue("level",  typeof(int));
			stamina = (float)info.GetValue("stamina",typeof(float));

			dizzy 		= (bool) info.GetValue("dizzy",  typeof(bool));
			gender 	 	= (char) info.GetValue("gender", typeof(char));
			currentLevel = (int) info.GetValue("levels", typeof(int));

			inventory = info.GetValue ("inventory", typeof(LinkedList<InventoryItem>)) as LinkedList<InventoryItem>;
			storage = info.GetValue ("storage", typeof(LinkedList<InventoryItem>)) as LinkedList<InventoryItem>;

			weapon 		= (Weapon) info.GetValue("weapon", typeof(Weapon));
			accessories = info.GetValue ("accessories", typeof(LinkedList<Accessory>)) as LinkedList<Accessory>;

			difficulty 		= (float) info.GetValue("difficulty",     typeof(float));
			soundVolume 	= (float) info.GetValue("soundVolume", 	  typeof(float));
			narrativeShown 	= (float) info.GetValue("narrativeShown", typeof(float));
			narrativeSoFar = (string) info.GetValue("narrativeSoFar", typeof(string));
			gotCore = (bool) info.GetValue("gotCore", typeof(bool));

			levelUpShown = (float) info.GetValue("levelUpShown", typeof(float));
			fallFirst 	 = (bool) info.GetValue("fallFirst",  typeof(bool));
			doorOpen 	 = (bool) info.GetValue("doorOpen",   typeof(bool));
			justWarped 	 = (bool) info.GetValue("justWraped", typeof(bool));

			fallUnlocked = (bool) info.GetValue("fallUnlocked", typeof(bool));
			fallActive = (bool)info.GetValue("fallActive", typeof(bool));
			warpUnlock = (bool)info.GetValue("warpUnlocked",typeof(bool));
			warpActive = (bool) info.GetValue("warpActive",typeof(bool));
			
			inventoryShipPieces = info.GetValue ("inventoryShipPieces", typeof(bool[])) as bool[];
			bonusObjs = info.GetValue ("bonusObjs", typeof(bool[])) as bool[];
			bonusObjsShown = info.GetValue ("bonusObjsShown", typeof(bool[])) as bool[];
			bonusObjsEnemiesKilled = info.GetValue("bonusObjsEnemiesKilled", typeof(bool[])) as bool[];
			deadEnemies = (int) info.GetValue("deadEnemies", typeof(int));
			shipPieces = (int) info.GetValue("shipPieceCount", typeof(int));

			/**
			 * Saving & loading only between levels
			 * */
			inventory_LevelStart = new LinkedList<InventoryItem>(inventory);
			storage_LevelStart = new LinkedList<InventoryItem>(storage);
		}
	}

	public AttributeContainer myAttributes = new AttributeContainer();

	public void setStartAttributes ()
	{
		narrativeShown = 1;
		//1 = easy; 2 = difficult
		difficulty = 1;
		//0 = mute; 1 = on
		soundVolume = 1;
		narrativeSoFar = "";
		showWarpHint = true;
		showLevelUp = false;
		fallFirst = true;
		doorOpen = false;
		//Singleton
		if (instance) {
			DestroyImmediate (gameObject);
			return;
		}
		else {
			DontDestroyOnLoad (gameObject);
			instance = this;
		}
		myAttributes = new AttributeContainer ();
		healthLowering.SetActive (false);
		healthHealing.SetActive (false);
		staminaDrain.SetActive (false);
		healthAltered = 0f;
		staminaDrained = 0f;
		justWarped = false;
		giveAlarm = true;
		gender = '?';
		dizzy = false;
		setInitialXp (0);
		nextRegeneration = Time.time + REGEN_INTERVAL;
		lastDamage = 0;
		CurrentLevel = 0;
	}

	void Awake() {
		healthLowering = GameObject.Find ("Health_Lowering");
		healthHealing = GameObject.Find ("Health_Healing");
		staminaDrain = GameObject.Find ("Stamina_Drained");
		dizzyControl = GameObject.Find ("Dizzy");
		hudText = GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ();
		soundComponent = GameObject.Find ("Player").gameObject.GetComponent<Sounds> ();
		//must be GameObject.Find("Player") else it tries to acces what has been destroyed
		controllerComponent = GameObject.Find ("Player").gameObject.GetComponent<PlayerController> ();
		animatorComponent = GameObject.Find ("Character_Final").GetComponent<Animator> ();
	}

	const int HP_BASE = 100;
	const float HP_MULT = 1.8f;
	const int STAM_BASE = 20;
	const float STAM_MULT = 1.2f;
	const int XP_BASE = 100;
	const float XP_MULT = 2;
	const float ATTACK_MULT = 1.2f;	
	public const float STAMINA_LOSS = 0.5f;
	const int ATTACK_BASE = 6;
	const int CRIT_MULT = 2;

	const int SPEED_BASE = 5;
	const int FEMALE_EXTRA_STAMINA = 20;
	const int MALE_EXTRA_DAMAGE = 12;

	const float HIT_CHANCE_BASE = 0.6f;
	const float DIZZY_HC_REDUCE = 0.3f;

	const float RUN_STAMINA_DRAIN = 1;
	const float REGEN_HP = 0.05f;
	const float REGEN_STAMINA = 0.1f;
	const float REGEN_ATTACK_DELAY = 3;	
	const float REGEN_INTERVAL = 6;

	const int WARP_UNLOCK_LEVEL = 6;
	const int XP_GAIN_PER_MONSTER_LEVEL = 20;


	private int MAX_INVENTORY = 15;	//Inventory size kan verander met accessories
	public  int MAX_STORAGE = 50;		//Sodra die storage menu dit anders kan kry sure

	private Sounds soundComponent;
	private PlayerController controllerComponent;

	private float levelUpShown {
		get {return myAttributes.levelUpShown;}
		set {myAttributes.levelUpShown = value;}
	}
	public bool fallFirst{ 
		get {return myAttributes.fallFirst;} 
		set {myAttributes.fallFirst = value;}
	}
	public bool doorOpen{ 
		get {return myAttributes.doorOpen; }
		set {myAttributes.doorOpen = value;} 
	}	
	public bool justWarped { 
		get {return myAttributes.justWarped;}
		set {myAttributes.justWarped = value;} 
	}

	//Settings values
	public float difficulty { 
		get {return myAttributes.difficulty; }
		set {myAttributes.difficulty = value;}
	}

	public float soundVolume {
		get {return myAttributes.soundVolume;}
		set {myAttributes.soundVolume = value;} 
	}
	public float narrativeShown { 
		get {return myAttributes.narrativeShown;}
		set {myAttributes.narrativeShown = value;}
	}

	public String narrativeSoFar {
		get { return myAttributes.narrativeSoFar;}
		set { myAttributes.narrativeSoFar += value;}
	}

	public bool gotCore {
		get { return myAttributes.gotCore;}
		set { myAttributes.gotCore = value;}
	}

	/**
	 * Singleton
	 */
	public static PlayerAttributes instance;


	/****************************************************** Inventory en goed  *****************************************************/
	public LinkedList <InventoryItem> inventory {
		get {
			return myAttributes.inventory;
		} set {
			myAttributes.inventory = value;
		}
	}

	public InventoryItem removeFirstByName(string name) {
		var item = inventory.First;
		while (item != null) {
			if (item.Value.typeID.Equals(name)) {			
				inventory.Remove(item.Value);
				return item.Value;
			}
			item = item.Next;
		}
		return null;
	}

	public LinkedList <InventoryItem> storage {
		get {
		return myAttributes.storage;
		} set {
			myAttributes.storage = value;
		}
	}

	private LinkedList <InventoryItem> inventory_LevelStart {
		get {
			return myAttributes.inventory_LevelStart;
		} set {
			myAttributes.inventory_LevelStart = value;
		}
	}

	private LinkedList <InventoryItem> storage_LevelStart {
		get {
			return myAttributes.storage_LevelStart;
		} set {
			myAttributes.storage_LevelStart = value;
		}
	}
	public LinkedList <Accessory> accessories {
		get {
			return myAttributes.accessories;
		} set {
			myAttributes.accessories = value;
		}
	}


	/******************************************************* Public properties *****************************************************/ 
	private int ambienceClip = -1;

	public Weapon weapon {
		get {
			return myAttributes.weapon;
		} 
		set {
			myAttributes.weapon = value;
		}
	}

    public void returnToSaveSpot()
    {
		CurrentLevel++;
		save(0);
        Application.LoadLevel("SaveSpot");
        Resources.UnloadUnusedAssets();
    }
	
	public int CurrentLevel {get {
			return myAttributes.currentLevel;
		} set {
			myAttributes.currentLevel = value;
		}
	}

	public float hp {
		get {
			return myAttributes.hp;
		}
		private set {
			var max = maxHP ();
			if (value > max)
				myAttributes.hp = max;
			else
			myAttributes.hp = value;
		}
	}

	public int level {
		get {
			return myAttributes.level;
		}
		set {
			myAttributes.level = value;
		}
	}

	public float stamina {
		get {
			return myAttributes.stamina;
		}
		set {
			var max = maxStamina ();
			if (value > max)
				myAttributes.stamina = max;
			else
			myAttributes.stamina = value;
		}}

	public float xp {get {
			return myAttributes.xp;
		} private set {
			myAttributes.xp = value;
		}}

	public bool dizzy {get {
			return myAttributes.dizzy;
		} set {
			myAttributes.dizzy = value;
			dizzyControl.SetActive(value);
			if(value){
				ambienceClip = soundComponent.ambienceClip;
				soundComponent.playAmbienceSound(Sounds.DIZZY);
			} else {
				if(ambienceClip != -1)
					soundComponent.playAmbienceSound(ambienceClip);
			}
		}}
	
	public char gender {get {
			return myAttributes.gender;
		} private set  {
			myAttributes.gender = value;
		}}
		
	public void setGender(char value) {
		if (gender == '?') {
			if (value == 'f') {
				stamina += FEMALE_EXTRA_STAMINA;
			}
			gender = value;
		}
	}

	public int speed {
		get {
			int tmp = SPEED_BASE;
			foreach (Accessory a in accessories) {
				tmp += a.speed;
			}
			return tmp;
		}
	}

	public int damage { 
		get {
			int tmp = BaseDamage;
			if (gender == 'm') {
				tmp += MALE_EXTRA_DAMAGE;
			}
			if (weapon != null) {
				tmp += weapon.damage;
			}
			foreach (Accessory a in accessories) {
				tmp += a.damage;
			}
			return tmp;
		}
	}

	public int inventorySize {
		get {
			int tmp = MAX_INVENTORY;
			foreach (Accessory a in accessories) {
				tmp += a.inventory;
			}
			return tmp;
		}
	}

	public bool inventoryFull(){
		return (inventory.Count >= inventorySize);
	}

	public float hitChance { 
		get {
			float tmp = HIT_CHANCE_BASE;
			
			foreach (Accessory a in accessories) {
				tmp += a.hitChance;
			}
			
			//if dizzy lose some hitChance
			if (dizzy) {
				tmp -= DIZZY_HC_REDUCE;
			}
			
			return tmp;
		}
	}
	
	public float critChance { 
		get {
			float tmp = 0.05f;
			foreach (Accessory a in accessories) {
				tmp += a.critChance;
			}
			return tmp;
			}
	}

	public int maxAccessories { 
		get {
			return (level >= 5) ? 2 : 1;
		}
	}

	public float lastDamage {get; set;} //Last time damage taken
	public static bool giveAlarm;
	private float nextRegeneration, healthAltered, staminaDrained;
	private Text hudText;
	private bool showWarpHint;
	private bool showLevelUp;
	private GameObject healthLowering, healthHealing, staminaDrain, door, dizzyControl;
	private Animator animatorComponent;



    /**************************************************** Monobehaviour functions *********************************************
	 * Start - Called after creation
	 * Update - Called Every frame
	 * ***********************************************************************************************************************/
	void Start () {
		//1 = show; 0 = hide
		setStartAttributes ();	
	}

	void Update() {

		dizzyControl.transform.RotateAround(dizzyControl.transform.position, dizzyControl.transform.forward, 5f);
		door = GameObject.Find ("Door");
		if (Application.loadedLevelName == "SaveSpot" && doorOpen && door != null) {
			GameObject.Find("Door").SetActive(false);
		}

		if (stamina <= 0) { 
			stamina = 0;
			if(staminaDrained == 0f){
				staminaDrained = Time.time;
			}
			staminaDrain.SetActive(true);
		}

		if (staminaDrained != 0f && Time.time > staminaDrained + 0.5f) {
			staminaDrain.SetActive(false);
			staminaDrained = 0f;
		}

		if (healthAltered != 0f && Time.time > healthAltered + 0.5f) {
			HideHealhtAltered();
		}
		/*if (levelXP (level) == 0) {
			GameObject.Find ("XP").GetComponent<Image> ().fillAmount = xp / levelXP (level+1);
		} else {*/
		GameObject.Find ("XP").GetComponent<Image> ().fillAmount = xp / getExpectedXP();
		//}

		if(GameObject.Find ("LevelUp").GetComponent<ParticleSystem> ().enableEmission == true && showLevelUp && Time.time >= levelUpShown){
			//GameObject.Find ("LevelUp").GetComponent<ParticleSystem> ().enableEmission = false;
			//GameObject.Find ("LevelUp").GetComponent<ParticleSystem> ().Clear();
			showLevelUp = false;
		}

		// Health regenration etc.
		if (!controllerComponent.paused) {


			if (this.hp <= 50 && giveAlarm) {
				soundComponent.playAlarmSound (1);
				showHealthAltered("lowering");
			} else {
				soundComponent.stopAlarmSound (1);
			}
			
			string tempMessage = levelUp ();
			if (tempMessage != "") {
				//PlayerLog.addStat (tempMessage);			
			}

			Warping warp = gameObject.GetComponent<Warping> ();

			if (level == WARP_UNLOCK_LEVEL && warp != null && showWarpHint) {
				Camera.main.GetComponent<HUD>().setLight("warp");
				warp.chooseDestinationUnlocked = true;
				showWarpHint = false;
			    
			    soundComponent.playComputerSound(Sounds.COMPUTER_WARPDESTINATION);
				hudText.text = "I have located all the warp points on the planet. I can now warp you to a chosen destination.\n\n";
				Canvas.ForceUpdateCanvases();
				Scrollbar scrollbar = GameObject.Find ("Scrollbar").GetComponent<Scrollbar> ();
				scrollbar.value = 0f;
				GameObject.Find("Player").GetComponent<Tutorial>().makeHint("Warp by walking onto a warp point. Destination warps have a 10s cool down time and drains health.", this.GetComponent<Tutorial>().Warp);
			}
			
			if (Time.time >= nextRegeneration) {
				nextRegeneration = Time.time + REGEN_INTERVAL;
				if (Time.time >= (lastDamage + REGEN_ATTACK_DELAY)) {
					if (hp < maxHP ()) {
						hp += (int)(maxHP () * REGEN_HP);
						showHealthAltered("heal");
						giveAlarm = false;
					}

					if(hp > maxHP()){
						hp = maxHP();
					}

					if (stamina < maxStamina ()) {
						stamina += (maxStamina () * REGEN_STAMINA);
						GameObject.Find("Stamina").GetComponent<Image>().fillAmount = stamina/maxStamina();
					}
				}
			}
		} else {
			nextRegeneration = Time.time + REGEN_INTERVAL;
		}
	}

	/************************************************************************ Initialization *********************************************************************/
	
	public void setInitialXp(float xp) {
		this.xp = xp;		
		level = determineLevel ();
		hp = maxHP();
		stamina = maxStamina ();
	}

	public void setAttributes (int xp, LinkedList <InventoryItem> inventory, LinkedList <InventoryItem> storage, Weapon w, LinkedList<Accessory> accesoories) {
		
		setInitialXp(xp);
		
		this.inventory = inventory;
		this.storage = storage;
		this.inventory_LevelStart = new LinkedList<InventoryItem>(inventory);
		this.storage_LevelStart = new LinkedList<InventoryItem>(storage);
	}

	/********************************************************** Level and XP *********************************************************************/
	public string levelUp() {
		float nextTreshold = getExpectedXP();
		if (xp >= nextTreshold) {
			level++;
			restoreHealthToFull();
			//hp = maxHP ();
			//showHealthAltered("healing");
			restoreStaminaToFull();
			//stamina = maxStamina ();
			soundComponent.playWorldSound (Sounds.LEVEL_UP);
			GameObject.Find ("LevelUp").GetComponent<ParticleSystem> ().enableEmission = true;
			GameObject.Find ("LevelUp").GetComponent<ParticleSystem> ().Emit(100);
			GameObject.Find ("Level").GetComponent<Text> ().text = "" + (int.Parse(GameObject.Find ("Level").GetComponent<Text> ().text)+1);
			levelUpShown = Time.time + 3;
			showLevelUp = true;
			return "You  are now level " + level;
		}
		return "";
	}
	
	/** Cheat */
	public void levelMeUp(){
		soundComponent.playWorldSound (Sounds.LEVEL_UP);
		//GameObject.Find ("LevelUp").GetComponent<ParticleSystem> ().enableEmission = true;
		//GameObject.Find ("Level").GetComponent<Text> ().text = "" + (int.Parse(GameObject.Find ("Level").GetComponent<Text> ().text)+1);
		levelUpShown = Time.time + 3;
		showLevelUp = true;
		xp = levelXP (level+1);
		hp = maxHP ();
		showHealthAltered ("heal");
		levelUp();
	}

	/** Returns the XP needed to get to level n */ 
	public int levelXP(int n) {
		return (int) (XP_BASE * (Mathf.Pow (XP_MULT, n - 1) / (XP_MULT - 1) - 1));
	}
	
	public float getExpectedXP() {
		return levelXP(level+1);
	}
	
	/** Determine level based on xp - Dont doubt the math, I tested it this time */ 
	public int determineLevel() {
		if (xp == 0) return 1;
		
		return Mathf.RoundToInt(Mathf.Log ( ((xp + XP_BASE) * (XP_MULT - 1) / XP_BASE) / Mathf.Log (XP_MULT) +0.5f));
	}


	
	public void resetXP(){
		xp = levelXP (level);
	}
	
	public string addXP(int value){
		xp += value;

		string temp = levelUp();
		if (temp == "") {
			return "You got " + value + "XP";
		} else {
			return temp;
		}
	}

	/***************************** Inventory ************************************/
	public GameObject butterKnife;
	public GameObject longSword;
	public GameObject warHammer;

	public void equipItem(InventoryItem item) {
		switch (item.type) {
			case 0:
				equipAccessory((Accessory) item);
				break;
			case 1:
				equipWeapon((Weapon) item);
				break;
			default:
				throw new RulesException("Unknown Item");
		}
	}

	public void unequipWeapon(){
		if (weapon != null)  {
			
			GameObject.Find("Character_Final").GetComponent<Animator>().SetBool("Weapon", false);

			if(weapon.typeID == "ButterKnife"){
				Destroy(GameObject.Find("ButterKnife(Clone)"));
			} else if(weapon.typeID == "Longsword"){
				Destroy(GameObject.Find("LongSword(Clone)"));
			} else if(weapon.typeID == "Warhammer"){
				Destroy(GameObject.Find("WarHammer(Clone)"));
			} else if(weapon.typeID == "BonusWeapon"){
				Destroy(GameObject.Find("LongSword(Clone)"));
			}

			addToInventory(weapon);
			weapon = null;
		}
	}
	/**
	 * Self Explanatory
	 * Return Error or Success Message
	 */
	void equipWeapon(Weapon weap) {
		if (weap == null) {
			throw new System.ArgumentNullException ("weap");
		}

		if (weap.level <= level) {
			inventory.Remove(weap);
			unequipWeapon();
			this.weapon = weap;

			GameObject.Find("Character_Final").GetComponent<Animator>().SetBool("Weapon", true);
			GameObject weaponPrefab = null;

			if(weapon.typeID == "ButterKnife"){
				weaponPrefab = Instantiate(butterKnife);
				weaponPrefab.transform.SetParent(GameObject.Find("mixamorig:RightHandMiddle1").transform);
				weaponPrefab.transform.localScale = new Vector3(10f, 10f, 7);
				weaponPrefab.transform.localPosition = new Vector3(3.4f, 1.11f, 1.02f);
				weaponPrefab.transform.localRotation = Quaternion.Euler(5.536556f, 265.9102f, 359.8019f);
			} else if(weapon.typeID == "Longsword"){
				weaponPrefab = Instantiate(longSword);
				weaponPrefab.transform.SetParent(GameObject.Find("mixamorig:RightHandMiddle1").transform);
				weaponPrefab.transform.localScale = new Vector3(10f, 10f, 20f);
				weaponPrefab.transform.localPosition = new Vector3(4.81f, 1.22f, 1.77f);
				weaponPrefab.transform.localRotation = Quaternion.Euler(2.872368f, 264.6271f, 359.299f);
			} else if(weapon.typeID == "Warhammer"){
				weaponPrefab = Instantiate(warHammer);
				weaponPrefab.transform.SetParent(GameObject.Find("mixamorig:RightHandMiddle1").transform);
				weaponPrefab.transform.localScale = new Vector3(15, 15, 20);
				weaponPrefab.transform.localPosition = new Vector3(4.149894f, 3.779963f, 3.159975f);
				weaponPrefab.transform.localRotation = Quaternion.Euler(24.22492f, 252.6553f, 311.9116f);
			} else if(weapon.typeID == "BonusWeapon"){
				weaponPrefab = Instantiate(longSword);
				weaponPrefab.transform.SetParent(GameObject.Find("mixamorig:RightHandMiddle1").transform);
				weaponPrefab.transform.localScale = new Vector3(10f, 10f, 20f);
				weaponPrefab.transform.localPosition = new Vector3(4.81f, 1.22f, 1.77f);
				weaponPrefab.transform.localRotation = Quaternion.Euler(2.872368f, 264.6271f, 359.299f);
			}

		} else {
			throw new RulesException("Weapon Level too high");
		}
	}

	void equipAccessory(Accessory a) {
		if (a == null) {
			throw new ArgumentNullException ("a");
		}

		if (accessories.Count < maxAccessories){
			inventory.Remove(a);
			accessories.AddLast(a);
		} else {
			throw new RulesException("Unequip another one first");
		}
	}

	public void unequipAccessory(Accessory a) {
		if (accessories.Contains (a)) {
			addToInventory(a);
			accessories.Remove (a);
		} else {
			throw new RulesException ("Accessory not equipped");
		}
	}
	
	public void addToStorage(InventoryItem item){
		if (storage.Count < MAX_STORAGE) {
			storage.AddLast (item);
		} else {
		throw new RulesException ("Storage full");
		}
	}

	public void addToInventory(InventoryItem a) {
		if (!inventory.Contains(a)) {
			if (!inventoryFull() || a.type == 3) {			
				inventory.AddLast (a);
			} else {
				throw new RulesException ("Inventory Full. Drop something else first");
			}
		}
	}

	public void saveInventoryAndStorage(){
		inventory_LevelStart.Clear ();
		foreach (InventoryItem item in inventory) {
			inventory_LevelStart.AddLast (item);
		}
		
		storage_LevelStart.Clear ();
		foreach (InventoryItem item in storage) {
			storage_LevelStart.AddLast(item);
		}
	}

	public void resetPlayer(){
		gotCore = false;
		inventory.Clear ();
		storage.Clear ();
		setInitialXp (0);
		doorOpen = false;
		narrativeSoFar = "";
		fallFirst = false;
	}

	public void resetInventoryAndStorage(){
		inventory.Clear ();
		foreach (InventoryItem item in inventory_LevelStart) {
			inventory.AddLast (item);
		}
		
		storage.Clear ();
		foreach (InventoryItem item in storage_LevelStart) {
			storage.AddLast(item);
		}
	}

	/************************ Health and Stamina *********************/

	public void showHealthAltered(String health){
		if (health == "heal") {
			animatorComponent.SetBool("Attacking", false);
			healthHealing.SetActive (true);
			healthLowering.SetActive(false);
			healthHealing.GetComponent<Image> ().fillAmount = hp / maxHP ();
		} else if (health != "reset"){
			healthLowering.SetActive (true);
			healthHealing.SetActive(false);
			healthLowering.GetComponent<Image> ().fillAmount = hp / maxHP ();
		}

		healthAltered = Time.time;

		GameObject.Find ("Health").GetComponent<Image> ().fillAmount = hp / maxHP ();
	}

	public void HideHealhtAltered(){
		healthLowering.SetActive (false);
		healthHealing.SetActive (false);
		healthAltered = 0f;
	}

	public void useHealthPack(InventoryItem item) {
		HealthPack tempItem = (HealthPack) item;
		int healthRegen = (int)(maxHP() * tempItem.healthValue);
		AddHealth(healthRegen);
		showHealthAltered ("heal");
	}	

	public void AddHealth(int val){
		hp += val;
		if (hp > maxHP ()) {
			restoreHealthToFull();
		}
	}

	public void restoreHealthToFull() {
		hp = maxHP ();
		showHealthAltered ("reset");
	}
	

	/** Returns true if dead */
	public bool loseHP(int loss) {
		if(Application.loadedLevelName != "SaveSpot"){
			if (Application.loadedLevelName == "Tutorial") {
				hp -= loss/2;
			} else {
				hp -= loss;
			}

			showHealthAltered("injured");
		}
		return isDead ();
	}
	
	public float maxHP() {
		var tmp = Mathf.RoundToInt(HP_BASE * Mathf.Pow(HP_MULT, level -1));		
		foreach (Accessory a in accessories) {
			tmp += a.hp;
		}
		return tmp;
	}
	
	public bool isDead() {
		if(hp <=0 && !animatorComponent.GetBool("Dead")){
			soundComponent.playDeathSound(Sounds.DEAD_PLAYER);
			animatorComponent.SetBool("Dead", true);
		}
		return hp <= 0;
	}
	
	public int maxStamina() {
		var tmp =  Mathf.RoundToInt(HP_BASE * Mathf.Pow(HP_MULT, level -1));	
		foreach (Accessory a in accessories) {
			tmp += a.stamina;
		}
		if (gender == 'f')
			tmp += FEMALE_EXTRA_STAMINA;
		return tmp;
	}
		
	public void drainStamina(){
		if (Application.loadedLevelName != "SaveSpot") {
			if(Application.loadedLevelName == "Tutorial"){
				stamina -= RUN_STAMINA_DRAIN/4;
			} else {
				stamina -= RUN_STAMINA_DRAIN;//0.5f;
			}
			GameObject.Find("Stamina").GetComponent<Image>().fillAmount = stamina/maxStamina();
		}
	}
	
	public void restoreStaminaToFull(){
		stamina = maxStamina ();
		GameObject.Find("Stamina").GetComponent<Image>().fillAmount = stamina/maxStamina();
	}

	/*********************** Attack *****************************/

	public string attack(Enemy e) {
		//string name = e.typeID;
	/*	if (soundComponent.characterAudio.isPlaying && soundComponent.characterClip < Sounds.SWORD_HIT) {
			soundComponent.stopSound("character");
		}*/
		animatorComponent.SetBool("Attacking", true);
		string message = "Miss! ";

		if(stamina > 0){
			if(GameObject.Find("Health").GetComponent<Image>().isActiveAndEnabled == false){
				this.GetComponent<Tutorial>().showHealthHint();
			}
			e.lastDamage = Time.time;
			float ran = UnityEngine.Random.value;
			float hc = hitChance;

			if (ran <= hc) {			
				message = "Hit! ";
			
				float cc = critChance;
				int tmpdamage = damage;

				/*Critical Hit */
				if (ran <= cc) {

					tmpdamage *= CRIT_MULT;
					message = "Critical Hit! ";

					if (weapon != null){
						if (weapon.typeID == "Warhammer") {
							soundComponent.playCharacterSound (Sounds.HAMMER_CRIT);
						} else {
							soundComponent.playCharacterSound (Sounds.SWORD_CRIT);
						}
					} else {
						soundComponent.playCharacterSound (Sounds.FISTS_CRIT);
					}
				} else {
					/* Non-crit Sounds */

					if (weapon != null){
						if (weapon.typeID == "Warhammer") {
							soundComponent.playCharacterSound (Sounds.HAMMER_HIT);
						} else {
							soundComponent.playCharacterSound (Sounds.SWORD_HIT);
						}
					} else {
						soundComponent.playCharacterSound (Sounds.FISTS_HIT);
					}
				}

				bool dead = e.loseHP(tmpdamage);

				if (weapon != null) {
					stamina -= weapon.staminaLoss;
					GameObject.Find("Stamina").GetComponent<Image>().fillAmount = stamina/maxStamina();
				} else {
					stamina -= STAMINA_LOSS;
					GameObject.Find("Stamina").GetComponent<Image>().fillAmount = stamina/maxStamina();
				}

				if (dead) {					
					message += " Monster died!\n";
					message += addXP(e.level * XP_GAIN_PER_MONSTER_LEVEL) + "\n";
				} else {
					//add Stats to message
					message += e.getHealth()+"/"+e.getMaxHp();
				}
			} else if (message == "Miss! ") {
				soundComponent.playCharacterSound(Sounds.MISS);
				message += e.getHealth () + "/" + e.getMaxHp ();
			}
		} else {
			soundComponent.playCharacterSound(Sounds.MISS);
		}

		return message;
	}

    private int BaseDamage
    {
        get { return Mathf.RoundToInt((ATTACK_BASE + level - 1)*Mathf.Pow(ATTACK_MULT, level - 1)); }
    }

    public void save(int slot) {
		var file = getSaveName (slot);
		Stream stream = File.Open (file, FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter();	

		bformatter.Serialize(stream, myAttributes);
		stream.Close();
	}

	private String getSaveName(int slot) {
		Directory.CreateDirectory("saves");
		return "saves/save_" + slot + ".sav";
	}

	public String readData(int slot){
		AttributeContainer temp;
		try {
			var saveName = getSaveName(slot);
			if (!File.Exists(saveName))
				return "No save file";
			Stream stream = File.Open(saveName, FileMode.Open);
			BinaryFormatter bformatter = new BinaryFormatter();		
			temp = (AttributeContainer)bformatter.Deserialize(stream);
			stream.Close();
			
			if (temp != null && temp.level != 0 /*&& temp.gotCore*/) {
				return (File.GetCreationTime(saveName) + "\nLevel: " + temp.level + "\nXp: " + temp.xp + "/" + levelXP(temp.level+1));
			}
		} catch (IOException) {
			return "No save file";
		}
		return "No save file";
	}
	
	public void load(int slot) {
		AttributeContainer temp;
		try {
		    var saveName = getSaveName(slot);
            if (!File.Exists(saveName))
                throw new IOException("Save file doesnt exsit");
		    Stream stream = File.Open(saveName, FileMode.Open);
			BinaryFormatter bformatter = new BinaryFormatter();		
			temp = (AttributeContainer)bformatter.Deserialize(stream);
			stream.Close();

			if (temp != null) {
				myAttributes = temp;
			}
		} catch (Exception e) {
			throw new Exception("Cannot load savegame: " + e.Message);
		}
	}
}
