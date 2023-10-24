using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerFirstPerson : MonoBehaviour
{

    public static PlayerControllerFirstPerson instance;

    // Using Unity CharacterController
    CharacterController characterController;

    // movement, sensitivity values
    public float walkSpeed = 7f, runSpeed = 11f, jumpSpeed = 7.5f, gravity = 20f;
    public float mouseSens = 2f, mouseVerticalLimit = 70f;
    public bool canMove = true;

    // movement Vector and Rotation
    private Vector3 moveDir = Vector3.zero;
    private float xRotation = 0;

    // Camera OBJ
    [SerializeField] Camera playerCam;



    // before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        }

       characterController = GetComponent<CharacterController>();
        LockCursor(true);
    }

    // once per frame
    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // if can move and running else walking
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        float movementDirectionY = moveDir.y;
        moveDir = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded) {
            moveDir.y = jumpSpeed;
        } else {
            moveDir.y = movementDirectionY;
        }

        // Apply gravity. applied as an acceleration (ms^-2)
        if (!characterController.isGrounded) {
            moveDir.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDir * Time.deltaTime);

        // Player and Camera rotation
        if (canMove) {
            // sensitivity
            xRotation += -Input.GetAxis("Mouse Y") * mouseSens;
            // vertical limits
            xRotation = Mathf.Clamp(xRotation, -mouseVerticalLimit , mouseVerticalLimit);

            playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * mouseSens, 0);
        }
    }

    public void LockCursor(bool isLocked) {
        if (isLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canMove = true;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canMove = false;
        }
    }
}
