using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipTrap : Trap
{
    [SerializeField] float duration;
    [SerializeField] Vector3 force;
    [SerializeField] bool fall;


    protected override void Impede(Shopper shopper)
    {
        shopper.Slip(force, duration, fall);
    }
}
