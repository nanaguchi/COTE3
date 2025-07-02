using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    
    // 初期設定が完了したかを追跡するためのフラグ
    private bool isInitialSetupComplete = false;

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

        // 0番目のタブ（重力）を初期選択状態にする
        SelectTab(0);
    }

    // LateUpdateは、すべてのUpdate処理が終わった後に呼ばれます。
    // UIの初期化が完了した、より確実なタイミングで処理を行うために使います。
    void LateUpdate()
    {
        // この初期設定処理が、一度だけ実行されるように制御します
        if (!isInitialSetupComplete && propertiesPanel.activeInHierarchy)
        {
            SetupSlidersForGravity();
            isInitialSetupComplete = true; // フラグを立てて、二度と実行されないようにする
        }
    }

    public void SelectTab(int tabIndex)
    {
        for (int i = 0; i < underlines.Length; i++)
        {
            if (underlines[i] != null)
            {
                underlines[i].SetActive(false);
            }
        }

        if (tabIndex < underlines.Length && underlines[tabIndex] != null)
        {
            underlines[tabIndex].SetActive(true);
        }

        // タブがクリックされたら、スライダーの設定をやり直す
        if (tabIndex == 0)
        {
            SetupSlidersForGravity();
        }
        // 今後、ここに温度(tabIndex == 1)などの処理を追加していく
    }
    
    void SetupSlidersForGravity()
    {
        if (allPlanets == null || allSliders == null || allPlanets.Length != allSliders.Length || allPlanets.Length == 0)
        {
            // データが不十分な場合は、エラーメッセージを出して処理を中断
            Debug.LogError("惑星データ(All Planets)とスライダー(All Sliders)の数が一致しないか、設定されていません。");
            return;
        }

        // スライダーの最大値を太陽の重力に設定（0番目が太陽と仮定）
        float maxGravity = allPlanets[0].gravity;

        for (int i = 0; i < allSliders.Length; i++)
        {
            Slider slider = allSliders[i];
            PlanetData planet = allPlanets[i];
            
            // スライダーが正しく設定されているか確認
            if (slider == null || planet == null) continue;

            // スライダーが動かされた時の処理（リスナー）を一度クリア
            slider.onValueChanged.RemoveAllListeners();

            // スライダーの範囲と初期値を設定
            slider.minValue = 0;
            slider.maxValue = maxGravity;
            slider.value = planet.gravity; // min/max設定後にvalueを設定

            // スライダーが動かされた時に、どの惑星の重力を更新するかを再設定
            int planetIndex = i; 
            slider.onValueChanged.AddListener((newValue) => {
                allPlanets[planetIndex].gravity = newValue;
            });
        }
    }



    public void TogglePanelVisibility()
    {
        isPanelOpen = !isPanelOpen;
        UpdatePanelAndButtonState();
    }

    private void UpdatePanelAndButtonState()
    {
        if (propertiesPanel == null || toggleButtonRect == null || toggleButtonText == null) return;

        propertiesPanel.SetActive(isPanelOpen);

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

