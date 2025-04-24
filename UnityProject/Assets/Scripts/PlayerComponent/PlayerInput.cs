using System;
using UnityEngine;
using Unity.Netcode;
public class PlayerInput : NetworkBehaviour
{
    public event EventHandler OnShootAction;
    
    private PlayerInputActions playerInputActions;

    private bool _gamePaused;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += Shoot_performed;
        EventManager.OnPauseGameEvent += OnGamePause;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        playerInputActions.Player.Shoot.performed -= Shoot_performed;
        EventManager.OnPauseGameEvent -= OnGamePause;
    }
    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!IsOwner)
        {
            return;
        }
        if (_gamePaused) return;
        OnShootAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        
        Vector2 inputVector = playerInputActions.Player.Walk.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    private void OnGamePause(bool pause)
    {
        _gamePaused = pause;
    }
}
