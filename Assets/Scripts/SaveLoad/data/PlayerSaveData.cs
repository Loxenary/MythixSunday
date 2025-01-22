using System;

[Serializable]
public class PlayerSaveData : ISaveData
{
    public string FileName => "PlayerData.json";
}