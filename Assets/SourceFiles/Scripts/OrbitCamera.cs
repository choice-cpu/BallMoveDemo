using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 pivotPoint = Vector3.zero;

    [Header("缩放")]
    public float minDistance = 3f;
    public float maxDistance = 25f;
    public float zoomSpeed = 0.01f;

    [Header("旋转")]
    public float rotateSpeed = 0.25f;
    public Vector2 pitchLimit = new Vector2(-25f, 80f);

    float yaw, pitch, distance;

    void Start()
    {
        Vector3 offset = transform.position - GetPivot();
        distance = Mathf.Clamp(offset.magnitude, minDistance, maxDistance);
        if (offset.sqrMagnitude < 0.0001f) offset = Vector3.back;

        Vector3 dir = offset.normalized;
        pitch = Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        yaw = Mathf.Atan2(-dir.x, -dir.z) * Mathf.Rad2Deg;
    }

    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.isPressed || mouse.rightButton.isPressed)
        {
            Vector2 delta = mouse.delta.ReadValue();
            yaw += delta.x * rotateSpeed;
            pitch = Mathf.Clamp(pitch - delta.y * rotateSpeed, pitchLimit.x, pitchLimit.y);
        }

        float scroll = mouse.scroll.ReadValue().y;
        if (scroll != 0f)
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);

        Vector3 pivot = GetPivot();
        transform.position = pivot + Quaternion.Euler(pitch, yaw, 0f) * Vector3.back * distance;
        transform.LookAt(pivot);
    }

    Vector3 GetPivot()
    {
        return target != null ? target.position : pivotPoint;
    }
}
