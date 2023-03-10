using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int totalHealth;
    protected int currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = totalHealth;
        Debug.Log("Health is: " + totalHealth);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Health is: " + currentHealth);

        if (currentHealth <= 0)
        {
            //die - overriden for Player to restart level
            Destroy(gameObject);
        }
    }
}
