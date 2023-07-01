using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject dagger;
    public AudioClip daggerAttackSFX;
    public bool canAttack = true;
    public bool isAttacking = false;
    public float attackCooldown = 1.0f;

    private void Update()
    {
        if (!PauseMenuScript.isPaused)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (canAttack)
                {
                    DaggerAttack();
                }
            }
        }
    }

    public void DaggerAttack()
    {
        canAttack = false;
        isAttacking = true;
        //Animation
        //Animator anim = dagger.GetComponent<Animator>();
        //anim.SetTrigger("Attack");
        //AudioSource ac = GetComponent<AudioSource>();
        //ac.PlayOneShot(daggerAttackSFX);
        StartCoroutine(ResetAttackCooldown());
    }

    private IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackingState());
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator ResetAttackingState()
    {
        //AnimationTime
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

}
