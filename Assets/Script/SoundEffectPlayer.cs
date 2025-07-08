using UnityEngine;
using UnityEngine.UI;

public class SoundEffectPlayewr : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private AudioSource bgmAudio;

    void Start()
    {

        bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);


        UpdateBGMVolume(bgmSlider.value);
    }

    void UpdateBGMVolume(float value)
    {
        // 音量は0.0〜1.0の範囲
        bgmAudio.volume = value;
    }
}
