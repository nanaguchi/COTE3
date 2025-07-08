using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour
{
    [Header("�V�~�����[�V�����Ώ�")]
    public PlanetData[] allPlanets;
    public Light sunLight;

    [Header("���z�̐ݒ�")]
    public Material sunMaterial;
    public Gradient sunColorGradient;

    [Header("�d�̓V�~�����[�V�����ݒ�")]
    public float collapseGravityThreshold = 500f;
    public float disintegrationGravityThreshold = 0.1f;
    public float disappearDuration = 1.5f;

    // ���ǉ��F���z�����݂��ǂ�����ǐՂ���t���O
    private bool isSunAlive = true;

     public void ResetGameToInitialState()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        // ���t���[���A�����@���ƌ����ڂ�K�p����
        ApplyPhysicsAndVisuals();

        // ���ύX�_�F�����ɂ��������z�̏��ł����m����if���͕s�v�Ȃ̂ō폜���܂�
    }

    // ���ǉ��F�f�����O�����������郁�\�b�h
    void ReleasePlanetsFromOrbit()
    {
        Debug.Log("���z�����ł��܂����I�S�f�����O���𗣒E���܂��B");

        // ���z�ȊO�̑S�Ă̘f�������[�v
        for (int i = 1; i < allPlanets.Length; i++)
        {
            if (allPlanets[i] != null)
            {
                // �f���̋O������X�N���v�g���擾 (���Ȃ��̃X�N���v�g���ɍ��킹�Ă�������)
                ObjectMove motionScript = allPlanets[i].GetComponent<ObjectMove>();
                if (motionScript != null)
                {
                    // �O�����~�����閽�߂��Ăяo��
                    motionScript.GoRogue();
                }
            }
        }
    }

    // --- �ȉ��A�����̃��\�b�h�i�ύX�Ȃ��j ---

    void ApplyPhysicsAndVisuals()
    {
        if (isSunAlive) // ���z�����݂Ȏ��������s
        {
            UpdateSunVisuals();
        }
        UpdateGravityEffects();
        // UpdateAllOrbits(); // �����̍s��ObjectMove���e���s���̂ŕs�v�ɂȂ�܂�
    }
    
    void UpdateGravityEffects()
    {
        // �S�Ă̘f�����`�F�b�N
        foreach (PlanetData planet in allPlanets)
        {
            if (planet == null || !planet.gameObject.activeSelf) continue;

            // ���ǉ��F�����u���G�v�Ƀ`�F�b�N�������Ă�����A���̓V�̂̏��Ŕ�����X�L�b�v����
            if (planet.isIndestructible)
            {
                continue; // ���̓V�̂̃`�F�b�N�Ɉڂ�
            }

            // --- ��ԕω��̔��� ---
            
            // 1. �d�͂��������ĕ��󂷂�ꍇ
            if (planet.gravity > collapseGravityThreshold)
            {
                TriggerExplosion(planet, "�d�͕���");
                continue;
            }

            // 2. �d�͂��シ���ď��ł���ꍇ
            if (planet.gravity < disintegrationGravityThreshold)
            {
                TriggerExplosion(planet, "�d�͂ɂ�����");
                continue;
            }
        }
    }

    void TriggerExplosion(PlanetData planet, string reason)
    {
        Debug.Log(planet.planetName + " �� " + reason + " ���܂����B");
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

        // ������ �������炪�C������ ������
        if (planet == allPlanets[0]) // �������ł���̂����z�Ȃ�
        {
            // �����ڂƌ�������
            if(planetRenderer != null) planetRenderer.enabled = false;
            if(sunLight != null) sunLight.enabled = false;
            
            // ���d�v�F�����ŁA���̘f���ɋO���𗣒E����悤���߂���
            ReleasePlanetsFromOrbit();
        }
        else
        {
            // ���z�ȊO�̓V�̂Ȃ�A�I�u�W�F�N�g���Ɣ�\���ɂ���
            planet.gameObject.SetActive(false);
        }
        
        // ������ �����܂ł��C������ ������

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

    

    // ���̃��\�b�h�͊e�f�����ʂɍs���悤�ɂȂ����̂ŁA��ɂ��邩�폜���܂�
    void UpdateAllOrbits() {}
}