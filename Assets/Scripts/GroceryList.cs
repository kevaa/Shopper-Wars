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

    void UpdateGroceriesText(Dictionary<GroceryName, int> groceries)
    {
        String text = "";
        foreach (var key in groceries.Keys)
        {
            text += $"{key.ToString()}: {groceries[key]}\n";
        }
        textGUI.text = text;
    }
}

