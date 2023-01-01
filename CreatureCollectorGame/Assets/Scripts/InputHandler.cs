using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace player
{
    public class InputHandler : MonoBehaviour
    {
        public bool inCombat;

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool dodgeFlag;
        public bool sprintFlag;
        public bool jumpFlag;

        private PlayerControls inputActions;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        private bool jumpTrigger;

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.Jump.performed += _ => jumpTrigger = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleDodgeInput(delta);
            HandleSprintInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        public bool JumpInput(float delta)
        {
            if(jumpTrigger == true)
            {
                jumpTrigger = false;
                return true;
            }
            return false;
        }

        private void HandleDodgeInput(float delta)
        {
            if (inCombat != true)
                return;

            dodgeFlag = inputActions.PlayerCombatActions.Dodge.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        }

        private void HandleSprintInput(float delta)
        {
            if (inCombat)
                return;

            sprintFlag = inputActions.PlayerMovement.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        }
    }
}

