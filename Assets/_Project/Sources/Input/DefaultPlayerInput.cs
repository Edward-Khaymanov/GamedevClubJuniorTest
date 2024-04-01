using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ClubTest
{
    public class DefaultPlayerInput : IPlayerInput
    {
        private readonly PlayerInputActions _actions;
        private InputActionMap _currentActionMap;
        private InputAction _moveAction;

        public event Action<InputAction.CallbackContext> Shoot;
        public event Action InventoryOpen;
        public event Action InventoryClose;

        public DefaultPlayerInput()
        {
            _actions = new PlayerInputActions();
            _currentActionMap = _actions.Game.Get();
            SetActions();
        }

        public Vector2 MoveDirection => _moveAction == null ? Vector2.zero : _moveAction.ReadValue<Vector2>();

        public void Enable()
        {
            Subscribe();
            _currentActionMap.Enable();
        }

        public void Disable()
        {
            _currentActionMap.Disable();
            Unsubscribe();
        }

        private void SwitchActionMap(InputActionMap actionMap)
        {
            if (_currentActionMap == actionMap)
                return;

            Disable();
            _currentActionMap = actionMap;
            SetActions();
            Enable();
        }

        private void Subscribe()
        {
            _actions.Game.Shoot.started += InvokeShoot;
            _actions.Game.Shoot.canceled += InvokeShoot;
            _actions.Game.OpenInventory.performed += OpenInventory;
            _actions.Inventory.CloseInventory.performed += CloseInventory;
        }

        private void Unsubscribe()
        {
            _actions.Game.Shoot.started -= InvokeShoot;
            _actions.Game.Shoot.canceled -= InvokeShoot;
            _actions.Game.OpenInventory.performed -= OpenInventory;
            _actions.Inventory.CloseInventory.performed -= CloseInventory;
        }

        private void SetActions()
        {
            _moveAction = _currentActionMap.FindAction("Move");
        }

        private void InvokeShoot(InputAction.CallbackContext ctx)
        {
            Shoot?.Invoke(ctx);
        }

        private void OpenInventory(InputAction.CallbackContext ctx = default)
        {
            InventoryOpen?.Invoke();
            SwitchActionMap(_actions.Inventory.Get());
        }

        private void CloseInventory(InputAction.CallbackContext ctx = default)
        {
            InventoryClose?.Invoke();
            SwitchActionMap(_actions.Game.Get());
        }

    }
}