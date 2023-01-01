using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private PlayerManager playerManager;
        private Transform cameraObject;
        private InputHandler inputHandler;
        [HideInInspector] public Vector3 moveDirection;

        [HideInInspector] public Transform myTransform;
        [HideInInspector] public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        public bool dodging;
        [HideInInspector] public Vector3 dodgeDir;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] private float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] private float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField] private float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer; 

        [Header("Movement Stats")]
        [SerializeField] private float movementSpeed = 5;
        [SerializeField] private float sprintSpeed = 7.5f;
        [SerializeField] public float dodgeSpeed = 16;
        [SerializeField] private float rotationSpeed = 10;
        [SerializeField] private float fallingSpeed = 45;
        [SerializeField] private float jumpPower = 60;

        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement

        public Vector3 normalVector;
        private Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDirection = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDirection = cameraObject.forward * inputHandler.vertical;
            targetDirection += cameraObject.right * inputHandler.horizontal;

            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = myTransform.forward;
            }

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }

        public void HandleMovement(float delta)
        {
            if (playerManager.isInteracting)
                return;

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if(inputHandler.sprintFlag)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                moveDirection *= speed;
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleDodge(float delta)
        {
            if (playerManager.isInteracting)
                return;

            if (inputHandler.dodgeFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                moveDirection.Normalize();
                moveDirection.y = 0;

                if (inputHandler.moveAmount > 0)
                {
                    DodgeMovement dm = animatorHandler.anim.GetBehaviour<DodgeMovement>();
                    if (dm.playerLocomotion == null) { dm.playerLocomotion = this; }
                    //dm.dir = moveDirection;

                    animatorHandler.PlayTargetAnimation("Dodge", true);
                    Quaternion dodgeRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = dodgeRotation;
                    dodgeDir = moveDirection;
                }
                else
                {
                    BackstepMovement bm = animatorHandler.anim.GetBehaviour<BackstepMovement>();
                    if (bm.playerLocomotion == null) { bm.playerLocomotion = this; }
                    //bm.dir = myTransform.forward; 

                    animatorHandler.PlayTargetAnimation("Backstep", true);
                    dodgeDir = -myTransform.forward;
                }
            }
        }

        public void DodgeMovement(Vector3 dir, float delta)
        {
            if (dodging != true)
                return;

            dir.Normalize();
            Debug.Log(dir);

            float speed = dodgeSpeed;
            dir *= speed;


            Vector3 projectedVelocity = Vector3.ProjectOnPlane(dir, normalVector);

            Debug.Log(projectedVelocity);
            Debug.Log(Vector3.back);
            rigidbody.velocity = Vector3.zero;
            rigidbody.velocity = projectedVelocity;
        }

        public void HandleJumping(float delta)
        {
            if (playerManager.isInteracting)
                return;

            if(inputHandler.jumpFlag && playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
                playerManager.isInAir = true;

                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                moveDirection.Normalize();
                moveDirection.y = 0;

                animatorHandler.PlayTargetAnimation("Jump", true);
                animatorHandler.anim.SetBool("isJumping", true);

                float speed = movementSpeed;
                if (playerManager.isSprinting) { speed = sprintSpeed; }

                rigidbody.AddForce(Vector3.up * jumpPower);
                rigidbody.AddForce(moveDirection * speed);
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            if (playerManager.isJumping)
                return;

            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if(playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 7f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if(Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if(playerManager.isInAir)
                {
                    if(inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for " + inAirTimer);
                        animatorHandler.PlayTargetAnimation("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Locomotion", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if(playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if(playerManager.isGrounded)
            {
                if(playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }
        }


        #endregion

        #region Jump



        #endregion
    }
}

