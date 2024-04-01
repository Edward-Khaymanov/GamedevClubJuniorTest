using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ClubTest
{
    public interface IPlayerInput
    {
        public event Action<InputAction.CallbackContext> Shoot;

        public event Action InventoryOpen;
        public event Action InventoryClose;

        public Vector2 MoveDirection { get; }

        public void Enable();
        public void Disable();
    }
}