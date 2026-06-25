using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlHub : MonoBehaviour
{
    //Input System configuration
    private PlayerInputs playerInputs;

    public event Action<Vector2> OnMoveActive;
    public event Action OnMoveStop;
    public event Action OnShootActive;
    public event Action OnShootStop;
    public event Action OnJumpActive;
    public event Action OnJumpStop;

    public event Action<Vector2> OnMouseAimActive;
    public event Action OnMouseAimStop;

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        if (playerInputs == null)
            playerInputs = new PlayerInputs();

        playerInputs.Enable();
        playerInputs.PlayerGround.Movement.performed += OnMovementStart;
        playerInputs.PlayerGround.Movement.canceled += OnMovementStop;
        playerInputs.PlayerGround.Shoot.performed += OnShootStart;
        playerInputs.PlayerGround.Shoot.canceled += OnShotStop;
        playerInputs.PlayerGround.Jump.performed += OnJumpPerformed;
        playerInputs.PlayerGround.Jump.canceled += OnJumpCancelled;

        playerInputs.PlayerGround.CameraAim.performed += OnCameraAimPerform;
        playerInputs.PlayerGround.CameraAim.canceled += OnCamerAimStop;
    }

    private void OnCamerAimStop(InputAction.CallbackContext obj)
    {
        OnMouseAimStop?.Invoke();
    }

    private void OnCameraAimPerform(InputAction.CallbackContext obj)
    {
        OnMouseAimActive?.Invoke(obj.ReadValue<Vector2>());
    }

    private void OnJumpCancelled(InputAction.CallbackContext obj)
    {
        OnJumpStop?.Invoke();
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        OnJumpActive?.Invoke();
    }

    private void OnShotStop(InputAction.CallbackContext obj)
    {
        OnShootStop?.Invoke();
    }

    private void OnShootStart(InputAction.CallbackContext obj)
    {
        OnShootActive?.Invoke();
    }

    private void OnMovementStop(InputAction.CallbackContext obj)
    {
        OnMoveStop?.Invoke();
    }
    private void OnMovementStart(InputAction.CallbackContext obj)
    {
        Vector2 val = obj.ReadValue<Vector2>();
        OnMoveActive?.Invoke(val);
    }


    private void OnDisable()
    {
        if (playerInputs == null)
            return;

        playerInputs.Disable();
        playerInputs.PlayerGround.Movement.performed -= OnMovementStart;
        playerInputs.PlayerGround.Movement.canceled -= OnMovementStop;
        playerInputs.PlayerGround.Shoot.performed -= OnShootStart;
        playerInputs.PlayerGround.Shoot.canceled -= OnShotStop;
        playerInputs.PlayerGround.Jump.performed -= OnJumpPerformed;
        playerInputs.PlayerGround.Jump.canceled -= OnJumpCancelled;
    }

}
