using UnityEngine;
using UnityEngine.UI;

public class SoundEffectPlayer : MonoBehaviour 
{
    [SerializeField] private Slider bgmSlider;  
    [SerializeField] private AudioSource bgmAudio; 

    [SerializeField] private Slider seSlider;    
    [SerializeField] private AudioSource seAudio;  

    // クリック音用のAudioClipを追加
    [SerializeField] private AudioClip clickSound; 

    void Start()
    {
        
        Debug.Log("初期BGM音量: " + bgmAudio.volume);
        Debug.Log("初期SE音量: " + seAudio.volume);

        // BGMスライダー
        if (bgmSlider != null && bgmAudio != null)
        {
            bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
            UpdateBGMVolume(bgmSlider.value);
        }
        else
        {
            Debug.LogWarning("BGM Slider or AudioSource not assigned for BGM in SoundEffectPlayer.", this);
        }

        // SEスライダー
        if (seSlider != null && seAudio != null)
        {
            seSlider.onValueChanged.AddListener(UpdateSEVolume);
            UpdateSEVolume(seSlider.value);
        }
        else
        {
            Debug.LogWarning("SE Slider or AudioSource not assigned for SE in SoundEffectPlayer.", this);
        }
    }

    public void UpdateBGMVolume(float value)
    {
        if (bgmAudio != null)
        {
            bgmAudio.volume = value;
        }
    }

    public void UpdateSEVolume(float value)
    {
        if (seAudio != null)
        {
            seAudio.volume = value;
        }
    }

    public void PlayClickSE()
    {
        if (seAudio != null && clickSound != null)
        {
            seAudio.PlayOneShot(clickSound); 
        }
        else
        {
            Debug.LogWarning("SE AudioSource or Click Sound not assigned for PlayClickSE.", this);
        }
    }
}