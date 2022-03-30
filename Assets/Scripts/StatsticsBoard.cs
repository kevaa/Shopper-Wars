using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class StatsticsBoard : MonoBehaviour
{
    TextMeshProUGUI textGUI;

    private static StatsticsBoard instance;
    public static StatsticsBoard Instance { get { return instance; } }

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

        if (!PlayerPrefs.HasKey("GamePlayed"))
        {
            PlayerPrefs.SetInt("GamePlayed", 0);
        }
        if (!PlayerPrefs.HasKey("FirstPlace"))
        {
            PlayerPrefs.SetInt("FirstPlace", 0);
        }
        if (!PlayerPrefs.HasKey("SecondPlace"))
        {
            PlayerPrefs.SetInt("SecondPlace", 0);
        }
        if (!PlayerPrefs.HasKey("ThirdPlace"))
        {
            PlayerPrefs.SetInt("ThirdPlace", 0);
        }
        if (!PlayerPrefs.HasKey("TotalWin"))
        {
            PlayerPrefs.SetInt("TotalWin", 0);
        }
        if (!PlayerPrefs.HasKey("SkinUnclocked"))
        {
            PlayerPrefs.SetInt("SkinUnclocked", 0);
        }
        if (!PlayerPrefs.HasKey("TimeRecord"))
        {
            PlayerPrefs.SetFloat("TimeRecord", 0f);
        }
        SaveData();

    }

    private void Start()
    {
        refreshStatsBoard();
    }

    public void SetNumberGamePlayed(int n)
    {
        PlayerPrefs.SetInt("GamePlayed", n);
    }

    public int GetNumberGamePlayed()
    {
        return PlayerPrefs.GetInt("GamePlayed");
    }

    public void SetNumberFirstPlace(int n)
    {
        PlayerPrefs.SetInt("FirstPlace", n);
    }

    public int GetNumberFirstPlace()
    {
        return PlayerPrefs.GetInt("FirstPlace");
    }
    public void SetNumberSecondPlace(int n)
    {
        PlayerPrefs.SetInt("SecondPlace", n);
    }

    public int GetNumberSecondPlace()
    {
        return PlayerPrefs.GetInt("SecondPlace");
    }

    public void SetNumberThirdPlace(int n)
    {
        PlayerPrefs.SetInt("ThirdPlace", n);
    }
    public int GetNumberThirdPlace()
    {
        return PlayerPrefs.GetInt("ThirdPlace");
    }
    public void SetNumberTotalWin(int n)
    {
        PlayerPrefs.SetInt("TotalWin", n);
    }
    public int GetNumberTotalWin()
    {
        return PlayerPrefs.GetInt("TotalWin");
    }

    public void SetNumberTotalSkinUnlocked(int n)
    {
        PlayerPrefs.SetInt("SkinUnclocked", n);
    }

    public int GetNumberTotalSkinUnlocked()
    {
        return PlayerPrefs.GetInt("SkinUnclocked");
    }

    public void SetTimeRecord(float time)
    {
        PlayerPrefs.SetFloat("TimeRecord", time);
    }

    public float GetTimeRecord()
    {
        return PlayerPrefs.GetFloat("TimeRecord");
    }

    public void clearAllData()
    {
        PlayerPrefs.DeleteAll();
        refreshStatsBoard();
    }

    public void SaveData()
    {
        PlayerPrefs.Save();
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
