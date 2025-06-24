
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class OrbitLineDrawer : MonoBehaviour
{
    public PlanetData planetData;
    public Transform orbitCenter;
    public int segments = 100; // 円の分割数

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = true;
        
        DrawOrbit();
    }

    void DrawOrbit()
    {
        float angle = 0f;
        float radius = planetData.orebit_radius * 10f;
        

        for (int i = 0; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * angle;
            Vector3 pos = new Vector3(
                Mathf.Cos(rad) * radius,
                0f,
                Mathf.Sin(rad) * radius
            );
            lineRenderer.SetPosition(i, orbitCenter.position + pos);
            angle += 360f / segments;
        }


    }
}
