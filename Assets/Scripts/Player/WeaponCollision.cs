using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    public AttackManager attackManager;
    public GameObject hitParticle;
    public int weaponDmg;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && attackManager.isAttacking)
        {
            other.GetComponent<EnemyManager>().EnemyTakeDmg(weaponDmg);
            //other.GetComponent<Animator>().SetTrigger("Hit");
            //Instantiate(hitParticle, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), other.transform.rotation);
        }
    }
}
