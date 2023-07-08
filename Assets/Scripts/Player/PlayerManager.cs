using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GJ
{
    public class PlayerManager : MonoBehaviour
    {

        InputHandler inputHandler;
        Animator animator;
        CameraHandler cameraHandler;
        PlayerMovement playerMovement;



        [Header("Player Flags")]
        public bool isInteracting;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        void Start()
        {
            cameraHandler = CameraHandler.singleton;
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Awake()
        {

        }

        void Update()
        {
            float delta = Time.deltaTime;

            inputHandler.TickInput(delta);
            playerMovement.HandleMovement(delta);
            playerMovement.HandleRollingAndSprinting(delta);
            playerMovement.HandleFalling(delta, playerMovement.moveDir);
        }
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;


            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        private void LateUpdate()
        {
            isInteracting = animator.GetBool("isInteracting");
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            isSprinting = inputHandler.b_Input;

            if (isInAir)
            {
                playerMovement.inAirTimer = playerMovement.inAirTimer + Time.deltaTime;
            }
        }

        private void PlayerTakeDmg(int dmg)
        {

        }

        private void PlayerHeal(int heal)
        {

        }
    }
}
