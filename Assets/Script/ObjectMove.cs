using UnityEngine;

public class PlanetMotion : MonoBehaviour
{
    public PlanetData planetData;         // インスペクターで設定
    public Transform orbitCenter;         // 太陽などの中心天体

    private float angle;

    void Update()
    {
        if (planetData == null || orbitCenter == null) return;

        // --- 自転処理 ---
        transform.Rotate(Vector3.up, (360 /planetData.rotation_speed / 3600) * Time.deltaTime * 1440);

        // --- 公転処理 ---
        angle += (360 / (planetData.revolution_speed*3600)) * Time.deltaTime * 86400;
        float radian = angle * Mathf.Deg2Rad;
        Vector3 orbitPos = new Vector3(
            Mathf.Cos(radian) * planetData.orebit_radius * 10,
            0,
            Mathf.Sin(radian) * planetData.orebit_radius * 10 
        );
        transform.position = orbitCenter.position + orbitPos;
    }
}