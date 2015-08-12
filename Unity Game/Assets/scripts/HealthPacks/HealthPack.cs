public abstract class HealthPack : InventoryItem {

	public readonly float healthValue;
	public HealthPack(string typeID, float healthValue) : base(2, typeID) {
		this.healthValue = healthValue;
	}
}