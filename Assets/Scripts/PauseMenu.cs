using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    bool gameEnded = false;
    [SerializeField] bool changeTimeScale;

    public event Action OnPause = delegate { };
    public event Action OnResume = delegate { };

    private void Start()
    {
        GameManager.Instance.OnGameEnd += GameEnded;
    }
    public void Pause()
    {
        if (!gameEnded)
        {
            if (changeTimeScale)
            {
                Time.timeScale = 0;
            }
            pauseMenu.SetActive(true);
            OnPause();
            AudioListener.pause = true;
        }
    }

    void GameEnded()
    {
        gameEnded = true;
    }
    public void Resume()
    {
        if (changeTimeScale)
        {
            Time.timeScale = 1;
        }
        pauseMenu.SetActive(false);
        AudioListener.pause = false;

        OnResume();
    }

    public void Restart()
    {
        if (changeTimeScale)
        {
            Time.timeScale = 1;
        }
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void ExitToMenu()
    {
        if (changeTimeScale)
        {
            Time.timeScale = 1;
        }

        //remove duplicate managers from dontdestroyonload
        Destroy(SkinManager.Instance.gameObject);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
