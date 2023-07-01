using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    public float walkSpeed = 10;
    public float sprintSpeed = 20;
    private float moveSpeed;
    public float jumpHeight = 2.5f;
    [HideInInspector] public Vector3 dir, jumpUp;
    float horizontalInput, verticalInput;
    bool jumpInput;
    public CharacterController controller;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePosition;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        GetDirectionAndMove();
        Gravity();
        Jump();
    }

    void GetDirectionAndMove()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        dir = transform.forward * verticalInput + transform.right * horizontalInput;

        if(Input.GetButton("Sprint"))
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        controller.Move(dir.normalized * moveSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        spherePosition = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePosition, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePosition, controller.radius - 0.05f);
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravity);
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
