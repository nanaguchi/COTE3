using UnityEngine;

public class MoonOrbitController : MonoBehaviour
{
    [Header("設定")]
    public PlanetData parentPlanetData; // 親である地球のPlanetData
    public float initialOrbitRadius = 38.4f; // 地球と月の初期の軌道半径 (単位は適宜調整)
    // public float escapeThreshold = 0.3f; // ★★★ この行は不要になったので削除（またはコメントアウト）★★★
    public float escapeSpeed = 200f;     // 離脱時のスピード（値を大きくしました）

    private bool hasEscaped = false;     // すでに離脱したかどうかのフラグ
    private Vector3 escapeDirection;     // 離脱する方向

    void Start()
    {
        // ゲーム開始時に、親惑星の初期質量を記録しておく
        if (parentPlanetData != null)
        {
            parentPlanetData.initialMass = parentPlanetData.mass;
        }
    }

    void Update()
    {
        // ★★★ このデバッグ用コードを追加 ★★★
        Debug.Log("現在の地球の重力: " + parentPlanetData.gravity);

        if (parentPlanetData == null) return;
        // すでに重力圏を離脱している場合の処理
        if (hasEscaped)
        {
            transform.Translate(escapeDirection * escapeSpeed * Time.deltaTime, Space.World);
            return;
        }

        // --- 通常の軌道計算 ---

        // ★★★ 変更点：地球の重力が10未満になったら離脱するように変更 ★★★
        if (parentPlanetData.gravity < 5.0f)
{
    hasEscaped = true;
    // 離脱する方向を計算（現在の進行方向）
    Vector3 tangent = Vector3.Cross(transform.position - parentPlanetData.transform.position, Vector3.up);
    escapeDirection = tangent.normalized;
    Debug.Log("月が地球の重力圏を離脱しました！");

    // ★★★ 確認用コード：条件が成立したら、月の色を赤に変える ★★★
    GetComponent<Renderer>().material.color = Color.red;

    return;
}

        // 1. 親惑星の質量が初期値からどれだけ減ったか計算
        float massRatio = (float)(parentPlanetData.mass / parentPlanetData.initialMass);
        
        // 2. 質量に応じて軌道半径を計算（質量が減るほど半径が広がる）
        float currentOrbitRadius = initialOrbitRadius / Mathf.Max(massRatio, 0.01f);

        // 3. 新しい軌道半径で公転を続ける
        float revolutionSpeed = 365f / 27.3f; // 地球の公転に対する月の公転速度
        transform.RotateAround(parentPlanetData.transform.position, Vector3.up, revolutionSpeed * Time.deltaTime);
        
        // 親惑星からの距離を調整
        Vector3 direction = (transform.position - parentPlanetData.transform.position).normalized;
        transform.position = parentPlanetData.transform.position + direction * currentOrbitRadius;
    }
}