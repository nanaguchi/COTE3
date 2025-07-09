using UnityEngine;

public class PlanetVisualController : MonoBehaviour
{
    [Header("対象データ")]
    public PlanetData planetData;
    
    [Header("温度のしきい値")]
    public float freezingPoint = 0f;    // 凍結が始まる温度
    public float deepFreezePoint = -50f; // 完全に凍り付く温度
    public float boilingPoint = 100f;   // 乾燥が始まる温度
    public float scorchedPoint = 500f;  // 完全に焼け付く温度
    public float lavaPoint = 800f;      // 溶岩化する温度

    [Header("状態別の色（ティントカラー）")]
    public Color iceTintColor = new Color(0.8f, 0.95f, 1.0f); // 氷の色（薄い水色）
    public Color dryTintColor = new Color(1.0f, 0.7f, 0.5f); // 乾燥した色（赤茶色）
    public Color lavaEmissionColor = new Color(1.0f, 0.4f, 0.0f); // 溶岩の発光色（オレンジ）

    private Material planetMaterial;

    void Start()
    {
        Renderer planetRenderer = GetComponent<Renderer>();
        if (planetRenderer != null)
        {
            // マテリアルのインスタンスを作成して、他のオブジェクトに影響が出ないようにする
            planetMaterial = planetRenderer.material; 
        }
    }

    public void UpdateVisuals()
    {
        if (planetData == null || planetMaterial == null) return;

        Color finalTintColor = Color.white; // 通常はティントなし(元のテクスチャの色)
        Color finalEmissionColor = Color.black;
        bool enableEmission = false;

        float temp = planetData.temperature;

        // 温度に応じて、最終的な色や発光を決める
        if (temp <= freezingPoint)
        {
            // 氷点から完全凍結点までの割合を計算 (0から1)
            float t = Mathf.InverseLerp(freezingPoint, deepFreezePoint, temp);
            finalTintColor = Color.Lerp(Color.white, iceTintColor, t);
        }
        else if (temp >= boilingPoint && temp < lavaPoint)
        {
            // 沸点から完全乾燥点までの割合を計算 (0から1)
            float t = Mathf.InverseLerp(boilingPoint, scorchedPoint, temp);
            finalTintColor = Color.Lerp(Color.white, dryTintColor, t);
        }
        else if (temp >= lavaPoint)
        {
            enableEmission = true;
            // 溶岩化すると地表は暗くなり、発光する
            float lavaRatio = Mathf.InverseLerp(scorchedPoint, lavaPoint, temp);
            finalTintColor = Color.Lerp(dryTintColor, new Color(0.1f, 0.1f, 0.1f), lavaRatio);

            float emissionIntensity = Mathf.InverseLerp(lavaPoint, lavaPoint + 700f, temp);
            finalEmissionColor = lavaEmissionColor * emissionIntensity;
        }

        // マテリアルの色と発光を更新
        planetMaterial.SetColor("_BaseColor", finalTintColor);

        if (enableEmission)
        {
            planetMaterial.EnableKeyword("_EMISSION");
            planetMaterial.SetColor("_EmissionColor", finalEmissionColor);
        }
        else
        {
            planetMaterial.DisableKeyword("_EMISSION");
        }
    }
}