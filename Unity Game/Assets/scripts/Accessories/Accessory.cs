public abstract class Accessory : InventoryItem {

	public int Damage {get; protected set; }

	public  int Stamina {get; protected set;}

	public int HP  { get; protected set; }

	public int HitChance  { get; protected set; }

	public int CritChance { get; protected set; }

	public int Inventory {get; protected set; }

	public int Speed {get; protected set;}

	public Accessory(string typeID) : base(0, typeID) {}
}
