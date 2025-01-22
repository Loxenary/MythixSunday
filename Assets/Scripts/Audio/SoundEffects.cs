using UnityEngine;

public class SoundEffect : Audio
{
    public SoundEffect(string key, AudioClip audioClip, float clipVolume) : base(key, audioClip, clipVolume){
        
    }
    private static float s_soundEffectMasterVolume = 0;
    public static float SoundEffectMasterVolume{
        get{
            return s_soundEffectMasterVolume;
        }   
        set{
            while(value > 1){
                value /= 100;
            }
            s_soundEffectMasterVolume = value;
        }
    }
}


public class SFXCooldown{
    public float LastPlayedTime;
    public float CooldownDuration;
}