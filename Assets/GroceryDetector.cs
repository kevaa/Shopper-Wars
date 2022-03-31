using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroceryDetector : MonoBehaviour
{
    public event Action<Pickup> OnPickupDetected = delegate { };
    private void OnTriggerEnter(Collider collider)
    {
        Pickup item = collider.GetComponent<Pickup>();
        if (item != null)
        {
            OnPickupDetected(item);
        }
    }
}
