using UnityEngine;

public class CameraFocusController : MonoBehaviour
{
    [Header("UI設定")]
    [Tooltip("クリックで表示する情報パネル")]
    public GameObject planetInfoPanel;

    [Header("フリームーブモード設定")]
    [Tooltip("カメラの基本移動速度")]
    public float freeMoveSpeed = 1000f;
    [Tooltip("マウスでの視点移動の感度")]
    public float freeLookSpeed = 2f;

    [Header("フォーカスモード設定")]
    [Tooltip("天体にどれだけ近づくかの倍率（天体の半径 × この値）")]
    public float distanceMultiplier = 3f;
    [Tooltip("天体に近づくときのカメラの移動速度")]
    public float focusSpeed = 5f;

    private Transform focusTarget;
    private float rotationX;
    private float rotationY;

    void Start()
    {
        if (planetInfoPanel != null)
        {
            planetInfoPanel.SetActive(false);
        }

        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;
    }

    void Update()
    {
        HandleClickInput();
    }

    void LateUpdate()
    {
        if (focusTarget != null)
        {
            HandleFocusMode();
        }
        else
        {
            HandleFreeMoveMode();
        }
    }

    private void HandleClickInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current != null &&
                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == focusTarget)
                {
                    RemoveFocus();
                }
                else
                {
                    SetFocus(hit.transform);
                }
            }
            else
            {
                RemoveFocus();
            }
        }
    }

    private void HandleFocusMode()
    {
        float targetRadius = 0.5f;
        Renderer rend = focusTarget.GetComponent<Renderer>();
        if (rend != null)
        {
            targetRadius = Mathf.Max(rend.bounds.extents.x, rend.bounds.extents.y, rend.bounds.extents.z);
        }
        Vector3 targetPosition = focusTarget.position - (transform.forward * targetRadius * distanceMultiplier);

        Quaternion targetRotation = Quaternion.LookRotation(focusTarget.position - transform.position);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * focusSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * focusSpeed);
    }

    private void HandleFreeMoveMode()
    {
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * freeLookSpeed;
            rotationY -= Input.GetAxis("Mouse Y") * freeLookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (Input.GetKey(KeyCode.E)) move.y += 1;
        if (Input.GetKey(KeyCode.Q)) move.y -= 1;

        transform.position += move * freeMoveSpeed * Time.deltaTime;
    }

    private void SetFocus(Transform newTarget)
    {
        focusTarget = newTarget;
        if (planetInfoPanel != null)
        {
            planetInfoPanel.SetActive(true);
        }
    }

    private void RemoveFocus()
    {
        focusTarget = null;
        if (planetInfoPanel != null)
        {
            planetInfoPanel.SetActive(false);
        }
    }
}