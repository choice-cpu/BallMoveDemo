using UnityEngine;

public class BallMove : MonoBehaviour
{
    [Header("路径")]
    public Vector3 pointA = new Vector3(-4, 0, 0);
    public Vector3 pointB = new Vector3(4, 0, 0);

    [Header("运动")]
    public float speed = 2.5f;
    public float waitTime = 0.4f;
    public float curveStrength = 0.7f;
    public bool startFromA = true;

    float t = 0;
    float waitTimer = 0;
    bool moving = true;
    bool forward = true;

    void Start()
    {
        forward = startFromA;
        t = startFromA ? 0f : 1f;
        transform.position = GetPosition(t);
    }

    void Update()
    {
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
        if (distance < 0.0001f) return;

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

    Vector3 GetPosition(float progress)
    {
        float p = progress + curveStrength / (2f * Mathf.PI) * Mathf.Sin(2f * Mathf.PI * progress);
        return Vector3.Lerp(pointA, pointB, p);
    }
}
