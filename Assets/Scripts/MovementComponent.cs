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

    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponent<PlayerControls>();
        rb = GetComponent<Rigidbody>();
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
    }

    public void OnRun(InputValue i_value)
    {
        playerController.isRunning = i_value.isPressed;
    }

    public void OnJump(InputValue i_value)
    {
        if (playerController.isJumping)
            return;

        playerController.isJumping = true;
        rb.AddForce((transform.up + PlayerDirection) * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !playerController.isJumping)
            return;

        playerController.isJumping = false;
    }
}
