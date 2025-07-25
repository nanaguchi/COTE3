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
    public Color iceTintColor = new Color(0.8f, 0.95f, 1.0f); 
    public Color dryTintColor = new Color(1.0f, 0.7f, 0.5f); 
    public Color lavaEmissionColor = new Color(1.0f, 0.4f, 0.0f); 

    private Material planetMaterial;

    void Start()
    {
        Renderer planetRenderer = GetComponent<Renderer>();
        if (planetRenderer != null)
        {
            planetMaterial = planetRenderer.material; 
        }
    }

    public void UpdateVisuals()
    {
        if (planetData == null || planetMaterial == null) return;

        Color finalTintColor = Color.white;
        Color finalEmissionColor = Color.black;
        bool enableEmission = false;

        float temp = planetData.temperature;

        if (temp <= freezingPoint)
        {
            float t = Mathf.InverseLerp(freezingPoint, deepFreezePoint, temp);
            finalTintColor = Color.Lerp(Color.white, iceTintColor, t);
        }
        else if (temp >= boilingPoint && temp < lavaPoint)
        {
            float t = Mathf.InverseLerp(boilingPoint, scorchedPoint, temp);
            finalTintColor = Color.Lerp(Color.white, dryTintColor, t);
        }
        else if (temp >= lavaPoint)
        {
            enableEmission = true;
            float lavaRatio = Mathf.InverseLerp(scorchedPoint, lavaPoint, temp);
            finalTintColor = Color.Lerp(dryTintColor, new Color(0.1f, 0.1f, 0.1f), lavaRatio);

            float emissionIntensity = Mathf.InverseLerp(lavaPoint, lavaPoint + 700f, temp);
            finalEmissionColor = lavaEmissionColor * emissionIntensity;
        }

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