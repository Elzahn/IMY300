public abstract class Accessory : InventoryItem {

	public int damage {get; protected set; }

	public  int stamina {get; protected set;}

	public int hp  { get; protected set; }

	public int hitChance  { get; protected set; }

	public float critChance { get; protected set; }

	public int inventory {get; protected set; }

	public int speed {get; protected set;}

	public Accessory(string typeID) : base(0, typeID) {}
}
