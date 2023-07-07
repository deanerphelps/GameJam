using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{
    public class PlayerMovement : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDir;

        [HideInInspector] public AnimatorHandler animatorHandler;
        [HideInInspector] public Transform myTransform;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Stats")]
        [SerializeField] float walkSpeed = 10;
        [SerializeField] float rollSpeed = 15;
        [SerializeField] float runSpeed = 20;
        [SerializeField] float rotationSpeed = 10;

        public bool isSprinting;
        void Start()
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

            isSprinting = inputHandler.b_Input;
            inputHandler.TickInput(delta);
            HandleMovement(delta);
            HandleRollingAndSprinting(delta);
        }

        #region Movement
        Vector3 normalVector, targetPosition;

        private void HandleRotation(float deltaTime)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();

            if (targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * deltaTime);

            myTransform.rotation = targetRotation;
        }

        public void HandleMovement(float deltaTime)
        {
            if (inputHandler.rollFlag)
                return;

            moveDir = cameraObject.forward * inputHandler.vertical;
            moveDir += cameraObject.right * inputHandler.horizontal;
            moveDir.Normalize();
            moveDir.y = 0;

            float speed = walkSpeed;

            if (inputHandler.sprintFlag)
            {
                speed = runSpeed;
                isSprinting = true;
                moveDir *= speed;
            }
            else
            {
                speed = walkSpeed;
                moveDir *= speed;
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDir, normalVector);
            if(!inputHandler.rollFlag)
                rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, isSprinting);

            if (animatorHandler.canRotate)
            {
                HandleRotation(deltaTime);
            }
        }

        public void HandleRollingAndSprinting(float deltaTime)
        {
            if (animatorHandler.anim.GetBool("isInteracting"))
            {
               animatorHandler.StopRotation(); 
               return;
            }
                

            if (inputHandler.rollFlag)
            {
                moveDir = cameraObject.forward * inputHandler.vertical;
                moveDir += cameraObject.right * inputHandler.horizontal;
                moveDir.Normalize();
                moveDir.y = 0;

                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("rollFwd", true);
                    Quaternion rollRotation = Quaternion.LookRotation(moveDir);
                    myTransform.rotation = rollRotation;

                    float speed = rollSpeed;
                    moveDir *= speed;
                    Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDir, normalVector);
                    rigidbody.velocity = projectedVelocity;

                }

                else
                {
                    //animatorHandler.PlayTargetAnimation("backstep", true);
                }
            }

            animatorHandler.CanRotate();
        }
        #endregion
    }
}
