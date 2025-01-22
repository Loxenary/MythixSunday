using System;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton Setup
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AudioManager();
            }
            return _instance;
        }
        set
        {
            if (_instance != null)
            {
                // Prevent changes to the singleton instance
            }
        }
    }
    #endregion

    #region Audio Settings
    [Header("Music Settings")]
    [SerializeField] private AudioSource MusicSource; // Audio source for background music

    [Header("Audio Sources")]
    [SerializeField] private AudioSource SFXSource; // Audio source for sound effects

    [Tooltip("Default time interval between playing the same SFX (in seconds)")]
    public float defaultSfxCooldown = 0.1f; // Default cooldown for SFX

    private List<Music> _musicTracks; // List of available music tracks
    private List<SoundEffect> _soundEffects; // List of available sound effects

    private Dictionary<string, Music> _musicDictionary; // Dictionary for mapping music keys to tracks
    private Dictionary<string, SoundEffect> _soundEffectDictionary; // Dictionary for mapping sound effect keys to effects
    private Dictionary<string, SFXCooldown> _sfxCooldownDictionary; // Dictionary to track SFX cooldowns
    #endregion

    #region Properties
    public List<Music> MusicTracks => _musicTracks; // Exposes music tracks as a read-only list

    public List<SoundEffect> SoundEffects => _soundEffects; // Exposes sound effects as a read-only list
    #endregion

    #region Initialization
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the instance if it doesn't already exist
            DontDestroyOnLoad(gameObject); // Keep the AudioManager between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        // Initialize audio dictionaries
        InitializeMusicTracks();
        InitializeSoundEffects();
    }

    private void InitializeMusicTracks()
    {
        _musicDictionary = new Dictionary<string, Music>();

        foreach (Music music in _musicTracks)
        {
            if (!_musicDictionary.ContainsKey(music.Key))
            {
                _musicDictionary.Add(music.Key, music);
            }
            else
            {
                Debug.LogWarning($"Duplicate music key found: {music.Key} with Id {music.MusicId}");
            }
        }
    }

    private void InitializeSoundEffects()
    {
        _soundEffectDictionary = new Dictionary<string, SoundEffect>();

        foreach (SoundEffect soundEffect in _soundEffects)
        {
            if (!_soundEffectDictionary.ContainsKey(soundEffect.Key))
            {
                _soundEffectDictionary.Add(soundEffect.Key, soundEffect);
            }
            else
            {
                Debug.LogWarning($"Duplicate sound effect key found : {soundEffect.Key}");
            }
        }
    }
    #endregion

    #region Music Management
    public void PlayMusic(string key, bool loop = true, float? volume = -1, bool? forceChangeMusic = false)
    {
        if (MusicSource.isPlaying && (forceChangeMusic ?? false))
        {
            return; // Do nothing if music is already playing and force change isn't requested
        }

        if (_musicDictionary.TryGetValue(key, out Music music))
        {
            // Set volume and play the music
            float vol = (volume ?? music.ClipVolume) * Music.MusicMasterVolume * Audio.MasterVolume;
            MusicSource.clip = music.AudioClip;
            MusicSource.volume = vol;
            MusicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music not found for key: {key}");
        }
    }
    #endregion

    #region Sound Effect Management
    public void PlaySFX(string key, float? volume = 100, float? customCooldown = null)
    {
        if (_soundEffectDictionary.TryGetValue(key, out SoundEffect soundEffect))
        {
            if (CanPlaySFX(key, customCooldown))
            {
                // Set volume and play the sound effect
                float vol = (volume ?? soundEffect.ClipVolume) * SoundEffect.SoundEffectMasterVolume * Audio.MasterVolume;
                SFXSource.PlayOneShot(soundEffect.AudioClip, vol);
                SetSFXCooldown(key, customCooldown ?? defaultSfxCooldown);
            }
            else
            {
                Debug.LogWarning($"SFX {key} is on cooldown");
            }
        }
        else
        {
            Debug.LogError($"Sound effect not found for key: {key}");
        }
    }

    private bool CanPlaySFX(string key, float? customCooldown)
    {
        if (!_sfxCooldownDictionary.TryGetValue(key, out SFXCooldown cooldown))
        {
            return true; // If no cooldown is found, allow SFX to play
        }

        // Check if the time interval between last SFX and current is greater than or equal to cooldown
        return Time.time - cooldown.LastPlayedTime >= (customCooldown ?? cooldown.CooldownDuration);
    }

    private void SetSFXCooldown(string key, float cooldownDuration)
    {
        if (!_sfxCooldownDictionary.ContainsKey(key))
        {
            _sfxCooldownDictionary[key] = new SFXCooldown();
        }
        _sfxCooldownDictionary[key].LastPlayedTime = Time.time;
        _sfxCooldownDictionary[key].CooldownDuration = cooldownDuration;
    }
    #endregion

    #region Volume Management
    public void SetMasterVolume(float volume)
    {
        Audio.MasterVolume = volume;
    }

    public void SetMusicMasterVolume(float volume)
    {
        Music.MusicMasterVolume = volume;
    }

    public void SetSFXMasterVolume(float volume)
    {
        SoundEffect.SoundEffectMasterVolume = volume;
    }
    #endregion

    #region Additional Methods
    // Additional methods for future use can be added here.
    #endregion
}
