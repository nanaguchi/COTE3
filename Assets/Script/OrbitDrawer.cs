using UnityEngine;

// このスクリプトがアタッチされたオブジェクトにLineRendererコンポーネントを必須にする
[RequireComponent(typeof(LineRenderer))]
public class OrbitDrawer : MonoBehaviour
{
    [Header("軌道の設定")]
    [Range(3, 360)]
    public int segments = 100; // 軌道を構成する頂点の数（多いほど滑らか）
    public float radius = 579.1; // 軌道の半径

    void Start()
    {
        // このオブジェクトについているLineRendererコンポーネントを取得して描画する
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        DrawOrbit(lineRenderer);
    }

    // 軌道を描画するメインの処理
    void DrawOrbit(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = segments; // 頂点の数を設定
        lineRenderer.loop = true; // 始点と終点を自動で結び、円を閉じる

        // 円周上の各点の座標を計算して設定
        for (int i = 0; i < segments; i++)
        {
            float angle = ((float)i / (float)segments) * 2f * Mathf.PI; // 角度をラジアンで計算
            float x = Mathf.Sin(angle) * radius; // X座標
            float z = Mathf.Cos(angle) * radius; // Z座標

            lineRenderer.SetPosition(i, new Vector3(x, 0f, z)); // 計算した座標を線に設定
        }
    }
}