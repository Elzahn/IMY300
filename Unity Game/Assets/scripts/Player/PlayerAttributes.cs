using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class PlayerAttributes : MonoBehaviour {

	[Serializable()]
	private class AttributeContainer : ISerializable {
		public AttributeContainer(){}

		public int xp;
		public float hp;

		public int level;
		public float stamina;	
		public bool dizzy;
		
		public char gender;

		public int levelsComplete;

		public Weapon weapon;

		public LinkedList <InventoryItem> inventory = new LinkedList<InventoryItem>();
		public LinkedList <InventoryItem> storage = new LinkedList<InventoryItem> ();
		public LinkedList <InventoryItem> inventory_LevelStart = new LinkedList<InventoryItem>();
		public LinkedList <InventoryItem> storage_LevelStart = new LinkedList<InventoryItem>();
		public LinkedList <Accessory> accessories = new LinkedList<Accessory>();

		public void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			info.AddValue("xp", xp);
			info.AddValue("levels", levelsComplete);
			info.AddValue("inventory", inventory);
			info.AddValue("storage", storage);
			info.AddValue("weapon", weapon);
			info.AddValue("accessories", accessories);
		}
		
		AttributeContainer(SerializationInfo info, StreamingContext context) {
			xp = (int)info.GetValue("xp", typeof(int));

			levelsComplete = (int)info.GetValue("levels", typeof(int));
			inventory = (LinkedList<InventoryItem>) info.GetValue("inventory", typeof(LinkedList<InventoryItem>));
			storage = (LinkedList<InventoryItem>) info.GetValue("storage", typeof(LinkedList<InventoryItem>));

			/**
			 * Saving & loading only between levels
			 * */
			inventory_LevelStart = new LinkedList<InventoryItem>(inventory);
			storage_LevelStart = new LinkedList<InventoryItem>(storage);
		}
	}

	private AttributeContainer myAttributes = new AttributeContainer();

	const int HP_BASE = 100;
	const float HP_MULT = 1.8f;
	const int STAM_BASE = 20;
	const float STAM_MULT = 1.2f;
	const int XP_BASE = 100;
	const float XP_MULT = 2;
	const float ATTACK_MULT = 1.2f;	
	const int ATTACK_BASE = 6;
	const int CRIT_MULT = 2;
	
	const int SPEED_BASE = 5;
	const int FEMALE_EXTRA_STAMINA = 20;
	const int MALE_EXTRA_DAMAGE = 12;

	const float HIT_CHANCE_BASE = 0.6f;
	const float DIZZY_HC_REDUCE = 0.3f;

	const float RUN_STAMINA_DRAIN = 1;
	const float REGEN_HP = 0.1f;
	const float REGEN_STAMINA = 0.1f;
	const float REGEN_ATTACK_DELAY = 3;	
	const float REGEN_INTERVAL = 6;

	const int WARP_UNLOCK_LEVEL = 6;
	const int XP_GAIN_PER_MONSTER_LEVEL = 20;


	private int MAX_INVENTORY = 15;	//Inventory size kan verander met accessories
	public  int MAX_STORAGE = 50;		//Sodra die storage menu dit anders kan kry sure

	private Sounds soundComponent;
	private PlayerController controllerComponent;

	/**
	 * Singleton
	 */
	public static PlayerAttributes instance;

	public bool justWarped { get; set; }

	/****************************************************** Inventory en goed  *****************************************************/
	public LinkedList <InventoryItem> inventory {
		get {
			return myAttributes.inventory;
		} set {
			myAttributes.inventory = value;
		}
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
	public Weapon weapon {
		get {
			return myAttributes.weapon;
		} 
		set {
			myAttributes.weapon = value;
		}
	}
	
	public int levelsComplete {get {
			return myAttributes.levelsComplete;
		} set {
			myAttributes.levelsComplete = value;
		}
	}

	public float hp {
		get {
			return myAttributes.hp;
		}
		private set {
			myAttributes.hp = value;
		}
	}

	public int level {
		get {
			return myAttributes.level;
		}
		private set {
			myAttributes.level = value;
		}
	}

	public float stamina {
		get {
			return myAttributes.stamina;
		}
		set {
			myAttributes.stamina = value;
		}}

	public int xp {get {
			return myAttributes.xp;
		} private set {
			myAttributes.xp = value;
		}}

	public bool dizzy {get {
			return myAttributes.dizzy;
		} set {
			myAttributes.dizzy = value;
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
			int tmp = baseDamage ();
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
	private float nextRegeneration;
	private Text hudText;
	private bool showWarpHint;

	/**************************************************** Monobehaviour functions *********************************************
	 * Start - Called after creation
	 * Update - Called Every frame
	 * ***********************************************************************************************************************/
	void Start () {
		showWarpHint = true;

		//Singleton
		if (instance) {
			DestroyImmediate(gameObject);
			return;
		} else {
			DontDestroyOnLoad (gameObject);
			instance = this;
		}

		justWarped = false;
		giveAlarm = true;
		this.gender = '?';
		this.dizzy = false;

		this.setInitialXp(0);
		this.nextRegeneration = Time.time + REGEN_INTERVAL;
		this.lastDamage = 0;
		this.levelsComplete = 0;

		hudText = GameObject.Find ("HUD_Expand_Text").GetComponent<Text> ();

		this.soundComponent = GameObject.Find("Player").gameObject.GetComponent<Sounds>(); //must be GameObject.Find("Player") else it tries to acces what has been destroyed
		this.controllerComponent = GameObject.Find("Player").gameObject.GetComponent<PlayerController> (); //must be GameObject.Find("Player") else it tries to acces what has been destroyed
	}

	void Update() {
		if (stamina < 0) 
			stamina = 0;
		// Health regenration etc.
		if (!controllerComponent.paused) {
			
			if (this.hp <= 50 && giveAlarm) {
				soundComponent.playAlarmSound (1);
			} else {
				soundComponent.stopAlarmSound (1);
			}
			
			string tempMessage = levelUp ();
			if (tempMessage != "") {
				//PlayerLog.addStat (tempMessage);			
			}

			Warping warp = this.gameObject.GetComponent<Warping> ();
			if (level == WARP_UNLOCK_LEVEL && warp != null && showWarpHint) {
				warp.chooseDestinationUnlocked = true;
				showWarpHint = false;
				GameObject.Find("Player").GetComponent<Sounds>().playComputerSound(Sounds.COMPUTER_WARPDESTINATION);
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
						GameObject.Find("Health").GetComponent<Image>().fillAmount = hp/maxHP();
						giveAlarm = false;
					}

					if (stamina < maxStamina ()) {
						stamina += (maxStamina () * REGEN_STAMINA);
						//visual stamina regen here
					}
				}
				
			}
		} else {
			nextRegeneration = Time.time + REGEN_INTERVAL;
		}
	}

	/************************************************************************ Initialization *********************************************************************/
	
	public void setInitialXp(int xp) {
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
		int nextTreshold = getExpectedXP();
		if (xp >= nextTreshold) {
			level++;
			hp = maxHP ();
			stamina = maxStamina ();
			
			return "You  are now level " + level;
		}
		return "";
	}
	
	/** Cheat */
	public void levelMeUp(){
		xp = levelXP (level+1);
		hp = maxHP ();
		GameObject.Find("Health").GetComponent<Image>().fillAmount = hp/maxHP();
		levelUp();
	}

	/** Returns the XP needed to get to level n */ 
	public int levelXP(int n) {
		return (int) (XP_BASE * (Mathf.Pow (XP_MULT, n - 1) / (XP_MULT - 1) - 1));
	}
	
	public int getExpectedXP() {
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
			throw new System.ArgumentNullException ("weapon");
		}

		if (weap.level <= level) {
			inventory.Remove(weap);
			unequipWeapon();
			this.weapon = weap;
		} else {
			throw new RulesException("Weapon Level too high");
		}
	}

	void equipAccessory(Accessory a) {
		if (a == null) {
			throw new System.ArgumentNullException ("weapon");
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
			if (inventory.Count < MAX_INVENTORY ) {			
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
	public void useHealthPack(InventoryItem item) {
		HealthPack tempItem = (HealthPack) item;
		int healthRegen = (int)(maxHP() * tempItem.healthValue);
		addHealth(healthRegen);
	}	

	public void addHealth(int val){
		this.hp += val;
		if (this.hp > this.maxHP ()) {
			restoreHealthToFull();
		}
	}

	public void restoreHealthToFull() {
		hp = maxHP ();
		GameObject.Find("Health").GetComponent<Image>().fillAmount = hp/maxHP();
	}
	

	/** Returns true if dead */
	public bool loseHP(int loss) {
		if (Application.loadedLevelName != "Tutorial") {
			hp -= loss;
			GameObject.Find("Health").GetComponent<Image>().fillAmount = hp/maxHP();
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
			this.stamina -= RUN_STAMINA_DRAIN;//0.5f;
		}
	}
	
	public void restoreStaminaToFull(){
		stamina = maxStamina ();
	}

	/*********************** Attack *****************************/

	public string attack(Enemy e) {
		//string name = e.typeID;
		e.lastDamage = Time.time;
		float ran = UnityEngine.Random.value;
		float hc = hitChance;
		string message = "Miss! ";

		if (ran <= hc) {			
			message = "Hit! ";
		
			float cc = this.critChance;
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
			}

			if (dead) {					
				message += " Monster died!\n";
				message += addXP(e.level * XP_GAIN_PER_MONSTER_LEVEL) + "\n";
			} else {
				//add Stats to message
				message += e.getHealth()+"/"+e.getMaxHp();
			}
		} 
		if (message == "Miss! ") {
			soundComponent.playCharacterSound(Sounds.MISS);
			message += e.getHealth () + "/" + e.getMaxHp ();
		}

		//PlayerLog.addStat (message);
		return message;
	}


	int baseDamage() {
		return  Mathf.RoundToInt((ATTACK_BASE + level -1) * Mathf.Pow (ATTACK_MULT, level - 1));
	}

	public void save(int slot) {
		var file = getSaveName (slot);

		Stream stream = File.Open (file, FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter();	

		bformatter.Serialize(stream, myAttributes);
		stream.Close();
	}

	private String getSaveName(int slot) {
		System.IO.Directory.CreateDirectory("saves");
		return "saves/save_" + slot + ".sav";
	}

	public void load(int slot) {
		AttributeContainer temp;
		try {
		Stream stream = File.Open(getSaveName(slot), FileMode.Open);
		BinaryFormatter bformatter = new BinaryFormatter();		
		temp = (AttributeContainer)bformatter.Deserialize(stream);
		stream.Close();

			if (temp != null) {
				myAttributes = temp;
				setInitialXp(xp);
			}
		} catch (IOException) {
			throw(new IOException("Cannot load savegame."));
		}

	}
}
