using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{
    public class PlayerMovement : MonoBehaviour
    {
        PlayerManager playerManager;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDir;

        [HideInInspector] public AnimatorHandler animatorHandler;
        [HideInInspector] public Transform myTransform;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceNeededToFall = 1f;
        [SerializeField] float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField] float walkSpeed = 10;
        [SerializeField] float rollSpeed = 15;
        [SerializeField] float runSpeed = 20;
        [SerializeField] float rotationSpeed = 10;
        [SerializeField] float fallingSpeed = 15;

        void Start()
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

            if (playerManager.isInteracting)
                return;

            moveDir = cameraObject.forward * inputHandler.vertical;
            moveDir += cameraObject.right * inputHandler.horizontal;
            moveDir.Normalize();
            moveDir.y = 0;

            float speed = walkSpeed;

            if (inputHandler.sprintFlag)
            {
                speed = runSpeed;
                playerManager.isSprinting = true;
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

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

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

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDir = Vector3.zero;
            }

            if(playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDir * fallingSpeed / 20f);
            }

            Vector3 dir = moveDir;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToFall, Color.red, 0.1f, false);
            if(Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToFall, ignoreForGroundCheck))
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
                        animatorHandler.PlayTargetAnimation("landing", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Standard Locomotion", false);
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

                if(playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (walkSpeed / 2);
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
    }
}
