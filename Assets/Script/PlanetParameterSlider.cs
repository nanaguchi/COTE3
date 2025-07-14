using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PlanetParameterSlider : MonoBehaviour
{
    [Header("操作対象の惑星データ")]
    public PlanetData targetPlanetData;

    [Header("操作するパラメータ")]
    public ParameterType parameterType;

    [Header("パラメータの範囲")]
    public float minValue = 1f;    // 周期の最小値（最も速い）
    public float maxValue = 1000f; // 周期の最大値（最も遅い）

    public enum ParameterType
    {
        RotationSpeed,
        RevolutionSpeed
        // 他のパラメータもここに追加
    }

    private Slider slider;
    private bool hasBeenInitialized = false;

    void OnEnable()
    {
        slider = GetComponent<Slider>();
        if (slider == null || targetPlanetData == null)
        {
            this.enabled = false;
            return;
        }

        if (!hasBeenInitialized)
        {
            InitializeSliderValue();
            hasBeenInitialized = true;
        }
        
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnDisable()
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    public void InitializeSliderValue()
    {
        if (slider == null || targetPlanetData == null) return;

        float currentValue = 0f;
        switch (parameterType)
        {
            case ParameterType.RotationSpeed:
                currentValue = targetPlanetData.rotation_speed;
                break;
            case ParameterType.RevolutionSpeed:
                currentValue = targetPlanetData.revolution_speed;
                break;
        }
        // スライダーの値を設定（周期とスライダーの値の関係を逆にする）
        slider.value = Mathf.InverseLerp(maxValue, minValue, currentValue);
    }

    public void OnSliderValueChanged(float value)
    {
        if (targetPlanetData == null) return;

        // スライダーの値を周期に変換（スライダーの値が大きいほど、周期は短く=速くなる）
        float newValue = Mathf.Lerp(maxValue, minValue, value);

        if (newValue <= 0) newValue = 0.001f;

        switch (parameterType)
        {
            case ParameterType.RotationSpeed:
                targetPlanetData.rotation_speed = newValue;
                break;
            case ParameterType.RevolutionSpeed:
                targetPlanetData.revolution_speed = newValue;
                break;
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(targetPlanetData);
        }
#endif
    }
}