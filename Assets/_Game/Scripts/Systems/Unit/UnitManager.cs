using UnityEngine;
using System.Collections.Generic;
using RTS.Core;

namespace RTS.Systems
{
    public class UnitManager : MonoBehaviour
    {
        private static UnitManager instance;
        public static UnitManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<UnitManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("UnitManager");
                        instance = go.AddComponent<UnitManager>();
                    }
                }
                return instance;
            }
        }

        private Dictionary<int, List<UnitEntity>> playerUnits = new Dictionary<int, List<UnitEntity>>();
        private List<UnitEntity> allUnits = new List<UnitEntity>();

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterUnit(UnitEntity unit)
        {
            allUnits.Add(unit);

            if (!playerUnits.ContainsKey(unit.OwnerID))
            {
                playerUnits[unit.OwnerID] = new List<UnitEntity>();
            }
            playerUnits[unit.OwnerID].Add(unit);

            Debug.Log($"Unit registered: {unit.name} for Player {unit.OwnerID}");
        }

        public void UnregisterUnit(UnitEntity unit)
        {
            allUnits.Remove(unit);

            if (playerUnits.ContainsKey(unit.OwnerID))
            {
                playerUnits[unit.OwnerID].Remove(unit);
            }

            Debug.Log($"Unit unregistered: {unit.name}");
        }

        public List<UnitEntity> GetPlayerUnits(int playerID)
        {
            if (playerUnits.ContainsKey(playerID))
            {
                return new List<UnitEntity>(playerUnits[playerID]);
            }
            return new List<UnitEntity>();
        }

        public List<UnitEntity> GetAllUnits()
        {
            return new List<UnitEntity>(allUnits);
        }
    }
}