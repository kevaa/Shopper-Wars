using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class Player : Shopper
{
    public event Action OnEquipped = delegate { };

    public event Action<int> OnTrapEquipped = delegate { };
    public event Action OnTrapPlaced = delegate { };

    public event Action OnEnteredPickupRadius = delegate { };
    public event Action OnLeftPickupRadius = delegate { };
    bool stalled = false;
    public float moveSpeed { get; private set; }
    [SerializeField] float defaultMoveSpeed;

    protected override void Start()
    {
        base.Start();
        moveSpeed = defaultMoveSpeed;

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Push();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (trapCount > 0 && !coroutineActive)
            {
                OnTrapPlaced();
            }
            PlaceTrap();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentPickup != null)
            {
                var item = currentPickup.PickupItem();
                currentPickup.gameObject.SetActive(false);
                if (item != null)
                {
                    OnEquipped();
                    var trap = item.GetComponent<Trap>();
                    var weapon = item.GetComponent<Weapon>();
                    if (trap != null)
                    {
                        EquipTrap(item);
                        OnTrapEquipped(trap.GetTrapCount());
                    }
                    else if (weapon != null)
                    {
                        EquipWeapon(item);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var pickup = other.GetComponent<Pickup>();
        if (pickup != null)
        {
            currentPickup = pickup;
            OnEnteredPickupRadius();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var pickup = other.GetComponent<Pickup>();
        if (pickup != null)
        {
            currentPickup = null;
            OnLeftPickupRadius();
        }
    }

    public override void Stall(float speedReduction, float duration)
    {
        if (!stalled)
        {
            StartCoroutine(StallCoroutine(speedReduction, duration));
        }
    }


    IEnumerator StallCoroutine(float speedReduction, float duration)
    {
        stalled = true;
        moveSpeed *= speedReduction;
        yield return new WaitForSeconds(duration);
        moveSpeed = defaultMoveSpeed;
        stalled = false;
    }
}
