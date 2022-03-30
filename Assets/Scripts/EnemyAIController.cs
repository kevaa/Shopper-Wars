using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIController : Shopper
{
    public event Action OnEquipped = delegate { };

    public event Action<int> OnTrapEquipped = delegate { };
    public event Action<GroceryName> OnFoundAll = delegate { };

    NavMeshAgent navMeshAgent;
    float randomDestDistance = 30f;
    [SerializeField] float defaultMoveSpeed;

    Transform target;
    AudioSource audioSource;
    float slipAnimTime = 2f;

    bool debuffed;

    enum state {Wander, Seek}
    state currentState = state.Wander;
    Pickup focus = null;

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

    }
    protected override void Start()
    {
        base.Start();
        animator.SetBool("IsWalking", true);
        navMeshAgent.speed = defaultMoveSpeed;
        navMeshAgent.SetDestination(GetRandomWanderPos(randomDestDistance, -1));
        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    void OnGameEnd()
    {
        audioSource.Stop();
    }
    void Update()
    {
        HandleWalkingAnim();

        var ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 0.2f))
        {   
            if(weapon.GetName() != GroceryName.Default)
            {
                Push();
            }
        }

        if (currentState == state.Wander)
        {
            WanderState();
        }
        if(currentState == state.Seek)
        {
            SeekState();
        }
    }

    void HandleWalkingAnim()
    {
        if (navMeshAgent.velocity.magnitude > 0f)
        {
            animator.SetBool("IsWalking", true);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
            audioSource.Stop();

        }
    }
    void WanderState()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(GetRandomWanderPos(randomDestDistance, -1));
        }
    }
    void SeekState()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            var item = focus.PickupItem();
            var groceryName = focus.GetGroceryName();
            focus.gameObject.SetActive(false);
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
            currentState = state.Wander;
        }
    }

    public Vector3 GetRandomWanderPos(float dist, int layermask)
    {
        var randomSurroundingPos = transform.position + (UnityEngine.Random.insideUnitSphere * dist);
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomSurroundingPos, out navMeshHit, dist, layermask);

        return navMeshHit.position;
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
        navMeshAgent.speed *= speedReduction;
        yield return new WaitForSeconds(duration);
        navMeshAgent.speed = defaultMoveSpeed;
        debuffed = false;
    }

    public override void Slip(Vector3 force, float dur, bool fall)
    {
        StartCoroutine(SlipCoroutine(force, dur, fall));
    }

    IEnumerator SlipCoroutine(Vector3 force, float dur, bool fall)
    {
        debuffed = true;
        navMeshAgent.speed = 0f;
        rb.velocity = transform.TransformDirection(force);
        if (fall)
        {
            yield return new WaitForSeconds(.25f);
            animator.SetBool("Slip", true);
        }
        yield return new WaitForSeconds(dur);
        if (fall)
        {
            animator.SetBool("Slip", false);
            yield return new WaitForSeconds(slipAnimTime);
        }
        navMeshAgent.speed = defaultMoveSpeed;
        debuffed = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Pickup item = collider.GetComponent<Pickup>();
        if (item != null)
        {
            if (NeedGrocery(item.GetGroceryName()))
            {
                navMeshAgent.SetDestination(item.GetComponent<Transform>().position);
                focus = item;
                currentState = state.Seek;
            }
        }
    }
}