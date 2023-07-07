using GJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    InputHandler inputHandler;
    Animator animator;
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        inputHandler.isInteracting = animator.GetBool("isInteracting");
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
    }

    private void PlayerTakeDmg(int dmg)
    {

    }

    private void PlayerHeal(int heal)
    {

    }
}
