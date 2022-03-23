using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    bool gameEnded = false;
    [SerializeField] bool changeTimeScale;
    private void Start()
    {
        GameManager.Instance.OnGameEnd += GameEnded;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded)
        {
            if (changeTimeScale)
            {
                Time.timeScale = 0;
            }
            pauseMenu.SetActive(true);
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
    }

    public void Restart()
    {
        if (changeTimeScale)
        {
            Time.timeScale = 1;
        }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
