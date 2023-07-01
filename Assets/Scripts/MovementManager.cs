using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    public float moveSpeed = 3;
    [HideInInspector] public Vector3 dir;
    float horizontalInput, verticalInput;
    public CharacterController controller;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePosition;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetDirectionAndMove();
        Gravity();
    }

    void GetDirectionAndMove()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        dir = transform.forward * verticalInput + transform.right * horizontalInput;

        controller.Move(dir.normalized * moveSpeed * Time.deltaTime);

    }

    bool IsGrounded()
    {
        spherePosition = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if(Physics.CheckSphere(spherePosition, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if(!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if(velocity.y < 0 ) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePosition, controller.radius - 0.05f);
    }
}
