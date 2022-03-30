using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    bool gameEnded;
    float gameTime = 180f;
    float elapseTime = 0f;
    public int gameMinutes { get; private set; }
    public int gameSeconds { get; private set; }
    [SerializeField] GameObject endGameMenu;
    public event Action OnGameEnd = delegate { };
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }
    void Update()
    {
        if (!gameEnded)
        {
            gameMinutes = Mathf.FloorToInt(gameTime / 60);
            gameSeconds = Mathf.RoundToInt(gameTime % 60);

            if (gameTime <= 0)
            {
                EndGame();
            }
            else
            {
                gameTime -= Time.deltaTime;
                elapseTime += Time.deltaTime;
            }
        }
    }

    void EndGame()
    {
        gameEnded = true;
        OnGameEnd();
        endGameMenu.SetActive(true);
        StartCoroutine(FadeInEndMenu());

        // save stats
        StatsticsBoard.Instance.SetTimeRecord(elapseTime);
        StatsticsBoard.Instance.IncNumberGamePlayed();

        // check place

        StatsticsBoard.Instance.SaveData();
    }

    IEnumerator FadeInEndMenu()
    {
        var canvasGroup = endGameMenu.GetComponent<CanvasGroup>();
        var seconds = 0f;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, seconds);
            seconds += Time.deltaTime;
            yield return null;
        }
    }
}
