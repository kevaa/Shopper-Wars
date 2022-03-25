using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public abstract class Shopper : MonoBehaviour, IPushable
{
    GameObject equippedTrapPrefab;

    [SerializeField] GameObject defaultWeaponPrefab;
    public int trapCount { get; private set; }
    Dictionary<GroceryName, int> groceries;
    protected Rigidbody rb;
    float attackAnimTime = .6f;
    [SerializeField] Transform trapSpawnTransform;
    [SerializeField] Transform weaponTransform;
    public event Action<Dictionary<GroceryName, int>> OnGroceriesChanged = delegate { };
    Weapon weapon;

    public bool coroutineActive { get; private set; } = false; // When shopper doing action like placing trap/pushing
    protected Animator animator;
    protected Pickup currentPickup;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    protected virtual void Start()
    {
        groceries = new Dictionary<GroceryName, int>();
        EquipWeapon(defaultWeaponPrefab);
    }

    protected void PlaceTrap()
    {
        if (!coroutineActive && trapCount > 0)
        {
            StartCoroutine(PlaceTrapCoroutine());
        }
    }
    protected void Push()
    {
        if (!coroutineActive)
        {
            StartCoroutine(PushCoroutine());
        }
    }

    IEnumerator PushCoroutine()
    {
        coroutineActive = true;
        animator.SetTrigger("IsAttacking");
        yield return new WaitForSeconds(attackAnimTime / 3);
        weapon.EnableCol();
        yield return new WaitForSeconds(attackAnimTime / 3);
        weapon.DisableCol();
        yield return new WaitForSeconds(attackAnimTime / 3);
        coroutineActive = false;
    }
    IEnumerator PlaceTrapCoroutine()
    {
        coroutineActive = true;
        animator.SetTrigger("IsAttacking");
        yield return new WaitForSeconds(attackAnimTime / 3);
        Instantiate(equippedTrapPrefab, trapSpawnTransform.position, trapSpawnTransform.rotation);
        trapCount--;
        coroutineActive = false;
    }

    public void EquipTrap(GameObject trapPrefab)
    {
        equippedTrapPrefab = trapPrefab;
        var trap = trapPrefab.GetComponent<Trap>();
        trapCount = trap.GetTrapCount();
        AddGrocery(trap);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
        }
        var weaponGO = Instantiate(weaponPrefab, weaponTransform);
        weapon = weaponGO.GetComponent<Weapon>();
        weapon.SetShopperTransform(transform);
        AddGrocery(weapon);
    }

    public void GetPushed(Vector3 force)
    {
        rb.velocity = force;
    }

    void AddGrocery(IGrocery grocery)
    {
        var groceryName = grocery.GetName();
        if (groceryName != GroceryName.Default)
        {
            if (groceries.ContainsKey(groceryName))
            {
                groceries[groceryName]++;
            }
            else
            {
                groceries[groceryName] = 1;
            }
            OnGroceriesChanged(groceries);
        }
    }

    public abstract void Stall(float speedReduction, float duration);
    public abstract void Slip(Vector3 force, float duration);


}