using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SkinManager : MonoBehaviour
{

    public List<GameObject> skinPrefabs;

    public GameObject selectedSkin;


    public List<Transform> skinButtons;

    public GameObject skinSelectGlow;

    //quick and dirty singleton
    private static SkinManager _instance;

    public static SkinManager Instance { get { return _instance; } }

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        UnlockSkins(StatsticsBoard.Instance.GetNumberTotalSkinUnlocked());
    }

    public void SetSkin(int skinIndex)
    {
        if (skinIndex >= 0 && skinIndex < skinPrefabs.Count)
        {
            selectedSkin = skinPrefabs[skinIndex];
            skinSelectGlow.transform.position = skinButtons[skinIndex].transform.position;
        }
    }

    public void UnlockSkins(int skinCount)
    {
        if (skinCount > skinButtons.Count - 1) skinCount = skinButtons.Count - 1;

        for (int i = 0; i < skinCount; i++)
        {
            skinButtons[i + 1].gameObject.SetActive(true);
        }

    }

}
