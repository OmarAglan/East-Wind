using UnityEngine;
using UnityEngine.Events;

public class TestListener : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.AddListener("Test", _OnTest);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("Test", _OnTest);
    }

    private void _OnTest()
    {
        Debug.Log("Received The Event");
    }
}
