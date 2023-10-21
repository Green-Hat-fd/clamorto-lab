using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    bool gameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI;

    [SerializeField] List<MonoBehaviour> scriptToBlock;



    void Start()
    {
        gameIsPaused = false;
        Resume();
    }
    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

        EnableAllScripts(true);
    }
    
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        EnableAllScripts(false);
    }

    public void EnableAllScripts(bool enabled)
    {
        foreach (MonoBehaviour scr in scriptToBlock)
        {
            scr.enabled = enabled;
        }
    }
    
    
    public void LoadMenu()
    {
        Debug.Log("Loading menu");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}