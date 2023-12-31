using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJ
{
    public class AnimatorHandler : MonoBehaviour
    {
        PlayerManager playerManager;
        PlayerMovement playerMovement;
        public Animator anim;
        InputHandler inputHandler;
        int vertical;
        int horizontal;
        public bool canRotate;

        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerMovement = GetComponentInParent<PlayerMovement>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < .55f)
                v = 0.5f;
            else if (verticalMovement > 0.55f)
                v = 1;
            else if (verticalMovement < 0 && verticalMovement < -.55f)
                v = -.5f;
            else if (verticalMovement < -0.55f)
                v = -1;
            else
                v = 0;
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < .55f)
                h = 0.5f;
            else if (horizontalMovement > 0.55f)
                h = 1;
            else if (horizontalMovement < 0 && horizontalMovement < -.55f)
                h = -.5f;
            else if (horizontalMovement < -0.55f)
                h = -1;
            else
                h = 0;
            #endregion

            if(isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting) 
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnimation, 0.2f);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            playerMovement.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            //playerMovement.rigidbody.velocity = velocity;
        }
    }
}
