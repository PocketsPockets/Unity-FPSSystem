using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSBaseController : MonoBehaviour
{
    #region Variables
    protected CharacterController charController;
    [SerializeField]
    Camera camera;
    float moveVertical;
    float moveHorizontal;
    //X axis rotation for mouse look
    float rotationX;
    //Y axis rotation for mouse look
    float rotationY;
    float verticalVelocity;
    #endregion

    #region Public
    //Vertical mouse sensitivity
    public float verticalSensitivity;
    //Horizontal mouse sensitivity
    public float horizontalSensitivity;
    //Gravity multiplier, lower values will result in a greater gravitational pull downwards
    //default to -15 as I feel this produces the best result
    public float gravity = -15f;
    //Greater values equals a stronger jump, 1.5 by default which produces a semi strong jump
    public float jumpForce = 1.5f;
    //The character controllers slope limit when grounded, default to 60
    public float groundedSlopeLimit = 60f;
    //Can the player crouch or not? True by default
    public bool canCrouch = true;
    //Can the player jump or not? True by default
    public bool canJump = true;
    //Is the player crouched or not?
    public bool isCrouch;
    #endregion

    #region Monobehavior
    void Awake()
    {
        charController = GetComponent<CharacterController>();
    }
    #endregion

    #region Class Methods
    //Handles the basic movement of the player using Unity's standard input system
    public void PlayerMovement(float speed)
    {
        //Standard fps movement input using WASD
        moveVertical = Input.GetAxis("Vertical") * speed;
        moveHorizontal = Input.GetAxis("Horizontal") * speed;
        //Mouse look input and clamping 
        rotationX = Input.GetAxisRaw("Mouse X") * horizontalSensitivity;
        rotationY -= Input.GetAxisRaw("Mouse Y") * verticalSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        //Movement
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        Vector3 Jump = new Vector3(0, verticalVelocity, 0);
        movement = movement.normalized * Time.deltaTime * speed;
        transform.Rotate(0, rotationX, 0);
        camera.transform.localRotation = Quaternion.Euler(rotationY, 0, 0);
        movement = transform.rotation * movement;
        //Jumping, crouching will be automatically set to false at the start of a jump; 
        //however the player will still be able to crouch mid jump unless canCrouch is false
        if (Input.GetKey(KeyCode.Space) && verticalVelocity == 0 && canJump)
        {
            verticalVelocity += Mathf.Sqrt(jumpForce * -2f * gravity);
            isCrouch = false;
        }

        charController.Move(movement);
        charController.Move(Jump * Time.deltaTime);
    }
    //Crouch method, sets isCrouch to true, halves the character controller's height, 
    //offsets the controllers center to prevent clipping through floor. Does not effect speed
    public void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isCrouch)
        {
            isCrouch = true;
            charController.height = charController.height / 2f;
            charController.center = Vector3.down * (1 - charController.height) / 8.0f;
        }
        else if (Input.GetKeyDown(KeyCode.C) && isCrouch)
        {
            isCrouch = false;
        }
    }

    public void Gravity()
    {
        if (charController.isGrounded)
            verticalVelocity = 0;
        else
            verticalVelocity += gravity * Time.deltaTime;
        
        if (verticalVelocity > 0.5)
            charController.slopeLimit = 90;
        else 
            charController.slopeLimit = groundedSlopeLimit;
    }
    #endregion
}
