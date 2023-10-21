using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOptionsScript : MonoBehaviour
{
    [SerializeField] OptionsSO_Script opt_SO;

    [Header("—— UI elements ——")]
    [SerializeField] Slider sl_musVolume;
    [SerializeField] Slider sl_sfxVolume;
    [SerializeField] Toggle tg_fullscreen;



    void OnEnable()
    {
        //Aggiorna le opzioni
        //ogni volta che si attiva
        UpdateOptions();
    }


    public void UpdateOptions()
    {
        sl_musVolume.value = opt_SO.GetMusicVolume_Percent();
        sl_sfxVolume.value = opt_SO.GetSoundVolume_Percent();

        tg_fullscreen.isOn = Screen.fullScreen;
    }
}
