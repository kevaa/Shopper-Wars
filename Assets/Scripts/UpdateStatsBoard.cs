using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class UpdateStatsBoard : MonoBehaviour
{
    TextMeshProUGUI textGUI;

    private static UpdateStatsBoard instance;
    public static UpdateStatsBoard Instance { get { return instance; } }

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

        textGUI = GetComponent<TextMeshProUGUI>();
        refreshStatsBoard();
    }

    private void Start()
    {
        refreshStatsBoard();
    }

    private void Update()
    {
        refreshStatsBoard();
    }

    private void refreshStatsBoard()
    {
        String text = "";
        text += "Total Game Played: " + StatsticsBoard.Instance.GetNumberGamePlayed() + "\n";
        text += "\n";
        text += "Total Wins: " + StatsticsBoard.Instance.GetNumberTotalWin() + "\n";
        text += "1st Place: " + StatsticsBoard.Instance.GetNumberFirstPlace() + "\n";
        text += "2nd Place: " + StatsticsBoard.Instance.GetNumberSecondPlace() + "\n";
        text += "3rd Place: " + StatsticsBoard.Instance.GetNumberThirdPlace() + "\n";
        text += "\n";
        text += "Current Record: " + StatsticsBoard.Instance.GetTimeRecord() + "\n";
        text += "\n";
        text += "Skins Unlocked: " + StatsticsBoard.Instance.GetNumberTotalSkinUnlocked() + "\n";
        textGUI.text = text;
    }
}
