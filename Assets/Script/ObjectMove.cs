using UnityEngine;

public class PlanetMotion : MonoBehaviour
{
    public PlanetData planetData;    // インスペクターで設定
    public Transform orbitCenter;   // 太陽などの中心天体

    void Start()
    {
        // ゲーム開始時に、TimeControllerの初期時間に合わせて一度状態を更新する
        if (TimeController.Instance != null)
        {
            UpdatePlanetState(TimeController.Instance.simulationTime);
        }
    }

    void Update()
    {
        // TimeControllerのインスタンスがなければ何もしない
        if (TimeController.Instance == null) return;
        
        // ★要件2: TimeControllerが管理する現在のシミュレーション時間を取得
        float currentTime = TimeController.Instance.simulationTime;

        // 取得した時間に基づいて、惑星の位置と回転を更新
        UpdatePlanetState(currentTime);
    }

    /// <summary>
    /// 指定された絶対時間に基づいて、惑星の状態（公転位置と自転角度）を計算し、反映する
    /// </summary>
    /// <param name="time">シミュレーション開始からの経過時間（単位：時間）</param>
    void UpdatePlanetState(float time)
    {
        if (planetData == null || orbitCenter == null) return;

        // --- 公転位置の計算 ---
        // planetDataの公転周期(revolution_speed)が0でないことを確認
        if (Mathf.Abs(planetData.revolution_speed) > 0.001f)
        {
            // 1時間あたりに進む公転角度を計算
            float revolutionDegreesPerHour = 360f / planetData.revolution_speed;
            
            // 現在のシミュレーション時間における公転の総角度を計算
            float currentRevolutionAngle = revolutionDegreesPerHour * time;

            // 角度をラジアンに変換
            float radian = currentRevolutionAngle * Mathf.Deg2Rad;
            
            // 公転半径から位置を計算
            Vector3 orbitPos = new Vector3(
                Mathf.Cos(radian) * planetData.orebit_radius * 10,
                0,
                Mathf.Sin(radian) * planetData.orebit_radius * 10 
            );
            transform.position = orbitCenter.position + orbitPos;
        }

        if (Mathf.Abs(planetData.rotation_speed) > 0.001f)
    {
        float rotationDegreesPerHour = 360f / planetData.rotation_speed;
        float currentRotationAngle = rotationDegreesPerHour * time;

        // 自転軸の傾きをZ軸に適用
        Quaternion axialTilt = Quaternion.Euler(0, 0, planetData.angle);

        // Y軸に沿って自転
        Quaternion rotation = Quaternion.AngleAxis(currentRotationAngle, Vector3.up);

        // 傾きと回転の合成
        transform.rotation = axialTilt * rotation;
    }
    }
}