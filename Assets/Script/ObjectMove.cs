using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [Header("基本設定")]
    public PlanetData planetData;
    public Transform orbitCenter;

    [Header("離脱設定")]
    public float escapeSpeedMultiplier = 2.0f;

    // --- 状態管理のための変数 ---
    private bool isOrbiting = true;
    private Vector3 previousPosition; // 速度計算用

    // --- 離脱後に使用する変数 ---
    private float escapeTime;
    private Vector3 escapePosition;
    private Vector3 escapeVelocity;

    void OnEnable()
    {
        // 太陽の重力崩壊イベントにGoRogueメソッドを登録
        SunEventManager.OnSunGravityCollapse += GoRogue;
    }

    void OnDisable()
    {
        // オブジェクトが破棄される際などに、登録したメソッドを解除
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
            // --- 1. 軌道運動中 ---
            UpdatePlanetState(currentTime);

            // 離脱の瞬間の速度を計算するために、常に位置を記録し続ける
            if (Time.deltaTime > 0)
            {
                // ここで計算される速度が、離脱時の初速となる
                escapeVelocity = (transform.position - previousPosition) / Time.deltaTime;
                previousPosition = transform.position;
            }
        }
        else
        {
            // --- 2. 軌道離脱後 ---
            // 離脱してからの経過時間を計算
            float timeSinceEscape = currentTime - escapeTime;
            
            // 「離脱時の位置」＋「速度 × 経過時間」で現在の位置を算出
            transform.position = escapePosition + (escapeVelocity * escapeSpeedMultiplier * timeSinceEscape);
        }
    }

    /// <summary>
    /// 軌道を離脱する処理。イベントから一度だけ呼ばれる。
    /// </summary>
    public void GoRogue()
    {
        if (!isOrbiting) return; // 既に離脱済みの場合は何もしない

        Debug.Log(gameObject.name + " が軌道を離脱しました。");
        isOrbiting = false;

        // ★★★ 離脱した瞬間の状態を記録 ★★★
        escapeTime = TimeController.Instance.simulationTime;
        escapePosition = transform.position;
        // escapeVelocity はUpdateで常に計算されている最新の値を使用する
    }

    // ... UpdatePlanetStateと、その中で使う回転処理などは変更不要です ...
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