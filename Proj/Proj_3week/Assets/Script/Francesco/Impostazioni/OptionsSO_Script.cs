using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Scriptable Objects/Options (S.O.)", fileName = "Options_SO")]
public class OptionsSO_Script : ScriptableObject
{
    //Menu Principale
    #region Scene

    public void OpenNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void OpenChooseScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    #endregion


    #region Esci dal gioco

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion


    //Opzioni
    #region Volume e Audio

    [Space(15)]
    [SerializeField] AudioMixer generalMixer;
    [SerializeField] AnimationCurve audioCurve;
    [Range(0, 110)]
    [SerializeField] float musicVolume = 0f;
    [Range(0, 110)]
    [SerializeField] float soundVolume = 0f;

    ///<summary></summary>
    /// <param name="vM"> new volume, in range [0; 1.1]</param>
    public void ChangeMusicVolume(float vM)
    {
        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("musVol", audioCurve.Evaluate(vM));

        musicVolume = vM * 100;
    }
    ///<summary></summary>
    /// <param name="vS"> new volume, in range [0; 1.1]</param>
    public void ChangeSoundVolume(float vS)
    {
        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("sfxVol", audioCurve.Evaluate(vS));

        soundVolume = vS * 100;
    }

    ///<summary></summary>
    /// <param name="vM"> new volume, in range [0; 11]</param>
    public void ChangeMusicVolumeTen(float vM)
    {
        vM /= 10;

        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("musVol", audioCurve.Evaluate(vM));

        musicVolume = vM * 100;
    }
    ///<summary></summary>
    /// <param name="vS"> new volume, in range [0; 11]</param>
    public void ChangeSoundVolumeTen(float vS)
    {
        vS /= 10;

        //Puts as volume in the mixer between [-80; 5] dB
        generalMixer.SetFloat("sfxVol", audioCurve.Evaluate(vS));

        soundVolume = vS * 100;
    }

    public AnimationCurve GetVolumeCurve() => audioCurve;

    public float GetMusicVolume() => audioCurve.Evaluate(musicVolume);
    public float GetMusicVolume_Percent() => musicVolume / 100;
    public float GetSoundVolume() => audioCurve.Evaluate(soundVolume);
    public float GetSoundVolume_Percent() => soundVolume / 100;

    #endregion


    #region Fullscreen

    [Space(15)]
    [SerializeField] bool fullscreen = true;

    public void ToggleFullscreen(bool yn)
    {
        Screen.fullScreen = yn;

        fullscreen = yn;
    }

    public bool GetIsFullscreen() => fullscreen;

    #endregion


    //Altro
    #region Altre funzioni

    #endregion
}
