using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal, vertical, moveAmount, mouseX, mouseY;

        PlayerControls inputActions;
        CameraHandler cameraHandler;

        Vector2 movementInput, cameraInput;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if(cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            }
        }
        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += inputActions => cameraInput = inputActions.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        public void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float deltaTime)
        {
            MoveInput(deltaTime);
        }
        public void MoveInput(float deltaTime)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
    }
}
