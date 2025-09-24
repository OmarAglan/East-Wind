using UnityEngine;
using System.Collections;
using RTS.Data;
using RTS.Core;

namespace RTS.Components
{
    public class AttackerComponent : MonoBehaviour
    {
        [Header("Combat Settings")]
        [SerializeField] private WeaponData primaryWeapon;
        [SerializeField] private WeaponData secondaryWeapon;
        [SerializeField] private Transform turretTransform; // For units with turrets
        [SerializeField] private Transform weaponMountPoint;

        [Header("Current State")]
        [SerializeField] private IEntity currentTarget;
        [SerializeField] private float lastFireTime;
        [SerializeField] private bool isInRange;
        [SerializeField] private bool canSeeTarget;

        private MoverComponent mover;
        private float nextFireTime;

        public IEntity CurrentTarget => currentTarget;
        public bool HasTarget => currentTarget != null && currentTarget.IsAlive;
        public bool IsEngaging => HasTarget && isInRange;

        public delegate void CombatStateChanged(IEntity target);
        public event CombatStateChanged OnTargetAcquired;
        public event CombatStateChanged OnTargetLost;

        void Awake()
        {
            mover = GetComponent<MoverComponent>();
        }

        public void Initialize(WeaponData primary, WeaponData secondary = null)
        {
            primaryWeapon = primary;
            secondaryWeapon = secondary;
        }

        public void SetTarget(IEntity target)
        {
            if (currentTarget != target)
            {
                var previousTarget = currentTarget;
                currentTarget = target;

                if (target != null)
                {
                    OnTargetAcquired?.Invoke(target);
                }
                else if (previousTarget != null)
                {
                    OnTargetLost?.Invoke(previousTarget);
                }
            }
        }

        public void AttackTarget(IEntity target)
        {
            SetTarget(target);

            // Move to attack range if we have a mover
            if (mover != null && target != null)
            {
                float attackRange = GetEffectiveRange();
                float distance = Vector3.Distance(transform.position, target.Transform.position);

                if (distance > attackRange)
                {
                    // Move to attack range
                    Vector3 direction = (transform.position - target.Transform.position).normalized;
                    Vector3 attackPosition = target.Transform.position + direction * (attackRange * 0.9f);
                    mover.MoveTo(attackPosition);
                }
            }
        }

        void Update()
        {
            if (!HasTarget) return;

            UpdateTargeting();

            if (canSeeTarget && isInRange)
            {
                TryFire();
            }
        }

        private void UpdateTargeting()
        {
            if (currentTarget == null || !currentTarget.IsAlive)
            {
                SetTarget(null);
                return;
            }

            float distance = Vector3.Distance(transform.position, currentTarget.Transform.position);
            float range = GetEffectiveRange();

            isInRange = distance <= range;
            canSeeTarget = CheckLineOfSight(currentTarget);

            // Face target
            if (isInRange)
            {
                RotateTowardsTarget();
            }
        }

        private void TryFire()
        {
            if (Time.time < nextFireTime) return;
            if (primaryWeapon == null) return;

            Fire(primaryWeapon);

            // Calculate next fire time based on rate of fire
            float fireInterval = 60f / primaryWeapon.rateOfFire; // Convert RPM to seconds
            nextFireTime = Time.time + fireInterval;
        }

        private void Fire(WeaponData weapon)
        {
            // Apply damage to target
            var targetHealth = currentTarget.GameObject.GetComponent<HealthComponent>();
            if (targetHealth != null)
            {
                float damage = CalculateDamage(weapon);
                targetHealth.TakeDamage(damage, weapon.damageType);
            }

            // Spawn projectile if exists
            if (weapon.projectilePrefab != null && weaponMountPoint != null)
            {
                SpawnProjectile(weapon);
            }

            // Play fire sound
            if (weapon.fireSound != null)
            {
                AudioSource.PlayClipAtPoint(weapon.fireSound, transform.position);
            }

            // Muzzle flash
            if (weapon.muzzleFlashPrefab != null && weaponMountPoint != null)
            {
                var flash = Instantiate(weapon.muzzleFlashPrefab, weaponMountPoint.position, weaponMountPoint.rotation);
                Destroy(flash, 0.1f);
            }
        }

        private void SpawnProjectile(WeaponData weapon)
        {
            // We'll implement projectile system later
            // For now, instant hit
        }

        private float CalculateDamage(WeaponData weapon)
        {
            float damage = weapon.damage;

            // Apply accuracy
            if (Random.Range(0f, 1f) > weapon.accuracy)
            {
                damage *= 0.5f; // Glancing hit
            }

            // Apply moving accuracy penalty if applicable
            if (mover != null && mover.IsMoving)
            {
                damage *= weapon.movingAccuracyMultiplier;
            }

            return damage;
        }

        private void RotateTowardsTarget()
        {
            if (turretTransform != null)
            {
                // Rotate turret towards target
                Vector3 direction = (currentTarget.Transform.position - turretTransform.position).normalized;
                direction.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, targetRotation, Time.deltaTime * 5f);
            }
            else
            {
                // Rotate entire unit
                Vector3 direction = (currentTarget.Transform.position - transform.position).normalized;
                direction.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

        private bool CheckLineOfSight(IEntity target)
        {
            // Simple raycast for now
            Vector3 start = transform.position + Vector3.up;
            Vector3 end = target.Transform.position + Vector3.up;

            if (Physics.Raycast(start, (end - start).normalized, out RaycastHit hit, GetEffectiveRange()))
            {
                return hit.collider.gameObject == target.GameObject;
            }

            return false;
        }

        private float GetEffectiveRange()
        {
            if (primaryWeapon != null)
                return primaryWeapon.range;
            return 10f; // Default range
        }
    }
}