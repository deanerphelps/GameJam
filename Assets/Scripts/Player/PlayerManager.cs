using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    void Start()
    {
        GameManager.gameManager.SetPlayerMaxHealth();
    }

    void Update()
    {
        if (!PauseMenuScript.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                PlayerTakeDmg(20);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                PlayerHeal(20);
            }
        }
    }

    private void PlayerTakeDmg(int dmg)
    {
        GameManager.gameManager.playerHealth.DmgUnit(dmg);
        GameManager.gameManager.SetPlayerHealth();
        Debug.Log(GameManager.gameManager.playerHealth.Health);
    }

    private void PlayerHeal(int heal)
    {
        GameManager.gameManager.playerHealth.HealUnit(heal);
        GameManager.gameManager.SetPlayerHealth();
        Debug.Log(GameManager.gameManager.playerHealth.Health);
    }
}
