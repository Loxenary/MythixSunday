using UnityEngine;

public abstract class Audio 
{
    protected Audio(string key, AudioClip audioClip, float clipVolume){ 
        _key = key;
        _audioClip = audioClip;
        _volume = clipVolume;
    }

    private static float s_masterVolume = 0;

    protected readonly string _key;
    protected readonly AudioClip _audioClip;
    protected float _volume = 100; 
 
    public string Key => _key;
    public AudioClip AudioClip => _audioClip;

    public static float MasterVolume{
        get { return s_masterVolume; }
        set{
            if(value >= 0 && value <= 100){
                s_masterVolume = value;
            }
        }
    }
    public float ClipVolume{
        get { return _volume; }
        set{
            while(value > 1){
                value /=100;
            }
            _volume = value;
        }
    }
    
}