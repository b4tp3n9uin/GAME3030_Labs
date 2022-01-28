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
    public GameObject followTransform;

    // Movement References
    Vector2 LookInput = Vector2.zero;
    Vector2 InputMovement = Vector2.zero;
    Vector3 PlayerDirection = Vector3.zero;
    Animator playerAnim;

    public float aimSensitivity = 1.0f;

    public readonly int MovementXHash = Animator.StringToHash("MovementX");
    public readonly int MovementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    public readonly int isRunningHash = Animator.StringToHash("IsRunning");
    public readonly int isFiringHash = Animator.StringToHash("IsFiring");
    public readonly int isReloadingHash = Animator.StringToHash("IsReloading");

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
        AdjustTransformCamera();
        MovementCheck();
    }

    void AdjustTransformCamera()
    {
        // X-Axis Rotation
        followTransform.transform.rotation *= Quaternion.AngleAxis(LookInput.x * aimSensitivity, Vector3.up);
        // Y-Axis Rotation
        followTransform.transform.rotation *= Quaternion.AngleAxis(LookInput.y * aimSensitivity, Vector3.left);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        followTransform.transform.localEulerAngles = angles;

        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
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

    public void OnLook(InputValue i_value)
    {
        LookInput = i_value.Get<Vector2>();

        //if aim up down, adjust animations to have a mask and properly animate.
    }

    public void OnAim(InputValue i_value)
    {
        playerController.isAiming = i_value.isPressed;
        //animations
    }

    public void OnReload(InputValue i_value)
    {
        playerController.isReloading = i_value.isPressed;

        // Set up reload animations.
        playerAnim.SetBool(isReloadingHash, playerController.isReloading);
    }

    public void OnFire(InputValue i_value)
    {
        playerController.isFiring = i_value.isPressed;

        // Set up firing animations.
        playerAnim.SetBool(isFiringHash, playerController.isFiring);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !playerController.isJumping)
            return;

        playerController.isJumping = false;
        playerAnim.SetBool(isJumpingHash, playerController.isJumping);
    }
}
