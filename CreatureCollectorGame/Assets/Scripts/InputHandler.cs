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
        public bool isInteracting;

        private PlayerControls inputActions;
        public CameraHandler cameraHandler;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        private bool jumpTrigger;

        private void Start()
        {
            cameraHandler = CameraHandler.singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if(cameraHandler != null)
            {
                cameraHandler.followTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            }
        }

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
            if (inCombat) { HandleDodgeInput(delta); }
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
            dodgeFlag = inputActions.PlayerCombatActions.Dodge.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        }

        private void HandleSprintInput(float delta)
        {
            sprintFlag = inputActions.PlayerMovement.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        }
    }
}

