using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [Header("基本設定")]
    public PlanetData planetData;
    public Transform orbitCenter;

    [Header("衛星用の設定 (月にのみ設定)")]
    public PlanetData parentPlanetData;
    public float escapeGravityThreshold = 5.0f;

    // ★★★ 1. 離脱速度の倍率を追加 ★★★
    [Header("離脱設定")]
    public float escapeSpeedMultiplier = 2.0f; // 離脱速度の倍率 (2.0で2倍速)

    private bool isOrbiting = true;
    private Vector3 rogueVelocity;
    private Vector3 previousPosition;

    void Start()
    {
        if (orbitCenter != null || parentPlanetData != null)
        {
            previousPosition = transform.position;
        }
    }

    void Update()
    {
        if (parentPlanetData != null && isOrbiting)
        {
            if (parentPlanetData.gravity < escapeGravityThreshold)
            {
                GoRogue();
            }
        }

        if (isOrbiting)
        {
            if (TimeController.Instance == null) return;
            
            float currentTime = TimeController.Instance.simulationTime;
            UpdatePlanetState(currentTime);

            if (Time.deltaTime > 0)
            {
                 rogueVelocity = (transform.position - previousPosition) / Time.deltaTime;
                 previousPosition = transform.position;
            }
        }
        else
        {
            // ★★★ 2. 計算した速度に倍率をかける ★★★
            transform.position += (rogueVelocity * escapeSpeedMultiplier) * Time.deltaTime;
        }
    }

    // ... (UpdatePlanetStateとGoRogueメソッドは変更なし) ...
    void UpdatePlanetState(float time)
    {
        if (planetData == null) return;

        Transform currentOrbitCenter = (parentPlanetData != null) ? parentPlanetData.transform : orbitCenter;
        if (currentOrbitCenter == null) return;

        if (Mathf.Abs(planetData.revolution_speed) > 0.001f)
        {
            float revolutionDegreesPerHour = 360f / planetData.revolution_speed;
            float currentRevolutionAngle = revolutionDegreesPerHour * time;
            float radian = currentRevolutionAngle * Mathf.Deg2Rad;
            Vector3 orbitPos = new Vector3(
                Mathf.Cos(radian) * planetData.orebit_radius * 10,
                0,
                Mathf.Sin(radian) * planetData.orebit_radius * 10 
            );
            transform.position = currentOrbitCenter.position + orbitPos;
        }
        if (Mathf.Abs(planetData.rotation_speed) > 0.001f)
        {
            float rotationDegreesPerHour = 360f / planetData.rotation_speed;
            float currentRotationAngle = rotationDegreesPerHour * time / 10;
            Quaternion axialTilt = Quaternion.Euler(0, 0, planetData.angle);
            Quaternion rotation = Quaternion.AngleAxis(currentRotationAngle, Vector3.up);
            transform.rotation = axialTilt * rotation;
        }
    }
    
    public void GoRogue()
    {
        if (!isOrbiting) return;
        Debug.Log(gameObject.name + " が軌道を離脱しました。");
        isOrbiting = false;
    }
}