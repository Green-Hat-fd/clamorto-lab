using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [SerializeField] List<MonoBehaviour> scriptsToDeactivate;
    //[SerializeField] MusicPlaylist levelMusicPl;


    private void Awake()
    {
        //Attiva ogni script nella lista
        ActivateScripts(true);
    }


    public void ActivateScripts(bool active)
    {
        foreach (MonoBehaviour scr in scriptsToDeactivate)
        {
            scr.enabled = active;
        }
    }

    /*public void ActivateLevelMusic(bool active)
    {
        if (active)
            levelMusicPl.ShuffleNStartPlaylist();
        else
            levelMusicPl.StopAllMusic();
    }//*/
}
