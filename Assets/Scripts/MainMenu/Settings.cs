using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class Settings : MonoBehaviour, ISaveLoad
{

    [Header("Button")]
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _saveButton;
    
    public event Action BackButtonPressed;

    [Header("Slider")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private AudioManager _audioManager;
   
    private void Awake()
    {
        _audioManager = AudioManager.Instance;
        try{
            HandleInvalidMaxValue(masterSlider);
            HandleInvalidMaxValue(musicSlider);
            HandleInvalidMaxValue(sfxSlider);
        }catch(Exception e){
            Debug.LogError(e.Message);
        }
    }

    private void HandleInvalidMaxValue(Slider slider){
        if(slider.maxValue != 100){
            throw new Exception($"Max value of ${slider.name} should be set to 100");
        }
    }

    private void Start()
    {
        Load();
        masterSlider.onValueChanged.AddListener((newValue) => _audioManager.SetMasterVolume(newValue));
        musicSlider.onValueChanged.AddListener((newValue) => _audioManager.SetMusicMasterVolume(newValue));
        sfxSlider.onValueChanged.AddListener((newValue) => _audioManager.SetSFXMasterVolume(newValue));
        _backButton.onClick.AddListener(() => BackButtonPressed.Invoke());
        _saveButton.onClick.AddListener(() => Save());
    }

    private void Save(){
        SettingsSaveData data = new();
        data.SetData(masterSlider.value, musicSlider.value, sfxSlider.value);
        SaveLoadManager.Save(data);
    }

    private void Load(){
        SettingsSaveData data = SaveLoadManager.Load<SettingsSaveData>();
        masterSlider.value = data.MasterVolume;
        musicSlider.value = data.MusicVolume;
        sfxSlider.value = data.MusicVolume;
        _audioManager.SetMasterVolume(data.MasterVolume);
        _audioManager.SetMusicMasterVolume(data.MusicVolume);
        _audioManager.SetSFXMasterVolume(data.SFXVolume);
    }

    //Explicitly interface implementation to hide from Settings API
    void ISaveLoad.Save()
    {
        Save();
    }

    void ISaveLoad.Load()
    {
        Load();
    }
}