using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] public bool InvertMouseY = false;

    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private PlayerController playerController;


    [SerializeField] public Vector2 lookInput;
    [SerializeField] public Vector2 movementInput;
 


    private void Awake()
    {
        playerControls = new PlayerControls();
        playerController = GetComponent<PlayerController>();
   
    }

    private void Update()
    {
        lookInput = playerControls.OnFoot.Look.ReadValue<Vector2>();
        movementInput = playerControls.OnFoot.Movement.ReadValue<Vector2>();
    }


    
    private void OnEnable()
    {
        playerControls.Enable();

 

        playerControls.OnFoot.Sprint.started += context => playerController.HandleSprinting(context);

        playerControls.OnFoot.Sprint.canceled += context => playerController.ResetMovementSpeed(context);

        playerControls.OnFoot.Jump.started += context => playerController.HandleJump(context);

        playerControls.OnFoot.Crouch.started += context => playerController.HandleCrouch(context);

        playerControls.OnFoot.Crouch.canceled += context => playerController.ResetMovementSpeed(context);

        playerControls.OnFoot.Crouch.canceled += context => playerController.ResetPlayerHeight(context);

        playerControls.OnFoot.Fire.started += context => playerController.PlayerFire(context);

        playerControls.OnFoot.Reload.started += context => playerController.PlayerReload(context);

    
    }


    private void OnDisable()
    {
        playerControls.Disable();

    

        playerControls.OnFoot.Sprint.started -= context => playerController.HandleSprinting(context);

        playerControls.OnFoot.Sprint.canceled -= context => playerController.ResetMovementSpeed(context);

        playerControls.OnFoot.Jump.started -= context => playerController.HandleJump(context);

        playerControls.OnFoot.Crouch.started -= context => playerController.HandleCrouch(context);

        playerControls.OnFoot.Crouch.canceled -= context => playerController.ResetMovementSpeed(context);

        playerControls.OnFoot.Crouch.canceled -= context => playerController.ResetPlayerHeight(context);

        playerControls.OnFoot.Fire.started -= context => playerController.PlayerFire(context);

        playerControls.OnFoot.Reload.started -= context => playerController.PlayerReload(context);

       

      

       

 
    }
}