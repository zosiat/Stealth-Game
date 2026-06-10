using System.Collections;
using UnityEngine;
using TMPro;

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

    [Header("hide ability")]
    public float hideDuration = 3f;

    [Header("UI")]
    public TMP_Text hideMessageText;

    private CharacterController controller;
    private float verticalLookRotation;
    private bool isCrouching;
    private bool isHidden;
    private bool hideUsed;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (hideMessageText != null)
        {
            hideMessageText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        MovePlayer();
        LookAround();
        HandleCrouch();
        HandleHideAbility();
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float currentSpeed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }

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

        cameraHolder.localRotation =
            Quaternion.Euler(verticalLookRotation, 0, 0);
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
        {
            isCrouching = true;

            controller.height = crouchingHeight;
            cameraHolder.localPosition =
                new Vector3(0, 0.2f, 0);
        }
        else
        {
            isCrouching = false;

            controller.height = standingHeight;
            cameraHolder.localPosition =
                new Vector3(0, 0.6f, 0);
        }
    }

    void HandleHideAbility()
    {
        if (Input.GetKeyDown(KeyCode.E) && !hideUsed)
        {
            StartCoroutine(HideForSeconds());
        }
    }

    IEnumerator HideForSeconds()
    {
        hideUsed = true;
        isHidden = true;

        if (hideMessageText != null)
        {
            hideMessageText.text = "HIDDEN!";
            hideMessageText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(hideDuration);

        isHidden = false;

        if (hideMessageText != null)
        {
            hideMessageText.text = "Hide Ability Used";
        }

        yield return new WaitForSeconds(2.5f);

        if (hideMessageText != null)
        {
            hideMessageText.gameObject.SetActive(false);
        }
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }

    public bool IsHidden()
    {
        return isHidden;
    }

    public void SetHidden(bool hidden)
    {
        isHidden = hidden;
    }
}