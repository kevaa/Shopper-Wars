using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
[RequireComponent(typeof(TextMeshProUGUI))]
public class GroceryList : MonoBehaviour
{
    TextMeshProUGUI textGUI;
    [SerializeField] Player player;

    private void Awake()
    {
        textGUI = GetComponent<TextMeshProUGUI>();
        player.OnGroceriesChanged += UpdateGroceriesText;
    }

    void UpdateGroceriesText(Dictionary<GroceryName, int> groceries, Dictionary<GroceryName, int> list)
    {
        String text = "";
        int cur_sum = 0;
        int total_sum = 0;
        foreach (var key in groceries.Keys)
        {
            text += $"{key.ToString()}: {groceries[key]} / {list[key]}\n";
            cur_sum += groceries[key];
            total_sum += list[key];
        }
        text += $"\nTotal groceries collected:\n {cur_sum} / {total_sum}\n";
        textGUI.text = text;
    }
}

