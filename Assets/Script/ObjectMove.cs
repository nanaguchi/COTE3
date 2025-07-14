using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [Header("基本設定")]
    public PlanetData planetData;
    public Transform orbitCenter;

    [Header("衛星用の設定 (月にのみ設定)")]
    public PlanetData parentPlanetData;
    public float escapeGravityThreshold = 5.0f;

    [Header("離脱設定")]
    public float escapeSpeedMultiplier = 2.0f;

    private bool isOrbiting = true;
    private Vector3 rogueVelocity;
    private Vector3 previousPosition;

    // ★変更点1: 現在の角度を保持する変数を追加
    private float currentRevolutionAngle = 0f;
    private float currentRotationAngle = 0f;

    void Start()
    {
        if (orbitCenter != null || parentPlanetData != null)
        {
            previousPosition = transform.position;
        }
        // ★必要であれば、ここで初期位置をランダム化したり、
        // 保存された値から角度を復元したりできます。
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
            
            // ★変更点2: 絶対時間ではなく、フレームごとの「時間の進み」を取得
            // TimeControllerにtimeMultiplier（時間倍率）があると仮定
            float simulationDeltaTime = Time.deltaTime * TimeController.Instance.timeMultiplier;

            // ★変更点3: フレームごとに角度を更新
            if (Mathf.Abs(planetData.revolution_speed) > 0.001f)
            {
                float revolutionDegreesPerSecond = 360f / planetData.revolution_speed;
                currentRevolutionAngle += revolutionDegreesPerSecond * simulationDeltaTime;
            }
            if (Mathf.Abs(planetData.rotation_speed) > 0.001f)
            {
                float rotationDegreesPerSecond = 360f / planetData.rotation_speed;
                currentRotationAngle += rotationDegreesPerSecond * simulationDeltaTime;
            }

            // ★変更点4: 更新された角度を使って位置と回転を適用
            UpdatePlanetState();

            // 軌道離脱時の速度を計算 (この部分は変更なし)
            if (Time.deltaTime > 0)
            {
                rogueVelocity = (transform.position - previousPosition) / Time.deltaTime;
                previousPosition = transform.position;
            }
        }
        else
        {
            transform.position += (rogueVelocity * escapeSpeedMultiplier) * Time.deltaTime;
        }
    }

    // ★変更点5: time引数をなくし、保持している角度を使うように変更
    void UpdatePlanetState()
    {
        if (planetData == null) return;
        
        Transform currentOrbitCenter = (parentPlanetData != null) ? parentPlanetData.transform : orbitCenter;
        if (currentOrbitCenter == null) return;

        // 公転の計算 (currentRevolutionAngleを使用)
        float radian = currentRevolutionAngle * Mathf.Deg2Rad;
        Vector3 orbitPos = new Vector3(
            Mathf.Cos(radian) * planetData.orebit_radius * 10,
            0,
            Mathf.Sin(radian) * planetData.orebit_radius * 10 
        );
        transform.position = currentOrbitCenter.position + orbitPos;

        // 自転の計算 (currentRotationAngleを使用)
        Quaternion axialTilt = Quaternion.Euler(0, 0, planetData.angle);
        Quaternion rotation = Quaternion.AngleAxis(currentRotationAngle / 10, Vector3.up);
        transform.rotation = axialTilt * rotation;
    }
    
    // ... (GoRogue, SetRevolutionPeriod, SetRotationPeriodメソッドは変更なし) ...
    public void GoRogue()
    {
        if (!isOrbiting) return;
        Debug.Log(gameObject.name + " が軌道を離脱しました。");
        isOrbiting = false;
    }
    public void SetRevolutionPeriod(float period)
    {
        if (planetData != null && period > 0)
        {
            planetData.revolution_speed = period;
        }
    }
    public void SetRotationPeriod(float period)
    {
        if (planetData != null && period > 0)
        {
            planetData.rotation_speed = period;
        }
    }
}