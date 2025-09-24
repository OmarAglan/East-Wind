using UnityEngine;

namespace RTS.Components
{
    public class SelectableComponent : MonoBehaviour
    {
        [Header("Selection")]
        [SerializeField] private bool isSelected = false;
        [SerializeField] private GameObject selectionIndicator;

        public bool IsSelected => isSelected;

        public delegate void SelectionChanged(bool selected);
        public event SelectionChanged OnSelectionChanged;

        void Start()
        {
            // Create selection indicator if not assigned
            if (selectionIndicator == null)
            {
                CreateSelectionIndicator();
            }

            SetSelected(false);
        }

        private void CreateSelectionIndicator()
        {
            // Create a simple selection circle
            GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            indicator.transform.SetParent(transform);
            indicator.transform.localPosition = new Vector3(0, 0.1f, 0);
            indicator.transform.localScale = new Vector3(2f, 0.01f, 2f);

            // Remove collider
            Destroy(indicator.GetComponent<Collider>());

            // Set material (you'll want to create a proper selection material)
            var renderer = indicator.GetComponent<Renderer>();
            renderer.material.color = new Color(0, 1, 0, 0.5f);

            selectionIndicator = indicator;
        }

        public void SetSelected(bool selected)
        {
            isSelected = selected;

            if (selectionIndicator != null)
            {
                selectionIndicator.SetActive(selected);
            }

            OnSelectionChanged?.Invoke(selected);
        }

        public void OnHover(bool hovering)
        {
            // Visual feedback for hovering
            // We'll implement this later
        }
    }
}