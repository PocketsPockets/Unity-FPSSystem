using UnityEngine;

public class ModernFPSPlayer : FPSBaseController
{
    //A more modern FPS controller example that supports sprinting using the FPSBaseController class
    #region Variables
    //The speed the player will move at on input
    public float currentSpeed;
    //currentSpeed will return this when not sprinting or crouched
    public float baseSpeed;
    //currentSpeed will raise to this speed when sprinting
    public float maximumSpeed;
    public float playerHeight;
    //How fast the player picks up speed when sprinting, default to 10
    public float sprintAcceleration = 10f;
    //Is the player sprinting or not?
    bool isSprint;
    #endregion

    #region Monobehavior
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        PlayerMovement(currentSpeed);
        Crouch();

        if (Input.GetKey(KeyCode.LeftShift))
            Sprint();
        else
            isSprint = false;
        if (!isSprint && !isCrouch)
            currentSpeed = baseSpeed;

        if (isCrouch)
            currentSpeed = baseSpeed / 4f;
        if (charController.height < playerHeight && !isCrouch)
        {
            charController.height = charController.height + 10.0f * Time.deltaTime;
            charController.center = new Vector3(0, 0, 0);
        }
        if (charController.height > playerHeight)
            charController.height = playerHeight;
        
    }

    void FixedUpdate()
    {
        Gravity();
    }
    #endregion

    #region Other
    void Sprint()
    {
        if(isCrouch)
        {
            return;
        }
        isSprint = true;
        currentSpeed += sprintAcceleration * Time.deltaTime;
        if (currentSpeed > maximumSpeed)
            currentSpeed = maximumSpeed;
    }
    #endregion
}
