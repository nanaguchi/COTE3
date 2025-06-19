using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;
    public float zoomSpeed = 10f;
    public float minZoom = 2f;
    public float maxZoom = 1000f;

    float rotationX = 0f;
    float rotationY = 0f;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        Cursor.lockState = CursorLockMode.None;  // 初期は自由にしておく
        Cursor.visible = true;
    }

    void Update()
    {
        // --- 右クリックしてる間だけマウス視点移動 ---
        if (Input.GetMouseButton(1))  // 右クリック中
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // --- キーボード移動 ---
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (Input.GetKey(KeyCode.E)) move += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) move += Vector3.down;

        transform.position += move * moveSpeed * Time.deltaTime;

        // --- ズーム ---
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