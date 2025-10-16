using UnityEngine;

public class E : MonoBehaviour
{
    [Header("Circle settings")]
    public float radius = 1f;
    [Range(3, 1024)]
    public int segments = 64;
    public bool loop = true;

    [Header("Line settings")]
    public float startWidth = 0.05f;
    public float endWidth = 0.05f;

    [Tooltip("If true, redraw every frame (for dynamic radius/segments).")]
    public bool updateEveryFrame = false;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.loop = loop;
        lr.useWorldSpace = true; // always in world space
        lr.startWidth = startWidth;
        lr.endWidth = endWidth;

        // Assign a simple material if none exists
        if (lr.sharedMaterial == null)
        {
            var mat = new Material(Shader.Find("Unlit/Color"));
            mat.color = Color.white;
            lr.material = mat;
        }

        DrawCircle();
    }

    void Update()
    {
        if (updateEveryFrame)
            DrawCircle();
    }

    public void DrawCircle()
    {
        if (segments < 3) segments = 3;

        int pointCount = loop ? segments : segments + 1;
        lr.positionCount = pointCount;

        float angleStep = 2f * Mathf.PI / segments;

        for (int i = 0; i < pointCount; i++)
        {
            int idx = (i < segments) ? i : 0;
            float angle = idx * angleStep;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // Circle centered at world origin
            lr.SetPosition(i, new Vector3(x, y, 0f));
        }

        lr.startWidth = startWidth;
        lr.endWidth = endWidth;
        lr.loop = loop;
        lr.useWorldSpace = true;
    }
}
