using UnityEngine;

public class CelestialBodyController : MonoBehaviour
{
    // --- Inspectorで設定する項目 ---

    [Header("公転の設定")]
    public Transform orbitCenter; // 公転の中心となる天体（例：太陽）
    public float orbitSpeed = 10f; // 公転の速さ（度/秒）

    [Header("自転の設定")]
    public float rotationSpeed = 10f; // 自転の速さ（度/秒）

    // Update is called once per frame
    void Update()
    {
        // --- 1. 自転処理 ---
        // 自分自身のY軸を中心に、指定した速度で回転させる
        // Time.deltaTimeを掛けることで、フレームレートに依存しない滑らかな動きになる
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);


        // --- 2. 公転処理 ---
        // もし公転の中心(orbitCenter)が設定されていれば、公転を行う
        if (orbitCenter != null)
        {
            // orbitCenterの周りを、Y軸を軸として、指定した速度で公転させる
            transform.RotateAround(orbitCenter.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
    }
}