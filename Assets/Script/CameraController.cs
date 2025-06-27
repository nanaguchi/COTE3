using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("フォーカス設定")]
    public Transform focusTarget;
    public float distance = 20f;
    public float focusChangeSpeed = 5f;

    [Header("カメラ操作設定")]
    public float rotationSpeed = 120f;
    public float zoomSpeed = 20f;
    public Vector2 distanceMinMax = new Vector2(5f, 100f);

    private FreeCameraController freeCamController;
    private float yaw = 0f;
    private float pitch = 0f;

    void Awake()
    {
        // 最初にフリームーブ用カメラを取得しておく
        freeCamController = GetComponent<FreeCameraController>();
    }

    void Update()
    {
        // 左クリックでフォーカス対象を決定
        if (Input.GetMouseButtonDown(0))
        {
            // UIをクリックした場合は無視
            if (UnityEngine.EventSystems.EventSystem.current != null && 
                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 天体にヒットした場合
                SetFocus(hit.transform);
            }
            else
            {
                // 何もない場所をクリックした場合
                RemoveFocus();
            }
        }
    }

    void LateUpdate()
    {
        // フォーカス対象がいなければ、このスクリプトは何もしない
        if (focusTarget == null) return;
        
        // --- ここからフォーカス中のカメラ制御 ---
        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -85f, 85f);
        }

        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, distanceMinMax.x, distanceMinMax.y);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = focusTarget.position + rotation * new Vector3(0, 0, -distance);
        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, focusChangeSpeed * Time.deltaTime);
        transform.LookAt(focusTarget.position);
    }
    
    void SetFocus(Transform newTarget)
    {
        // すでに同じものをフォーカスしていたら何もしない
        if (focusTarget == newTarget) return;

        Debug.Log("フォーカスを設定: " + newTarget.name); // 念のためログは残します
        focusTarget = newTarget;
        
        // ★重要：フォーカスした瞬間に、フリームーブカメラを強制的に無効化！
        if (freeCamController != null)
        {
            freeCamController.enabled = false;
        }

        // 新しいフォーカス対象にカメラを向けるための初期角度を計算
        Vector3 directionToTarget = (focusTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        
        yaw = lookRotation.eulerAngles.y;
        pitch = lookRotation.eulerAngles.x;
    }
    
    void RemoveFocus()
    {
        if (focusTarget == null) return;

        Debug.Log("フォーカスを解除");
        focusTarget = null;

        // ★重要：フォーカスを解除した瞬間に、フリームーブカメラを有効化！
        if (freeCamController != null)
        {
            freeCamController.enabled = true;
        }
    }
}