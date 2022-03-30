using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player : Shopper
{
    public event Action OnEquipped = delegate { };

    public event Action<int> OnTrapEquipped = delegate { };
    public event Action<GroceryName> OnFoundAll = delegate { };

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

    public GameObject graphicsChild;

    GameObject skin;

    protected override void Awake()
    {
        base.Awake();
        skin = Instantiate(SkinManager.Instance.selectedSkin, graphicsChild.transform);

        skin.transform.localPosition = Vector3.zero;
        skin.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    protected override void Start()
    {
        Transform weaponHolder = skin.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/R_hand_container/WeaponHolder");
        if (weaponHolder != null)
        {
            weaponTransform = weaponHolder;
        }
        else Debug.Log("didn't find a weapon holder");

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
                var groceryName = currentPickup.GetGroceryName();
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

                    if (!NeedGrocery(groceryName))
                    {
                        OnFoundAll(groceryName);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var pickup = other.GetComponent<Pickup>();
        if (pickup != null && NeedGrocery(pickup.GetGroceryName()))
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

    public override void Slip(Vector3 force, float duration, bool fall)
    {
        if (!debuffed)
        {
            StartCoroutine(SlipCoroutine(force, duration, fall));
        }
    }

    IEnumerator SlipCoroutine(Vector3 force, float duration, bool fall)
    {
        debuffed = true;
        moveSpeed = 0f;
        rb.velocity = transform.TransformVector(force);
        if (fall)
        {
            yield return new WaitForSeconds(.25f);
            animator.SetBool("Slip", true);
        }
        yield return new WaitForSeconds(duration != 0 ? duration : .1f);
        if (fall)
        {
            animator.SetBool("Slip", false);
            yield return new WaitForSeconds(slipAnimTime);
        }
        moveSpeed = defaultMoveSpeed;
        debuffed = false;
    }
}
