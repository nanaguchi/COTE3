// SimulationManager.cs の基本的な形
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public PlanetData[] allPlanets; // インスペクターで全天体を設定
    public Light sunLight;          // 太陽の光源を設定

    void Update()
    {
        // 毎フレーム、全天体の状態をチェックし、物理法則を適用する
        ApplyPhysicsAndVisuals();
    }

    void ApplyPhysicsAndVisuals()
    {
        // これからここに、様々な物理シミュレーションの処理を書いていく
        // 例：太陽の温度変化を反映させる処理
        UpdateSunVisuals();
        
        // 例：全天体の軌道を計算する処理
        UpdateAllOrbits();
    }
    
    // 以下に各処理の具体的なメソッドを追加していく
    // インスペクターで設定する項目を追加
[Header("太陽の設定")]
public PlanetData sunData;
public Material sunMaterial; // 太陽オブジェクトのマテリアル
public Gradient sunColorGradient; // 温度と色を対応させるグラデーション

// SimulationManager.cs の中のメソッドを書き換え

void UpdateSunVisuals()
{
    // nullチェック: ここでSun Materialが設定されているか確認
    if (sunData == null || sunMaterial == null || sunLight == null) return;

    float maxTemp = 5500f;
    // 温度が0にならないように下限を設定
    float currentTemp = Mathf.Max(sunData.temperature, 1.0f); 
    float intensityRatio = currentTemp / maxTemp;

    // Gradientから新しい色を取得
    Color newSunColor = sunColorGradient.Evaluate(intensityRatio);

    // ★★★ 修正ポイント１ ★★★
    // 太陽オブジェクト自体を強く発光させる
    // これで太陽が緑色に光ります
    sunMaterial.SetColor("_EmissionColor", newSunColor * 2.0f); // 2.0fを掛けて明るく光らせる
    sunMaterial.color = newSunColor; // オブジェクト自体の基本色も変更

    // ★★★ 修正ポイント２ ★★★
    // シーンを照らす光の色は、真っ白か、少しだけ太陽の色を混ぜる程度にする
    // これで他の惑星が緑色に染まるのを防ぎます
    sunLight.color = Color.Lerp(Color.white, newSunColor, 0.25f); // 75%の白と25%の太陽色を混ぜる

    // 太陽の光の強さを温度に連動させる
    sunLight.intensity = 2f * intensityRatio;

}
    void UpdateAllOrbits() { /* ... */ }
}