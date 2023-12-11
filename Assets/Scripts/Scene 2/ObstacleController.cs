using System;
using Unity.Mathematics;
using Unity.VisualScripting;

using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private float timeAccumulator = 0.0f;

    public float Time;
    public Vector2 StartPos;
    public Vector2 EndPos;
    public float StartScale;
    public float EndScale;

    public event EventHandler OnMoveEnd;

    public float Percentage { get; private set; } = 0.0f;

    void Update()
    {
        timeAccumulator += UnityEngine.Time.deltaTime;
        Percentage = timeAccumulator / Time;
        
        if (Percentage >= 1.0f)
        {
            OnMoveEnd?.Invoke(this, new EventArgs());
            return;
        }

        Percentage = Percentage * Percentage;

        float scale = math.lerp(StartScale, EndScale, Percentage);
        transform.localScale = new Vector3(scale, scale, 1.0f);
        transform.localPosition = math.lerp(StartPos, EndPos, Percentage).ConvertTo<Vector2>();
    }
}
