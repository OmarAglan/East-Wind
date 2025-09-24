using UnityEngine;
using System.Collections.Generic;
using RTS.Data;

namespace RTS.Core
{
    public abstract class BaseEntity : MonoBehaviour, IEntity
    {
        [Header("Entity Core")]
        [SerializeField] protected int entityID;
        [SerializeField] protected int ownerID;

        // Cached references
        protected Dictionary<System.Type, object> cachedComponents = new Dictionary<System.Type, object>();

        // Properties
        public int EntityID => entityID;
        public int OwnerID => ownerID;
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        public abstract bool IsAlive { get; }

        protected virtual void Awake()
        {
            // Generate unique ID (will be replaced with deterministic ID for multiplayer)
            entityID = GetInstanceID();
            CacheComponents();
        }

        protected virtual void Start()
        {
            RegisterEntity();
        }

        protected virtual void OnDestroy()
        {
            UnregisterEntity();
        }

        protected abstract void CacheComponents();
        protected abstract void RegisterEntity();
        protected abstract void UnregisterEntity();

        public T GetEntityComponent<T>() where T : class
        {
            var type = typeof(T);
            if (cachedComponents.ContainsKey(type))
            {
                return cachedComponents[type] as T;
            }

            var component = GetComponent<T>();
            if (component != null)
            {
                cachedComponents[type] = component;
            }
            return component;
        }

        public virtual void OnDestroyed()
        {
            // Override in derived classes for death effects, sounds, etc.
        }

        public void SetOwner(int playerID)
        {
            ownerID = playerID;
        }
    }
}