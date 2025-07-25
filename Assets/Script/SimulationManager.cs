using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;               
using UnityEngine.SceneManagement;

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

    private List<PlanetVisualController> visualControllers = new List<PlanetVisualController>();

    private bool isSunAlive = true;

    void Start()
    {
        visualControllers = FindObjectsOfType<PlanetVisualController>().ToList();
    }

    public void ResetGameToInitialState()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        ApplyPhysicsAndVisuals();
    }
    
    void ApplyPhysicsAndVisuals()
    {
        if (isSunAlive) 
        {
            UpdateSunVisuals();
        }
        UpdateGravityEffects();
        UpdateAllPlanetVisuals();
  
    }

    void UpdateAllPlanetVisuals()
    {
        foreach (var controller in visualControllers)
        {
            if (controller != null)
            {
                controller.UpdateVisuals();
            }
        }
    }

    void ReleasePlanetsFromOrbit()
    {
        Debug.Log("太陽が消滅しました！全惑星が軌道を離脱します。");
        for (int i = 1; i < allPlanets.Length; i++)
        {
            if (allPlanets[i] != null)
            {
                ObjectMove motionScript = allPlanets[i].GetComponent<ObjectMove>();
                if (motionScript != null)
                {
                    motionScript.GoRogue();
                }
            }
        }
    }

    void UpdateGravityEffects()
    {
        foreach (PlanetData planet in allPlanets)
        {
            if (planet == null || !planet.gameObject.activeSelf) continue;
            if (planet.isIndestructible) continue;

            if (planet.gravity > collapseGravityThreshold)
            {
                TriggerExplosion(planet, "重力崩壊");
                continue;
            }
            if (planet.gravity < disintegrationGravityThreshold)
            {
                TriggerExplosion(planet, "重力による崩壊");
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

        planet.isIndestructible = true;

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

        if (planet == allPlanets[0]) 
        {
            isSunAlive = false;
            if(planetRenderer != null) planetRenderer.enabled = false;
            if(sunLight != null) sunLight.enabled = false;
            ReleasePlanetsFromOrbit();
        }
        else
        {
            planet.gameObject.SetActive(false);
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
}