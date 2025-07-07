using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq; // 最大値・最小値の取得を簡単にするために追加

public class PropertiesPanelController : MonoBehaviour
{
    [Header("タブ設定")]
    public Button[] tabButtons;
    public GameObject[] underlines;

    [Header("パネル設定")]
    public GameObject propertiesPanel;
    public Button toggleButton;

    [Header("対象データ")]
    public PlanetData[] allPlanets;
    public Slider[] allSliders;

    private bool isPanelOpen = false;
    private RectTransform toggleButtonRect;
    private TextMeshProUGUI toggleButtonText;
    
    // 現在選択されているタブのインデックス
    private int currentTabIndex = 0;

    void Start()
    {
        if (toggleButton != null)
        {
            toggleButtonRect = toggleButton.GetComponent<RectTransform>();
            toggleButtonText = toggleButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        // 初期状態を「閉じる」に設定
        isPanelOpen = false;
        UpdatePanelAndButtonState();
        propertiesPanel.SetActive(false); // 最初は非表示にしておく

        // タブボタンにリスナーを設定
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i; // クロージャのためのローカル変数
            tabButtons[i].onClick.AddListener(() => SelectTab(index));
        }
        
        // 0番目のタブ（重力）を初期選択状態にする
        SelectTab(0);
    }
    
    // パネルが表示された最初のフレームでスライダーを更新する
    void OnEnable()
    {
        if (propertiesPanel.activeSelf)
        {
            UpdateSlidersForCurrentTab();
        }
    }

    public void SelectTab(int tabIndex)
    {
        currentTabIndex = tabIndex;
        for (int i = 0; i < underlines.Length; i++)
        {
            if (underlines[i] != null)
            {
                underlines[i].SetActive(i == tabIndex);
            }
        }
        
        // パネルが表示されている場合のみスライダーを更新
        if(isPanelOpen)
        {
            UpdateSlidersForCurrentTab();
        }
    }
    
    // 現在のタブに応じてスライダー設定メソッドを呼び出す
    private void UpdateSlidersForCurrentTab()
    {
        if (allPlanets == null || allSliders == null || allPlanets.Length != allSliders.Length)
        {
            Debug.LogError("惑星データ(All Planets)とスライダー(All Sliders)の数が一致しないか、設定されていません。");
            return;
        }

        switch (currentTabIndex)
        {
            case 0: // 重力
                SetupSlidersForGravity();
                break;
            case 1: // 温度
                SetupSlidersForTemperature();
                break;
            case 2: // 自転
                SetupSlidersForRotationSpeed();
                break;
            case 3: // 公転
                SetupSlidersForRevolutionSpeed();
                break;
            case 4: // 質量
                SetupSlidersForMass();
                break;
        }
    }
    
    // --- 各パラメータのスライダー設定メソッド ---

    void SetupSlidersForGravity()
    {
        float minVal = allPlanets.Min(p => p.gravity);
        float maxVal = allPlanets.Max(p => p.gravity);
        
        for (int i = 0; i < allSliders.Length; i++)
        {
            // ジェネリックなメソッドを呼び出す
            SetupSlider(allSliders[i], allPlanets[i], minVal, maxVal, allPlanets[i].gravity, 
                (newValue) => allPlanets[i].gravity = newValue);
        }
    }

    void SetupSlidersForTemperature()
    {
        // PlanetDataに temperature フィールド (float型) があると仮定
        float minVal = allPlanets.Min(p => p.temperature);
        float maxVal = allPlanets.Max(p => p.temperature);

        for (int i = 0; i < allSliders.Length; i++)
        {
            SetupSlider(allSliders[i], allPlanets[i], minVal, maxVal, allPlanets[i].temperature,
                (newValue) => allPlanets[i].temperature = newValue);
        }
    }

    void SetupSlidersForRotationSpeed()
    {
        // PlanetDataに rotation_speed フィールド (float型, 単位：時間) があると仮定
        float minVal = allPlanets.Min(p => p.rotation_speed);
        float maxVal = allPlanets.Max(p => p.rotation_speed);
        
        for (int i = 0; i < allSliders.Length; i++)
        {
            SetupSlider(allSliders[i], allPlanets[i], minVal, maxVal, allPlanets[i].rotation_speed,
                (newValue) => allPlanets[i].rotation_speed = newValue);
        }
    }

    void SetupSlidersForRevolutionSpeed()
    {
        // PlanetDataに revolution_speed フィールド (float型, 単位：日) があると仮定
        // 太陽は公転しないため、0として計算から除外する (p.revolution_speed > 0)
        float minVal = 0f;
        float maxVal = allPlanets.Where(p => p.revolution_speed > 0).Max(p => p.revolution_speed);

        for (int i = 0; i < allSliders.Length; i++)
        {
            SetupSlider(allSliders[i], allPlanets[i], minVal, maxVal, allPlanets[i].revolution_speed,
                (newValue) => allPlanets[i].revolution_speed = newValue);
        }
    }

    void SetupSlidersForMass()
    {
        // PlanetDataに mass フィールド (double型) があると仮定
        // 値が巨大なためdoubleを使用
        double minVal = allPlanets.Min(p => p.mass);
        double maxVal = allPlanets.Max(p => p.mass);

        for (int i = 0; i < allSliders.Length; i++)
        {
            Slider slider = allSliders[i];
            PlanetData planet = allPlanets[i];
            if (slider == null || planet == null) continue;

            slider.onValueChanged.RemoveAllListeners();
            // doubleの巨大な値をfloatのスライダーで扱うため、対数的に値を変換する
            slider.minValue = 0;
            slider.maxValue = 1; // 0から1の範囲に正規化
            
            // 現在の質量の値を0-1の範囲に変換して設定
            slider.value = (float)((System.Math.Log10(planet.mass) - System.Math.Log10(minVal)) / (System.Math.Log10(maxVal) - System.Math.Log10(minVal)));

            int planetIndex = i;
            slider.onValueChanged.AddListener((sliderValue) => {
                // スライダーの値を元の質量のスケールに逆変換
                double logVal = System.Math.Log10(minVal) + sliderValue * (System.Math.Log10(maxVal) - System.Math.Log10(minVal));
                allPlanets[planetIndex].mass = System.Math.Pow(10, logVal);
            });
        }
    }
    
    // --- 汎用スライダー設定メソッド ---
    
    private void SetupSlider(Slider slider, PlanetData planet, float min, float max, float currentValue, System.Action<float> onValueChanged)
    {
        if (slider == null || planet == null) return;

        slider.onValueChanged.RemoveAllListeners();
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = currentValue;
        slider.onValueChanged.AddListener(newValue => onValueChanged(newValue));
    }
    
    
    // --- パネル表示切り替え (既存のコード) ---

    public void TogglePanelVisibility()
    {
        isPanelOpen = !isPanelOpen;
        propertiesPanel.SetActive(isPanelOpen);

        // パネルが開かれたときに、現在のタブのスライダーを更新する
        if (isPanelOpen)
        {
            UpdateSlidersForCurrentTab();
        }
        
        UpdatePanelAndButtonState();
    }

    private void UpdatePanelAndButtonState()
    {
        // (このメソッドの中身は変更なし)
        if (propertiesPanel == null || toggleButtonRect == null || toggleButtonText == null) return;

        // propertiesPanel.SetActive(isPanelOpen); // TogglePanelVisibilityに移動

        if (isPanelOpen)
        {
            toggleButtonText.text = "▼";
            toggleButtonRect.anchorMin = new Vector2(0.5f, 0.5f);
            toggleButtonRect.anchorMax = new Vector2(0.5f, 0.5f);
            toggleButtonRect.pivot = new Vector2(0.5f, 0.5f);
            toggleButtonRect.anchoredPosition = new Vector2(85, 100); 
        }
        else
        {
            toggleButtonText.text = "◄";
            toggleButtonRect.anchorMin = new Vector2(1f, 0.5f);
            toggleButtonRect.anchorMax = new Vector2(1f, 0.5f);
            toggleButtonRect.pivot = new Vector2(1f, 0.5f);
            toggleButtonRect.anchoredPosition = new Vector2(0, 100);
        }
    }
}