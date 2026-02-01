using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    private CharacterController characterController;

    #region Player Controller Variables

    [Header("Movement")]
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float gravity = 20f;

    [Header("Jump")]
    public float jumpPower = 8f;

    [Header("Mouse Look")]
    public float lookSensitivity = 4.5f;

    private float lookSensitivityMultiplier = 5f;
    public float lookXLimit = 45f;

    #endregion Player Controller Variables

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    private float rotationX = 0;
    private float verticalVelocity = 0;

    /// <summary>
    /// Initializes component references and subscribes to input events.
    /// </summary>
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        // Get input actions from PlayerInput
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];

        // Subscribe to jump event
        // OnJump is called any time the jump action is performed
        jumpAction.performed += OnJump;
    }

    /// <summary>
    /// Locks and hides the cursor for first-person gameplay.
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Called once per frame. Handles player movement and camera rotation.
    /// </summary>
    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    /// <summary>
    /// Handles player movement including WASD input, sprinting, jumping, and gravity.
    /// Moves the character using the CharacterController component.
    /// </summary>
    private void HandleMovement()
    {
        // Read input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        bool isSprinting = sprintAction.IsPressed();

        // Calculate speed
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        // Transform input to world space (horizontal movement only)
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 horizontalMove = (forward * moveInput.y + right * moveInput.x) * currentSpeed;

        // Apply gravity and handle jumping
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            // Small downward force to keep grounded
            verticalVelocity = -2f; 
        }
        else
        {
            // Apply gravity when in air
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Combine horizontal and vertical movement
        Vector3 movement = new Vector3(
            horizontalMove.x * Time.deltaTime,
            verticalVelocity * Time.deltaTime,
            horizontalMove.z * Time.deltaTime
        );

        characterController.Move(movement);
    }

    /// <summary>
    /// Handles mouse input for first-person camera control.
    /// Rotates the player body horizontally and the camera vertically with clamping.
    /// </summary>
    private void HandleMouseLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        // Horizontal rotation (player body)
        transform.Rotate(Vector3.up * lookInput.x * (lookSensitivity * lookSensitivityMultiplier) * Time.deltaTime);

        // Vertical rotation (camera only) with clamping
        rotationX -= lookInput.y * (lookSensitivity * lookSensitivityMultiplier) * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    /// <summary>
    /// Callback invoked when the jump action is performed.
    /// Sets the vertical velocity to jumpPower if the character is grounded.
    /// </summary>
    /// <param name="context">Input action callback context from the Input System.</param>
    private void OnJump(InputAction.CallbackContext context)
    {
        //Vertical velocity for jump is set here, but applied in HandleMovement
        if (characterController.isGrounded)
        {
            verticalVelocity = jumpPower;
        }
    }

    /// <summary>
    /// Cleans up input event subscriptions when the component is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        // Cleanup event subscription
        if (jumpAction != null)
        {
            jumpAction.performed -= OnJump;
        }
    }
}
