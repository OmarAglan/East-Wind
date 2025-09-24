using UnityEngine;
using RTS.Data;

namespace RTS.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [Header("Health Stats")]
        [SerializeField] private float currentHealth;
        [SerializeField] private float maxHealth;
        [SerializeField] private float armor;
        [SerializeField] private ArmorType armorType;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public float HealthPercentage => currentHealth / maxHealth;
        public bool IsAlive => currentHealth > 0;

        public delegate void HealthChanged(float current, float max);
        public event HealthChanged OnHealthChanged;

        public delegate void EntityDied();
        public event EntityDied OnDied;

        public void Initialize(float maxHP, float armorValue, ArmorType type)
        {
            maxHealth = maxHP;
            currentHealth = maxHP;
            armor = armorValue;
            armorType = type;
        }

        public void TakeDamage(float damage, DamageType damageType)
        {
            if (!IsAlive) return;

            // Calculate actual damage based on armor
            float actualDamage = CalculateDamage(damage, damageType);

            currentHealth -= actualDamage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (!IsAlive)
            {
                Die();
            }
        }

        private float CalculateDamage(float baseDamage, DamageType damageType)
        {
            // Simple damage calculation for now
            // We'll expand this with a proper damage table later
            float armorMultiplier = 1f;

            // Basic armor effectiveness
            switch (armorType)
            {
                case ArmorType.Light:
                    armorMultiplier = damageType == DamageType.HighExplosive ? 1.5f : 1f;
                    break;
                case ArmorType.Heavy:
                    armorMultiplier = damageType == DamageType.Kinetic ? 0.5f : 0.75f;
                    break;
                case ArmorType.Reactive:
                    armorMultiplier = damageType == DamageType.HEAT ? 0.3f : 1f;
                    break;
            }

            // Apply armor reduction
            float damageReduction = armor / (armor + 100f); // Simple armor formula
            float finalDamage = baseDamage * armorMultiplier * (1f - damageReduction);

            return Mathf.Max(1f, finalDamage); // Minimum 1 damage
        }

        public void Heal(float amount)
        {
            if (!IsAlive) return;

            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        private void Die()
        {
            OnDied?.Invoke();

            // Get entity and call its OnDestroyed method
            var entity = GetComponent<Core.IEntity>();
            entity?.OnDestroyed();

            // Destroy the GameObject after a delay for death animation
            Destroy(gameObject, 2f);
        }
    }
}