using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [Header("基本設定")]
    public PlanetData planetData;
    public Transform orbitCenter;

    [Header("離脱設定")]
    public float escapeSpeedMultiplier = 0.1f;
    private bool isOrbiting = true;
    private Vector3 previousPosition; // 速度計算用
    private float escapeTime;
    private Vector3 escapePosition;
    private Vector3 escapeVelocity;

    void OnEnable()
    {
        SunEventManager.OnSunGravityCollapse += GoRogue;
    }

    void OnDisable()
    {
        SunEventManager.OnSunGravityCollapse -= GoRogue;
    }

    void Start()
    {
        if (orbitCenter != null)
        {
            previousPosition = transform.position;
        }
    }

    void Update()
    {
        if (TimeController.Instance == null) return;
        float currentTime = TimeController.Instance.simulationTime;

        if (isOrbiting)
        {
            UpdatePlanetState(currentTime);

            if (Time.deltaTime > 0)
            {
                escapeVelocity = (transform.position - previousPosition) / Time.deltaTime;
                previousPosition = transform.position;
            }
        }
        else
        {
            float timeSinceEscape = currentTime - escapeTime;

            transform.position = escapePosition + (escapeVelocity * escapeSpeedMultiplier * timeSinceEscape);
        }
    }

    public void GoRogue()
    {
        if (!isOrbiting) return;

        Debug.Log(gameObject.name + " が軌道を離脱しました。");
        isOrbiting = false;
        escapeTime = TimeController.Instance.simulationTime;
        escapePosition = transform.position;
    }

    void UpdatePlanetState(float time)
    {
        if (planetData == null) return;

        Transform currentOrbitCenter = (transform.parent != null && isOrbiting) ? transform.parent : orbitCenter;
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
}