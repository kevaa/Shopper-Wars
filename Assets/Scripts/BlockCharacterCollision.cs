using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCharacterCollision : MonoBehaviour
{
    [SerializeField] Collider characterCollider;
    Collider collisionBlockerCollider;
    void Start()
    {
        collisionBlockerCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(characterCollider, collisionBlockerCollider);
    }

}
