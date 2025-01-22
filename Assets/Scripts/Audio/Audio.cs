using System;
using UnityEngine;

[Serializable]
public class Audio 
{
    protected Audio(string key, AudioClip audioClip, float clipVolume){ 
        _key = key;
        _audioClip = audioClip;
        _volume = clipVolume;
    }

    private static float s_masterVolume = 1;

    [SerializeField] protected string _key;
    [SerializeField] protected AudioClip _audioClip;
    [SerializeField, Range(0, 1)] 
    protected float _volume = 1; 
 
    public string Key => _key;
    public AudioClip AudioClip => _audioClip;

    public static float MasterVolume{
        get { return s_masterVolume; }
        set{
            while(value > 1){
                value /= 100;
            }
            Debug.Log($"Setting Master Value to {value}");                s_masterVolume = value;
        }
    }
    public float ClipVolume{
        get { return _volume; }
        set{
            while(value > 1){
                value /=100;
            }
            Debug.Log($"Setting Clip {_key} Volume to {value}");
            _volume = value;
        }
    }
    
}