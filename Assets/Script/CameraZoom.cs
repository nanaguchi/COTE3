using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 10f;         // ズームの速さ
    public float minZoomDistance = 5f;    // 最小距離
    public float maxZoomDistance = 50f;   // 最大距離

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // カメラの視点を前後に動かす（ズーム）
        if (scroll != 0f)
        {
            Vector3 direction = transform.forward;
            Vector3 newPosition = transform.position + direction * scroll * zoomSpeed;

            // ズーム距離の制限
            float distance = Vector3.Distance(newPosition, Vector3.zero); // 原点を中心とするなら
            if (distance >= minZoomDistance && distance <= maxZoomDistance)
            {
                transform.position = newPosition;
            }
        }

        

    }
}