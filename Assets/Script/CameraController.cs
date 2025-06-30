using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("モード共通設定")]
    public LayerMask focusLayer;

    [Header("フリームーブモード設定")]
    public float freeMoveSpeed = 1000f;
    public float freeLookSpeed = 2f;
    public float freeZoomSpeed = 1000f;

    [Header("フォーカスモード設定")]
    [Tooltip("この半径を超えた天体を「巨大」とみなす しきい値")]
    public float largeObjectThreshold = 500f;
    [Tooltip("通常の天体の半径に対する、フォーカス距離の倍率")]
    public float defaultDistanceMultiplier = 2.5f;
    [Tooltip("巨大な天体の半径に対する、フォーカス距離の倍率")]
    public float largeObjectDistanceMultiplier = 1.8f;
    [Tooltip("「SaturnFocus」タグが付いた天体専用の、フォーカス距離の倍率")]
    public float saturnDistanceMultiplier = 1.5f; // ★追加：土星専用の倍率
    [Tooltip("フォーカスが切り替わる際の、カメラが目標に近づく速度")]
    public float focusChangeSpeed = 5f;
    [Tooltip("フォーカス時の周回回転の速度")]
    public float focusRotationSpeed = 120f;
    [Tooltip("フォーカス時のズーム速度")]
    public float focusZoomSpeed = 20f;
    [Tooltip("フォーカス可能な距離の最小・最大値")]
    public Vector2 focusDistanceMinMax = new Vector2(5f, 5000f);
    [Tooltip("フォーカス解除時に、少し後ろに下がる距離")]
    public float defocusKickback = 20f;
    
    // (Start, Update, LateUpdateなどの他のメソッドに変更はありません)
    
    private Transform focusTarget;
    private Vector3 focusPoint;
    private float focusDistance;
    private float yaw, pitch;
    private float rotationX, rotationY;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            if (UnityEngine.EventSystems.EventSystem.current != null && 
                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()){
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, focusLayer)){
                if (focusTarget != null && hit.transform == focusTarget){
                    RemoveFocus();
                } else {
                    SetFocus(hit.transform);
                }
            } else {
                if (focusTarget != null){
                    RemoveFocus();
                }
            }
        }
    }

    void LateUpdate()
    {
        if (focusTarget != null){
            HandleFocusMode();
        } else {
            HandleFreeMoveMode();
        }
    }
    
    void HandleFocusMode()
    {
        Renderer rend = focusTarget.GetComponent<MeshRenderer>() ?? focusTarget.GetComponentInChildren<MeshRenderer>();
        if (rend != null){
            focusPoint = rend.bounds.center;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (Input.GetMouseButton(0)){
            yaw += Input.GetAxis("Mouse X") * focusRotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * focusRotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -85f, 85f);
        }
        focusDistance -= Input.GetAxis("Mouse ScrollWheel") * focusZoomSpeed;
        focusDistance = Mathf.Clamp(focusDistance, focusDistanceMinMax.x, focusDistanceMinMax.y);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPosition = focusPoint + rotation * new Vector3(0, 0, -focusDistance);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * focusChangeSpeed);
        transform.LookAt(focusPoint);
    }

    void HandleFreeMoveMode()
    {
        if (Input.GetMouseButton(1)){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            rotationX += Input.GetAxis("Mouse X") * freeLookSpeed;
            rotationY -= Input.GetAxis("Mouse Y") * freeLookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        if (Input.GetKey(KeyCode.E)) move.y += 1;
        if (Input.GetKey(KeyCode.Q)) move.y -= 1;
        transform.position += move * freeMoveSpeed * Time.deltaTime;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * scroll * freeZoomSpeed * Time.deltaTime;
    }

    void SetFocus(Transform newTarget)
    {
        focusTarget = newTarget;
        
        float targetRadius = 0.5f;
        Renderer rend = newTarget.GetComponent<MeshRenderer>() ?? newTarget.GetComponentInChildren<MeshRenderer>();

        if (rend != null)
        {
            focusPoint = rend.bounds.center;
            targetRadius = Mathf.Max(rend.bounds.extents.x, rend.bounds.extents.y, rend.bounds.extents.z);
        }
        else
        {
            focusPoint = newTarget.position;
        }
        
        // ★★★ ここからが、今回の修正の核心部分です ★★★
        float desiredDistance;
        
        // 1. まず、特別な「SaturnFocus」タグが付いているかチェック
        if (newTarget.CompareTag("SaturnFocus"))
        {
            // 【土星の場合】
            desiredDistance = targetRadius * saturnDistanceMultiplier;
        }
        // 2. 次に、巨大な天体かどうかをチェック
        else if (targetRadius > largeObjectThreshold)
        {
            // 【土星以外の巨大な天体の場合】
            desiredDistance = targetRadius * largeObjectDistanceMultiplier;
        }
        // 3. それ以外
        else
        {
            // 【通常の天体の場合】
            desiredDistance = targetRadius * defaultDistanceMultiplier;
        }
        // ★★★ ここまで ★★★
        
        focusDistance = Mathf.Clamp(desiredDistance, focusDistanceMinMax.x, focusDistanceMinMax.y);
        
        Quaternion lookRotation = Quaternion.LookRotation(focusPoint - transform.position);
        yaw = lookRotation.eulerAngles.y;
        pitch = lookRotation.eulerAngles.x;
    }
    
    void RemoveFocus()
    {
        if (focusTarget == null) return;
        transform.position -= transform.forward * defocusKickback;
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;
        focusTarget = null;
    }
}