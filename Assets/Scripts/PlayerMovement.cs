using UnityEngine;

//[RequireComponent(typeof(Animator))]
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
    bool isWalking;
    float actionMovementMultiplier = .01f;
    [SerializeField] Joystick joystick;
    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        mAudioSource = GetComponent<AudioSource>();

    }
    private void Start()
    {
        mAnimator = GetComponentInChildren<Animator>();

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
        var keyboardHorizontal = Input.GetAxis("Horizontal");
        var keyboardVertical = Input.GetAxis("Vertical");

        horizontal = keyboardHorizontal != 0f ? keyboardHorizontal : joystick.Horizontal;
        vertical = keyboardVertical != 0f ? keyboardVertical : joystick.Vertical;
        movement.Set(horizontal, 0f, vertical);
        movement.Normalize();
        isWalking = player.moveSpeed != 0 && IsWalking(horizontal, vertical);
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
        if (isWalking && movement != Vector3.zero)
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
