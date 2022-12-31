using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerLocomotion : MonoBehaviour
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

        [Header("Stats")]
        [SerializeField] private float movementSpeed = 5;
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

            grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (inputHandler.JumpInput(delta))
            {
                Jump();
            }
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            if (grounded) { moveDirection.y = 0; }

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

        #region Movement

        private Vector3 normalVector;
        private Vector3 targetPosition;
        private void HandleRotation(float delta)
        {
            Vector3 targetDirection = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDirection = cameraObject.forward * inputHandler.vertical;
            targetDirection += cameraObject.right * inputHandler.horizontal;

            targetDirection.Normalize();
            targetDirection.y = 0;

            if(targetDirection == Vector3.zero)
            {
                targetDirection = myTransform.forward;
            }

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
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

