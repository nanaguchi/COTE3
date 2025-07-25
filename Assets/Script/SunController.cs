using UnityEngine;

public class SunController : MonoBehaviour
{
    public GameObject explosionPrefab;
    private PlanetData planetData;
    private bool hasExploded = false;

    void Start()
    {
        planetData = GetComponent<PlanetData>();

        if (planetData == null)
        {
            Debug.LogError("PlanetDataスクリプトが太陽オブジェクトに見つかりません！");
        }
    }

    void Update()
    {
        if (planetData != null && !hasExploded)
        {
            if (planetData.gravity <= 0)
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        hasExploded = true;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}