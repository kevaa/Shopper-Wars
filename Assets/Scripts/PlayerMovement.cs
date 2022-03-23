using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    Vector3 movement;
    Quaternion rotation = Quaternion.identity;
    Rigidbody rb;
    AudioSource mAudioSource;
    bool gameEnded = false;
    [SerializeField] float rotSpeed = 10f;

    Animator mAnimator;

    Player player;
    float actionMovementMultiplier = .1f;
    [SerializeField] Joystick joystick;
    private void Awake()
    {
        player = GetComponent<Player>();
        mAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mAudioSource = GetComponent<AudioSource>();

    }
    private void Start()
    {
        GameManager.Instance.OnGameEnd += OnGameEnd;
    }

    void OnGameEnd()
    {
        gameEnded = true;
        mAudioSource.Stop();
    }

    void Update()
    {
        var horizontal = 0f;
        var vertical = 0f;
        if (gameEnded)
        {
            return;
        }
        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;
        movement.Set(horizontal, 0f, vertical);
        movement.Normalize();
        var isWalking = IsWalking(horizontal, vertical);
        mAnimator.SetBool("IsWalking", isWalking);
        if (isWalking)
        {
            if (!mAudioSource.isPlaying)
            {
                mAudioSource.Play();
            }
        }
        else
        {
            mAudioSource.Stop();
        }
    }
    private void FixedUpdate()
    {
        if (movement != Vector3.zero)
        {
            rb.MovePosition(rb.position + (movement * player.moveSpeed * Time.fixedDeltaTime * (player.coroutineActive ? actionMovementMultiplier : 1)));
            var desiredForward = Vector3.RotateTowards(transform.forward, movement, rotSpeed * Time.fixedDeltaTime, 0f);
            rotation = Quaternion.LookRotation(desiredForward);
            rb.MoveRotation(rotation);
        }
    }

    private bool IsWalking(float horizontal, float vertical)
    {
        var hasVerticalUnput = !Mathf.Approximately(vertical, 0f);
        var hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        return hasHorizontalInput || hasVerticalUnput;
    }
}
