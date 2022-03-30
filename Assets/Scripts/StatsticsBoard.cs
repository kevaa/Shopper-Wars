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

    public void IncNumberGamePlayed()
    {
        PlayerPrefs.SetInt("GamePlayed", PlayerPrefs.GetInt("GamePlayed") + 1);
    }

    public int GetNumberGamePlayed()
    {
        return PlayerPrefs.GetInt("GamePlayed");
    }

    public void IncNumberFirstPlace()
    {
        PlayerPrefs.SetInt("FirstPlace", PlayerPrefs.GetInt("FirstPlace") + 1);
    }

    public int GetNumberFirstPlace()
    {
        return PlayerPrefs.GetInt("FirstPlace");
    }
    public void IncNumberSecondPlace()
    {
        PlayerPrefs.SetInt("SecondPlace", PlayerPrefs.GetInt("SecondPlace") + 1);
    }

    public int GetNumberSecondPlace()
    {
        return PlayerPrefs.GetInt("SecondPlace");
    }

    public void IncNumberThirdPlace()
    {
        PlayerPrefs.SetInt("ThirdPlace", PlayerPrefs.GetInt("ThirdPlace") + 1);
    }
    public int GetNumberThirdPlace()
    {
        return PlayerPrefs.GetInt("ThirdPlace");
    }
    public void IncNumberTotalWin()
    {
        PlayerPrefs.SetInt("TotalWin", PlayerPrefs.GetInt("TotalWin") + 1);
    }
    public int GetNumberTotalWin()
    {
        return PlayerPrefs.GetInt("TotalWin");
    }

    public void IncNumberTotalSkinUnlocked()
    {
        PlayerPrefs.SetInt("SkinUnclocked", PlayerPrefs.GetInt("SkinUnclocked") + 1);
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
        text += "Skin unlocked: " + StatsticsBoard.Instance.GetNumberTotalSkinUnlocked() + "\n";
        textGUI.text = text;
    }

}
