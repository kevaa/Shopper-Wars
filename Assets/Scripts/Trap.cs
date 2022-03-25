using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]

public abstract class Trap : MonoBehaviour, IGrocery
{
    [SerializeField] int trapTriggerMax; // number of times trap can be triggered before being disappearing
    [SerializeField] int trapCount;
    int trapTriggerCount = 0;
    bool hitSomething;
    [SerializeField] AudioClip setSound;
    [SerializeField] AudioClip triggerSound;
    AudioSource audioSource;
    [SerializeField] GroceryName groceryName;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public event Action OnHitOtherPlayer = delegate { };
    public int GetTrapCount()
    {
        return trapCount;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!hitSomething)
        {
            hitSomething = true;

            var shopper = other.GetComponent<Shopper>();
            if (trapTriggerCount < trapTriggerMax && shopper != null)
            {
                trapTriggerCount++;
                audioSource.PlayOneShot(triggerSound, .25f);
                Impede(shopper);
                OnHitOtherPlayer();
                if (trapTriggerCount >= trapTriggerMax - 1)
                {
                    Destroy(this.gameObject, .5f);
                }
            }

            hitSomething = false;
        }
    }

    // How trap affects player implemented by subclass
    protected abstract void Impede(Shopper shopper);

    void OnEnable()
    {
        audioSource.PlayOneShot(setSound, .25f);
    }

    public GroceryName GetName()
    {
        return groceryName;
    }
}
