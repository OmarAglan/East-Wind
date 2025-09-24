using UnityEngine;

namespace RTS.Data
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "RTS/Data/Unit")]
    public class UnitData : ScriptableObject
    {
        [Header("Identity")]
        public string unitName;
        public string designation;  // e.g., "M1A2", "T-90M"
        public Faction faction;
        public UnitType unitType;

        [Header("Stats")]
        public float maxHealth;
        public float armor;         // mm RHA equivalent
        public ArmorType armorType;

        [Header("Movement")]
        public float moveSpeed;     // m/s
        public float turnSpeed;     // degrees/s
        public float acceleration;
        public bool canMove = true;

        [Header("Combat")]
        public WeaponData primaryWeapon;
        public WeaponData secondaryWeapon;  // Optional
        public float visionRange;
        public float detectionRange;  // For stealth units

        [Header("Production")]
        public int costSupplies;
        public int costPower;
        public float buildTime;      // seconds
        public int commandPointCost; // Population cost

        [Header("Requirements")]
        public BuildingData requiredBuilding;
        public TechLevel requiredTechLevel;

        [Header("Visuals")]
        public GameObject modelPrefab;
        public Sprite icon;
    }

    public enum Faction
    {
        USA,
        Russia
    }

    public enum UnitType
    {
        Infantry,
        LightVehicle,
        HeavyVehicle,
        Artillery,
        Aircraft,
        Helicopter,
        Naval
    }

    public enum ArmorType
    {
        None,
        Light,
        Medium,
        Heavy,
        Reactive,
        Composite
    }

    public enum TechLevel
    {
        Tier1,
        Tier2,
        Tier3
    }
}