using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Mixer References")]
    public AudioMixer mainAudioMixer;
    
    [Header("UI References")]
    public GameObject settingsPanel;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    
    [Header("Exposed Mixer Parameters")]
    public string musicParameter = "Music";  
    public string sfxParameter = "SFX";      
    
    [Header("PlayerPrefs Keys")]
    public string musicVolumeKey = "MusicVolume";
    public string sfxVolumeKey = "SFXVolume";
    
    void Start()
    {
        InitializeSettings();
        
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    
    void InitializeSettings()
    {
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        
        LoadSettings();
    }
    
    public void ToggleSettingsMenu()
    {
        if (settingsPanel != null)
        {
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);
            
            Time.timeScale = isActive ? 1f : 0f;
            
            if (!isActive) LoadSettings();
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        float dB = volume > 0.0001f ? 20f * Mathf.Log10(volume) : -80f;
        mainAudioMixer.SetFloat(musicParameter, dB);
        PlayerPrefs.SetFloat(musicVolumeKey, volume);
    }
    
    public void SetSFXVolume(float volume)
    {
        float dB = volume > 0.0001f ? 20f * Mathf.Log10(volume) : -80f;
        mainAudioMixer.SetFloat(sfxParameter, dB);
        PlayerPrefs.SetFloat(sfxVolumeKey, volume);
    }
    
    private void LoadSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat(musicVolumeKey, 0.75f);
        musicVolumeSlider.SetValueWithoutNotify(musicVolume);
        SetMusicVolume(musicVolume);
        
        float sfxVolume = PlayerPrefs.GetFloat(sfxVolumeKey, 0.75f);
        sfxVolumeSlider.SetValueWithoutNotify(sfxVolume);
        SetSFXVolume(sfxVolume);
    }
    
    public void SaveSettings()
    {
        PlayerPrefs.Save();
        ToggleSettingsMenu();
    }
    
    public void ResetToDefaults()
    {
        float defaultVolume = 0.75f;
        musicVolumeSlider.value = defaultVolume;
        sfxVolumeSlider.value = defaultVolume;
        
        SetMusicVolume(defaultVolume);
        SetSFXVolume(defaultVolume);
    }
}