using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatsticsBoard : MonoBehaviour
{
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
        PlayerPrefs.Save();

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

}
