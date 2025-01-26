using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SettingsSlider
{
    public Slider Slider;
    public Button Button;
    public Image Image;
    public Sprite MuteSprite;
    public Sprite UnMuteSprite;

    public void SliderVolumeChange(float volume)
    {
        if (volume <= 0)
        {
            Image.sprite = MuteSprite;
        }
        else
        {
            Image.sprite = UnMuteSprite;
        }
    }

    public void Mute()
    {
        Slider.value = 0;
    }

    public void UnMute()
    {
        Slider.value = 0.1f;
    }
}

public class PauseUI : MonoBehaviour, ISaveLoad
{
    [Header("Button")]
    [SerializeField] private Button _BackToMenu;
    [SerializeField] private Button _CloseSettings;
    [SerializeField] private Button _saveButton;

    [Header("Slider")]
    [SerializeField] private SettingsSlider musicSlider;
    [SerializeField] private SettingsSlider sfxSlider;

    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.Instance;

        // Add listeners to buttons
        _BackToMenu.onClick.AddListener(MainMenu);
        _CloseSettings.onClick.AddListener(CloseSettings);
        _saveButton.onClick.AddListener(Save);

        // Add listeners to sliders
        musicSlider.Slider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.Slider.onValueChanged.AddListener(OnSFXVolumeChanged);

        // Add listeners to mute/unmute buttons
        musicSlider.Button.onClick.AddListener(ToggleMusicMute);
        sfxSlider.Button.onClick.AddListener(ToggleSFXMute);

        // Initialize UI
        CloseSettings();
    }

    public void OpenSettings()
    {
        Load();
        gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }

    public void Save()
    {
        SettingsSaveData data = new SettingsSaveData
        {
            MasterVolume = 1,
            MusicVolume = musicSlider.Slider.value,
            SFXVolume = sfxSlider.Slider.value
        };
        SaveLoadManager.Save(data);
    }
    
    public void Load()
    {
        SettingsSaveData data = SaveLoadManager.Load<SettingsSaveData>();
        if (data != null)
        {
            musicSlider.Slider.value = data.MusicVolume;
            sfxSlider.Slider.value = data.SFXVolume;
            _audioManager.SetMasterVolume(data.MasterVolume);
            _audioManager.SetMusicMasterVolume(data.MusicVolume);
            _audioManager.SetSFXMasterVolume(data.SFXVolume);

            // Update slider visuals
            musicSlider.SliderVolumeChange(data.MusicVolume);
            sfxSlider.SliderVolumeChange(data.SFXVolume);
        }
    }

    private void OnMusicVolumeChanged(float volume)
    {
        _audioManager.SetMusicMasterVolume(volume);
        musicSlider.SliderVolumeChange(volume);
    }

    private void OnSFXVolumeChanged(float volume)
    {
        _audioManager.SetSFXMasterVolume(volume);
        sfxSlider.SliderVolumeChange(volume);
    }

    private void ToggleMusicMute()
    {
        if (musicSlider.Slider.value > 0)
        {
            musicSlider.Mute();
        }
        else
        {
            musicSlider.UnMute();
        }
        OnMusicVolumeChanged(musicSlider.Slider.value);
    }

    private void ToggleSFXMute()
    {
        if (sfxSlider.Slider.value > 0)
        {
            sfxSlider.Mute();
        }
        else
        {
            sfxSlider.UnMute();
        }
        OnSFXVolumeChanged(sfxSlider.Slider.value);
    }

    public void GoToShop()
    {
        MySceneManager.Instance.LoadScene(SceneEnum.SHOP);
    }

    public void MainMenu()
    {
        MySceneManager.Instance.LoadScene(SceneEnum.MAIN_MENU);
    }
}