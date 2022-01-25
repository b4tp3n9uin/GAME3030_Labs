using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    // variables for movement speed
    [SerializeField]
    float walkSpeed = 5;
    [SerializeField]
    float runSpeed = 10;
    [SerializeField]
    float jumpForce = 5;

    // PlayerControls class reference
    PlayerControls playerController;
    Rigidbody rb;

    // Movement References
    Vector2 InputMovement = Vector2.zero;
    Vector3 PlayerDirection = Vector3.zero;
    Animator playerAnim;

    public readonly int MovementXHash = Animator.StringToHash("MovementX");
    public readonly int MovementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    public readonly int isRunningHash = Animator.StringToHash("IsRunning");

    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponent<PlayerControls>();
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementCheck();
    }

    void MovementCheck()
    {
        if (playerController.isJumping)
            return;

        if (!(InputMovement.magnitude > 0))
            PlayerDirection = Vector3.zero;

        PlayerDirection = transform.forward * InputMovement.y + transform.right * InputMovement.x;
        float currentSpd = playerController.isRunning ? runSpeed : walkSpeed;

        Vector3 movementDirection = PlayerDirection * (currentSpd * Time.deltaTime);

        transform.position += movementDirection;
    }

    // Action Functions for Move, Run, n Jump
    public void OnMovement(InputValue i_value)
    {
        InputMovement = i_value.Get<Vector2>();
        playerAnim.SetFloat(MovementXHash, InputMovement.x);
        playerAnim.SetFloat(MovementYHash, InputMovement.y);
    }

    public void OnRun(InputValue i_value)
    {
        playerController.isRunning = i_value.isPressed;
        playerAnim.SetBool(isRunningHash, playerController.isRunning);
    }

    public void OnJump(InputValue i_value)
    {
        if (playerController.isJumping)
            return;

        playerController.isJumping = true;
        rb.AddForce((transform.up + PlayerDirection) * jumpForce, ForceMode.Impulse);

        playerAnim.SetBool(isJumpingHash, playerController.isJumping);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !playerController.isJumping)
            return;

        playerController.isJumping = false;
        playerAnim.SetBool(isJumpingHash, playerController.isJumping);
    }
}
