using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]

public class TimeIndicator : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] GameManager gm;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        text.text = string.Format("{0:00}:{1:00}", gm.gameMinutes, gm.gameSeconds);
    }
}
