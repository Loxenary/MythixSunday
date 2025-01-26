using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class StatsObjects : MonoBehaviour, ISaveLoad
{
    public TextMeshProUGUI liveText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    private ResourcesSaveData _savesData;
    private DamageData _damageData;

    private void Start(){
        Load();
    }

    public void Load(){
        _savesData = SaveLoadManager.Load<ResourcesSaveData>();
        if(_savesData != null && _damageData != default){
            liveText.text = _savesData.Lives.ToString();
            healthText.text = _savesData.Health.ToString();
        }else{
            liveText.text = 1.ToString();
            healthText.text = 100.ToString();
        }

        _damageData = SaveLoadManager.Load<DamageData>();
        if(_damageData != null && _damageData != default){
            damageText.text = _damageData.DamageDealt.ToString();
        }else{
            damageText.text = 10.ToString();
        }
    }
    
    public void Save(){}
}