using UnityEngine;

// MonoBehaviourを継承しているので、[CreateAssetMenu]属性は不要です。削除しました。
public class PlanetData : MonoBehaviour
{
    [Header("基本情報")]
    public string planetName;       // 惑星名
    [TextArea(3, 5)]
    public string brief_info;       // ツールチップ用情報
    [TextArea(5, 10)]
    public string detailed_info;    // 詳細パネル用情報
    public string detailSceneName;  // 詳細シーン名

    [Header("物理パラメータ")]
    public double mass;             // 質量 (kg) - 値が大きいためdouble型
    public float radius;            // 半径 (km)
    public float rotation_speed;    // 自転周期 (時間単位)
    public float revolution_speed;  // 公転周期 (日単位)
    public float orebit_radius;     // 軌道半径 (原文ママ)
    public float gravity;           // 表面重力 (m/s^2)
    public float temperature;       // 平均表面温度 (°C)

    [Header("シミュレーション用パラメータ")]
    public float initial_position_x;
    public float initial_position_y;
    public float initial_position_z;
    public float scale;
    public float angle;
    public float initialgravity; // ★追加：初期の重力値
    public GameObject explosionEffectPrefab; // ★追加：爆発エフェクトのプレハブ

    public bool isIndestructible = false; // ★この行を追加
    public double initialMass;
}