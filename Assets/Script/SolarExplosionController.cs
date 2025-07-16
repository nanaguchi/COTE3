using UnityEngine;

public class SolarExplosionController : MonoBehaviour
{
    [Header("破片設定")]
    public GameObject fragmentPrefab;
    public int fragmentCount = 30;
    public float explosionForce = 500f;
    public float explosionRadius = 5f;

    [Header("参照設定")]
    public PlanetData planetData; // 太陽のPlanetData

    private bool hasTriggered = false; // イベントを一度だけ発行するためのフラグ

    void Update()
    {
        // まだイベントを発行しておらず、重力が3.7以下になったら
        if (!hasTriggered && planetData != null && planetData.gravity <= 3.7f)
        {
            hasTriggered = true; // フラグを立てて二度と実行しないようにする

            // ★★★ 変更点: シンプルなイベント発行を呼び出す ★★★
            SunEventManager.TriggerSunGravityCollapse();
            
            Explode();
        }
    }

    void Explode()
    {
        // ... (この中身は変更なし) ...
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