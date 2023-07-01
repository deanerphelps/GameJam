using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }
    public Slider healthBar;

    public UnitHealth playerHealth = new UnitHealth(100, 100);

    void Awake()
    {
        if(gameManager != null && gameManager != this)
        {
            Destroy(this);
        }

        else
        {
            gameManager = this;
        }
    }

    public void SetPlayerMaxHealth()
    {
        healthBar.maxValue = playerHealth.MaxHealth;
        healthBar.value = playerHealth.Health;
    }

    public void SetPlayerHealth()
    {
        healthBar.value = playerHealth.Health;
    }
}
