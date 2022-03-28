using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;

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
        var ordered = list.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        String text = "Leaderboard \n\n";
        foreach (var key in ordered.Keys)
        {
            text += $"{key}: {list[key]} \n";
        }
        textGUI.text = text;
    }
}

