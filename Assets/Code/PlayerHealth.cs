using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Health is: " + currentHealth);

        if (currentHealth <= 0)
        {
            //reset game
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
