using System;
using UnityEngine;


[Serializable]
public class Music : Audio{

    public Music(string key, AudioClip audioClip, float clipVolume) : base(key, audioClip, clipVolume){
        
        _music_id = ++s_lastCreatedMusicId;

        Debug.Assert(_music_id > 0, "Static Music ID isn't working");        
    }

    private static float s_musicMasterVolume = 0;
    private static int s_lastCreatedMusicId = 0;

    private readonly int _music_id;

    public int MusicId => _music_id;    

    public static float MusicMasterVolume{
        get{return s_musicMasterVolume;}
        set{
            while(value > 1){
                value /= 100;            
            }
            s_musicMasterVolume = value;
        }
    }

}