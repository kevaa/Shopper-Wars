using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject ammoIndicatorPrefab;
    Stack<GameObject> ammoIndicators;

    private void Awake()
    {
        player.OnTrapEquipped += OnTrapEquipped;
        player.OnTrapPlaced += OnTrapPlaced;
    }
    private void Start()
    {
        ammoIndicators = new Stack<GameObject>();
    }

    void OnTrapEquipped(int trapCount)
    {
        while (ammoIndicators.Count < trapCount)
        {
            ammoIndicators.Push(Instantiate(ammoIndicatorPrefab, transform));
        }
        while (ammoIndicators.Count > trapCount)
        {
            var ammoImage = ammoIndicators.Pop();
            Destroy(ammoImage);
        }
    }
    void OnTrapPlaced()
    {
        var ammoImage = ammoIndicators.Pop();
        Destroy(ammoImage);
    }

}
