using UnityEngine;
using RTS.Data;
using RTS.Components;
using RTS.Systems;

namespace RTS.Core
{
    public class UnitEntity : BaseEntity
    {
        [Header("Unit Data")]
        [SerializeField] private UnitData unitData;

        // Components
        private HealthComponent healthComponent;
        private MoverComponent moverComponent;
        private AttackerComponent attackerComponent;
        private SelectableComponent selectableComponent;
        private FogRevealerComponent fogRevealerComponent;

        public UnitData Data => unitData;
        public override bool IsAlive => healthComponent != null && healthComponent.IsAlive;

        public void Initialize(UnitData data, int playerID)
        {
            unitData = data;
            SetOwner(playerID);
            InitializeComponents();
        }

        protected override void CacheComponents()
        {
            healthComponent = GetComponent<HealthComponent>();
            moverComponent = GetComponent<MoverComponent>();
            attackerComponent = GetComponent<AttackerComponent>();
            selectableComponent = GetComponent<SelectableComponent>();
            fogRevealerComponent = GetComponent<FogRevealerComponent>();
        }

        private void InitializeComponents()
        {
            // Initialize Health
            if (healthComponent != null)
            {
                healthComponent.Initialize(unitData.maxHealth, unitData.armor, unitData.armorType);
            }

            // Initialize Movement
            if (moverComponent != null && unitData.canMove)
            {
                moverComponent.Initialize(unitData.moveSpeed, unitData.turnSpeed, unitData.acceleration);
            }

            // Initialize Combat
            if (attackerComponent != null && unitData.primaryWeapon != null)
            {
                attackerComponent.Initialize(unitData.primaryWeapon, unitData.secondaryWeapon);
            }

            // Initialize Vision
            if (fogRevealerComponent != null)
            {
                fogRevealerComponent.Initialize(unitData.visionRange, unitData.detectionRange);
            }
        }

        protected override void RegisterEntity()
        {
            // Register with Unit Manager
            if (Systems.UnitManager.Instance != null)
            {
                Systems.UnitManager.Instance.RegisterUnit(this);
            }
        }

        protected override void UnregisterEntity()
        {
            // Unregister from Unit Manager
            if (Systems.UnitManager.Instance != null)
            {
                Systems.UnitManager.Instance.UnregisterUnit(this);
            }
        }

        public override void OnDestroyed()
        {
            base.OnDestroyed();
            // Spawn death effects, play sounds, etc.
            // EventManager.OnUnitDestroyed?.Invoke(this);
        }
    }
}