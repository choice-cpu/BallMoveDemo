using UnityEngine;

/// <summary>
/// 让物体在 A、B 两点之间往返移动。
/// 到达端点后停顿 waitTime 秒，再折返。
/// 类名必须与文件名一致（BallMove.cs）。
/// </summary>
public class BallMove : MonoBehaviour
{
    [Header("路径")]
    public Vector3 pointA = new Vector3(-4, 0, 0);   // 端点 A
    public Vector3 pointB = new Vector3(4, 0, 0);    // 端点 B

    [Header("运动")]
    public float speed = 2.5f;          // 移动速度（世界单位 / 秒）
    public float waitTime = 0.4f;       // 到达端点后停多久
    public float curveStrength = 0.7f;  // 缓动强度：0 = 匀速，越大中间越慢
    public bool startFromA = true;      // 是否从 A 点出发

    float t = 0;            // 路径进度：0 在 A 点，1 在 B 点
    float waitTimer = 0;    // 端点停留计时
    bool moving = true;     // 是否处于移动状态
    bool forward = true;    // 当前是 A→B 还是 B→A

    void Start()
    {
        forward = startFromA;
        t = startFromA ? 0f : 1f;
        transform.position = GetPosition(t);
    }

    void Update()
    {
        // 到端点先停一会儿
        if (!moving)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                moving = true;
            }
            return;
        }

        float distance = Vector3.Distance(pointA, pointB);
        if (distance < 0.0001f) return;   // 防止 A、B 重合时除零

        // 把「速度」换算成「进度增量」：走完 AB 全程需要 distance / speed 秒
        float step = Time.deltaTime * speed / distance;
        t += forward ? step : -step;

        if (t >= 1f || t <= 0f)
        {
            t = Mathf.Clamp01(t);
            forward = !forward;
            moving = false;
            waitTimer = 0f;
        }

        transform.position = GetPosition(t);
    }

    // 叠加一个 sin 曲线做缓动：两端快、中间慢
    // 实际速度 = speed * (1 + curveStrength * cos(2πt))
    Vector3 GetPosition(float progress)
    {
        float p = progress + curveStrength / (2f * Mathf.PI) * Mathf.Sin(2f * Mathf.PI * progress);
        return Vector3.Lerp(pointA, pointB, p);
    }
}
