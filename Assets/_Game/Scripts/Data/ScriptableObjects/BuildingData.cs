using UnityEngine;
using System.Collections.Generic;

namespace RTS.Data
{
    [CreateAssetMenu(fileName = "New Building", menuName = "RTS/Data/Building")]
    public class BuildingData : ScriptableObject
    {
        [Header("Identity")]
        public string buildingName;
        public Faction faction;
        public BuildingType buildingType;

        [Header("Stats")]
        public float maxHealth;
        public float armor;
        public Vector2Int gridSize;  // Size in grid cells (e.g., 3x3)

        [Header("Production")]
        public int costSupplies;
        public int powerRequirement;  // Negative = generates power
        public float buildTime;

        [Header("Capabilities")]
        public List<UnitData> producableUnits;
        public List<ResearchData> availableResearch;

        [Header("Requirements")]
        public BuildingData requiredBuilding;
        public TechLevel requiredTechLevel;

        [Header("Visuals")]
        public GameObject modelPrefab;
        public GameObject constructionPrefab;
        public Sprite icon;
    }

    public enum BuildingType
    {
        CommandCenter,
        Production,
        Defense,
        Support,
        Resource,
        Superweapon
    }
}