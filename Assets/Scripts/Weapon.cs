using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour, IGrocery
{
    [SerializeField] Vector3 pushForce;
    Collider col;
    bool hit;
    AudioSource audioSource;
    [SerializeField] AudioClip attackSound;

    [SerializeField] GroceryName groceryName;
    [SerializeField] float attackAnimMult = 1f;

    Shopper parentShopper;

    Transform shopperTransform;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    // pushes other player based on force of weapon
    private void OnTriggerEnter(Collider other)
    {
        var shopper = other.GetComponent<Shopper>();
        if (shopper != null && shopper != parentShopper)
        {
            audioSource.PlayOneShot(attackSound, .2f);
            shopper.GetPushed(shopperTransform.TransformVector(pushForce));
        }
    }

    // Only enable collider when playing attack animation then disable afterwards
    public void EnableCol()
    {
        col.enabled = true;
    }

    public void DisableCol()
    {
        col.enabled = false;
    }

    public void SetShopperTransform(Transform _t)
    {
        shopperTransform = _t;
    }

    public void SetShopper(Shopper _shopper)
    {
        parentShopper = _shopper;
    }

    public GroceryName GetName()
    {
        return groceryName;
    }

    public float GetAttackAnimMult()
    {
        return attackAnimMult;
    }
}