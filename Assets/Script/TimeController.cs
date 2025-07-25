using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    [Header("UI設定")]
    [Tooltip("時間制御用のスライダーをここに設定")]
    public Slider timeSlider;

    [Header("再生設定")]
    [Tooltip("時間の再生速度。")]
    public float playbackSpeed = 24f;

    [Header("シミュレーション時間")]
    [Tooltip("現在のシミュレーション時間（単位：時間）。")]
    public float simulationTime;

    private bool isDraggingSlider = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    void Start()
    {
        if (timeSlider != null)
        {
            simulationTime = 0f;
            timeSlider.value = simulationTime;
        }
    }

    void Update()
    {
        if (isDraggingSlider)
        {
            simulationTime = timeSlider.value;
        }
        else
        {
            simulationTime += Time.deltaTime * playbackSpeed;
            
            // 時間が最大値を超えたら最小値に戻る
            if (simulationTime > timeSlider.maxValue)
            {
                simulationTime = timeSlider.maxValue-2000000;
            }
            timeSlider.value = simulationTime;
        }
    }
    
    public void OnPointerDown()
    {
        isDraggingSlider = true;
    }

    public void OnPointerUp()
    {
        isDraggingSlider = false;
    }
}