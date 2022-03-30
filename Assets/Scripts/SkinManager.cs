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

    public void SetSkin(int skinIndex)
    {
        if (skinIndex > 0 && skinIndex < skinPrefabs.Count)
        {
            selectedSkin = skinPrefabs[skinIndex];
            skinSelectGlow.transform.position = skinButtons[skinIndex].transform.position;
        }
    }
}
