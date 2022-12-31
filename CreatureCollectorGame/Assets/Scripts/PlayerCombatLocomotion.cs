using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerCombatLocomotion : MonoBehaviour
    {
        private Transform cameraObject;
        private InputHandler inputHandler;
        private Vector3 moveDirection;

        [HideInInspector] public Transform myTransform;
        [HideInInspector] public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        private bool grounded;
        public Transform groundCheck;
        private float groundDistance = 0.4f;
        public LayerMask groundMask;

        public bool dodging;
        private Vector3 dodgeDir;

        [Header("Stats")]
        [SerializeField] private float movementSpeed = 5;
        [SerializeField] public float dodgeSpeed = 10;
        [SerializeField] private float rotationSpeed = 10;
        [SerializeField] private float jumpHeight = 5;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();
        }

        public void Update()
        {
            float delta = Time.deltaTime;

            inputHandler.TickInput(delta);
            if (dodging != true) { HandleMovement(delta); }
            else { DodgeMovement(dodgeDir, delta); }
            HandleDodge(delta);

            grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (inputHandler.JumpInput(delta))
            {
                Jump();
            }
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
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            moveDirection *= speed;
            
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleDodge(float delta)
        {
            if (animatorHandler.anim.GetBool("isInteracting"))
                return;

            if(inputHandler.dodgeFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                moveDirection.Normalize();
                moveDirection.y = 0;

                if (inputHandler.moveAmount > 0)
                {
                    DodgeMovement dm = animatorHandler.anim.GetBehaviour<DodgeMovement>();
                    if(dm.playerCombatLocomotion == null) { dm.playerCombatLocomotion = this; }
                    //dm.dir = moveDirection;

                    animatorHandler.PlayTargetAnimation("Dodge", true);
                    Quaternion dodgeRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = dodgeRotation;
                    dodgeDir = moveDirection;
                }
                else
                {
                    BackstepMovement bm = animatorHandler.anim.GetBehaviour<BackstepMovement>();
                    if (bm.playerCombatLocomotion == null) { bm.playerCombatLocomotion = this; }
                    //bm.dir = myTransform.forward; 

                    animatorHandler.PlayTargetAnimation("Backstep", true);
                    dodgeDir = -myTransform.forward;
                }
            }
        }

        public void DodgeMovement(Vector3 dir, float delta)
        {
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

        public void HandleSprinting(float delta)
        {

        }

        #endregion

        #region Jump

        private void Jump()
        {
            if (grounded == true)
            {
                grounded = false;
                Debug.Log("Jump Successful");
                rigidbody.AddForce(transform.up * jumpHeight);
            }
        }

        #endregion
    }
}
