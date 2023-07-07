using GJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    InputHandler inputHandler;
    Animator animator;
    CameraHandler cameraHandler;
    PlayerMovement playerMovement;

    

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
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
    }

    private void PlayerTakeDmg(int dmg)
    {

    }

    private void PlayerHeal(int heal)
    {

    }
}
