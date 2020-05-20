using UnityEngine;

public class FPSPlayer : FPSBaseController
{
    //FPSController example using the FPSBaseController class
    #region Variables
    public float currentSpeed;
    public float baseSpeed;
    public float playerHeight;
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
        if (isCrouch)
        {
            currentSpeed = baseSpeed / 4f;
        }
        else currentSpeed = baseSpeed;

        if(charController.height < playerHeight && !isCrouch)
        {
            charController.height = charController.height + 10.0f * Time.deltaTime;
            charController.center = new Vector3(0, 0, 0);
        }
        if(charController.height > playerHeight)
        {
            charController.height = playerHeight;
        }
    }

    void FixedUpdate()
    {
        Gravity();
    }
    #endregion
}
