using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallTrap : Trap
{
    [SerializeField] float stallDuration;
    [SerializeField] float stallReductionMultiplier;
    protected override void Impede(Shopper shopper)
    {
        shopper.Stall(stallReductionMultiplier, stallDuration);
    }
}
