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

            //Player Locomotion
            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.DodgeMovement(playerLocomotion.dodgeDir, delta);
            playerLocomotion.HandleDodge(delta);
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
        }
    }
}
