using UnityEngine;
using System.Collections;

public class SimulationManager : MonoBehaviour
{
    [Header("シミュレーション対象")]
    public PlanetData[] allPlanets;
    public Light sunLight;

    [Header("太陽の設定")]
    public Material sunMaterial;
    public Gradient sunColorGradient;

    [Header("重力シミュレーション設定")]
    public float collapseGravityThreshold = 500f;
    public float disintegrationGravityThreshold = 0.1f;
    public float disappearDuration = 1.5f;

    // ★追加：太陽が健在かどうかを追跡するフラグ
    private bool isSunAlive = true;

    void Update()
    {
        // 毎フレーム、物理法則と見た目を適用する
        ApplyPhysicsAndVisuals();

        // ★変更点：ここにあった太陽の消滅を検知するif文は不要なので削除します
    }

    // ★追加：惑星を軌道から解放するメソッド
    void ReleasePlanetsFromOrbit()
    {
        Debug.Log("太陽が消滅しました！全惑星が軌道を離脱します。");

        // 太陽以外の全ての惑星をループ
        for (int i = 1; i < allPlanets.Length; i++)
        {
            if (allPlanets[i] != null)
            {
                // 惑星の軌道制御スクリプトを取得 (あなたのスクリプト名に合わせてください)
                ObjectMove motionScript = allPlanets[i].GetComponent<ObjectMove>();
                if (motionScript != null)
                {
                    // 軌道を停止させる命令を呼び出す
                    motionScript.GoRogue();
                }
            }
        }
    }

    // --- 以下、既存のメソッド（変更なし） ---

    void ApplyPhysicsAndVisuals()
    {
        if (isSunAlive) // 太陽が健在な時だけ実行
        {
            UpdateSunVisuals();
        }
        UpdateGravityEffects();
        // UpdateAllOrbits(); // ←この行はObjectMoveが各自行うので不要になります
    }
    
    void UpdateGravityEffects()
    {
        // 全ての惑星をチェック
        foreach (PlanetData planet in allPlanets)
        {
            if (planet == null || !planet.gameObject.activeSelf) continue;

            // ★追加：もし「無敵」にチェックが入っていたら、この天体の消滅判定をスキップする
            if (planet.isIndestructible)
            {
                continue; // 次の天体のチェックに移る
            }

            // --- 状態変化の判定 ---
            
            // 1. 重力が強すぎて崩壊する場合
            if (planet.gravity > collapseGravityThreshold)
            {
                TriggerExplosion(planet, "重力崩壊");
                continue;
            }

            // 2. 重力が弱すぎて消滅する場合
            if (planet.gravity < disintegrationGravityThreshold)
            {
                TriggerExplosion(planet, "重力による消滅");
                continue;
            }
        }
    }

    void TriggerExplosion(PlanetData planet, string reason)
    {
        Debug.Log(planet.planetName + " が " + reason + " しました。");
        StartCoroutine(FadeAndDestroy(planet));
    }

    IEnumerator FadeAndDestroy(PlanetData planet)
    {
        Renderer planetRenderer = planet.GetComponent<Renderer>();
        Vector3 originalScale = planet.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < disappearDuration)
        {
            float progress = elapsedTime / disappearDuration;
            float alpha = Mathf.Lerp(1f, 0f, progress);
            float scale = Mathf.Lerp(1f, 0f, progress);
            if (planetRenderer != null && planetRenderer.material.HasProperty("_Color"))
            {
                Color newColor = planetRenderer.material.color;
                newColor.a = alpha;
                planetRenderer.material.color = newColor;
            }
            planet.transform.localScale = originalScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (planet.explosionEffectPrefab != null)
        {
            Instantiate(planet.explosionEffectPrefab, planet.transform.position, Quaternion.identity);
        }

        // ★★★ ここからが修正部分 ★★★
        if (planet == allPlanets[0]) // もし消滅するのが太陽なら
        {
            // 見た目と光を消す
            if(planetRenderer != null) planetRenderer.enabled = false;
            if(sunLight != null) sunLight.enabled = false;
            
            // ★重要：ここで、他の惑星に軌道を離脱するよう命令する
            ReleasePlanetsFromOrbit();
        }
        else
        {
            // 太陽以外の天体なら、オブジェクトごと非表示にする
            planet.gameObject.SetActive(false);
        }
        
        // ★★★ ここまでが修正部分 ★★★

        planet.transform.localScale = originalScale;
        if(planetRenderer != null && planetRenderer.material.HasProperty("_Color"))
        {
            Color originalColor = planetRenderer.material.color;
            originalColor.a = 1f;
            planetRenderer.material.color = originalColor;
        }
    }
    
    void UpdateSunVisuals()
    {
        if (allPlanets.Length == 0 || allPlanets[0] == null || sunMaterial == null || sunLight == null) return;
        PlanetData sunData = allPlanets[0];
        float maxTemp = 5500f;
        float currentTemp = Mathf.Max(sunData.temperature, 1.0f);
        float intensityRatio = currentTemp / maxTemp;
        Color newSunColor = sunColorGradient.Evaluate(intensityRatio);
        sunMaterial.SetColor("_EmissionColor", newSunColor * 2.0f);
        sunMaterial.color = newSunColor;
        sunLight.color = Color.Lerp(Color.white, newSunColor, 0.25f);
        sunLight.intensity = 2f * intensityRatio;
    }

    // このメソッドは各惑星が個別に行うようになったので、空にするか削除します
    void UpdateAllOrbits() {}
}