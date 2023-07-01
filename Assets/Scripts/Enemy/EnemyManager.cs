using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int setMaxHealth = 100;
    public UnitHealth enemyHealth;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = new UnitHealth(setMaxHealth, setMaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth.Health <= 0)
        {
            Destroy(enemy);
        }
    }

    private void EnemyTakeDmg(int dmg)
    {
        enemyHealth.DmgUnit(dmg);
        Debug.Log(enemyHealth.Health);
    }

    private void EnemyHeal(int heal)
    {
        enemyHealth.HealUnit(heal);
        Debug.Log(enemyHealth.Health);
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyTakeDmg(10);
    }
}
