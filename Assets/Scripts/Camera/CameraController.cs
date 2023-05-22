using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float _panSpeed = 20f;
    public float _panBorderThickness = 10f;
    public Vector2 _panLimit;

    public float _scrollSpeed = 3f;
    public float _minY = 10f;
    public float _maxy = 80f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - _panBorderThickness)
        {
            pos.z += _panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= _panBorderThickness)
        {
            pos.z -= _panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - _panBorderThickness)
        {
            pos.x += _panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= _panBorderThickness)
        {
            pos.x -= _panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * _scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -_panLimit.x, _panLimit.x);
        pos.y = Mathf.Clamp(pos.y, _minY, _maxy);
        pos.z = Mathf.Clamp(pos.z, -_panLimit.y, _panLimit.y);

        transform.position = pos;
    }
}
