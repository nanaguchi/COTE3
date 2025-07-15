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
        if (!hasExploded && planetData.gravity <= 3.7f)
        {
            Debug.Log("重力0になったので爆発を実行");
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
            // 爆発力を与える
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            // ランダムに回転させる（演出強化）
            rb.AddTorque(Random.onUnitSphere * 50f);

            // デバッグログ（確認用）
            Debug.Log($"破片 {i} に爆発力とトルクを加えました");
        }
        else
        {
            // Rigidbodyがない場合の警告
            Debug.LogWarning($"破片 {i} に Rigidbody が見つかりません！");
        }
    }
        

        // 元の太陽を削除（または非表示にしたい場合は SetActive(false) に変更）
        Destroy(gameObject);
    }
}
