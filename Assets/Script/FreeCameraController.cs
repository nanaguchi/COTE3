using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float moveSpeed = 10f;     // 移動速度
    public float lookSpeed = 2f;      // 視点回転の感度
    public float zoomSpeed = 10f;     // ズーム速度（マウスホイール）
    public float minZoom = 2f;        // ズーム最小距離
    public float maxZoom = 100f;
    float rotationX = 0f;
    float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // プレイ中にマウス固定
    }

    void Update()
    {
        // マウスによるカメラ回転
        rotationX += Input.GetAxis("Mouse X") * lookSpeed;
        rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // 上下回転の制限

        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);

        // キーボードによる移動（矢印キー対応）
        float moveX = Input.GetAxis("Horizontal"); // ← →
        float moveZ = Input.GetAxis("Vertical");   // ↑ ↓

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // 高さ変更（上下移動）：Qキーで下、Eキーで上
        if (Input.GetKey(KeyCode.E)) move += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) move += Vector3.down;

        transform.position += move * moveSpeed * Time.deltaTime;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Vector3 forward = transform.forward;
            Vector3 newPos = transform.position + forward * scroll * zoomSpeed;

            float distance = Vector3.Distance(newPos, transform.position + forward * 0f);
            if (distance >= minZoom && distance <= maxZoom)
            {
                transform.position = newPos;
            }
        }
    }
}