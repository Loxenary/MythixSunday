using System;
using UnityEngine;

[Serializable]
public class SoundEffect : Audio
{
    public SoundEffect(string key, AudioClip audioClip, float clipVolume) : base(key, audioClip, clipVolume){
        
    }
    private static float s_soundEffectMasterVolume = 1;
    public static float SoundEffectMasterVolume{
        get{
            return s_soundEffectMasterVolume;
        }   
        set{
            while(value > 1){
                value /= 100;
            }
            Debug.Log($"Setting Sound Effect Volume to {value}");
            s_soundEffectMasterVolume = value;
        }
    }
}


public class SFXCooldown{
    public float LastPlayedTime;
    public float CooldownDuration;
}