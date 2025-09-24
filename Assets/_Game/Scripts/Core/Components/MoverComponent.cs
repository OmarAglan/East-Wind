using UnityEngine;
using UnityEngine.AI;

namespace RTS.Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoverComponent : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSpeed = 120f;
        [SerializeField] private float acceleration = 8f;
        [SerializeField] private float stoppingDistance = 0.5f;

        private NavMeshAgent agent;
        private Vector3 targetPosition;
        private bool isMoving = false;

        public bool IsMoving => isMoving;
        public Vector3 TargetPosition => targetPosition;
        public float CurrentSpeed => agent != null ? agent.velocity.magnitude : 0f;

        public delegate void MovementStateChanged(bool moving);
        public event MovementStateChanged OnMovementStateChanged;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                agent = gameObject.AddComponent<NavMeshAgent>();
            }
        }

        public void Initialize(float speed, float turnSpd, float accel)
        {
            moveSpeed = speed;
            turnSpeed = turnSpd;
            acceleration = accel;

            // Apply to NavMeshAgent
            agent.speed = moveSpeed;
            agent.angularSpeed = turnSpeed;
            agent.acceleration = acceleration;
            agent.stoppingDistance = stoppingDistance;
        }

        public void MoveTo(Vector3 position)
        {
            if (agent == null || !agent.isOnNavMesh) return;

            targetPosition = position;
            agent.isStopped = false;
            agent.SetDestination(position);

            if (!isMoving)
            {
                isMoving = true;
                OnMovementStateChanged?.Invoke(true);
            }
        }

        public void Stop()
        {
            if (agent == null || !agent.isOnNavMesh) return;

            agent.isStopped = true;

            if (isMoving)
            {
                isMoving = false;
                OnMovementStateChanged?.Invoke(false);
            }
        }

        void Update()
        {
            if (!agent || !agent.isOnNavMesh) return;

            // Check if we've reached destination
            if (isMoving)
            {
                if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
                {
                    if (agent.velocity.sqrMagnitude < 0.01f)
                    {
                        isMoving = false;
                        OnMovementStateChanged?.Invoke(false);
                    }
                }
            }
        }

        public void SetFormationPosition(Vector3 position, Quaternion rotation)
        {
            // Used for group movement in formation
            MoveTo(position);
            // We'll implement formation rotation later
        }

        public bool CanReachPosition(Vector3 position)
        {
            NavMeshPath path = new NavMeshPath();
            return agent.CalculatePath(position, path) && path.status == NavMeshPathStatus.PathComplete;
        }
    }
}