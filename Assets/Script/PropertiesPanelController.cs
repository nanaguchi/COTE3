using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    private int currentTabIndex = 0;

    void Start()
    {
        if (toggleButton != null)
        {
            toggleButtonRect = toggleButton.GetComponent<RectTransform>();
            toggleButtonText = toggleButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        isPanelOpen = false;
        UpdatePanelAndButtonState();
        propertiesPanel.SetActive(false);

        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => SelectTab(index));
        }
        SelectTab(0);
    }

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
        if (isPanelOpen)
        {
            UpdateSlidersForCurrentTab();
        }
    }

    private void UpdateSlidersForCurrentTab()
    {
        if (allPlanets == null || allSliders == null || allPlanets.Length != allSliders.Length)
        {
            Debug.LogError("惑星データ(All Planets)とスライダー(All Sliders)の数が一致しないか、設定されていません。");
            return;
        }

        switch (currentTabIndex)
        {
            case 0: SetupSlidersForGravity(); break;
            case 1: SetupSlidersForTemperature(); break;
            case 2: SetupSlidersForRotationSpeed(); break;
            case 3: SetupSlidersForRevolutionSpeed(); break;
            case 4: SetupSlidersForMass(); break;
        }
    }

    void SetupSlidersForGravity()
    {
        float minVal = allPlanets.Min(p => p.gravity);
        float maxVal = allPlanets.Max(p => p.gravity);
        for (int i = 0; i < allSliders.Length; i++)
        {
            // ★★★ この行が重要です ★★★
            PlanetData targetPlanet = allPlanets[i];
            SetupSlider(allSliders[i], targetPlanet, minVal, maxVal, targetPlanet.gravity,
                (newValue) => targetPlanet.gravity = newValue);
        }
    }

    void SetupSlidersForTemperature()
    {
        float minVal = allPlanets.Min(p => p.temperature);
        float maxVal = allPlanets.Max(p => p.temperature);
        for (int i = 0; i < allSliders.Length; i++)
        {
            // ★★★ この行が重要です ★★★
            PlanetData targetPlanet = allPlanets[i];
            SetupSlider(allSliders[i], targetPlanet, minVal, maxVal, targetPlanet.temperature,
                (newValue) => targetPlanet.temperature = newValue);
        }
    }

    void SetupSlidersForRotationSpeed()
    {
        float minVal = allPlanets.Min(p => p.rotation_speed);
        float maxVal = allPlanets.Max(p => p.rotation_speed);
        for (int i = 0; i < allSliders.Length; i++)
        {
            // ★★★ この行が重要です ★★★
            PlanetData targetPlanet = allPlanets[i];
            SetupSlider(allSliders[i], targetPlanet, minVal, maxVal, targetPlanet.rotation_speed,
                (newValue) => targetPlanet.rotation_speed = newValue);
        }
    }

    void SetupSlidersForRevolutionSpeed()
    {
        float minVal = 0f;
        float maxVal = allPlanets.Where(p => p.revolution_speed > 0).Max(p => p.revolution_speed);
        for (int i = 0; i < allSliders.Length; i++)
        {
            // ★★★ この行が重要です ★★★
            PlanetData targetPlanet = allPlanets[i];
            SetupSlider(allSliders[i], targetPlanet, minVal, maxVal, targetPlanet.revolution_speed,
                (newValue) => targetPlanet.revolution_speed = newValue);
        }
    }

    void SetupSlidersForMass()
    {
        double minVal = allPlanets.Where(p => p.mass > 0).Min(p => p.mass);
        double maxVal = allPlanets.Max(p => p.mass);
        for (int i = 0; i < allSliders.Length; i++)
        {
            Slider slider = allSliders[i];
            PlanetData planet = allPlanets[i];
            if (slider == null || planet == null) continue;
            slider.onValueChanged.RemoveAllListeners();
            slider.minValue = 0;
            slider.maxValue = 1;
            if (maxVal > minVal && planet.mass > 0)
            {
                double logMin = System.Math.Log10(minVal);
                double logMax = System.Math.Log10(maxVal);
                slider.value = (float)((System.Math.Log10(planet.mass) - logMin) / (logMax - logMin));
            }

            int planetIndex = i;
            slider.onValueChanged.AddListener((sliderValue) => {
                double logMin = System.Math.Log10(minVal);
                double logMax = System.Math.Log10(maxVal);
                double logVal = logMin + sliderValue * (logMax - logMin);
                allPlanets[planetIndex].mass = System.Math.Pow(10, logVal);
            });
        }
    }

    private void SetupSlider(Slider slider, PlanetData planet, float min, float max, float currentValue, System.Action<float> onValueChanged)
    {
        if (slider == null || planet == null) return;
        slider.onValueChanged.RemoveAllListeners();
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = currentValue;
        slider.onValueChanged.AddListener(newValue => onValueChanged(newValue));
    }

    public void TogglePanelVisibility()
    {
        isPanelOpen = !isPanelOpen;
        propertiesPanel.SetActive(isPanelOpen);
        if (isPanelOpen)
        {
            UpdateSlidersForCurrentTab();
        }
        UpdatePanelAndButtonState();
    }

    private void UpdatePanelAndButtonState()
    {
        if (propertiesPanel == null || toggleButtonRect == null || toggleButtonText == null) return;
        if (isPanelOpen)
        {
            toggleButtonText.text = "▼";
            toggleButtonRect.anchorMin = new Vector2(0.5f, 0.5f);
            toggleButtonRect.anchorMax = new Vector2(0.5f, 0.5f);
            toggleButtonRect.pivot = new Vector2(0.5f, 0.5f);
            toggleButtonRect.anchoredPosition = new Vector2(243, 180);
        }
        else
        {
            toggleButtonText.text = "◄";
            toggleButtonRect.anchorMin = new Vector2(1f, 0.5f);
            toggleButtonRect.anchorMax = new Vector2(1f, 0.5f);
            toggleButtonRect.pivot = new Vector2(1f, 0.5f);
            toggleButtonRect.anchoredPosition = new Vector2(0, 180);
        }
    }
}