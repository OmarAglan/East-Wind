using UnityEngine;

namespace RTS.Components
{
    public class FogRevealerComponent : MonoBehaviour
    {
        [Header("Vision Settings")]
        [SerializeField] private float visionRange = 10f;
        [SerializeField] private float detectionRange = 8f;
        [SerializeField] private int ownerID;

        public float VisionRange => visionRange;
        public float DetectionRange => detectionRange;
        public int OwnerID => ownerID;

        void Start()
        {
            // Register with Fog of War system when we implement it
            RegisterWithFogSystem();
        }

        void OnDestroy()
        {
            // Unregister from Fog of War system
            UnregisterFromFogSystem();
        }

        public void Initialize(float vision, float detection)
        {
            visionRange = vision;
            detectionRange = detection;

            var entity = GetComponent<Core.IEntity>();
            if (entity != null)
            {
                ownerID = entity.OwnerID;
            }
        }

        private void RegisterWithFogSystem()
        {
            // TODO: Implement when we create FogOfWarManager
            // FogOfWarManager.Instance?.RegisterRevealer(this);
        }

        private void UnregisterFromFogSystem()
        {
            // TODO: Implement when we create FogOfWarManager
            // FogOfWarManager.Instance?.UnregisterRevealer(this);
        }

        void OnDrawGizmosSelected()
        {
            // Vision range in green
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawWireSphere(transform.position, visionRange);

            // Detection range in yellow
            Gizmos.color = new Color(1, 1, 0, 0.2f);
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}