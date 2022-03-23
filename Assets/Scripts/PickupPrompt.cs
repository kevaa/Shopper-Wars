using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PickupPrompt : MonoBehaviour
{
    [SerializeField] Player player;
    CanvasGroup canvasGroup;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        player.OnEnteredPickupRadius += ShowPrompt;
        player.OnLeftPickupRadius += ConcealPrompt;
        player.OnEquipped += ConcealPrompt;
    }

    void ShowPrompt()
    {
        canvasGroup.alpha = 1;
    }
    void ConcealPrompt()
    {
        canvasGroup.alpha = 0;
    }
}
