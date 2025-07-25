using UnityEngine;

public class SolarExplosionController : MonoBehaviour
{
    [Header("破片設定")]
    public GameObject fragmentPrefab;
    public int fragmentCount = 30;
    public float explosionForce = 500f;
    public float explosionRadius = 5f;

    [Header("参照設定")]
    public PlanetData planetData; 
    private bool hasTriggered = false; 

    void Update()
    {
        // 重力が3.7以下
        if (!hasTriggered && planetData != null && planetData.gravity <= 3.7f)
        {
            hasTriggered = true; 
            SunEventManager.TriggerSunGravityCollapse();
            
            Explode();
        }
    }

    void Explode()
    {
        for (int i = 0; i < fragmentCount; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * 1.5f;
            Quaternion spawnRot = Random.rotation;
            GameObject fragment = Instantiate(fragmentPrefab, spawnPos, spawnRot);

            Rigidbody rb = fragment.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                rb.AddTorque(Random.onUnitSphere * 50f);
            }
        }
        
        Destroy(gameObject);
    }
}