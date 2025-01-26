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

    private void Start()
    {

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
        gameObject.SetActive(true);
        Load();
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
            AudioManager.Instance.SetMasterVolume(data.MasterVolume);
            AudioManager.Instance.SetMusicMasterVolume(data.MusicVolume);
            AudioManager.Instance.SetSFXMasterVolume(data.SFXVolume);

            // Update slider visuals
            musicSlider.SliderVolumeChange(data.MusicVolume);
            sfxSlider.SliderVolumeChange(data.SFXVolume);
        }
    }

    private void OnMusicVolumeChanged(float volume)
    {
        AudioManager.Instance.SetMusicMasterVolume(volume);
        musicSlider.SliderVolumeChange(volume);
        Save();
    }

    private void OnSFXVolumeChanged(float volume)
    {
        AudioManager.Instance.SetSFXMasterVolume(volume);
        sfxSlider.SliderVolumeChange(volume);
        Save();
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
        MySceneManager.Instance.LoadSceneWithMusic(SceneEnum.SHOP, "ShopBGM");
    }

    public void MainMenu()
    {
        MySceneManager.Instance.LoadSceneWithMusic(SceneEnum.MAIN_MENU, "MainMenuBGM");
    }
}