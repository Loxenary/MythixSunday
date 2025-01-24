using UnityEngine;

public class SettingsSaveData : ISaveData
{
    public string FileName => "Settings";
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;    

    public void SetData(float master, float music, float sFX){
        MasterVolume = master;
        MusicVolume = music;
        SFXVolume = sFX;
    }
}