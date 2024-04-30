using UnityEngine;

public class TestEmitter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.TriggerEvent("Test");
    }
}
