using UnityEngine;

public class SunExpansionController : MonoBehaviour
{
    [Header("参照設定")]
    [Tooltip("監視対象となる太陽のPlanetDataを設定します。")]
    public PlanetData planetData;

    [Header("膨張設定")]
    [Tooltip("この重力値に達した時に膨張が最大になります。")]
    public float targetGravity = 3.7f;

    [Tooltip("最終的な膨張サイズ（元の大きさに対する倍率）。")]
    public Vector3 maxScale = new Vector3(5f, 5f, 5f);

    // --- 内部で使用する変数 ---
    private Vector3 initialScale;       // 膨張開始前の元のスケール
    private float initialGravity;       // 膨張開始前の元の重力
    private bool hasCapturedInitialState = false; // 初期状態を記録したかどうかのフラグ

    void Start()
    {
        // もしインスペクターからplanetDataが設定されていなければ、自分自身から取得を試みる
        if (planetData == null)
        {
            planetData = GetComponent<PlanetData>();
        }

        if (planetData == null)
        {
            Debug.LogError("SunExpansionController: PlanetDataが見つかりません！このスクリプトを無効にします。");
            this.enabled = false; // スクリプトが機能しないようにする
            return;
        }

        // 起動時のスケールを初期値として保存
        initialScale = transform.localScale;
    }

    void Update()
    {
        // PlanetDataから現在の重力を取得
        float currentGravity = planetData.gravity;

        // --- 1. 膨張開始前の初期状態を一度だけ記録する ---
        // まだ記録しておらず、現在の重力が膨張の目標値より大きい場合
        // (つまり、まだ膨張が始まっていない最初の安定状態)
        if (!hasCapturedInitialState && currentGravity > targetGravity)
        {
            initialGravity = currentGravity;
            hasCapturedInitialState = true;
            Debug.Log($"太陽の初期状態を記録しました。初期重力: {initialGravity}");
        }

        // --- 2. スケールを更新する ---
        // 初期状態が記録済みの場合のみ処理を行う
        if (hasCapturedInitialState)
        {
            // 膨張期間内（初期重力 > 現在重力 > 最終重力）の場合
            if (currentGravity < initialGravity && currentGravity > targetGravity)
            {
                // 膨張の進行度を計算 (0.0 ～ 1.0 の範囲)
                // 例: initial=10, target=3.7 の時、currentが8なら進行度は (10-8)/(10-3.7) = 2/6.3 = 0.31
                float progress = (initialGravity - currentGravity) / (initialGravity - targetGravity);

                // Lerp (線形補間) を使って、進行度に応じたスケールを滑らかに計算
                transform.localScale = Vector3.Lerp(initialScale, maxScale, progress);
            }
            // ターゲットの重力以下になったら、最大サイズで固定
            else if (currentGravity <= targetGravity)
            {
                 transform.localScale = maxScale;
            }
            // それ以外（重力が初期値より増えるなど）の場合は初期サイズに戻す
            else
            {
                transform.localScale = initialScale;
            }
        }
    }
}