using UnityEngine;

namespace RTS.Data
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "RTS/Data/Weapon")]
    public class WeaponData : ScriptableObject
    {
        [Header("Basic Properties")]
        public string weaponName;
        public float damage;
        public float rateOfFire;  // Rounds per minute
        public float range;        // In meters

        [Header("Damage Properties")]
        public DamageType damageType;
        public float armorPenetration;  // mm of RHA equivalent
        public float blastRadius;       // 0 for direct fire weapons

        [Header("Accuracy")]
        public float accuracy = 1f;     // 1 = perfect, 0 = terrible
        public float movingAccuracyMultiplier = 0.5f;

        [Header("Ammunition")]
        public int magazineSize = -1;   // -1 = unlimited
        public float reloadTime;

        [Header("Projectile")]
        public GameObject projectilePrefab;
        public float projectileSpeed;   // m/s, 0 = instant hit

        [Header("Effects")]
        public AudioClip fireSound;
        public GameObject muzzleFlashPrefab;
    }

}