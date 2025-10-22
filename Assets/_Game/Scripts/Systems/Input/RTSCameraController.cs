using UnityEngine;

namespace RTS.Input
{
    public class RTSCameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private float panSpeed = 20f;
        [SerializeField] private float panBorderThickness = 10f;
        [SerializeField] private Vector2 panLimit = new Vector2(100, 100);
        
        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 20f;
        [SerializeField] private float minZoom = 20f;
        [SerializeField] private float maxZoom = 120f;
        [SerializeField] private float zoomSmoothness = 5f;
        
        [Header("Rotation Settings")]
        [SerializeField] private float rotationSpeed = 100f;
        
        private Camera cam;
        private float currentZoom = 60f;
        private float targetZoom = 60f;
        private float currentRotation = 0f;
        
        void Start()
        {
            cam = GetComponent<Camera>();
            if (cam == null)
                cam = GetComponentInChildren<Camera>();
            
            // Set initial position
            transform.position = new Vector3(50, 50, 50);
            transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        }
        
        void Update()
        {
            HandlePanning();
            HandleZoom();
            HandleRotation();
        }
        
        private void HandlePanning()
        {
            Vector3 pos = transform.position;
            Vector3 move = Vector3.zero;
            
            // Keyboard panning (WASD or Arrow Keys)
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float vertical = UnityEngine.Input.GetAxis("Vertical");
            
            move += transform.right * horizontal;
            move += transform.forward * vertical;
            move.y = 0;
            
            // Edge panning
            if (UnityEngine.Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                move += transform.forward;
                move.y = 0;
            }
            if (UnityEngine.Input.mousePosition.y <= panBorderThickness)
            {
                move -= transform.forward;
                move.y = 0;
            }
            if (UnityEngine.Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                move += transform.right;
            }
            if (UnityEngine.Input.mousePosition.x <= panBorderThickness)
            {
                move -= transform.right;
            }
            
            // Middle mouse button drag
            if (UnityEngine.Input.GetMouseButton(2))
            {
                float dragX = UnityEngine.Input.GetAxis("Mouse X");
                float dragY = UnityEngine.Input.GetAxis("Mouse Y");
                
                move -= transform.right * dragX * panSpeed;
                move -= transform.forward * dragY * panSpeed;
                move.y = 0;
            }
            
            // Apply movement
            move.Normalize();
            pos += move * panSpeed * Time.deltaTime;
            
            // Apply limits
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
            
            transform.position = pos;
        }
        
        private void HandleZoom()
        {
            // Mouse wheel zoom
            float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                targetZoom -= scroll * zoomSpeed;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }
            
            // Smooth zoom
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSmoothness);
            
            // Apply zoom by adjusting Y position
            Vector3 pos = transform.position;
            pos.y = currentZoom;
            transform.position = pos;
            
            // Adjust camera angle based on zoom
            float angle = Mathf.Lerp(30f, 60f, (currentZoom - minZoom) / (maxZoom - minZoom));
            transform.rotation = Quaternion.Euler(angle, currentRotation, 0);
        }
        
        private void HandleRotation()
        {
            // Q and E for rotation
            if (UnityEngine.Input.GetKey(KeyCode.Q))
            {
                currentRotation -= rotationSpeed * Time.deltaTime;
            }
            if (UnityEngine.Input.GetKey(KeyCode.E))
            {
                currentRotation += rotationSpeed * Time.deltaTime;
            }
            
            // Apply rotation
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentRotation, 0);
        }
        
        public Vector3 GetMouseWorldPosition()
        {
            Ray ray = cam.ScreenPointToRay(UnityEngine.Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            
            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
            
            return Vector3.zero;
        }
    }
}