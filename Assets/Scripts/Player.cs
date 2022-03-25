using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player : Shopper
{
    public event Action OnEquipped = delegate { };

    public event Action<int> OnTrapEquipped = delegate { };
    public event Action OnTrapPlaced = delegate { };

    public event Action OnEnteredPickupRadius = delegate { };
    public event Action OnLeftPickupRadius = delegate { };
    bool debuffed = false;
    public float moveSpeed { get; private set; }
    [SerializeField] float defaultMoveSpeed;

    [SerializeField] ButtonState attackButton;
    [SerializeField] ButtonState equipButton;
    [SerializeField] ButtonState setTrapButton;
    float slipAnimTime = 2f;

    protected override void Start()
    {
        base.Start();
        moveSpeed = defaultMoveSpeed;

    }
    void Update()
    {
        if (attackButton.pressed || Input.GetKeyDown(KeyCode.P))
        {
            Push();
        }
        else if (setTrapButton.pressed || Input.GetKeyDown(KeyCode.O))
        {
            if (trapCount > 0 && !coroutineActive)
            {
                OnTrapPlaced();
            }
            PlaceTrap();
        }
        if (equipButton.pressed || Input.GetKeyDown(KeyCode.E))
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
        if (!debuffed)
        {
            StartCoroutine(StallCoroutine(speedReduction, duration));
        }
    }

    IEnumerator StallCoroutine(float speedReduction, float duration)
    {
        debuffed = true;
        moveSpeed *= speedReduction;
        yield return new WaitForSeconds(duration);
        moveSpeed = defaultMoveSpeed;
        debuffed = false;
    }

    public override void Slip(Vector3 force, float duration)
    {
        if (!debuffed)
        {
            StartCoroutine(SlipCoroutine(force, duration));
        }
    }

    IEnumerator SlipCoroutine(Vector3 force, float duration)
    {
        debuffed = true;
        moveSpeed = 0f;
        rb.velocity = transform.TransformVector(force);
        yield return new WaitForSeconds(.25f);
        animator.SetBool("Slip", true);
        yield return new WaitForSeconds(duration != 0 ? duration : .1f);
        animator.SetBool("Slip", false);
        yield return new WaitForSeconds(slipAnimTime);
        moveSpeed = defaultMoveSpeed;
        debuffed = false;
    }
}
