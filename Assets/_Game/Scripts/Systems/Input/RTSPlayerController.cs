using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using RTS.Core;
using RTS.Components;

namespace RTS.Input
{
    public class RTSPlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private int playerID = 0;
        [SerializeField] private Color teamColor = Color.blue;
        
        [Header("Selection")]
        [SerializeField] private RectTransform selectionBoxImage;
        [SerializeField] private LayerMask selectableLayer = -1;
        [SerializeField] private LayerMask groundLayer = -1;
        
        private RTSCameraController cameraController;
        private Camera cam;
        
        // Selection
        private Vector3 startMousePosition;
        private bool isBoxSelecting = false;
        private List<SelectableComponent> currentSelection = new List<SelectableComponent>();
        private List<SelectableComponent> previewSelection = new List<SelectableComponent>();
        
        // Control Groups (Ctrl + 0-9)
        private Dictionary<int, List<SelectableComponent>> controlGroups = new Dictionary<int, List<SelectableComponent>>();
        
        void Start()
        {
            cam = Camera.main;
            cameraController = cam.GetComponent<RTSCameraController>();
            
            // Create selection box UI if not assigned
            if (selectionBoxImage == null)
            {
                CreateSelectionBox();
            }
            
            // Initialize control groups
            for (int i = 0; i <= 9; i++)
            {
                controlGroups[i] = new List<SelectableComponent>();
            }
        }
        
        void Update()
        {
            HandleSelection();
            HandleCommands();
            HandleControlGroups();
        }
        
        private void HandleSelection()
        {
            // Left mouse button down - start selection
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                startMousePosition = UnityEngine.Input.mousePosition;
                isBoxSelecting = false;
                
                // Check for single click selection
                Ray ray = cam.ScreenPointToRay(UnityEngine.Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectableLayer))
                {
                    SelectableComponent selectable = hit.collider.GetComponent<SelectableComponent>();
                    if (selectable != null)
                    {
                        // Check if this unit belongs to us
                        var entity = selectable.GetComponent<IEntity>();
                        if (entity != null && entity.OwnerID == playerID)
                        {
                            if (!UnityEngine.Input.GetKey(KeyCode.LeftShift))
                            {
                                ClearSelection();
                            }
                            
                            if (currentSelection.Contains(selectable))
                            {
                                RemoveFromSelection(selectable);
                            }
                            else
                            {
                                AddToSelection(selectable);
                            }
                        }
                    }
                    else if (!UnityEngine.Input.GetKey(KeyCode.LeftShift))
                    {
                        ClearSelection();
                    }
                }
                else if (!UnityEngine.Input.GetKey(KeyCode.LeftShift))
                {
                    ClearSelection();
                }
            }
            
            // Left mouse button held - box selection
            if (UnityEngine.Input.GetMouseButton(0))
            {
                if (Vector3.Distance(startMousePosition, UnityEngine.Input.mousePosition) > 10)
                {
                    isBoxSelecting = true;
                    UpdateSelectionBox();
                    UpdatePreviewSelection();
                }
            }
            
            // Left mouse button up - finish selection
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                if (isBoxSelecting)
                {
                    FinishBoxSelection();
                }
                
                selectionBoxImage.gameObject.SetActive(false);
                isBoxSelecting = false;
            }
        }
        
        private void HandleCommands()
        {
            // Right click - issue commands
            if (UnityEngine.Input.GetMouseButtonDown(1) && currentSelection.Count > 0)
            {
                Ray ray = cam.ScreenPointToRay(UnityEngine.Input.mousePosition);
                
                // Check if we clicked on an enemy
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectableLayer))
                {
                    var targetEntity = hit.collider.GetComponent<IEntity>();
                    if (targetEntity != null && targetEntity.OwnerID != playerID)
                    {
                        // Attack command
                        IssueAttackCommand(targetEntity);
                        return;
                    }
                }
                
                // Otherwise, it's a move command
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    IssueMoveCommand(hit.point);
                }
            }
            
            // Stop command (S key)
            if (UnityEngine.Input.GetKeyDown(KeyCode.S) && currentSelection.Count > 0)
            {
                IssueStopCommand();
            }
        }
        
        private void HandleControlGroups()
        {
            // Check for control group creation/selection
            for (int i = 0; i <= 9; i++)
            {
                KeyCode key = (KeyCode)((int)KeyCode.Alpha0 + i);
                
                if (UnityEngine.Input.GetKeyDown(key))
                {
                    if (UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(KeyCode.RightControl))
                    {
                        // Create control group
                        controlGroups[i] = new List<SelectableComponent>(currentSelection);
                        Debug.Log($"Control group {i} created with {currentSelection.Count} units");
                    }
                    else
                    {
                        // Select control group
                        SelectControlGroup(i);
                    }
                }
            }
        }
        
        private void IssueMoveCommand(Vector3 destination)
        {
            List<Vector3> positions = GetFormationPositions(destination, currentSelection.Count);
            
            for (int i = 0; i < currentSelection.Count; i++)
            {
                var mover = currentSelection[i].GetComponent<MoverComponent>();
                if (mover != null)
                {
                    mover.MoveTo(positions[i]);
                }
            }
            
            // Visual feedback - spawn move marker
            ShowCommandFeedback(destination, "Move");
        }
        
        private void IssueAttackCommand(IEntity target)
        {
            foreach (var selected in currentSelection)
            {
                var attacker = selected.GetComponent<AttackerComponent>();
                if (attacker != null)
                {
                    attacker.AttackTarget(target);
                }
            }
            
            // Visual feedback
            ShowCommandFeedback(target.Transform.position, "Attack");
        }
        
        private void IssueStopCommand()
        {
            foreach (var selected in currentSelection)
            {
                var mover = selected.GetComponent<MoverComponent>();
                if (mover != null)
                {
                    mover.Stop();
                }
                
                var attacker = selected.GetComponent<AttackerComponent>();
                if (attacker != null)
                {
                    attacker.SetTarget(null);
                }
            }
        }
        
        private List<Vector3> GetFormationPositions(Vector3 center, int unitCount)
        {
            List<Vector3> positions = new List<Vector3>();
            
            if (unitCount == 1)
            {
                positions.Add(center);
                return positions;
            }
            
            // Simple grid formation
            int columns = Mathf.CeilToInt(Mathf.Sqrt(unitCount));
            float spacing = 3f;
            
            for (int i = 0; i < unitCount; i++)
            {
                int row = i / columns;
                int col = i % columns;
                
                float x = (col - columns / 2f) * spacing;
                float z = (row - columns / 2f) * spacing;
                
                positions.Add(center + new Vector3(x, 0, z));
            }
            
            return positions;
        }
        
        private void UpdateSelectionBox()
        {
            if (!selectionBoxImage.gameObject.activeSelf)
                selectionBoxImage.gameObject.SetActive(true);
            
            Vector3 currentMousePosition = UnityEngine.Input.mousePosition;
            
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startMousePosition.x, currentMousePosition.x),
                Mathf.Min(startMousePosition.y, currentMousePosition.y),
                0
            );
            
            Vector3 upperRight = new Vector3(
                Mathf.Max(startMousePosition.x, currentMousePosition.x),
                Mathf.Max(startMousePosition.y, currentMousePosition.y),
                0
            );
            
            selectionBoxImage.position = lowerLeft;
            selectionBoxImage.sizeDelta = upperRight - lowerLeft;
        }
        
        private void UpdatePreviewSelection()
        {
            // Clear preview
            foreach (var selectable in previewSelection)
            {
                if (!currentSelection.Contains(selectable))
                {
                    selectable.SetSelected(false);
                }
            }
            previewSelection.Clear();
            
            // Get bounds
            Bounds selectionBounds = GetViewportBounds();
            
            // Find all units in bounds
            var allSelectables = FindObjectsOfType<SelectableComponent>();
            foreach (var selectable in allSelectables)
            {
                var entity = selectable.GetComponent<IEntity>();
                if (entity == null || entity.OwnerID != playerID)
                    continue;
                
                Vector3 screenPos = cam.WorldToViewportPoint(selectable.transform.position);
                if (selectionBounds.Contains(screenPos))
                {
                    previewSelection.Add(selectable);
                    selectable.SetSelected(true);
                }
            }
        }
        
        private void FinishBoxSelection()
        {
            if (!UnityEngine.Input.GetKey(KeyCode.LeftShift))
            {
                ClearSelection();
            }
            
            foreach (var selectable in previewSelection)
            {
                AddToSelection(selectable);
            }
            
            previewSelection.Clear();
        }
        
        private Bounds GetViewportBounds()
        {
            Vector3 v1 = cam.ScreenToViewportPoint(startMousePosition);
            Vector3 v2 = cam.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
            
            Vector3 min = Vector3.Min(v1, v2);
            Vector3 max = Vector3.Max(v1, v2);
            
            Bounds bounds = new Bounds();
            bounds.SetMinMax(min, max);
            
            return bounds;
        }
        
        private void AddToSelection(SelectableComponent selectable)
        {
            if (!currentSelection.Contains(selectable))
            {
                currentSelection.Add(selectable);
                selectable.SetSelected(true);
            }
        }
        
        private void RemoveFromSelection(SelectableComponent selectable)
        {
            currentSelection.Remove(selectable);
            selectable.SetSelected(false);
        }
        
        private void ClearSelection()
        {
            foreach (var selectable in currentSelection)
            {
                if (selectable != null)
                {
                    selectable.SetSelected(false);
                }
            }
            currentSelection.Clear();
        }
        
        private void SelectControlGroup(int groupNumber)
        {
            ClearSelection();
            
            // Remove any destroyed units
            controlGroups[groupNumber].RemoveAll(s => s == null);
            
            foreach (var selectable in controlGroups[groupNumber])
            {
                AddToSelection(selectable);
            }
            
            Debug.Log($"Selected control group {groupNumber} with {currentSelection.Count} units");
        }
        
        private void CreateSelectionBox()
        {
            GameObject selectionBox = new GameObject("SelectionBox");
            selectionBox.transform.SetParent(GameObject.Find("Canvas")?.transform ?? CreateCanvas().transform);
            
            selectionBoxImage = selectionBox.AddComponent<RectTransform>();
            var image = selectionBox.AddComponent<UnityEngine.UI.Image>();
            image.color = new Color(0, 1, 0, 0.25f);
            
            selectionBox.SetActive(false);
        }
        
        private GameObject CreateCanvas()
        {
            GameObject canvasGO = new GameObject("Canvas");
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            return canvasGO;
        }
        
        private void ShowCommandFeedback(Vector3 position, string commandType)
        {
            // TODO: Spawn visual feedback for commands
            Debug.Log($"{commandType} command issued at {position}");
        }
    }
}