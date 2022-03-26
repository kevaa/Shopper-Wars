using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] GameObject pickupPrefab;
    bool pickedUp = false;
    public GameObject PickupItem()
    {
        if (!pickedUp)
        {
            pickedUp = true;
            return pickupPrefab;
        }
        return null;
    }

    public GroceryName GetGroceryName()
    {
        return pickupPrefab.GetComponent<IGrocery>().GetName();
    }
}