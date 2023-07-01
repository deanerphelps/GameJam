using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)) 
        {
            PlayerTakeDmg(20);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerHeal(20);
        }
    }

    private void PlayerTakeDmg(int dmg)
    {
        GameManager.gameManager.playerHealth.DmgUnit(dmg);
        Debug.Log(GameManager.gameManager.playerHealth.Health);
    }

    private void PlayerHeal(int heal)
    {
        GameManager.gameManager.playerHealth.HealUnit(heal);
        Debug.Log(GameManager.gameManager.playerHealth.Health);
    }
}
