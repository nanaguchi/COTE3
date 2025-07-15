using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    // このクラスの唯一のインスタンス (シングルトン)
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

    // ユーザーがスライダーを操作中かどうかを判定するフラグ
    private bool isDraggingSlider = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    void Start()
    {
        // スライダーが存在する場合、初期値を設定します。
        if (timeSlider != null)
        {
            // ★変更点：シミュレーションの開始時間を常に「0」に設定します。
            // これが「惑星が一直線に並んだ状態」を意味します。
            simulationTime = 0f;
            
            // ★変更点：スライダーの見た目も、開始時間「0」に合わせます。
            // スライダーの範囲をマイナスからプラスに設定したため、これでつまみは中央に表示されます。
            timeSlider.value = simulationTime;
        }
    }

    void Update()
    {
        // ユーザーがスライダーをドラッグしている場合
        if (isDraggingSlider)
        {
            // スライダーの位置を正として、シミュレーション時間を更新する
            simulationTime = timeSlider.value;
        }
        // ユーザーが操作していない場合（自動再生）
        else
        {
            // 時間を自動で進める
            simulationTime += Time.deltaTime * playbackSpeed;
            
            // 時間が最大値を超えたら最小値に戻る（右端まで行ったら左端から再開）
            if (simulationTime > timeSlider.maxValue)
            {
                simulationTime = timeSlider.maxValue-2000000;//ここっここおこここここここっこお！
            }
            // ★補足：もし逆再生も考慮するなら、以下のコメントアウトを解除
            // else if (simulationTime < timeSlider.minValue)
            // {
            //     simulationTime = timeSlider.maxValue;
            // }

            // 自動で進んだ時間をスライダーの表示に反映させる
            timeSlider.value = simulationTime;
        }
    }
    
    // --- 以下は、スライダーのEvent Triggerから呼び出します ---

    public void OnPointerDown()
    {
        isDraggingSlider = true;
    }

    public void OnPointerUp()
    {
        isDraggingSlider = false;
    }
}