using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 轨道相机：按住左键或右键拖拽旋转视角，滚轮推拉远近。
/// 相机始终看向 target（本 Demo 里是那个来回运动的球）。
/// 类名必须与文件名一致（OrbitCamera.cs）。
/// </summary>
public class OrbitCamera : MonoBehaviour
{
    public Transform target;                  // 环绕目标，留空则使用 pivotPoint
    public Vector3 pivotPoint = Vector3.zero; // 手动指定的环绕中心

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
        // 从场景里摆好的相机位置反解出球坐标，编辑器里怎么摆就从哪里开始
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
