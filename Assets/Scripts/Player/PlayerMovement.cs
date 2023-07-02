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
        [SerializeField] float walkSpeed = 5;
        [SerializeField] float rotationSpeed = 10;
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

            inputHandler.TickInput(delta);

            moveDir = cameraObject.forward * inputHandler.vertical;
            moveDir += cameraObject.right * inputHandler.horizontal;
            moveDir.Normalize();
            moveDir.y = 0;

            float speed = walkSpeed;
            moveDir *= speed;
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDir, normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);
            
            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
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
        #endregion
    }
}
