using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isGrounded;
        public bool isInAir;
        public bool isJumping;

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            cameraHandler = CameraHandler.singleton;
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            //Input Handler
            isInteracting = anim.GetBool("isInteracting");
            isJumping = anim.GetBool("isJumping");

            //Player Locomotion
            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.DodgeMovement(playerLocomotion.dodgeDir, delta);
            playerLocomotion.HandleDodge(delta);
            playerLocomotion.HandleJumping(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.followTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            isSprinting = inputHandler.sprintFlag;
            inputHandler.dodgeFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.jumpFlag = false;
            

            if(isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }
    }
}
