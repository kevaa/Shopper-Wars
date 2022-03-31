using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class GameManager : MonoBehaviour
{
    float gameTime = 180f;
    float elapseTime = 0f;
    bool gameEnded = false;
    public int gameMinutes { get; private set; }
    public int gameSeconds { get; private set; }
    [SerializeField] GameObject endGameMenu;
    public event Action OnGameEnd = delegate { };
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    [SerializeField] Player player;

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
        if (PlayerPrefs.HasKey("EndGameEarly") && PlayerPrefs.GetInt("EndGameEarly") == 1)
        {
            player.OnFoundAll += EndGame;
        }

    }
    void Update()
    {
        if (!gameEnded)
        {
            if (gameTime <= 0)
            {
                EndGame();
            }
            else
            {
                gameTime -= Time.deltaTime;
                elapseTime += Time.deltaTime;
                gameMinutes = Mathf.FloorToInt(gameTime / 60);
                gameSeconds = Mathf.RoundToInt(gameTime % 60);
            }
        }
    }

    public void EndGame()
    {
        OnGameEnd();
        endGameMenu.SetActive(true);
        StartCoroutine(FadeInEndMenu());
        UpdateStatsBoard();
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

    private void UpdateStatsBoard()
    {
        // save stats
        StatsticsBoard.Instance.SetTimeRecord(elapseTime);
        StatsticsBoard.Instance.SetNumberGamePlayed(StatsticsBoard.Instance.GetNumberGamePlayed() + 1);

        // check place
        var rank = Spawner.Instance.getLeaderboard();
        var ordered = rank.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        var playerName = Spawner.Instance.GetPlayerName();
        ArrayList nameList = new ArrayList();
        foreach (var key in ordered.Keys)
        {
            nameList.Add(key);
        }
        Debug.Log(nameList);
        Debug.Log("PLAYER: " + playerName);
        if (nameList[0].Equals(playerName))
        {
            StatsticsBoard.Instance.SetNumberFirstPlace(StatsticsBoard.Instance.GetNumberFirstPlace() + 1);
            StatsticsBoard.Instance.SetNumberTotalWin(StatsticsBoard.Instance.GetNumberTotalWin() + 1);
        }
        if (nameList[1].Equals(playerName))
        {
            StatsticsBoard.Instance.SetNumberSecondPlace(StatsticsBoard.Instance.GetNumberSecondPlace() + 1);
            StatsticsBoard.Instance.SetNumberTotalWin(StatsticsBoard.Instance.GetNumberTotalWin() + 1);
        }
        if (nameList[2].Equals(playerName))
        {
            StatsticsBoard.Instance.SetNumberThirdPlace(StatsticsBoard.Instance.GetNumberThirdPlace() + 1);
            StatsticsBoard.Instance.SetNumberTotalWin(StatsticsBoard.Instance.GetNumberTotalWin() + 1);
        }

        StatsticsBoard.Instance.SaveData();
        gameEnded = true;
    }
}
