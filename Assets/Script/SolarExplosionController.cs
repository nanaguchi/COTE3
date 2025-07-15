using UnityEngine;

public class SolarExplosionController : MonoBehaviour
{
    [Header("破片設定")]
    public GameObject fragmentPrefab; // 破片のプレハブ
    public int fragmentCount = 30;
    public float explosionForce = 500f;
    public float explosionRadius = 5f;

    [Header("参照設定")]
    public PlanetData planetData; // 太陽のPlanetData

    private bool hasExploded = false;

    void Update()
    {
        // 重力が0以下で、まだ爆発していなければ爆発する
        if (!hasExploded && planetData.gravity <= 0f)
        {
            Explode();
            hasExploded = true;
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
            }
        }

        // 元の太陽を削除（または非表示にしたい場合は SetActive(false) に変更）
        Destroy(gameObject);
    }
}
