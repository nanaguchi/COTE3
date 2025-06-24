using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 10f;           // 通常ズーム速度
    public float fastZoomMultiplier = 3f;   // Shift時ズーム加速倍率
    public float panSpeed = 10f;            // パン移動速度
    public float panFastMultiplier = 3f;    // Shift時パン加速倍率
    public float minZoomDistance = 5f;      // 最小ズーム距離
    public float maxZoomDistance = 50f;     // 最大ズーム距離

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // ------------------------------
        // マウスホイールによるズーム
        // ------------------------------
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            float actualZoomSpeed = zoomSpeed;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                actualZoomSpeed *= fastZoomMultiplier;
            }

            Vector3 direction = transform.forward;
            Vector3 newPosition = transform.position + direction * scroll * actualZoomSpeed;

            float distance = Vector3.Distance(newPosition, Vector3.zero); // 中心が原点の場合
            if (distance >= minZoomDistance && distance <= maxZoomDistance)
            {
                transform.position = newPosition;
            }
        }

        // ------------------------------
        // Shift + 矢印キーでパン移動
        // ------------------------------
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            float actualPanSpeed = panSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftArrow))
                transform.position -= transform.right * actualPanSpeed;

            if (Input.GetKey(KeyCode.RightArrow))
                transform.position += transform.right * actualPanSpeed;

            if (Input.GetKey(KeyCode.UpArrow))
                transform.position += transform.up * actualPanSpeed;

            if (Input.GetKey(KeyCode.DownArrow))
                transform.position -= transform.up * actualPanSpeed;
        }
    }
}