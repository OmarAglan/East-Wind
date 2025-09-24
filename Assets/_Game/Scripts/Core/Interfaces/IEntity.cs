using UnityEngine;

namespace RTS.Core
{
    public interface IEntity
    {
        int EntityID { get; }
        int OwnerID { get; }
        Transform Transform { get; }
        GameObject GameObject { get; }
        bool IsAlive { get; }

        T GetEntityComponent<T>() where T : class;
        void OnDestroyed();
    }
}