using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(Animator))]

public abstract class Shopper : MonoBehaviour, IPushable
{
    GameObject equippedTrapPrefab;

    [SerializeField] GameObject defaultWeaponPrefab;
    public int trapCount { get; private set; }
    Dictionary<GroceryName, int> groceriesFound;
    protected Rigidbody rb;
    float attackAnimTime = .6f;
    [SerializeField] float attackAnimMult = 1f;
    [SerializeField] Transform trapSpawnTransform;
    [SerializeField] protected Transform weaponTransform;
    public event Action<Dictionary<GroceryName, int>, Dictionary<GroceryName, int>> OnGroceriesChanged = delegate { };
    Weapon weapon;

    public bool coroutineActive { get; private set; } = false; // When shopper doing action like placing trap/pushing
    protected Animator animator;
    protected Pickup currentPickup;
    protected String shoppername;

    Dictionary<GroceryName, int> groceryList;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groceryList = new Dictionary<GroceryName, int>();
        groceriesFound = new Dictionary<GroceryName, int>();
    }
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();

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
        yield return new WaitForSeconds(attackAnimTime / 3f * attackAnimMult);
        weapon.EnableCol();
        yield return new WaitForSeconds(.2f); // Ensure animator started animation

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }
        weapon.DisableCol();
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
        FoundGrocery(trap);
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
        FoundGrocery(weapon);
        attackAnimMult = weapon.GetAttackAnimMult();
        animator.SetFloat("AttackMult", attackAnimMult);
    }

    public void GetPushed(Vector3 force)
    {
        rb.velocity = force;
    }

    public abstract void Stall(float speedReduction, float duration);
    public abstract void Slip(Vector3 force, float duration, bool fall);

    public void AddToGroceryList(GroceryName groceryName)
    {
        if (groceryList.ContainsKey(groceryName))
        {
            groceryList[groceryName]++;
        }
        else
        {
            groceryList[groceryName] = 1;
            groceriesFound[groceryName] = 0;
        }
    }

    public Dictionary<GroceryName, int> GetGroceriesFound()
    {
        return groceriesFound;
    }
    public void FoundGrocery(IGrocery grocery)
    {
        var groceryName = grocery.GetName();
        if (groceriesFound.ContainsKey(groceryName) && NeedGrocery(groceryName))
        {
            groceriesFound[groceryName]++;
            OnGroceriesChanged(groceriesFound, groceryList);
            // update leaderboard
            updateLeaderboardHelper();
        }
    }


    public bool NeedGrocery(GroceryName groceryName)
    {
        return groceriesFound.ContainsKey(groceryName) ? groceriesFound[groceryName] < groceryList[groceryName] : false;
    }

    public string GetGroceryListString(Dictionary<GroceryName, int> list)
    {
        String s = "";
        foreach (KeyValuePair<GroceryName, int> kvp in groceriesFound)
        {
            //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            s += String.Format("Key = {0}, Value = {1}\n", kvp.Key, kvp.Value);
        }
        return s;
    }

    public void initGroceryList()
    {
        OnGroceriesChanged(groceriesFound, groceryList);
    }

    public String getShopperName()
    {
        return shoppername;
    }

    public void setShopperName(String name)
    {
        shoppername = name;
    }

    private void updateLeaderboardHelper()
    {
        Dictionary<string, int> temp = Spawner.Instance.getLeaderboard();
        int cur_sum = 0;
        foreach (var key in groceriesFound.Keys)
        {
            cur_sum += groceriesFound[key];
        }
        temp[getShopperName()] = cur_sum;
        Spawner.Instance.setLeaderboard(temp);
    }
}