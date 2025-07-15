using UnityEngine;

public class SunController : MonoBehaviour
{
    // Unityエディタから爆発エフェクトのプレハブをセット
    public GameObject explosionPrefab;

    // PlanetDataスクリプトを参照するための変数
    private PlanetData planetData;

    // 爆発を一度だけ実行するためのフラグ
    private bool hasExploded = false;

    void Start()
    {
        // このオブジェクトにアタッチされているPlanetDataスクリプトを取得
        planetData = GetComponent<PlanetData>();

        // もしPlanetDataが見つからなかったらエラーメッセージを表示
        if (planetData == null)
        {
            Debug.LogError("PlanetDataスクリプトが太陽オブジェクトに見つかりません！");
        }
    }

    void Update()
    {
        // PlanetDataが取得できていて、まだ爆発していない場合のみチェック
        if (planetData != null && !hasExploded)
        {
            // PlanetDataの重力が0以下になったかチェック
            if (planetData.gravity <= 0)
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        // 爆発フラグを立て、二度と爆発しないようにする
        hasExploded = true;

        // 1. 爆発エフェクトを生成
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        // 2. 太陽オブジェクト自体を消滅させる
        Destroy(gameObject);
    }
}