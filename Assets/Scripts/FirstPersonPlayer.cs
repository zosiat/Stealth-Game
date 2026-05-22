using UnityEngine;

public class FirstPersonPlayer : MonoBehaviour
{
    [Header("movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;

    [Header("mouse")]
    public float mouseSensitivity = 2f;
    public Transform cameraHolder;

    [Header("crouch")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;

    private CharacterController controller;
    private float verticalLookRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        LookAround();
        HandleCrouch();
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float currentSpeed = walkSpeed;

        //sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }

        //crouch speed
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
        {
            currentSpeed = crouchSpeed;
        }

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);

        cameraHolder.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
        {
            controller.height = crouchingHeight;

            cameraHolder.localPosition = new Vector3(0, 0.2f, 0);
        }
        else
        {
            controller.height = standingHeight;

            cameraHolder.localPosition = new Vector3(0, 0.6f, 0);
        }
    }
}