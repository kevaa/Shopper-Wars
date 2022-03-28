using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
[RequireComponent(typeof(TextMeshProUGUI))]
public class Leaderboard : MonoBehaviour
{
    TextMeshProUGUI textGUI;

    private void Awake()
    {
        textGUI = GetComponent<TextMeshProUGUI>();
        Spawner.Instance.UpdateLeaderboard += UpdateLeaderboardText;
    }

    void UpdateLeaderboardText(Dictionary<string, int> list)
    {
        String text = "Leaderboard \n";
        foreach (var key in list.Keys)
        {
            text += $"{key}: {list[key]} \n";
        }
        textGUI.text = text;
    }
}

