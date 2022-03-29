using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] Light light;
    bool pickedUp = false;

    public void Highlight()
    {
        light.enabled = true;
    }

    public void UnHighlight()
    {
        light.enabled = false;
    }
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