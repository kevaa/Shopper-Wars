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
    [SerializeField] float attackAnimMult = 1f;
    [SerializeField] Transform trapSpawnTransform;
    [SerializeField] Transform weaponTransform;
    public event Action<Dictionary<GroceryName, int>> OnGroceriesChanged = delegate { };
    Weapon weapon;

    public bool coroutineActive { get; private set; } = false; // When shopper doing action like placing trap/pushing
    protected Animator animator;
    protected Pickup currentPickup;

    Dictionary<GroceryName, bool> groceryList;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        groceryList = new Dictionary<GroceryName, bool>();
        groceries = new Dictionary<GroceryName, int>();
    }
    protected virtual void Start()
    {
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
            coroutineActive = true;
            StartCoroutine(PushCoroutine());
        }
    }

    IEnumerator PushCoroutine()
    {
        animator.SetTrigger("IsAttacking");
        Debug.Log("attacking");
        yield return new WaitForSeconds(attackAnimTime / 3f * attackAnimMult);
        weapon.EnableCol();
        yield return new WaitForSeconds(.2f); // Ensure animator started animation

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }
        weapon.DisableCol();
        Debug.Log("done");
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
        weapon.SetShopper(this);
        AddGrocery(weapon);
        attackAnimMult = weapon.GetAttackAnimMult();
        animator.SetFloat("AttackMult", attackAnimMult);
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
    public abstract void Slip(Vector3 force, float duration, bool fall);

    public void AddToGroceryList(GroceryName groceryName)
    {
        groceryList[groceryName] = false;
    }
    public void FoundGrocery(GroceryName groceryName)
    {
        if (groceryList.ContainsKey(groceryName) && !groceryList[groceryName])
        {
            groceryList[groceryName] = true;
            // TODO update grocery list UI
        }
    }
}