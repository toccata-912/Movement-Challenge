using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField]
    Movement movement;
    [SerializeField]
    MouseLook mouseLook;


    public PlayerControls controls;
    PlayerControls.GroundMovementActions groundMovement;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        controls = new PlayerControls();
        groundMovement = controls.GroundMovement;


        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.Jump.performed += _ => movement.OnJumpPressed();
        groundMovement.Crouch.performed += _ => movement.OnCrouchPressed();
        groundMovement.Crouch.canceled += _ => movement.OnCrouchCancelled();

        groundMovement.Sprint.performed += _ => movement.OnSprintPressed();
        //groundMovement.Sprint.canceled += _ => movement.OnSprintReleased();
        groundMovement.Jump.canceled += _ => movement.OnJumpCancelled();
        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

    }

    private void Update()
    {
        movement.RecieveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
